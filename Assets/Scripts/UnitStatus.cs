using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    public IDictionary<ResourceTypes, int> resources = new Dictionary<ResourceTypes, int>()
    {
        { ResourceTypes.wood, 0},
        { ResourceTypes.stone, 0},
    };
    public int maxResourceAmount;

    public void StorageWood(int amount, ResourceTypes type) //Palautetaan puut mitkä ei mahdu kyytiin.
    {
        Resources.ins.AddResource(amount, type);
    }
}
