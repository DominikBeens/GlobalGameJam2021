using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundWatchState : MonoState {

    [SerializeField] private float flippedDuration = 3f;
    [SerializeField] private float actionDelay = 1f;

    public override void Enter(params object[] data) {
        StartCoroutine(WatchRoutine());
    }

    public override void Exit() {
    }

    public override void Tick() {
    }

    private IEnumerator WatchRoutine()
    {
        BoardManager.Instance.FlipBoard();
        yield return new WaitForSeconds(actionDelay);
        yield return (BoardManager.Instance.PlayBoardActions());
        yield return new WaitForSeconds(flippedDuration);
        StateMachine.EnterState<RoundPlayState>();
    }
}
