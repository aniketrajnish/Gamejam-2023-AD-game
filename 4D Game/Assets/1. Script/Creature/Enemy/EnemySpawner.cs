using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SimpleObjectPool enemyPool;
    [SerializeField] private List<CreatureController> enemyList;
    [SerializeField] private float spawnDelay = 1f;
    [SerializeField] private float nextWaveDelay = 10f;
    [SerializeField] private int spawnEnemyCount = 5;
    [SerializeField] private int maxEnemyCount;

    private bool isSpawning = false;

    private void Start()
    {
        maxEnemyCount = spawnEnemyCount * 2;

        enemyList = new List<CreatureController>();
    }

    public void SpawnEnemyDefulat()
    {
        SpawnEnemy(spawnEnemyCount);
    }

    public void SpawnEnemy(int count)
    {
        if (!CheckEnemyCount())
        {
            StartCoroutine(NextWaveRoutine());
            return;
        }

        if(!isSpawning)
            StartCoroutine(SpawnEnemyRoutine(count));
    }

    public void DespawnEnemy(CreatureController enemy)
    {
        if (enemyList.Contains(enemy))
        {
            enemy.gameObject.SetActive(false);
            enemyList.Remove(enemy);
        }
    }

    private bool CheckEnemyCount()
    {
        if (enemyList.Count < maxEnemyCount)
        {
            return true;
        }
        else
        {
            int count = 0;
            foreach(var enemy in enemyList)
            {
                if (enemy.gameObject.activeSelf)
                {
                    count++;
                }
            }
            
            if (count < maxEnemyCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private IEnumerator SpawnEnemyRoutine(int count)
    {
        isSpawning = true;
        for (int i = 0; i < count; i++)
        {
            var enemy = enemyPool.GetPooledObject().GetComponent<CreatureController>();
            enemy.Init();
            enemy.transform.position = transform.position;
            enemyList.Add(enemy);
            enemy.gameObject.SetActive(true);
            yield return new WaitForSeconds(spawnDelay);
        }
        isSpawning = false;

        StartCoroutine(NextWaveRoutine());
    }

    private IEnumerator NextWaveRoutine()
    {
        yield return new WaitForSeconds(nextWaveDelay);
        SpawnEnemy(spawnEnemyCount);
    }
}
