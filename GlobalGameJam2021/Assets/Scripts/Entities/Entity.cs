using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    [Header("Move")]
    [SerializeField] protected float jumpHeight = 1.5f;
    [SerializeField] protected float jumpDuration = 0.4f;
    [SerializeField] protected Ease jumpEase;

    public virtual void MoveToTile(Tile tile) {
        Tile currentTile = BoardManager.Instance.GetTile(transform.position);
        currentTile.RemoveEntity();
    }
}
