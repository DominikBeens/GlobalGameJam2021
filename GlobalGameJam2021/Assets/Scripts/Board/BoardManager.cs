using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    public List<EntityPlacement> Level = new List<EntityPlacement>();
    [SerializeField] private GameObject tile;
    private Tile[,] board;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void BuildLevel(int sizeX, int sizeZ, List<EntityPlacement> entitiesToPlace)
    {
        board = new Tile[sizeX, sizeZ];
        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                board[x, z] = Instantiate(tile).GetComponent<Tile>();
                board[x, z].gameObject.transform.position = new Vector3(x, 0, z);
            }
        }

        for (int i = 0; i < entitiesToPlace.Count; i++)
        {
            Instantiate(entitiesToPlace[i].MyEntity, entitiesToPlace[i].position, entitiesToPlace[i].MyEntity.transform.rotation);
        }
    }

    public void FlipBoard()
    {
        StartCoroutine(FlipBoardIE());
    }

    public void TileToFlip(int tilex, int tilez)
    {
        board[tilex, tilez].FlipTile();
    }

    public IEnumerator FlipBoardIE()
    {
        yield return new WaitForSeconds(2f);

        for (int x = 0; x < board.GetLength(0); x++)
        {
            StartCoroutine(FlipBoardZ(x));
            yield return new WaitForSeconds(0.4f);
            board[x, 0].FlipTile();
        }
    }

    public IEnumerator FlipBoardZ(int xNumber)
    {
        for (int z = 0; z < board.GetLength(1); z++)
        {
            StartCoroutine(FlipBoardIE());
            yield return new WaitForSeconds(0.4f);
            board[xNumber, z].FlipTile();
        }
    }
}

[System.Serializable]
public struct EntityPlacement
{
    public Vector2 position;
    public GameObject MyEntity;
}
