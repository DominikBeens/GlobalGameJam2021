using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerEntity : Entity {

    public MoveActionData MoveActionData => (MoveActionData)availableActions.FirstOrDefault(x => x is MoveActionData);
    public MoveActionData AttackActionData => (MoveActionData)availableActions.FirstOrDefault(x => x is AttackActionData);

    protected override void ExecuteMoveAction(MoveActionData data) {
        base.ExecuteMoveAction(data);
        // get player determined position
        // move player
    }
}
