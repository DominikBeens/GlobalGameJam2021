using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : MonoState {

    public override void Enter(params object[] data) {
        Debug.LogError("GAME OVER!");
    }

    public override void Exit() {
    }

    public override void Tick() {
    }
}
