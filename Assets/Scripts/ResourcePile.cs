using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePile : MonoBehaviour
{

    public ResourceTypes type;

    void Start()
    {
        Resources.ins.storagePoints[type].Add(gameObject);       
    }

    public void StorageResources(int amount)
    {
        Resources.ins.AddResource(amount, type);
    }
}
