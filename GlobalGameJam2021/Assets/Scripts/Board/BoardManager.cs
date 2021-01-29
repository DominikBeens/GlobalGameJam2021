using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    public List<EntityPlacement> Level = new List<EntityPlacement>();
    [SerializeField] private GameObject tile;
    private Tile[,] board;
    public PlayerEntity testTest;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            FlipBoard();
        }
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
        for (int x = 0; x < board.GetLength(0); x++)
        {
            StartCoroutine(FlipBoardZ(x));
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator FlipBoardZ(int xNumber)
    {
        for (int z = 0; z < board.GetLength(1); z++)
        {
            yield return new WaitForSeconds(0.05f);
            board[xNumber, z].FlipTile();
        }
    }

    public void SelectMoveSpots(PlayerEntity toUse)
    {
        SelectDirection(toUse.MoveActionData.North, toUse.gameObject.transform.position, toUse.North.position);
        SelectDirection(toUse.MoveActionData.East, toUse.gameObject.transform.position, toUse.East.position);
        SelectDirection(toUse.MoveActionData.South, toUse.gameObject.transform.position, toUse.South.position);
        SelectDirection(toUse.MoveActionData.West, toUse.gameObject.transform.position, toUse.West.position);
        SelectDirection(toUse.MoveActionData.NorthEast, toUse.gameObject.transform.position, toUse.NorthEast.position);
        SelectDirection(toUse.MoveActionData.NorthWest, toUse.gameObject.transform.position, toUse.NorthWest.position);
        SelectDirection(toUse.MoveActionData.SouthEast, toUse.gameObject.transform.position, toUse.SouthEast.position);
        SelectDirection(toUse.MoveActionData.SouthWest, toUse.gameObject.transform.position, toUse.SouthWest.position);
    }

    private void SelectDirection(int Amount, Vector3 currentPos, Vector3 directionPos)
    {
        for (int i = 0; i < Amount; i++)
        {
            Vector3 pos = (currentPos - directionPos);
            Vector2 dir = new Vector2(directionPos.x, directionPos.z);
            SelectTile(dir - new Vector2(pos.x, pos.z) * i);
        }
    }

    private void SelectTile(Vector2 toSelect)
    {
        int x = Mathf.RoundToInt(toSelect.x);
        int y = Mathf.RoundToInt(toSelect.y);
        if (x >= 0 && x < board.GetLength(0) && y >= 0 && y < board.GetLength(1))
        {
            board[x, y].gameObject.transform.position += new Vector3(0, 0.2f, 0);
        }
    }

}

[System.Serializable]
public struct EntityPlacement
{
    public Vector3 position;
    public GameObject MyEntity;
}
