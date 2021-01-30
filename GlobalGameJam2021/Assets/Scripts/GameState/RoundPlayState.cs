﻿using UnityEngine;
using TMPro;

public class RoundPlayState : MonoState {

    [SerializeField] private TextMeshProUGUI roundText;

    private TextAnimator roundTextAnimator;
    private int round;

    public override void Initialize(MonoStateMachine stateMachine) {
        base.Initialize(stateMachine);
        roundText.text = string.Empty;
        roundTextAnimator = roundText.GetComponent<TextAnimator>();
    }

    public override void Enter(params object[] data) {
        round++;
        roundTextAnimator.ShowText($"Round {round}");

        BoardManager.Instance.FlipBoard(2);
        PlayerAbilityManager.Instance.ToggleButtonInteractability(true);
        PlayerAbilityManager.Instance.OnAbilitySelected += OnAbilitySelectedHandler;
    }


    public override void Exit() {
        PlayerAbilityManager.Instance.ToggleButtonInteractability(false);
        PlayerAbilityManager.Instance.OnAbilitySelected -= OnAbilitySelectedHandler;
    }

    public override void Tick() { }

    private void OnAbilitySelectedHandler(PlayerAbilityManager.AbilityType abilityType) {
        if (abilityType == PlayerAbilityManager.AbilityType.Flare) { return; }
        StateMachine.EnterState<RoundWatchState>();
    }
}
