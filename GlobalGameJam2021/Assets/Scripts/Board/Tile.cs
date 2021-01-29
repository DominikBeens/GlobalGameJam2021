using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isFlipped = false;

    public void FlipTile()
    {
        isFlipped = !isFlipped;
        gameObject.transform.localEulerAngles = new Vector3(0, 0, gameObject.transform.localEulerAngles.x + 180); //Test
    }

    public void Hover()
    {
        transform.position += new Vector3(0,0.1f,0);
    }

    public void UnHover()
    {
        transform.position += new Vector3(0, -0.1f, 0);
    }
}
