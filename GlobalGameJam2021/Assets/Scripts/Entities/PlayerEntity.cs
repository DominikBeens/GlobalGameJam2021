using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerEntity : Entity {

    public MoveActionData MoveActionData => (MoveActionData)availableActions.FirstOrDefault(x => x is MoveActionData);
    public MoveActionData AttackActionData => (MoveActionData)availableActions.FirstOrDefault(x => x is AttackActionData);

    public override void MoveToTile(Tile tile) {
        transform.position = tile.EntityHolder.position;
        transform.SetParent(tile.EntityHolder);
        tile.AddEntity(this);
    }
}
