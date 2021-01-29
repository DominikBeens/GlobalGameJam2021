using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<GameManager>
{
    [SerializeField] private GameObject tile;
    private Tile[,] board;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        BuildLevel(10, 10, new List<EntityPlacement>());
    }

    public void BuildLevel(int sizeX, int sizeY, List<EntityPlacement> entitiesToPlace)
    {
        board = new Tile[sizeX, sizeY];
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                board[x, y] = Instantiate(tile).GetComponent<Tile>();
                board[x, y].gameObject.transform.position = new Vector3(x, y, 0);
            }
        }
    }
}

public struct EntityPlacement
{
    public Vector2 position;

}
