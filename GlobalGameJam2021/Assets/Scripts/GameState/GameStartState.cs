using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartState : MonoState {

    public override void Enter(params object[] data) {
        StartCoroutine(StartRoutine());
    }

    public override void Exit() { }
    public override void Tick() { }

    private IEnumerator StartRoutine() {
        PlayerAbilityManager.Instance.Initialize(true);
        PlayerAbilityManager.Instance.ToggleButtonInteractability(false);

        yield return new WaitForSeconds(1f);

        StateMachine.EnterState<RoundPlayState>();
    }
}
