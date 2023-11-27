using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : SimpleSingleton<LevelManager>
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject levelParent;
    [SerializeField] private List<GameObject> levelList;
    [SerializeField] private List<EnemySpawner> spawnerList;

    private int currentLevelIndex = 0;

    public int LevelCount { get { return levelList.Count; } }

    public void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        EventCenter.RegisterEvent<OnDimensionChanging>(OnDimensionChanging);
    }

    private void OnDestroy()
    {
        EventCenter.UnRegisterEvent<OnDimensionChanging>(OnDimensionChanging);
    }

    public void ChangeLevel(int index)
    {
        if(index < 0 || index >= levelList.Count)
        {
            return;
        }

        levelList[currentLevelIndex].gameObject.SetActive(false);
        currentLevelIndex = index;
        MapManager.Instance.ChangePathMap(index);
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public void SpawnEnemies()
    {
        foreach(var spawner in spawnerList)
        {
            spawner.SpawnEnemyDefulat();
        }
    }

    private void OnDimensionChanging(OnDimensionChanging data)
    {
        if(data.isChanging)
        {
            if (data.NextLevelIndex < levelList.Count)
            {
                levelList[data.NextLevelIndex].gameObject.SetActive(true);
            }
            else
            {
                levelList[0].gameObject.SetActive(true);
            }
        }
    }
}
