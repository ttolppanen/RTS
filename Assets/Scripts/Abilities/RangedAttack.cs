using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public bool homing;
    public float damage;
    public float maxSpeed;

    Vector2 movingDirection;
    private Rigidbody2D rb;
    private Vector2 targetCoord;
    private GameObject target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void getTarget(GameObject enemy)
    {
        target = enemy;
        targetCoord = enemy.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (targetCoord != null)
        {
            if(homing)
            {
                targetCoord = target.transform.position;
                movingDirection = (targetCoord) - (Vector2)(transform.position);
                transform.rotation = UF.TurnUnit(movingDirection, -90f);
                rb.AddForce(rb.mass * movingDirection.normalized * GM.fixedAcceleration);
            }
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(homing)
        {
            if(col.gameObject == target)
            {
                col.gameObject.GetComponent<UnitStatus>().DealDamage(damage);
                Destroy(gameObject);
            }
        }
        else if(col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<UnitStatus>().DealDamage(damage);
            Destroy(gameObject);
        }
    }
}
