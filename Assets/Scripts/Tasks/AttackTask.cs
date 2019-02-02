using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTask : MonoBehaviour
{
    GameObject enemy;

    private void Start()
    {
        UnitMovement unitMov = GetComponent<UnitMovement>();
        UnitStatus unitStatus = GetComponent<UnitStatus>();
        enemy = unitMov.currentTask.objectives[0];
        if (unitStatus.isAlive)
        {
            enemy.GetComponent<UnitStatus>().DealDamage(100);
        }
        GetComponent<UnitMovement>().Stop();
    }
}
