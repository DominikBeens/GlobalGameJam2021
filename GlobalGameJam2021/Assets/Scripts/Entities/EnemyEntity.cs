using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : Entity {

    protected override void ExecuteMoveAction(MoveActionData data) {
        base.ExecuteMoveAction(data);
        Tile tile = data.GetRandomPosition();
        //BoardManager.Instance.MoveEntity(this, tile);
    }
}
