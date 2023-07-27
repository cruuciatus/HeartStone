using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public void MakeTurn()
    {
        StartCoroutine(EnemyTurn(GameManager.Instance.EnemyHandCards));
    }
    IEnumerator EnemyTurn(List<CardController> cards)
    {

        yield return new WaitForSeconds(1);
        int count = cards.Count == 1 ? 1 :
            Random.Range(0, cards.Count);

        for (int i = 0; i < count; i++)
        {
            if (GameManager.Instance.EnemyFieldCards.Count > 5 || GameManager.Instance.CurrentGame.Enemy.Mana == 0 || GameManager.Instance.EnemyHandCards.Count == 0)
                break;


            List<CardController> cardsList = cards.FindAll(x => GameManager.Instance.CurrentGame.Enemy.Mana >= x.Card.Manacost);

            if (cardsList.Count == 0)
                break;

            if (cardsList[0].Card.IsSpell)
            {
                CastSpell(cardsList[0]);
                yield return new WaitForSeconds(.51f);
            }else
            {
                cardsList[0].GetComponent<CardMovement>().MoveToField(GameManager.Instance.EnemyField);
                yield return new WaitForSeconds(.51f);
                cardsList[0].transform.SetParent(GameManager.Instance.EnemyField);
                cardsList[0].OnCast();
            }

        }
        yield return new WaitForSeconds(1);

        while (GameManager.Instance.EnemyFieldCards.Exists(x => x.Card.CanAttack))
        {
            var activeCard = GameManager.Instance.EnemyFieldCards.FindAll(x => x.Card.CanAttack)[0];
            bool hasProvocation = GameManager.Instance.PlayerFieldCards.Exists(x => x.Card.IsProvocation);

            if (hasProvocation || Random.Range(0, 2) == 0 &&
                GameManager.Instance.PlayerFieldCards.Count > 0)
            {
                CardController enemy;

                if (hasProvocation)
                {
                    enemy = GameManager.Instance.PlayerFieldCards.Find(x => x.Card.IsProvocation);

                }
                else
                {
                    enemy = GameManager.Instance.PlayerFieldCards[Random.Range(0, GameManager.Instance.PlayerFieldCards.Count)];
                }

                Debug.Log(activeCard.Card.Name + "(" + activeCard.Card.Attack + ";" + activeCard.Card.Defense + "--->" + enemy.Card.Name + "(" + enemy.Card.Attack + ";" + enemy.Card.Defense + ")");



                activeCard.Movement.MoveToTarget(enemy.transform);
                yield return new WaitForSeconds(.75f);

                GameManager.Instance.CardsFight(enemy, activeCard);
            }
            else
            {
                Debug.Log(activeCard.Card.Name + "(" + activeCard.Card.Attack + ")Attack Hero");
                activeCard.Card.CanAttack = false;

                activeCard.GetComponent<CardMovement>().MoveToTarget(GameManager.Instance.PlayerHero.transform);
                yield return new WaitForSeconds(.75f);

                GameManager.Instance.DamageHero(activeCard, false);
            }
            yield return new WaitForSeconds(.2f);

        }
        yield return new WaitForSeconds(1f);
        GameManager.Instance.ChangeTurn();
    }

    public void CastSpell(CardController card)
    {
        switch (((SpellCard)card.Card).SpellTarget)
        {
            case SpellCard.TargetType.noTarget:
                switch (((SpellCard)card.Card).Spell)
                {
                    case SpellCard.SpellType.healAllyFieldCards:
                        if (GameManager.Instance.EnemyFieldCards.Count > 0)
                        {
                            StartCoroutine(CastCard(card));
                        }
                        break;
                    case SpellCard.SpellType.damageEnemyFieldCards:
                        if (GameManager.Instance.PlayerFieldCards.Count > 0)
                        {
                            StartCoroutine(CastCard(card));
                        }
                        break;
                    case SpellCard.SpellType.healAllyHero:
                        StartCoroutine(CastCard(card));
                        break;
                    case SpellCard.SpellType.damageEnemyHero:
                        StartCoroutine(CastCard(card));
                        break;
                }

                break;
            case SpellCard.TargetType.allyCardTarget:
                if (GameManager.Instance.EnemyFieldCards.Count > 0)
                {
                    StartCoroutine(CastCard(card, GameManager.Instance.EnemyFieldCards[Random.Range(0,GameManager.Instance.EnemyFieldCards.Count)]));
                }
                break;
            case SpellCard.TargetType.enemyCardTarget:
                if (GameManager.Instance.PlayerFieldCards.Count > 0)
                {
                    StartCoroutine(CastCard(card, GameManager.Instance.PlayerFieldCards[Random.Range(0, GameManager.Instance.EnemyFieldCards.Count)]));
                }
                break;
        }
    }
    IEnumerator CastCard(CardController spell, CardController target = null)
    {
        if (((SpellCard)spell.Card).SpellTarget == SpellCard.TargetType.noTarget)
        {
            spell.GetComponent<CardMovement>().MoveToField(GameManager.Instance.EnemyField);
            yield return new WaitForSeconds(.51f);

            spell.OnCast();
        }
        else
        {
            spell.Info.ShowCardsInfo();
            spell.GetComponent<CardMovement>().MoveToTarget(target.transform);
            yield return new WaitForSeconds(.51f);

            GameManager.Instance.EnemyHandCards.Remove(spell);
            GameManager.Instance.EnemyFieldCards.Add(spell);
            GameManager.Instance.ReduceMana(false, spell.Card.Manacost);

            spell.Card.IsPlaced = true;
            spell.UseSpell(target);
        }

        string targetStr = target == null ? "no_target" : target.Card.Name;
        Debug.Log("AI spell cast:" + (spell.Card).Name + "target:" + targetStr);
    }
}