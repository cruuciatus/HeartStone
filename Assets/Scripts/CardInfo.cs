using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfo : MonoBehaviour
{
    public CardController cardController;
    public Image Logo;
    public TextMeshProUGUI Name, Attack, Defense, Manacost, AbilityDescription;
    public GameObject HighLightedObj;
    public GameObject HideObj;
   

    public Color NormalCol;
    public Color TargetCol;
    public Color SpellTargetCol;

    public void HideCardInfo()
    {
       
        HideObj.SetActive(true);
       
        Manacost.text = "";

    }

    public void ShowCardsInfo()
    {
        

        HideObj.SetActive(false);
        

        Logo.sprite = cardController.Card.Logo;
        Logo.preserveAspect = true;
        Name.text = cardController.Card.Name;
        AbilityDescription.text = cardController.Card.AbilityDescription;

        if (cardController.Card.IsSpell)
        {
            Defense.gameObject.SetActive(false);
            Attack.gameObject.SetActive(false);
        }
        RefreshData();
        


    }

    public void RefreshData()
    {
        Attack.text = cardController.Card.Attack.ToString();
        Defense.text = cardController.Card.Defense.ToString();
        Manacost.text = cardController.Card.Manacost.ToString();
    }

    public void HighLightCard(bool highlight)
    {
        HighLightedObj.SetActive(highlight);
    }


    public void HighlightManaAvaliability(int currentMana)
    {
        GetComponent<CanvasGroup>().alpha = currentMana >= cardController.Card.Manacost ? 1 : 0.5f;


    }

    public void HighLightAsTarget(bool highlight)
    {
        GetComponent<Image>().color = highlight ? TargetCol : NormalCol; 
    }
    public void HighLightSpellTarget(bool highlight)
    {
        GetComponent<Image>().color = highlight ? SpellTargetCol : NormalCol;
    }
}
