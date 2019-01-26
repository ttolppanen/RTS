using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitResources : MonoBehaviour
{
    public int carryingAmount;
    public ResourceTypes carryingType;
    public int maxResourceAmount;

    UnitMovement unitMov;

    private void Start()
    {
        unitMov = GetComponent<UnitMovement>();
    }

    public bool Storage(int amount) //Palautetaan puut mitkä ei mahdu kyytiin. True jos taskut on täynnä...
    {
        carryingAmount += amount;
        if (carryingAmount == maxResourceAmount)
        {
            return true;
        }
        return false;
    }

    public void UpdateCarryingType(ResourceTypes type)
    {
        if (type != carryingType)
        {
            carryingType = type;
            carryingAmount = 0;
        }
    }
}
