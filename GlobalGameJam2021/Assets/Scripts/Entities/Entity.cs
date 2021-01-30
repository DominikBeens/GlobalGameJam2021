using System.Collections.Generic;
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

    public virtual void MoveToTile(Tile tile) { }
}
