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

    private void Start()
    {
        print(currentHealth);
        if(currentHealth <= 0.0f)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            Color colour = new Color(0,0,0,1);
            sprite.color = colour;
            gameObject.tag = "Body";
            print("Hello");
            isAlive = false;
        }
    }

    public void DealDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            isAlive = false;
            gameObject.tag = "Body";
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

    public void resurrect()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Color colour = new Color(1, 1, 1, 1);
        sprite.color = colour;
        currentHealth = maxHealth;
        isAlive = true;
        gameObject.tag = "Unit";
    }
}
