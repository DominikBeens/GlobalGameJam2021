using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    public List<Level> Level = new List<Level>();
    [SerializeField] private GameObject tile;
    private PlayerEntity myPlayerEntity;
    public List<EnemyEntity> enimiesOnBoard = new List<EnemyEntity>();
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            myPlayerEntity.MoveToTile(GetTile(myPlayerEntity.transform.position));
        }
    }

    public void BuildLevel(int levelToLoad)
    {
        Level currentLevel = Level[levelToLoad];
        List<EntityPlacement> currentEntities = currentLevel.entities;

        GameObject tileHolder = new GameObject("[Tile Holder]");

        board = new Tile[currentLevel.sizeX, currentLevel.sizeY];
        for (int x = 0; x < currentLevel.sizeX; x++)
        {
            for (int z = 0; z < currentLevel.sizeY; z++)
            {
                board[x, z] = Instantiate(tile, tileHolder.transform).GetComponent<Tile>();
                board[x, z].gameObject.transform.position = new Vector3(x, 0, z);
            }
        }


        for (int i = 0; i < currentEntities.Count; i++)
        {
            int tilePlaceX = Mathf.RoundToInt(currentEntities[i].position.x);
            int tilePlaceY = Mathf.RoundToInt(currentEntities[i].position.z);
            Entity newEntity = Instantiate(currentEntities[i].MyEntity, currentEntities[i].position, currentEntities[i].MyEntity.transform.rotation, board[tilePlaceX, tilePlaceY].EntityHolder).GetComponent<Entity>();

            if (newEntity is EnemyEntity newEnemy)
            {
                enimiesOnBoard.Add(newEnemy);
            }

            board[tilePlaceX, tilePlaceY].AddEntity(newEntity);

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
        StartCoroutine(FlipActions());
    }

    private IEnumerator FlipBoardIE()
    {
        for (int x = 0; x < board.GetLength(0); x++)
        {
            StartCoroutine(FlipBoardIEDiaginal(x));
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator FlipBoardIEDiaginal(int xNumber)
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
    public void SelectPlayerMoveSpots()
    {
        if (HighlightedTiles.Count > 0)
        {
            DeSelectMoveSpots();
        }
        HighlightedTiles = SelectMoveSpots(myPlayerEntity.MoveActionData, myPlayerEntity);
        for (int i = 0; i < HighlightedTiles.Count; i++)
        {
            HighlightedTiles[i].Highlight();
        }
    }

    public List<Tile> SelectMoveSpots(MoveActionData toCheck, Entity newEntity)
    {
        List<Tile> newTiles = new List<Tile>();

        List<Tile> directionTiles = SelectDirection(toCheck.North, newEntity.gameObject.transform.position, newEntity.North.position);
        for (int i = 0; i < directionTiles.Count; i++)
        {
            if (directionTiles[i] != null)
            {
                newTiles.Add(directionTiles[i]);
            }
        }

        directionTiles = SelectDirection(toCheck.East, newEntity.gameObject.transform.position, newEntity.East.position);
        for (int i = 0; i < directionTiles.Count; i++)
        {
            if (directionTiles[i] != null)
            {
                newTiles.Add(directionTiles[i]);
            }
        }

        directionTiles = SelectDirection(toCheck.South, newEntity.gameObject.transform.position, newEntity.South.position);
        for (int i = 0; i < directionTiles.Count; i++)
        {
            if (directionTiles[i] != null)
            {
                newTiles.Add(directionTiles[i]);
            }
        }

        directionTiles = SelectDirection(toCheck.West, newEntity.gameObject.transform.position, newEntity.West.position);
        for (int i = 0; i < directionTiles.Count; i++)
        {
            if (directionTiles[i] != null)
            {
                newTiles.Add(directionTiles[i]);
            }
        }

        directionTiles = SelectDirection(toCheck.NorthEast, newEntity.gameObject.transform.position, newEntity.NorthEast.position);
        for (int i = 0; i < directionTiles.Count; i++)
        {
            if (directionTiles[i] != null)
            {
                newTiles.Add(directionTiles[i]);
            }
        }

        directionTiles = SelectDirection(toCheck.NorthWest, newEntity.gameObject.transform.position, newEntity.NorthWest.position);
        for (int i = 0; i < directionTiles.Count; i++)
        {
            if (directionTiles[i] != null)
            {
                newTiles.Add(directionTiles[i]);
            }
        }

        directionTiles = SelectDirection(toCheck.SouthEast, newEntity.gameObject.transform.position, newEntity.SouthEast.position);
        for (int i = 0; i < directionTiles.Count; i++)
        {
            if (directionTiles[i] != null)
            {
                newTiles.Add(directionTiles[i]);
            }
        }

        directionTiles = SelectDirection(toCheck.SouthWest, newEntity.gameObject.transform.position, newEntity.SouthWest.position);
        for (int i = 0; i < directionTiles.Count; i++)
        {
            if (directionTiles[i] != null)
            {
                newTiles.Add(directionTiles[i]);
            }
        }

        return newTiles;
    }

    private List<Tile> SelectDirection(int Amount, Vector3 currentPos, Vector3 directionPos)
    {
        List<Tile> newTiles = new List<Tile>();

        for (int i = 0; i < Amount; i++)
        {
            Vector3 pos = (currentPos - directionPos);
            Vector2 dir = new Vector2(directionPos.x, directionPos.z);
            newTiles.Add(SelectTile(dir - new Vector2(pos.x, pos.z) * i));
        }
        return newTiles;
    }

    private Tile SelectTile(Vector2 toSelect)
    {
        int x = Mathf.RoundToInt(toSelect.x);
        int y = Mathf.RoundToInt(toSelect.y);
        if (x >= 0 && x < board.GetLength(0) && y >= 0 && y < board.GetLength(1))
        {
            if (board[x, y].myEntity == null || !(board[x, y].myEntity is StaticEntity))
            {
                return board[x, y];
            }
        }

        return null;
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

    public Tile GetTile(Vector3 getTilePos)
    {
        int xPos = Mathf.RoundToInt(getTilePos.x);
        int yPos = Mathf.RoundToInt(getTilePos.z);

        if (xPos >= 0 && xPos < board.GetLength(0) && yPos >= 0 && yPos < board.GetLength(1))
        {
            return board[xPos, yPos];
        }
        else
        {
            return null;
        }
    }

    public void MovePlayer(Tile moveToTile)
    {
        if (HighlightedTiles.Count > 0)
        {
            DeSelectMoveSpots();
        }
        MoveEntity(moveToTile, myPlayerEntity);
    }


    private Entity toMove;
    private Tile moveToTile;

    public IEnumerator FlipActions()
    {
        yield return StartCoroutine(FlipBoardIE());
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator PlayBoardActions()
    {
        if (toMove != null && moveToTile != null)
        {
            toMove.MoveToTile(moveToTile);
            toMove = null;
            moveToTile = null;
        }

        yield return new WaitForSeconds(0.5f);

        foreach (EnemyEntity enemy in enimiesOnBoard)
        {
            enemy.ExecuteAction();
            yield return new WaitForSeconds(Random.Range(0.2f,0.5f));
        }
    }

    public bool MoveEntity(Tile newMoveToTile, Entity newToMove)
    {

        if (newMoveToTile.myEntity == null)
        {
            toMove = newToMove;
            moveToTile = newMoveToTile;
            return true;
        }
        else
        {
            return false;
        }
    }
}