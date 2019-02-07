using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour
{
    public static Resources ins;
    public IDictionary<ResourceTypes, List<GameObject>> storagePoints = new Dictionary<ResourceTypes, List<GameObject>>()
    {
        { ResourceTypes.wood, new List<GameObject>() },
        { ResourceTypes.stone, new List<GameObject>() },
    };
    public IDictionary<ResourceTypes, int> resources = new Dictionary<ResourceTypes, int>()
    {
        { ResourceTypes.wood, 100},
        { ResourceTypes.stone, 0},
        { ResourceTypes.soul, 10},
    };
    Text woodText;
    Text stoneText;
    Text soulText;


    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
        else
        {
            Destroy(this);
        }
        woodText = GameObject.Find("WoodText").GetComponent<Text>();
        stoneText = GameObject.Find("StoneText").GetComponent<Text>();
        soulText = GameObject.Find("SoulText").GetComponent<Text>();
        UpdateTexts();
    }

    public void AddResource(Resource resource)
    {
        resources[resource.type] += resource.amount;
        UpdateTexts();
    }

    public bool IsEnoughResources(Resource[] resourceCosts)
    {
        foreach (Resource resource in resourceCosts)
        { 
            if (resource.amount > resources[resource.type])
            {
                return false;
            }
        }
        return true;
    }

    public bool IsEnoughResources(Resource resource)
    {
        if (resource.amount > resources[resource.type])
        {
            return false;
        }
        return true;
    }

    public void RemoveResources(Resource[] resourceCosts)
    {
        foreach (Resource resource in resourceCosts)
        { 
            resources[resource.type] -= resource.amount;
        }
        UpdateTexts();
    }

    public void RemoveResources(Resource resource)
    {
        resources[resource.type] -= resource.amount;
        UpdateTexts();
    }

    private void UpdateTexts()
    {
        woodText.text = resources[ResourceTypes.wood] + "";
        stoneText.text = resources[ResourceTypes.stone] + "";
        soulText.text = resources[ResourceTypes.soul] + "";
    }

    public GameObject ClosestStoragePoint(Vector2 point, ResourceTypes type)
    {
        if (storagePoints[type].Count == 0)
        {
            return null;
        }
        else
        {
            float distance = 999999;
            GameObject closestStoragePoint = storagePoints[type][0];
            foreach (GameObject storagePoint in storagePoints[type])
            {
                float currentDistance = ((Vector2)storagePoint.transform.position - point).magnitude;
                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    closestStoragePoint = storagePoint;
                }
            }
            return closestStoragePoint;
        }
    }
}
public struct Resource
{
    public int amount;
    public ResourceTypes type;

    public Resource(ResourceTypes type, int amount)
    {
        this.amount = amount;
        this.type = type;
    }
}