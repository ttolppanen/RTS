using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodPile : MonoBehaviour
{
    void Start()
    {
        Resources.ins.storagePoints[ResourceTypes.wood].Add(gameObject);       
    }

    public void StorageResources(int amount)
    {
        Resources.ins.AddResource(amount, ResourceTypes.wood);
    }
}
