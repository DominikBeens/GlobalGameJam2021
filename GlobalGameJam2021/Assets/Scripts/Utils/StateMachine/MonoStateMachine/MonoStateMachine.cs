using System.Linq;
using UnityEngine;

public class MonoStateMachine : MonoState {

    [SerializeField] private Transform stateHolder;
    [SerializeField] private MonoState defaultState;

    public MonoState CurrentState { get; private set; }

    private MonoState[] states;

    protected virtual void Awake() {
        states = stateHolder?.GetComponentsInChildren<MonoState>(true);
        foreach (MonoState state in states) {
            state.gameObject.SetActive(true);
            state.Initialize(this);
            if (state == defaultState) {
                EnterState(state);
            } else {
                state.Exit();
            }
        }

        if (states.Length == 0) {
            Debug.LogWarning("No states found in statemachine!");
        }
    }

    private void OnDestroy() {
        foreach (MonoState state in states) {
            state.Deinitialize();
        }
    }

    private void Update() {
        Tick();
    }

    public override void Enter(params object[] data) {
    }

    public override void Exit() {
    }

    public override void Tick() {
        CurrentState?.Tick();
    }

    public void EnterState<T>(params object[] data) where T : MonoState {
        MonoState stateToEnter = GetState<T>();
        if (stateToEnter == null) {
            Debug.LogError($"Error entering MonoState: no state of type {typeof(T)} found!");
            return;
        }
        EnterState(stateToEnter, data);
    }

    public T GetState<T>() where T : MonoState {
        return (T)states?.FirstOrDefault(x => x.GetType() == typeof(T));
    }

    public void ExitCurrentState() {
        CurrentState?.Exit();
        CurrentState = null;
    }

    private void EnterState(MonoState state, params object[] data) {
        if (CurrentState == state) { return; }
        ExitCurrentState();
        CurrentState = state;
        CurrentState.Enter(data);
    }
}
