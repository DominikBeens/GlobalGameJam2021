using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class PlayerEntity : Entity {

    public MoveActionData MoveActionData => (MoveActionData)availableActions.FirstOrDefault(x => x is MoveActionData);
    public MoveActionData AttackActionData => (MoveActionData)availableActions.FirstOrDefault(x => x is AttackActionData);

    public override void MoveToTile(Tile tile) {
        base.MoveToTile(tile);
        transform.DOJump(tile.EntityHolder.position, jumpHeight, 1, jumpDuration).SetEase(jumpEase).OnComplete(() => {
            transform.SetParent(tile.EntityHolder);
            tile.AddEntity(this);
        });
    }
}
