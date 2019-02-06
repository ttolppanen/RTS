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

    IEnumerator StartAttacking()//Ei tarvi ihan joka frame tarkistaa onko vihuja lähellä... varmankaan
    {
        if (target == null)
        {
            Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, unitStatus.seeingDistance, LayerMask.GetMask("Unit"));
            if (colls.Length != 0)
            {
                target = FindClosestEnemy(colls);
            }
        }
        else if (UF.DistanceBetween2Units(transform.position, target.transform.position) >= unitStatus.seeingDistance)
        {
            target = null;
            unitMov.Stop();
        }
        if(unitStatus.currentState != UnitStates.attacking && target != null)
        {
            Task attackingTask = new Task(GM.tasks[TaskTypes.attack], new List<GameObject> { target }, unitStatus.attackingDistance);
            unitMov.GoDoATask(attackingTask);
        }
        yield return new WaitForSeconds(0.5f);
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
