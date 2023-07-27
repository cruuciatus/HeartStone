using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackHero : MonoBehaviour, IDropHandler

{
    public enum HeroType
    {
        Enemy,
        Player
    }

    public HeroType Type;
   
    public Color NormalCol;
    public Color TargetCol;

    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManager.Instance.IsPlayerTurn)
            return;
        CardController card = eventData.pointerDrag.GetComponent<CardController>();

        if (card && card.Card.CanAttack && Type == HeroType.Enemy && !GameManager.Instance.EnemyFieldCards.Exists(x => x.Card.IsProvocation))
        {
            
            GameManager.Instance.DamageHero(card, true);
        }
    }
    public void HighLightAsTarget(bool highlight)
    {
        GetComponent<Image>().color = highlight ? TargetCol : NormalCol;
    }
}
