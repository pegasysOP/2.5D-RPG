using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main game functionality controller, i.e. swapping between walking and battling
public class GameManager : MonoBehaviour
{
    enum GameState { Overworld, Battle }

    [Header("Cameras")]
    [SerializeField] CinemachineBrain cameraBrain;
    [SerializeField] CinemachineVirtualCamera overworldCam;
    [SerializeField] CinemachineVirtualCamera battleCam;

    PlayerController player;
    BattleManager battleManager;
    GameState gameState;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        battleManager = FindObjectOfType<BattleManager>();
    }

    void Start()
    {
        player.OnInteraction.AddListener(OnPlayerInteracted);
        battleManager.OnBattleOver.AddListener(OnBattleOver);
        battleManager.OnBattleRun.AddListener(OnBattleRun);

        SetGameState(GameState.Overworld);
    }

    // Run when the player interacts with an object
    void OnPlayerInteracted(IOverworldInteractable interactable)
    {
        if(interactable.Type == InteractableType.EnemyBattler)
        {
            Debug.Log("GM > Interacting with Enemy Battler, triggering battle");
            SetGameState(GameState.Battle);            
            StartCoroutine(battleManager.StartBattle(((EnemyBattler)interactable).GetParty()));
        }
    }

    // Run when the battle is over, returns to overworld
    void OnBattleOver(bool won)
    {
        // TODO: When 'won' is false, the player should lose the game

        if (won)
        {
            Debug.Log("Player won the battle");
        }
        else
        {
            Debug.Log("Player lost the battle");
        }

        SetGameState(GameState.Overworld);
    }

    // Run when a battle is starting
    void OnBattleRun()
    {
        SetGameState(GameState.Overworld);
    }

    // Swaps between overworld and battle environments
    void SetGameState(GameState state)
    {
        gameState = state;

        if (state == GameState.Overworld)
        {
            overworldCam.Priority = 100;
            battleCam.Priority = 0;
            cameraBrain.ActiveBlend = null;
            player.EnableMovement(true);
        }
        else if (state == GameState.Battle)
        {
            battleCam.Priority = 1000;
            overworldCam.Priority = 0;
            cameraBrain.ActiveBlend = null;
            player.EnableMovement(false);
        }
    }
}
