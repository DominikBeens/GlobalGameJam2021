using System.Collections;
using UnityEngine;

public class RoundWatchState : MonoState {

    [SerializeField] private float flippedDuration = 3f;
    [SerializeField] private float actionDelay = 1f;

    private Coroutine watchRoutine;

    public override void Enter(params object[] data) {
        watchRoutine = StartCoroutine(WatchRoutine());
    }

    public override void Exit() {
        if (watchRoutine != null) {
            StopCoroutine(watchRoutine);
        }
    }

    public override void Tick() { }

    private IEnumerator WatchRoutine() {
        BoardManager.Instance.FlipBoard();
        yield return new WaitForSeconds(actionDelay);
        yield return BoardManager.Instance.PlayBoardActions();
        yield return new WaitForSeconds(flippedDuration);
        StateMachine.EnterState<RoundPlayState>();
        watchRoutine = null;
    }
}
