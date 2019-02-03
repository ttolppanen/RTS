using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
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
        if(unitStatus.currentState != UnitStates.attacking)
        {
            Vector2Int start = UF.CoordinatePosition(transform.position);
            Vector2Int goal = UF.CoordinatePosition(target.transform.position);
            List<Vector2Int> path = Map.ins.AStarPathFinding(start, goal);
            Task attackingTask = new Task(GM.tasks[TaskTypes.attack], new List<GameObject> { target }, 1.5f);
            unitMov.Move(path, attackingTask);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(StartAttacking());
    }
}
