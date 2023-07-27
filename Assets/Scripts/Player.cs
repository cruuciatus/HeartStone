using UnityEngine;

public class Player
{
    public int HP, Mana, Manapool;
    const int maxManapool = 10;

    public Player()
    {
        HP = 30;
        Mana = Manapool = 2;
    }

    public void RestoreRoundMana()
    {
        Mana = Manapool;
    }
    public void IncreaseManapool()
    {
        Manapool = Mathf.Clamp(Manapool + 1, 0, maxManapool);
    }
    public void GetDamage(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, int.MaxValue);
    }
}
