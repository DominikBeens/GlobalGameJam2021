using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundWatchState : MonoState {

    [SerializeField] private float flippedDuration = 3f;

    public override void Enter(params object[] data) {
        BoardManager.Instance.FlipBoard();
        CoroutineHelper.Delay(() => {
            StateMachine.EnterState<RoundPlayState>();
        }, flippedDuration);
    }

    public override void Exit() {
    }

    public override void Tick() {
    }
}
