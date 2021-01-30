using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    public List<Level> Level = new List<Level>();
    [SerializeField] private GameObject tile;
    private PlayerEntity myPlayerEntity;
    public List<EnemyEntity> enemiesOnBoard = new List<EnemyEntity>();
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
            FlipBoard(0);
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

        enemiesOnBoard.Clear();
        for (int i = 0; i < currentEntities.Count; i++)
        {
            int tilePlaceX = Mathf.RoundToInt(currentEntities[i].position.x);
            int tilePlaceY = Mathf.RoundToInt(currentEntities[i].position.z);
            Entity newEntity = Instantiate(currentEntities[i].MyEntity, currentEntities[i].position, currentEntities[i].MyEntity.transform.rotation, board[tilePlaceX, tilePlaceY].EntityHolder).GetComponent<Entity>();

            if (newEntity is EnemyEntity newEnemy)
            {
                enemiesOnBoard.Add(newEnemy);
            }

            board[tilePlaceX, tilePlaceY].AddEntity(newEntity);

            if (newEntity is PlayerEntity playerEntity)
            {
                myPlayerEntity = playerEntity;
            }
        }
    }

    #region Flip Tiles
    public void TileToFlip(Vector3 position)
    {
        int xPos = Mathf.RoundToInt(position.x);
        int yPos = Mathf.RoundToInt(position.z);
        board[xPos, yPos].FlipTile();
    }

    public void FlipBoard(int sideToFlip)
    {
        StartCoroutine(FlipActions(sideToFlip));
    }

    private IEnumerator FlipBoardIE(int sideToFlip)
    {
        for (int x = 0; x < board.GetLength(0); x++)
        {
            StartCoroutine(FlipBoardIEDiaginal(x, sideToFlip));
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator FlipBoardIEDiaginal(int xNumber, int sideToFlip)
    {
        for (int z = 0; z < board.GetLength(1); z++)
        {
            yield return new WaitForSeconds(0.05f);
            board[xNumber, z].FlipTile(sideToFlip);
        }
    }

    #endregion

    #region SelectMovement

    private List<Tile> HighlightedTiles = new List<Tile>();
    public void SelectPlayerMoveSpots()
    {
        if (HighlightedTiles.Count > 0)
        {
            DeSelectSpots();
        }

        HighlightedTiles = SelectTileSpots(myPlayerEntity.MoveActionData, myPlayerEntity);

        for (int i = 0; i < HighlightedTiles.Count; i++)
        {
            HighlightedTiles[i].Highlight();
        }
    }

    public void SelectPlayerAttackSpots()
    {
        if (HighlightedTiles.Count > 0)
        {
            DeSelectSpots();
        }

        HighlightedTiles = SelectTileSpots(myPlayerEntity.AttackActionData, myPlayerEntity);

        for (int i = 0; i < HighlightedTiles.Count; i++)
        {
            HighlightedTiles[i].Highlight();
        }
    }

    public List<Tile> SelectTileSpots(TileActionData toCheck, Entity newEntity)
    {
        List<Tile> newTiles = new List<Tile>();
        newTiles.AddRange(SelectDirection(toCheck.North, newEntity.gameObject.transform.position, newEntity.North.position));
        newTiles.AddRange(SelectDirection(toCheck.East, newEntity.gameObject.transform.position, newEntity.East.position));
        newTiles.AddRange(SelectDirection(toCheck.South, newEntity.gameObject.transform.position, newEntity.South.position));
        newTiles.AddRange(SelectDirection(toCheck.West, newEntity.gameObject.transform.position, newEntity.West.position));
        newTiles.AddRange(SelectDirection(toCheck.NorthEast, newEntity.gameObject.transform.position, newEntity.NorthEast.position));
        newTiles.AddRange(SelectDirection(toCheck.NorthWest, newEntity.gameObject.transform.position, newEntity.NorthWest.position));
        newTiles.AddRange(SelectDirection(toCheck.SouthEast, newEntity.gameObject.transform.position, newEntity.SouthEast.position));
        newTiles.AddRange(SelectDirection(toCheck.SouthWest, newEntity.gameObject.transform.position, newEntity.SouthWest.position));
        for (int i = newTiles.Count - 1; i >= 0; i--)
        {
            if (newTiles[i] == null)
            {
                newTiles.RemoveAt(i);
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
            if (board[x, y].Entity == null || !(board[x, y].Entity is StaticEntity))
            {
                return board[x, y];
            }
        }

        return null;
    }

    public void DeSelectSpots()
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

    public List<Tile> GetTileRange(Vector3 startPoint, int range)
    {
        List<Tile> newTileList = new List<Tile>();

        if (range % 2 == 0)
        {
            int offset = range / 2;

            newTileList.Add(GetTile(new Vector3(startPoint.x + offset, 0, startPoint.z + offset)));
            newTileList.Add(GetTile(new Vector3(startPoint.x + offset, 0, startPoint.z - offset)));
            newTileList.Add(GetTile(new Vector3(startPoint.x - offset, 0, startPoint.z + offset)));
            newTileList.Add(GetTile(new Vector3(startPoint.x - offset, 0, startPoint.z - offset)));
        }
        else
        {
            int offset = (range + 3) / 2;
            int Loffset = range / 2;

            for (int i = 0; i < range; i++)
            {
                newTileList.Add(GetTile(new Vector3(startPoint.x + offset - 1, 0, startPoint.z + i - Loffset)));
            }

            for (int i = 0; i < range; i++)
            {
                newTileList.Add(GetTile(new Vector3(startPoint.x - offset + 1, 0, startPoint.z + i - Loffset)));
            }

            for (int i = 0; i < range; i++)
            {
                newTileList.Add(GetTile(new Vector3(startPoint.x + i - Loffset, 0, startPoint.z + offset - 1)));
            }

            for (int i = 0; i < range; i++)
            {
                newTileList.Add(GetTile(new Vector3(startPoint.x + i - Loffset, 0, startPoint.z - offset + 1)));
            }
        }

        return newTileList;
    }

    public void MovePlayer(Tile moveToTile)
    {
        if (HighlightedTiles.Count > 0)
        {
            DeSelectSpots();
        }
        toMove = myPlayerEntity;
        this.moveToTile = moveToTile;
    }

    private Tile toAttack;

    public void PlayerAttack(Tile toAttack)
    {
        if (HighlightedTiles.Count > 0)
        {
            DeSelectSpots();
        }

        this.toAttack = toAttack;
    }


    private Entity toMove;
    private Tile moveToTile;

    public IEnumerator FlipActions(int sideToFlip)
    {
        yield return StartCoroutine(FlipBoardIE(sideToFlip));
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator PlayBoardActions()
    {
        if (moveToTile != null && moveToTile.Entity && moveToTile.Entity is EnemyEntity)
        {
            GameStateMachine.Instance.EnterState<GameEndState>(false);
            yield break;
        }

        if (toMove != null && moveToTile != null)
        {
            toMove.MoveToTile(moveToTile);
            toMove = null;
            moveToTile = null;
        }
        else if (toAttack.Entity != null)
        {
            toAttack.Entity.isDestroyed = true;
        }
        else
        {
            ProjectileManager.Instance.SendBom(toAttack.transform.position);
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = enemiesOnBoard.Count - 1; i >= 0; i--)
        {
            enemiesOnBoard[i].ExecuteAction();
            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
        }
        if (enemiesOnBoard.Count <= 0)
        {
            GameStateMachine.Instance.EnterState<GameEndState>(true);
        }
    }

    public void RemoveEntityFromBoard(Entity toRemove)
    {
        if (toRemove is EnemyEntity enemy)
        {
            enemiesOnBoard.Remove(enemy);
        }
    }
}