using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    public UnitTypes unitType;
    public UnitStates currentState;
    public float maxHealth;
    public float currentHealth;
    public bool isAlive = true;
    public int soulsToLive;
    public int soulsNow;
    public float maxSpeed;
    public float attackingDistance; //Kuinka läheltä yltää lyömään
    public float seeingDistance; //Kuinka kaukaa agrotaan
    public float damage;
    public bool ranged;
    public GameObject rangedAttack;

    Animator anim;
    UnitMovement unitMov;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        unitMov = GetComponent<UnitMovement>();
    }

    private void Start()
    {
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public int howManySouls()
    {
        return soulsToLive - soulsNow;
    }

    public int soulsToSteal()
    {
        return soulsNow;
    }

    public void stealSouls()
    {
        Destroy(gameObject);
    }


    public void DealDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    void Die()
    {
        unitMov.Stop();
        MouseControl.ins.RemoveFromChosenUnits(gameObject);
        isAlive = false;
        anim.SetTrigger("Dead");
        gameObject.tag = "Body";
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void resurrect()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Color colour = new Color(1, 1, 1, 1);
        sprite.color = colour;
        currentHealth = maxHealth;
        isAlive = true;
        anim.SetTrigger("Resurrect");
        gameObject.tag = "Unit";
        gameObject.layer = LayerMask.NameToLayer("Unit");
    }
}
