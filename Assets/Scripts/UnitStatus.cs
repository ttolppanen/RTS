using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    public UnitTypes unitType;
    public UnitStates currentState;
    public float maxHealt;
    public float currentHealt;
    public bool isAlive = true;

    private void Start()
    {
        currentHealt = maxHealt;
    }

    public void TakeDamage(float damage)
    {
        currentHealt -= damage;
        if (currentHealt <= 0)
        {
            isAlive = false;
        }
    }

    public void Heal(float amount)
    {
        currentHealt += amount;
        if (currentHealt > maxHealt)
        {
            currentHealt = maxHealt;
        }
    }
}
