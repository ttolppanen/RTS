using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStatus : MonoBehaviour
{
    public Vector2Int size;
    public int woodCost;
    public int stoneCost;
    public int soulCost;

    public Resource[] FetchResourceCosts()
    {
        Resource[] resourceCosts = new Resource[]
        {
            new Resource(ResourceTypes.wood, woodCost),
            new Resource(ResourceTypes.stone, stoneCost),
            new Resource(ResourceTypes.soul, soulCost)
        };
        return resourceCosts;
    }
}
