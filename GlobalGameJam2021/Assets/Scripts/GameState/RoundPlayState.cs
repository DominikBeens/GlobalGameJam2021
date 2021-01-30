using UnityEngine;
using TMPro;

public class RoundPlayState : MonoState {

    [SerializeField] private TextMeshProUGUI roundText;

    private int round;

    public override void Initialize(MonoStateMachine stateMachine) {
        base.Initialize(stateMachine);
        roundText.text = string.Empty;
    }

    public override void Enter(params object[] data) {
        round++;
        roundText.text = $"Round {round}";

        BoardManager.Instance.FlipBoard();
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
