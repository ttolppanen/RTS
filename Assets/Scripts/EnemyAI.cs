using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public float seeingDistance;
    UnitMovement unitMov;
    UnitStatus unitStatus;
    public GameObject target;
    Vector2 movementDirection;

    private void Awake()
    {
        unitMov = GetComponent<UnitMovement>();
        unitStatus = GetComponent<UnitStatus>();
    }

    private void Start()
    {
        StartCoroutine(StartAttacking());
    }

    IEnumerator StartAttacking()//sekunnin välein päivitetään polkua vihuun jne...
    {
        if (target == null)
        {
            Collider2D[] colls = Physics2D.OverlapCircleAll((Vector2)(transform.position), seeingDistance, LayerMask.GetMask("Unit"));
            if (colls.Length != 0)
            {
                target = FindClosestEnemy(colls);
            }
        }
        else if (UF.DistanceBetween2Units(transform.position, target.transform.position) >= seeingDistance)
        {
            target = null;
            unitMov.Stop();
        }
        if(unitStatus.currentState != UnitStates.attacking && target != null)
        {
            Vector2Int start = UF.CoordinatePosition(transform.position);
            Vector2Int goal = UF.CoordinatePosition(target.transform.position);
            List<Vector2Int> path = Map.ins.AStarPathFinding(start, goal);
            Task attackingTask = new Task(GM.tasks[TaskTypes.attack], new List<GameObject> { target }, 1.5f);
            //unitMov.GoDoATask(path, attackingTask);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(StartAttacking());
    }

    GameObject FindClosestEnemy(Collider2D[] enemies)
    {
        float distance = 9999;
        GameObject newTarget = enemies[0].gameObject;
        foreach (Collider2D enemyColl in enemies)
        {
            GameObject enemy = enemyColl.gameObject;
            float newDistance = UF.DistanceBetween2Units(transform.position, enemy.transform.position);
            if (newDistance <= distance)
            {
                newTarget = enemy;
                distance = newDistance;
            }
        }
        return newTarget;
    }
}
