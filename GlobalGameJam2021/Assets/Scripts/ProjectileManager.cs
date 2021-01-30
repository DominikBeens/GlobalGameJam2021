using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ProjectileManager : Singleton<ProjectileManager> {

    [SerializeField] protected GameObject bom;
    [SerializeField] protected Ease bomEase;
    [SerializeField] protected float bomFallDuration = 1f;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void SendBom(Entity toDestroy) {
        Tile tile = BoardManager.Instance.GetTile(toDestroy.transform.position);

        tile.RemoveEntity();
        BoardManager.Instance.RemoveEntityFromBoard(toDestroy);

        SpawnBom(tile, 1.25f, 0.25f, 4, () => {
            BoardManager.Instance.TileToFlip(toDestroy.transform.position);
            Destroy(toDestroy.gameObject);
        });
    }

    public void SendBom(Vector3 toDestroy) {
        Tile tile = BoardManager.Instance.GetTile(toDestroy);
        SpawnBom(tile, 0.5f, 0.1f, 4);
    }

    private void SpawnBom(Tile tile, float bounceStrength, float bounceFallOff, int bounceRanges, Action onComplete = null) {
        GameObject projectile = Instantiate(bom, tile.transform.position + new Vector3(0, 8, 0), bom.transform.rotation);
        projectile.transform.DOMoveY(0, bomFallDuration).SetEase(bomEase).OnComplete(() => {

            BounceTile(tile, bounceStrength);
            for (int i = 0; i < bounceRanges; i++) {
                int index = i + 1;
                float strength = bounceStrength - (index * bounceFallOff);
                BounceTiles(BoardManager.Instance.GetTileRange(tile.transform.position, index), strength, index * 0.1f);
            }

            onComplete?.Invoke();
            Destroy(projectile);
        });
    }

    private void BounceTiles(List<Tile> tiles, float strength, float delay = 0f) {
        foreach (Tile tile in tiles) {
            BounceTile(tile, strength, delay);
        }
    }

    private void BounceTile(Tile tile, float strength, float delay = 0f) {
        tile.BounceDown(strength, delay);
    }
}
