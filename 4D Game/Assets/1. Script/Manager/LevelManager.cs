using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SimpleSingleton<LevelManager>
{
    [SerializeField] private GameObject player;

    public void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public GameObject GetPlayer()
    {
        return player;
    }
}
