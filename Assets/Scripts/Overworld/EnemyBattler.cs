using UnityEngine;

public class EnemyBattler : MonoBehaviour, IOverworldInteractable
{
    public InteractableType Type {  get { return InteractableType.EnemyBattler; } }

    [SerializeField] private BattlerParty party;

    public void Interact()
    {
        // this can be used in the future to play any animations, text before entering the battle
    }

    public BattlerParty GetParty()
    {
        return party;
    }
}
