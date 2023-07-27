using System.Collections.Generic;
using UnityEngine;




public class Card
{
    public enum AbilityType
    {
        noAbility,
        instantActive,
        doubleAttack,
        shield,
        provocation,
        regenerationEachTurn,
        counterAttack
    }
    
    public string Name;
    public string AbilityDescription;
    public Sprite Logo;
    public int Attack;
    public int Defense;
    public int Manacost;
    public bool IsMag;
    public bool CanAttack;
    public bool IsPlaced;

    public List<AbilityType> Abilities;
    public bool IsSpell;
    public bool IsAlive
    {
        get
        {
            return Defense > 0;
        }
    }
   
    public int TimesDealedDamage;

    public bool HasAbility
    {
        get
        {
            return Abilities.Count > 0;
        }
    }
    public bool IsProvocation
    {
        get
        {
            return Abilities.Exists(x => x == AbilityType.provocation);
        }
    }
    public Card(string name, string abilityDescription, string logoPath, int attack, int defense, int manacost, AbilityType abilityType = 0)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        Defense = defense;
        Manacost = manacost;
        CanAttack = false;
        IsPlaced = false;
        AbilityDescription = abilityDescription;
            
        Abilities = new List<AbilityType>();
        if (abilityType != 0)
        {
            Abilities.Add(abilityType);
        }


        TimesDealedDamage = 0;
    }

    public Card(Card card)
    {
        Name = card.Name;
        AbilityDescription = card.AbilityDescription;
        Logo = card.Logo;
        Attack = card.Attack;
        Defense = card.Defense;
        Manacost = card.Manacost;
        CanAttack = false;
        IsPlaced = false;

        Abilities = new List<AbilityType>(card.Abilities);
         TimesDealedDamage = 0;
    }



    public void GetDamage(int dmg)
    {
        if (dmg > 0)
        {
            if (Abilities.Exists(x => x == AbilityType.shield))
            {
                Abilities.Remove(AbilityType.shield);
            }
            else
            {

            }
            Defense -= dmg;
        }

    }
    public Card GetCopy()
    {
        return new Card(this);
    }
}

public class SpellCard : Card
{
    public enum SpellType
    {
        noSpell,
        healAllyFieldCards,
        damageEnemyFieldCards,
        healAllyHero,
        damageEnemyHero,
        healAllyCards,
        damageEnemyCards,
        shielOnAllyCards,
        provocationOnAllyCards,
        buffCardsDamage,
        debuffCardDamage
    }


    public enum TargetType
    {
        noTarget,
        allyCardTarget,
        enemyCardTarget
    }
    public TargetType SpellTarget;
    public SpellType Spell;
    public int SpellValue;

    public SpellCard(string name, string logoPath, string abilityDescription, int manacost, SpellType spellType = 0, int spellValue = 0, TargetType targetType = 0) : base(name, logoPath, abilityDescription, 0, 0, manacost)
    {
        IsSpell = true;
        Spell = spellType;
        SpellTarget = targetType;
        SpellValue = spellValue;
    }
    public SpellCard(SpellCard card) : base(card)
    {
        IsSpell = true;
        Spell = card.Spell;
        SpellTarget = card.SpellTarget;
        SpellValue = card.SpellValue;

    }
    public new SpellCard GetCopy()
    {
        return new SpellCard(this);
    }
}


public static class CardManagerList
{
    public static List<Card> AllCards = new List<Card>();


}

public class CardManager : MonoBehaviour
{
    
    public void Awake()
    {
            CardManagerList.AllCards.Add(new Card("Misha", "", "Sprites/Cards/Misha", 5, 5, 4));
            CardManagerList.AllCards.Add(new Card("Frostwolf", "", "Sprites/Cards/Frostwolf", 6, 5, 6));
            CardManagerList.AllCards.Add(new Card("Gnomish", "", "Sprites/Cards/Gnomish", 1, 1, 1));
            CardManagerList.AllCards.Add(new Card("Acidic", "", "Sprites/Cards/Acidic", 1, 2, 1));
            CardManagerList.AllCards.Add(new Card("Murlock", "", "Sprites/Cards/Murlock", 1, 2, 1));
            CardManagerList.AllCards.Add(new Card("Varrior", "", "Sprites/Cards/Varrior", 5, 6, 4));
            CardManagerList.AllCards.Add(new Card("Voodoo", "", "Sprites/Cards/Voodoo", 9, 2, 6));
            CardManagerList.AllCards.Add(new Card("Kobold", "", "Sprites/Cards/Kobold", 3, 6, 6));
            CardManagerList.AllCards.Add(new Card("Crocolisk", "", "Sprites/Cards/Crocolisk", 3, 3, 4));
            CardManagerList.AllCards.Add(new Card("Mechanic", "", "Sprites/Cards/Mechanic", 3, 5, 8));
        

        CardManagerList.AllCards.Add(new Card("Goldshire", "Provocation", "Sprites/Cards/Goldshire", 1, 2, 3, Card.AbilityType.provocation));
        CardManagerList.AllCards.Add(new Card("Shattered", "Regeneration", "Sprites/Cards/Shattered", 6, 5, 6, Card.AbilityType.regenerationEachTurn));
        CardManagerList.AllCards.Add(new Card("Darkscale", "DoubleAttack", "Sprites/Cards/Darkscale", 2, 3, 3, Card.AbilityType.doubleAttack));
        CardManagerList.AllCards.Add(new Card("Grimscale", "InstanseActive", "Sprites/Cards/Grimscale", 1, 2, 3, Card.AbilityType.instantActive));
        CardManagerList.AllCards.Add(new Card("Stormwind", "Shield", "Sprites/Cards/Stormwind", 1, 2, 3, Card.AbilityType.shield));
        CardManagerList.AllCards.Add(new Card("RaidLeader", "ConterAttack", "Sprites/Cards/RaidLeader", 5, 6, 7, Card.AbilityType.counterAttack));


        CardManagerList.AllCards.Add(new SpellCard("Archmage", "HealAllyFieldCardOn1", "Sprites/Cards/Archmage", 2, SpellCard.SpellType.healAllyFieldCards, 1, SpellCard.TargetType.noTarget));
        CardManagerList.AllCards.Add(new SpellCard("CoreHound","DamageEnemyFieldCardsOn3","Sprites/Cards/CoreHound", 2, SpellCard.SpellType.damageEnemyFieldCards, 3, SpellCard.TargetType.noTarget));
        CardManagerList.AllCards.Add(new SpellCard("Stonetusk", "DamageEnemyHeroOn1", "Sprites/Cards/Stonetusk", 2, SpellCard.SpellType.damageEnemyHero, 1, SpellCard.TargetType.noTarget));
        CardManagerList.AllCards.Add(new SpellCard("OrgHeal", "HealAllyHeroOn1", "Sprites/Cards/OrgHeal", 2,SpellCard.SpellType.healAllyHero, 1, SpellCard.TargetType.noTarget));
        CardManagerList.AllCards.Add(new SpellCard("Magma", "DamageEnemyCardsOn2", "Sprites/Cards/Magma", 2,  SpellCard.SpellType.damageEnemyCards, 2, SpellCard.TargetType.enemyCardTarget));
        CardManagerList.AllCards.Add(new SpellCard("Shieldmasta", "HealAllyCardsOn3", "Sprites/Cards/Shieldmasta", 2,  SpellCard.SpellType.healAllyCards, 3, SpellCard.TargetType.allyCardTarget));
        CardManagerList.AllCards.Add(new SpellCard("Stormpike", "ShieldOnAllyCards", "Sprites/Cards/Archmage",  2,  SpellCard.SpellType.shielOnAllyCards, 0, SpellCard.TargetType.allyCardTarget));
        CardManagerList.AllCards.Add(new SpellCard("WarGolem", "ProvocationOnAllyCards", "Sprites/Cards/Archmage", 2,  SpellCard.SpellType.provocationOnAllyCards, 0, SpellCard.TargetType.allyCardTarget));
        CardManagerList.AllCards.Add(new SpellCard("Dragon", "BuffCardsDamageOn2", "Sprites/Cards/MechanicalDragonling",  2,  SpellCard.SpellType.buffCardsDamage, 2, SpellCard.TargetType.allyCardTarget));
        CardManagerList.AllCards.Add(new SpellCard("Nightblade", "DebuffCardsDamageOn2", "Sprites/Cards/Nightblade",  2, SpellCard.SpellType.debuffCardDamage, 2, SpellCard.TargetType.enemyCardTarget));

    }
}
