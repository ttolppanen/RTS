using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTask : MonoBehaviour
{
    GameObject enemy;

    private void Start()
    {
        UnitMovement unitMov = GetComponent<UnitMovement>();
        enemy = unitMov.currentTask.objectives[0];
        UnitStatus enemyStatus = enemy.GetComponent<UnitStatus>();
        if (enemyStatus.isAlive)
        {
            enemyStatus.DealDamage(100);
        }
        GetComponent<UnitMovement>().Stop();
    }
}
