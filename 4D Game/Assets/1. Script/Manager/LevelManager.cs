using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SimpleSingleton<LevelManager>
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject levelParent;
    [SerializeField] private List<GameObject> levelList;

    private int currentLevelIndex = 0;

    public int LevelCount { get { return levelList.Count; } }

    public void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void ChangeLevel(int index)
    {
        if(index < 0 || index >= levelList.Count)
        {
            return;
        }
        currentLevelIndex = index;
        MapManager.Instance.ChangePathMap(index);
    }

    public GameObject GetPlayer()
    {
        return player;
    }

}
