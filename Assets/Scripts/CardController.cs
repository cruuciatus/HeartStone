using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public Card Card;
    public bool IsPlayerCard;
    public CardInfo Info;
    public CardMovement Movement;
    public CardAbility Ability;

    GameManager gameManager;

    public void Init(Card card, bool isPlayerCard)
    {
        Card = card;
        gameManager = GameManager.Instance;
        IsPlayerCard = isPlayerCard;

        if (isPlayerCard)
        {
            Info.ShowCardsInfo();
            GetComponent<AttackCard>().enabled = false;
        }
        else
        {
            Info.HideCardInfo();
        }
    }

    public void OnCast()
    {
        if (IsPlayerCard)
        {
            if (Card.IsSpell &&((SpellCard) Card).SpellTarget != SpellCard .TargetType.noTarget)
                return;
                
            
            gameManager.PlayerHandCards.Remove(this);
            gameManager.PlayerFieldCards.Add(this);
            gameManager.ReduceMana(true, Card.Manacost);
            gameManager.CheckCardsForManaAvaliablity();

        }
        else
        {
            gameManager.EnemyHandCards.Remove(this);
            gameManager.EnemyFieldCards.Add(this);
            gameManager.ReduceMana(false, Card.Manacost);
            Info.ShowCardsInfo();
        }
        Card.IsPlaced = true;
        if (Card.HasAbility)
            Ability.OnCast();

        if (Card.IsSpell)
            UseSpell(null);
        UIController.Instance.UpdateHPAndMana();
    }

    public void OnTakeDamage(CardController attacker = null)
    {
        CheckForAlive();
        Ability.OnDamageTake(attacker);
    }

    public void OnDamageDeal()
    {
        Card.TimesDealedDamage++;
        Card.CanAttack = false;
        Info.HighLightCard(false);

        if (Card.HasAbility)
        {
            Ability.OnDamageDeal();
        }
    }
    public void UseSpell(CardController target)
    {
        var spellCard = (SpellCard)Card;
        switch (spellCard.Spell)
        {
            case SpellCard.SpellType.healAllyFieldCards:
                var allyCards = IsPlayerCard ? gameManager.PlayerFieldCards : gameManager.EnemyFieldCards;
                foreach (var card in allyCards)
                {
                    card.Card.Defense += spellCard.SpellValue;
                    card.Info.RefreshData();
                }
                break;
            case SpellCard.SpellType.damageEnemyFieldCards:
                var enemyCards = IsPlayerCard ? new List<CardController>(gameManager.EnemyFieldCards) :
                                                new List<CardController>(gameManager.PlayerFieldCards);
                foreach (var card in enemyCards)
                    GiveDamageTo(card, spellCard.SpellValue);


                break;
            case SpellCard.SpellType.healAllyHero:
                if (IsPlayerCard)
                {
                    gameManager.CurrentGame.Player.HP += spellCard.SpellValue;
                }
                else
                {
                    gameManager.CurrentGame.Enemy.HP += spellCard.SpellValue;
                }
                UIController.Instance.UpdateHPAndMana();

                break;
            case SpellCard.SpellType.damageEnemyHero:
                if (IsPlayerCard)
                {
                    gameManager.CurrentGame.Enemy.HP -= spellCard.SpellValue;
                }
                else
                {
                    gameManager.CurrentGame.Player.HP -= spellCard.SpellValue;
                }
                UIController.Instance.UpdateHPAndMana();
                gameManager.CheckGorResult();

                break;
            case SpellCard.SpellType.healAllyCards:
                target.Card.Defense += spellCard.SpellValue;
                break;
            case SpellCard.SpellType.damageEnemyCards:
                GiveDamageTo(target, spellCard.SpellValue);
                break;
            case SpellCard.SpellType.shielOnAllyCards:
                if (!target.Card.Abilities.Exists(x => x == Card.AbilityType.shield))
                {
                    target.Card.Abilities.Add(Card.AbilityType.shield);
                }
                break;
            case SpellCard.SpellType.provocationOnAllyCards:
                if (!target.Card.Abilities.Exists(x => x == Card.AbilityType.provocation))
                {
                    target.Card.Abilities.Add(Card.AbilityType.provocation);
                }
                break;
            case SpellCard.SpellType.buffCardsDamage:
                target.Card.Attack += spellCard.SpellValue;
                break;
            case SpellCard.SpellType.debuffCardDamage:
                target.Card.Attack -= Mathf.Clamp(target.Card.Attack - spellCard.SpellValue, 0, int.MaxValue);
                break;
        }
        if (target != null)
        {
            target.Ability.OnCast();
            target.CheckForAlive();
        }
        DestroyCard();

    }


    public void GiveDamageTo(CardController card, int damage)
    {
        card.Card.GetDamage(damage);
        card.CheckForAlive();
        card.OnTakeDamage();
    }
    public void CheckForAlive()
    {
        if (Card.IsAlive)
        {
            Info.RefreshData();

        }
        else
        {
            DestroyCard();
        }
    }
    public void DestroyCard()
    {
        Movement.OnEndDrag(null);

        RemoveCardFromList(gameManager.EnemyFieldCards);
        RemoveCardFromList(gameManager.PlayerFieldCards);
        RemoveCardFromList(gameManager.EnemyHandCards);
        RemoveCardFromList(gameManager.PlayerHandCards);

        Destroy(gameObject);
    }
    void RemoveCardFromList(List<CardController> list)
    {
        if (list.Exists(x => x == this))
        {
            list.Remove(this);
        }
    }

}
