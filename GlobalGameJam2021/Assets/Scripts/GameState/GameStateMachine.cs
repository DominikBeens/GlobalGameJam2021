using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoStateMachine {

    public static GameStateMachine Instance { get; private set; }

    protected override void Awake() {
        Instance = this;
        base.Awake();
    }
}
