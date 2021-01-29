using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {

    [SerializeField] private List<EntityActionData> availableActions = new List<EntityActionData>();
    [Space]
    [SerializeField] private GameObject visual;

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
        }
    }

    protected virtual void ExecuteMoveAction(MoveActionData data) { }
}
