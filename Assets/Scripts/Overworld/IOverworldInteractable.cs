/// <summary>
/// All potential types of things the player can interact with
/// </summary>
public enum InteractableType
{
    EnemyBattler
}

/// <summary>
/// Interface for all things the player can interact with in the overworld
/// </summary>
public interface IOverworldInteractable
{
    public InteractableType Type { get; }

    public void Interact();
}