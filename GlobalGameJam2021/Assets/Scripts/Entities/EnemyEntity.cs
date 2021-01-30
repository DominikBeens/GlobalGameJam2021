using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : Entity {

    public override void MoveToTile(Tile tile) {
        base.MoveToTile(tile);
        transform.position = tile.EntityHolder.position;
        transform.SetParent(tile.EntityHolder);
        tile.AddEntity(this);
    }

    public void ExecuteAction()
    {

    }
}
