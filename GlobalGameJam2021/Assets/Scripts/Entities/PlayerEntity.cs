using DG.Tweening;

public class PlayerEntity : Entity {

    public override void MoveToTile(Tile tile) {
        base.MoveToTile(tile);
        tile.AddEntity(this);
        transform.DOJump(tile.EntityHolder.position, jumpHeight, 1, jumpDuration).SetEase(jumpEase).OnComplete(() => {
            transform.SetParent(tile.EntityHolder);
            tile.BounceArea();
        });
    }
}
