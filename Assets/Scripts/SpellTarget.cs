using UnityEngine;
using UnityEngine.EventSystems;

public class SpellTarget : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {

        if (!GameManager.Instance.IsPlayerTurn)
            return;
        CardController spell = eventData.pointerDrag.GetComponent<CardController>(),
                       target = GetComponent<CardController>();

        if (spell && spell.Card.IsSpell && spell.IsPlayerCard && target.Card.IsPlaced && GameManager.Instance.CurrentGame.Player.Mana >= spell.Card.Manacost)
        {
            var spellCard = (SpellCard)spell.Card; 
            if ((spellCard.SpellTarget == SpellCard.TargetType.allyCardTarget && target.IsPlayerCard) || (spellCard.SpellTarget == SpellCard.TargetType.enemyCardTarget && !target.IsPlayerCard))
            {
                GameManager.Instance.ReduceMana(true, spell.Card.Manacost);
                spell.UseSpell(target);
                GameManager.Instance.CheckCardsForManaAvaliablity();

            }
        }
    }
 }
