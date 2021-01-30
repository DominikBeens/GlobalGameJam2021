using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProjectileManager : Singleton<ProjectileManager>
{
    [SerializeField] protected GameObject bom;
    [SerializeField] protected Ease bomEase;
    [SerializeField] protected float bomFallDuration = 1f;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void SendBom(Entity toDestroy)
    {
        Tile tile = BoardManager.Instance.GetTile(toDestroy.transform.position);

        GameObject bomPorjectile = Instantiate(bom, toDestroy.transform.position + new Vector3(0, 8, 0), toDestroy.transform.rotation);
        bomPorjectile.transform.DOMoveY(0, bomFallDuration).SetEase(bomEase).OnComplete(() =>
        {
            Destroy(bomPorjectile);
            tile.BounceDown(1.5f);
            BoardManager.Instance.TileToFlip(toDestroy.transform.position);
            tile.RemoveEntity();
            BoardManager.Instance.RemoveEntityFromBoard(toDestroy);
            Destroy(toDestroy.gameObject);
        });
    }
    public void SendBom(Vector3 toDestroy)
    {
        Tile tile = BoardManager.Instance.GetTile(toDestroy);

        GameObject bomPorjectile = Instantiate(bom, toDestroy + new Vector3(0, 8, 0),bom.transform.rotation);
        bomPorjectile.transform.DOMoveY(0, bomFallDuration).SetEase(bomEase).OnComplete(() =>
        {
            Destroy(bomPorjectile);
            tile.BounceDown(0.5f);
        });
    }
}
