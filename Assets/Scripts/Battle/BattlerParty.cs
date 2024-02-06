using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The collection of individual Battlers capable of fighting on one side of a battle
public class BattlerParty : MonoBehaviour
{
    [SerializeField] List<Battler> party;

    // Returns the next battler in the party that is still alive, if there are none then return null
    public Battler GetNextBattler()
    {
        if (party.Count > 0)
        {
            foreach (Battler battler in party)
            {
                if (battler.HP >= 0)
                {
                    return battler;
                }
            }
        }

        return null;
    }

    public int NumberOfAliveBattlers
    {
        get
        {
            int counter = 0;
            if (party.Count > 0)
            {
                foreach (Battler battler in party)
                {
                    if (battler.HP >= 0)
                    {
                        counter++;
                    }
                }
            }
            return counter;
        }
    }

    public List<Battler> Party
    { get { return party; } }
}
