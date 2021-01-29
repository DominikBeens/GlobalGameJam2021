using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private static BoardManager instance;

    public static BoardManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BoardManager>();
            }

            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        DontDestroyOnLoad(gameObject);
    }
}
