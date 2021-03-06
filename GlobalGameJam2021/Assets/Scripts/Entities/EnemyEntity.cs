﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class EnemyEntity : Entity {
    [SerializeField] protected float rotateDuration = 0.4f;
    [SerializeField] protected Ease turnEase;
    [Space]
    [SerializeField, Range(0f, 1f)] private float standChance = 0.25f;

    public void ExecuteAction() {
        if (isDestroyed == true) {
            GetDestroyed();
            return;
        }

        foreach (Tile tile in GetAttackTiles()) {
            if (tile.Entity is PlayerEntity) {
                GameStateMachine.Instance.EnterState<GameEndState>(false);
                ProjectileManager.Instance.SendBom(tile.Entity,true);
                return;
            }
        }

        if (Random.value < standChance) {
            // Stand still and do nothing.
            return;
        }

        int randomInt = Random.Range(0, 2);

        if (rotateActionData.canUse == false || moveActionData.canUse == true && randomInt < 1)
        {
            MoveMe();
        } else if (rotateActionData.canUse == true && randomInt < 2) {
            RotateMe();
        }
    }

    public void MoveMe() {
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

    public void RotateMe() {
        List<Tile> rotateTiles = BoardManager.Instance.SelectTileSpots(RotateActionData, this);
        if (rotateTiles.Any(x => x.Entity == null)) {
            Tile tile = rotateTiles[Random.Range(0, rotateTiles.Count)];
            RotateToTile(tile);
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

    public void RotateToTile(Tile tile) {

        Vector3 targetPostition = new Vector3(tile.transform.position.x, transform.position.y, tile.transform.position.z);
        GameObject rotateTo = Instantiate(new GameObject(), transform.position, transform.rotation);

        rotateTo.transform.LookAt(targetPostition);
        transform.DORotate(rotateTo.transform.eulerAngles, rotateDuration).SetEase(turnEase);
        Destroy(rotateTo);
    }

    public List<Tile> GetAttackTiles() {
        return BoardManager.Instance.SelectTileSpots(attackActionData, this);
    }
}
