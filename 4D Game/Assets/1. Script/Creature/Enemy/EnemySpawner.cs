using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SimpleObjectPool enemyPool;
    [SerializeField] private List<CreatureController> enemyList;
    [SerializeField] private float spawnDelay = 1f;
    [SerializeField] private int maxEnemyCount = 5;

    private bool isSpawning = false;

    private void Start()
    {
        enemyList = new List<CreatureController>();
        SpawnEnemy(maxEnemyCount);
    }

    public void SpawnEnemy(int count)
    {
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

    private IEnumerator SpawnEnemyRoutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var enemy = enemyPool.GetPooledObject().GetComponent<CreatureController>();
            enemy.transform.position = transform.position;
            enemy.gameObject.SetActive(true);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
