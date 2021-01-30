using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    public List<EntityPlacement> Level = new List<EntityPlacement>();
    [SerializeField] private GameObject tile;
    private PlayerEntity myPlayerEntity;
    private Tile[,] board;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            FlipBoard();
        }
    }

    public void BuildLevel(int sizeX, int sizeZ, List<EntityPlacement> entitiesToPlace)
    {
        GameObject tileHolder = Instantiate(new GameObject("[Tile Holder]"));

        board = new Tile[sizeX, sizeZ];
        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                board[x, z] = Instantiate(tile, tileHolder.transform).GetComponent<Tile>();
                board[x, z].gameObject.transform.position = new Vector3(x, 0, z);
            }
        }


        for (int i = 0; i < entitiesToPlace.Count; i++)
        {
            int tilePlaceX = Mathf.RoundToInt(entitiesToPlace[i].position.x);
            int tilePlaceY = Mathf.RoundToInt(entitiesToPlace[i].position.z);
            Entity newEntity = Instantiate(entitiesToPlace[i].MyEntity, entitiesToPlace[i].position, entitiesToPlace[i].MyEntity.transform.rotation, board[tilePlaceX, tilePlaceY].visual).GetComponent<Entity>();

            board[tilePlaceX, tilePlaceY].myEntity = newEntity;

            if (newEntity is PlayerEntity playerEntity)
            {
                myPlayerEntity = playerEntity;
            }
        }
    }

    #region Flip Tiles
    public void TileToFlip(int tilex, int tilez)
    {
        board[tilex, tilez].FlipTile();
    }

    public void FlipBoard()
    {
        StartCoroutine(FlipBoardIE());
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

    #endregion

    #region SelectMovement


    private List<Tile> HighlightedTiles = new List<Tile>();
    public void SelectMoveSpots()
    {
        if (HighlightedTiles.Count > 0)
        {
            DeSelectMoveSpots();
        }
        HighlightedTiles = new List<Tile>();
        SelectDirection(myPlayerEntity.MoveActionData.North, myPlayerEntity.gameObject.transform.position, myPlayerEntity.North.position);
        SelectDirection(myPlayerEntity.MoveActionData.East, myPlayerEntity.gameObject.transform.position, myPlayerEntity.East.position);
        SelectDirection(myPlayerEntity.MoveActionData.South, myPlayerEntity.gameObject.transform.position, myPlayerEntity.South.position);
        SelectDirection(myPlayerEntity.MoveActionData.West, myPlayerEntity.gameObject.transform.position, myPlayerEntity.West.position);
        SelectDirection(myPlayerEntity.MoveActionData.NorthEast, myPlayerEntity.gameObject.transform.position, myPlayerEntity.NorthEast.position);
        SelectDirection(myPlayerEntity.MoveActionData.NorthWest, myPlayerEntity.gameObject.transform.position, myPlayerEntity.NorthWest.position);
        SelectDirection(myPlayerEntity.MoveActionData.SouthEast, myPlayerEntity.gameObject.transform.position, myPlayerEntity.SouthEast.position);
        SelectDirection(myPlayerEntity.MoveActionData.SouthWest, myPlayerEntity.gameObject.transform.position, myPlayerEntity.SouthWest.position);
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
            if (board[x, y].myEntity == null || !(board[x, y].myEntity is StaticEntity))
            {
                HighlightedTiles.Add(board[x, y]);
                board[x, y].Highlight();
            }
        }
    }

    public void DeSelectMoveSpots()
    {
        for (int i = 0; i < HighlightedTiles.Count; i++)
        {
            HighlightedTiles[i].UnHighlight();
        }

        HighlightedTiles.Clear();
    }
    #endregion

    public void MovePlayer(Tile moveToTile)
    {
        if (HighlightedTiles.Count > 0)
        {
            DeSelectMoveSpots();
        }
        MoveEntity(moveToTile, myPlayerEntity);
    }



    public bool MoveEntity(Tile moveToTile, Entity toMove)
    {
        if (moveToTile.myEntity == null)
        {
            board[Mathf.RoundToInt(toMove.transform.position.x), Mathf.RoundToInt(toMove.transform.position.z)].myEntity = null;
            moveToTile.myEntity = toMove;
            toMove.MoveToTile(moveToTile);
            return true;
        }
        else
        {
            return false;
        }

        /*toMove.gameObject.transform.SetParent(moveToTile.gameObject.transform);
        toMove.transform.position = moveToTile.transform.position;*/
    }
}

[System.Serializable]
public struct EntityPlacement
{
    public Vector3 position;
    public GameObject MyEntity;
}
