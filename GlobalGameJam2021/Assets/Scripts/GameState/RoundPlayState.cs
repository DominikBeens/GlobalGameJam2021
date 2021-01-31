using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class RoundPlayState : MonoState {

    public event Action OnRoundIncreased = delegate { };

    [SerializeField] private TextMeshProUGUI roundText;

    private Coroutine enterRoutine;
    private TextAnimator roundTextAnimator;
    private int round;

    public override void Initialize(MonoStateMachine stateMachine) {
        base.Initialize(stateMachine);
        roundText.text = string.Empty;
        roundTextAnimator = roundText.GetComponent<TextAnimator>();
    }

    public override void Enter(params object[] data) {
        enterRoutine = StartCoroutine(EnterRoutine());
    }

    public override void Exit() {
        PlayerAbilityManager.Instance.ToggleButtonInteractability(false);
        PlayerAbilityManager.Instance.OnAbilitySelected -= OnAbilitySelectedHandler;
        if (enterRoutine != null) {
            StopCoroutine(enterRoutine);
        }
    }

    public override void Tick() { }

    private void OnAbilitySelectedHandler(PlayerAbilityManager.AbilityType abilityType) {
        if (abilityType == PlayerAbilityManager.AbilityType.Flare) { return; }
        GameStateMachine.Instance.EnterState<RoundWatchState>();
    }

    private IEnumerator EnterRoutine() {
        round++;
        roundTextAnimator.ShowText($"Round {round}");
        OnRoundIncreased();

        BoardManager.Instance.FlipBoard(2);

        yield return ShowLethalTilesRoutine();
        yield return new WaitForSeconds(0.5f);

        PlayerAbilityManager.Instance.ToggleButtonInteractability(true);
        PlayerAbilityManager.Instance.OnAbilitySelected += OnAbilitySelectedHandler;

        enterRoutine = null;
    }

    private IEnumerator ShowLethalTilesRoutine() {
        if (!BoardManager.Instance.currentLevel.isTutorial) { yield break; }

        yield return new WaitForSeconds(1f);

        List<Tile> lethalTiles = new List<Tile>();
        foreach (EnemyEntity enemy in BoardManager.Instance.enemiesOnBoard) {
            lethalTiles.AddRange(enemy.GetAttackTiles());
        }
        foreach (Tile tile in lethalTiles) {
            tile.FlashLethal();
        }

        yield return new WaitForSeconds(0.5f);
    }
}
