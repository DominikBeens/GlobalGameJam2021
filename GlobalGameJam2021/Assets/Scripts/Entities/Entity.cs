﻿using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {

    [SerializeField] protected List<EntityActionData> availableActions = new List<EntityActionData>();
    [Space]
    [SerializeField] private GameObject visual;
    [Space]

    public Transform North;
    public Transform East;
    public Transform South;
    public Transform West;
    public Transform NorthEast;
    public Transform SouthEast;
    public Transform SouthWest;
    public Transform NorthWest;

    public void ExecuteAction() {
        if (availableActions.Count <= 0) { return; }
        EntityActionData action = availableActions[Random.Range(0, availableActions.Count)];
        ExecuteAction(action);
    }

    private void ExecuteAction(EntityActionData action) {
        switch (action) {
            case MoveActionData x:
                ExecuteMoveAction(x);
                break;
            case AttackActionData x:
                ExecuteAttackAction(x);
                break;
        }
    }

    protected virtual void ExecuteMoveAction(MoveActionData data) { }
    protected virtual void ExecuteAttackAction(AttackActionData data) { }
}
