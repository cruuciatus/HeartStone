using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum FieldType
{
    selfHand,
    selfField,
    enemyHand,
    enemyField
}

public class DropPlace : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{

    public FieldType Type;
    public void OnDrop(PointerEventData eventData)
    {

        if (Type != FieldType.selfField)
        {
            return;
        }
        CardController card = eventData.pointerDrag.GetComponent<CardController>();

        if (card && GameManager.Instance.IsPlayerTurn && GameManager.Instance.CurrentGame.Player.Mana >= card.Card.Manacost && !card.Card.IsPlaced)
        {
            if(!card.Card.IsSpell)
            card.Movement.DefaultParent = transform;
            card.OnCast();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || Type == FieldType.enemyField || Type == FieldType.enemyHand || Type == FieldType.selfHand) 
            return;
        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();

        if (card)
            card.DefaultTempCardParent = transform;


    }


    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();

        if (card&& card.DefaultTempCardParent == transform)
            card.DefaultTempCardParent = card.DefaultParent;


    }
}
