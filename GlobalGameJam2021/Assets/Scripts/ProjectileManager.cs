using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ProjectileManager : Singleton<ProjectileManager>
{

    [SerializeField] protected GameObject bomFriendly;
    [SerializeField] protected GameObject bomEnemy;
    [SerializeField] protected GameObject pof;
    [SerializeField] protected Ease bomEase;
    [SerializeField] protected float bomFallDuration = 1f;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void SendBom(Entity toDestroy,bool enemyBom)
    {
        Tile tile = BoardManager.Instance.GetTile(toDestroy.transform.position);

        tile.RemoveEntity();
        BoardManager.Instance.RemoveEntityFromBoard(toDestroy);

        SpawnBom(tile, 1.25f, 0.25f, 4, enemyBom, () =>
        {
            BoardManager.Instance.TileToFlip(toDestroy.transform.position);
            Destroy(toDestroy.gameObject);
        });
    }

    public void SendBom(Vector3 toDestroy)
    {
        Tile tile = BoardManager.Instance.GetTile(toDestroy);
        SpawnBom(tile, 0.5f, 0.1f, 4, false);
    }

    private void SpawnBom(Tile tile, float bounceStrength, float bounceFallOff, int bounceRanges, bool enemyBom, Action onComplete = null)
    {
        GameObject projectile;
        if (enemyBom == false)
        {
            projectile = Instantiate(bomEnemy, tile.transform.position + new Vector3(0, 8, 0), bomEnemy.transform.rotation);
        }
        else
        {
            projectile = Instantiate(bomFriendly, tile.transform.position + new Vector3(0, 8, 0), bomFriendly.transform.rotation);
        }
        projectile.transform.DOMoveY(0, bomFallDuration).SetEase(bomEase).OnComplete(() =>
        {

            BounceTile(tile, bounceStrength);
            GameObject currentPof = Instantiate(pof, tile.transform.position, pof.transform.rotation);
            for (int i = 0; i < bounceRanges; i++)
            {
                int index = i + 1;
                float strength = bounceStrength - (index * bounceFallOff);
                BounceTiles(BoardManager.Instance.GetTileRange(tile.transform.position, index), strength, index * 0.1f);
            }

            onComplete?.Invoke();
            Destroy(projectile);
        });
    }

    private void BounceTiles(List<Tile> tiles, float strength, float delay = 0f)
    {
        foreach (Tile tile in tiles)
        {
            if (!tile) { continue; }
            BounceTile(tile, strength, delay);
        }
    }

    private void BounceTile(Tile tile, float strength, float delay = 0f)
    {
        tile.BounceDown(strength, delay);
    }
}
