using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class EnemyEntity : Entity {

    public void ExecuteAction() {
        List<Tile> attackTiles = BoardManager.Instance.SelectTileSpots(attackActionData, this);
        foreach (Tile tile in attackTiles) {
            if (tile.Entity is PlayerEntity) {
                Debug.LogError("GAME OVER");
                return;
            }
        }

        List<Tile> moveTiles = BoardManager.Instance.SelectTileSpots(moveActionData, this);
        if (moveTiles.Any(x => x.Entity == null)) {
            Tile randomTile = null;
            while (!randomTile) {
                Tile tile = moveTiles[Random.Range(0, moveTiles.Count)];
                if (tile.Entity == null) {
                    randomTile = tile;
                }
            }
            MoveToTile(randomTile);
        }
    }

    public override void MoveToTile(Tile tile) {
        base.MoveToTile(tile);
        tile.AddEntity(this);
        transform.DOJump(tile.EntityHolder.position, jumpHeight, 1, jumpDuration).SetEase(jumpEase).OnComplete(() => {
            transform.SetParent(tile.EntityHolder);
            tile.BounceArea();
        });
    }
}
