using System.Collections;
using UnityEngine;

public class GameStartState : MonoState {

    public override void Enter(params object[] data) {
        StartCoroutine(StartRoutine());
    }

    public override void Exit() { }
    public override void Tick() { }

    private IEnumerator StartRoutine() {
        GameStateMachine.Instance.ToggleGameCanvas(true, 0.4f);
        GameStateMachine.Instance.ToggleGameEndCanvas(false, 0f);

        PlayerAbilityManager.Instance.Initialize();
        PlayerAbilityManager.Instance.ToggleButtonInteractability(false);

        yield return new WaitForSeconds(2f);

        StateMachine.EnterState<RoundPlayState>();
    }
}
