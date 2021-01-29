using UnityEngine;

public abstract class MonoState : MonoBehaviour {

    public MonoStateMachine StateMachine { get; private set; }
    public bool IsActive => StateMachine?.CurrentState == this;

    public virtual void Initialize(MonoStateMachine stateMachine) {
        StateMachine = stateMachine;
    }

    public virtual void Deinitialize() { }

    public abstract void Enter(params object[] data);
    public abstract void Exit();
    public abstract void Tick();
}