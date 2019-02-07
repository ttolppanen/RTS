using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTask : MonoBehaviour
{
    GameObject enemy;
    UnitMovement unitMov;
    UnitStatus unitStats;
    UnitStatus enemyStatus;
    AnimatorController animControl;
    Animator anim;
    Vector2 movingDirection;
    Rigidbody2D rb;

    private void Awake()
    {
        unitMov = GetComponent<UnitMovement>();
        enemy = unitMov.currentTask.objectives[0];
        enemyStatus = enemy.GetComponent<UnitStatus>();
        rb = GetComponent<Rigidbody2D>();
        unitStats = GetComponent<UnitStatus>();
        animControl = GetComponent<AnimatorController>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        GetComponent<UnitStatus>().currentState = UnitStates.attacking;
    }

    private void Update()
    {
        if (enemy == null || !enemyStatus.isAlive || UF.DistanceBetween2Units(transform.position, enemy.transform.position) > unitStats.attackingDistance + 0.5f)
        {
            unitMov.Stop();
        }

        if (animControl.allowedToDo)
        {
            if (UF.DistanceBetween2Units(transform.position, enemy.transform.position) <= unitStats.attackingDistance)
            {
                anim.SetBool("Attacking", true);
            }
            else
            {
                movingDirection = (Vector2)enemy.transform.position - (Vector2)transform.position;
                anim.SetBool("Attacking", false);
            }
        }
        else
        {
            movingDirection = Vector2.zero;
        }

        if (animControl.SpendAction())
        {
            enemyStatus.DealDamage(unitStats.damage);
        }
    }

    private void FixedUpdate()
    {
        if (movingDirection != Vector2.zero)
        {
            transform.rotation = UF.TurnUnit(movingDirection, -90f);
            rb.AddForce(rb.mass * movingDirection.normalized * GM.fixedAcceleration);
        }
        if (rb.velocity.magnitude > unitStats.maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * unitStats.maxSpeed;
        }
    }
}
