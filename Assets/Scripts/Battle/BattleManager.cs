using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

// The manager for the battle environment, handles turn taking, using of moves, running away etc.
public class BattleManager : MonoBehaviour
{
    enum BattleState { Starting, ActionSelection, AttackSelection, PlayerMove, EnemyMove, Ending }
    BattleState battleState;

    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleTextBox bottomBox;

    [System.NonSerialized] public UnityEvent<bool> OnBattleOver = new UnityEvent<bool>();
    [System.NonSerialized] public UnityEvent OnBattleRun = new UnityEvent();

    BattlerParty playerParty;
    BattlerParty enemyParty;

    private void Awake()
    {
        bottomBox.OnButtonPressed.AddListener(OnButtonPressed);
    }

    // Initialise the battle environment ready for the start of a new battle
    void Init(BattlerParty playerParty, BattlerParty enemyParty)
    {
        this.playerParty = playerParty;
        this.enemyParty = enemyParty;

        Battler player = playerParty.GetNextBattler();
        Battler enemy = enemyParty.GetNextBattler();

        player.Init();
        enemy.Init();

        playerUnit.SetDisplay(player);
        enemyUnit.SetDisplay(enemy);

    }

    // Initiates a new battle between the player, and the enemy
    public IEnumerator StartBattle(BattlerParty playerParty, BattlerParty enemyParty)
    {
        battleState = BattleState.Starting;
        Init(playerParty, enemyParty);

        bottomBox.ClearButtonTexts();
        yield return bottomBox.WriteToBottomText($"You encountered a {enemyUnit.Battler.Name}!");
        yield return new WaitForSeconds(2f);
        yield return SelectAction();
    }

    // Triggers the battle into the action slection state
    IEnumerator SelectAction()
    {
        bottomBox.ClearButtonTexts();
        yield return bottomBox.WriteToBottomText($"What should {playerUnit.Battler.Name} do?");
        bottomBox.SetButtonTexts(new List<string> { "Attack", "Ablity", "Item", "Run" });
        battleState = BattleState.ActionSelection;
    }

    // Triggers the battle into the attack slection state
    IEnumerator SelectAttack()
    {
        bottomBox.ClearButtonTexts();
        yield return bottomBox.WriteToBottomText($"Choose an attack");
        bottomBox.SetButtonTexts(playerUnit.Battler.Attacks);
        battleState = BattleState.AttackSelection;
    }

    // Choses and executes the enemies turn
    IEnumerator EnemyMove()
    {
        battleState = BattleState.EnemyMove;
        bottomBox.ClearButtonTexts();

        // TODO: In future enemy should have ability to perform other actions such as using items

        //Pick random attack
        Attack attack = enemyUnit.Battler.Attacks[Random.Range(0, enemyUnit.Battler.Attacks.Count)];
        yield return ExecuteAttack(attack, enemyUnit, playerUnit, SelectAttack());
    }

    // Executes the specified move, from the source to the target, then moves onto the next step
    IEnumerator ExecuteAttack(Attack attack, BattleUnit sourceBU, BattleUnit targetBU, IEnumerator nextStep)
    {
        Debug.Log($"{sourceBU.Name} status: {sourceBU.Battler.Status}");
        bottomBox.ClearButtonTexts();

        // Check if can attack due to stun
        if(sourceBU.Battler.Status == StatusID.STN)
        {
            // Randomise 50% chance
            if (Random.Range(0, 100) < 50)
            {
            yield return bottomBox.WriteToBottomText($"{sourceBU.Battler.Name} is stunned and cannot attack");
            yield return new WaitForSeconds(2f);
            StartCoroutine(nextStep);
            yield break;
            }

        }

        // Deal damage
        int calculatedDamage = (int) Mathf.Ceil((float)attack.Power * ((float)sourceBU.Battler.Attack / (float)targetBU.Battler.Defence));
        bool targetEliminated = targetBU.TakeDamage(calculatedDamage);
        
        yield return bottomBox.WriteToBottomText($"{sourceBU.Battler.Name} used {attack.Name}, it did {calculatedDamage} damage!");
        yield return new WaitForSeconds(2f);

        // Apply status
        bool statusApplied = false;
        if(attack.StatusEffect != StatusID.None && targetBU.Battler.Status == StatusID.None)
        {
            statusApplied = targetBU.ApplyStatus(attack.StatusEffect);
            if(statusApplied)
            {
                StatusCondition status = EffectsDatabase.StatusConditions[attack.StatusEffect];
                yield return bottomBox.WriteToBottomText($"{targetBU.Battler.Name} {status.InitialMessage}");
                yield return new WaitForSeconds(2f);
            }
        }




        // End the battle if the target was eliminated
        if (targetEliminated)
        {
            yield return bottomBox.WriteToBottomText($"{sourceBU.Battler.Name} eliminated {targetBU.Battler.Name}!");
            yield return new WaitForSeconds(2f);

            // TODO: Check if enemy team has any other remaining battlers (each side only has one for now)

            OnBattleOver.Invoke(sourceBU.IsPlayer); 
        }
        else
        {
            yield return nextStep;
        }
    }

    // Makes the payer flee, ending the battle
    IEnumerator PlayerRun()
    {
        battleState = BattleState.Ending;

        yield return bottomBox.WriteToBottomText($"You ran away from the battle");
        yield return new WaitForSeconds(2f);

        OnBattleRun.Invoke();
    }

    // Executes the funtionality of the buttons in the battle environments UI
    void OnButtonPressed(int buttonNo)
    {
        // Action Selection
        if (battleState == BattleState.ActionSelection)
        {
            if (buttonNo == 0)
            {
                StartCoroutine(SelectAttack());
            }
            else if (buttonNo == 1)
            {
                throw new NotImplementedException("Abilities are not yet implemented");
            }
            else if (buttonNo == 2)
            {
                throw new NotImplementedException("Items are not yet implemented");
            }
            else if (buttonNo == 3)
            {
                StartCoroutine(PlayerRun());
            }
        }
        // Attack Selection
        else if(battleState == BattleState.AttackSelection)
        {
            if(playerUnit.Battler.Attacks.Count > buttonNo && buttonNo < 4)
            {
                //StartCoroutine(PlayerAttack(playerUnit.Battler.Attacks[buttonNo]));

                // Use attack
                battleState = BattleState.PlayerMove;                
                StartCoroutine(ExecuteAttack(playerUnit.Battler.Attacks[buttonNo], playerUnit, enemyUnit, EnemyMove()));
            }
        }
    }
}