using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        transform.position = tile.EntityHolder.position;
        transform.SetParent(tile.EntityHolder);
        tile.AddEntity(this);
        tile.BounceArea();
    }
}
