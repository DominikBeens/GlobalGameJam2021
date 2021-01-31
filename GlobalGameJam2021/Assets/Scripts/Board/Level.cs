using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : ScriptableObject
{
    public List<EntityPlacement> entities = new List<EntityPlacement>();
    public int sizeX;
    public int sizeY;
    public string levelName;
    public int flareCooldown;
    public bool isTutorial;
    public float cameraYOffset;
}

[System.Serializable]
public struct EntityPlacement
{
    public Vector3 position;
    public GameObject MyEntity;
}