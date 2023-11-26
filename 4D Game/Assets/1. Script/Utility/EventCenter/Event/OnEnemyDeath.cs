using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnemyDeath : IEventWithData
{
    public GameObject DeadEnemy;

    public OnEnemyDeath(GameObject deadEnemy)
    {
        DeadEnemy = deadEnemy;
    }
}
