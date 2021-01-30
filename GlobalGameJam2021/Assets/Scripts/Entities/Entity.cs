using UnityEngine;
using DG.Tweening;

public abstract class Entity : MonoBehaviour {

    [SerializeField] protected TileActionData moveActionData;
    [SerializeField] protected TileActionData attackActionData;
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
    [SerializeField] protected float bomFallDuration = 0.4f;
    [SerializeField] protected Ease jumpEase;
    [SerializeField] protected Ease bomEase;
    [SerializeField] protected GameObject bom;

    public bool isDestroyed;

    public TileActionData MoveActionData => moveActionData;
    public TileActionData AttackActionData => attackActionData;

    public virtual void MoveToTile(Tile tile) {
        Tile currentTile = BoardManager.Instance.GetTile(transform.position);
        currentTile.RemoveEntity();
    }
    protected void GetDestroyed()
    {
        Tile tile = BoardManager.Instance.GetTile(transform.position);

        Instantiate(bom, transform.position + new Vector3(0,8,0), transform.rotation);
        transform.DOMoveY(0, bomFallDuration).SetEase(bomEase).OnComplete(() =>
        {
            transform.SetParent(tile.EntityHolder);
            BoardManager.Instance.GetTile(transform.position).BounceArea();
            BoardManager.Instance.TileToFlip(transform.position);
        });
    }

}
