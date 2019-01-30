using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public int resourceAmount;
    public ResourceTypes type;
    public bool isDead = false;

    public int CollectResource(int amount)
    {
        resourceAmount -= amount;
        if (resourceAmount <= 0)
        {
            Destroy(gameObject);
            isDead = true;
            Vector2Int resourcePos = UF.CoordinatePosition(transform.position);
            Map.ins.mapData[resourcePos.x, resourcePos.y] = (int)LandTypes.grass;
            return resourceAmount + amount;
        }
        return amount;
    }
}