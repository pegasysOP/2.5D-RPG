using System;
using System.Collections.Generic;
using UnityEngine;

// A single Battler, capable of attacking and taking damage
[Serializable]
public class Battler
{
    [SerializeField] string name;
    [SerializeField] Sprite sprite;

    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int defence;
    [SerializeField] int speed;
    int hp;

    [SerializeField] List<Attack> attacks;

    StatusID status;

    // Initialise the battler ready for the start of battle
    public void Init()
    {
        hp = maxHP;
        status = StatusID.None;
        // In future, HP and Status should not be reset going into the start of a battle
    }

    // Lowers the battlers health by the value specified, returns true if they are now dead
    public bool TakeDamage(int amount)
    {
        HP -= amount;
        if (HP <= 0)
        {
            HP = 0;
            return true;
        }
        return false;
    }

    //
    public bool ApplyStatus(StatusID status)
    {
        this.status = status;
        return true;
    }

    public string Name
    { get { return name; } }
    public Sprite Sprite
    { get { return sprite; } }
    public int MaxHP
    { get { return maxHP; } }
    public int Attack
    { get { return attack; } }
    public int Defence
    { get { return defence; } }
    public int Speed
    { get { return speed; } }

    public int HP
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp > maxHP)
            {
                hp = maxHP;
            }
            else if (hp < 0)
            {
                hp = 0;
            }
        }
    }

    public StatusID Status
    { get { return status; } }

    public List<Attack> Attacks
    { get { return attacks; } }
}

[Serializable]
public class Attack
{
    [SerializeField] string name;
    [SerializeField] int power;
    [SerializeField] StatusID statusEffect;
    
    public string Name
    { get { return name; } }

    public int Power
    { get { return power; } }

    public StatusID StatusEffect
    { get { return statusEffect; } }
}