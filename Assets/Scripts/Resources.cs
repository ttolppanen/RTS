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
    };
    Text woodText;
    Text stoneText;


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
        UpdateTexts();
    }

    public void AddResource(int amount, ResourceTypes type)
    {
        resources[type] += amount;
        UpdateTexts();
    }

    public bool IsEnoughResources(int[] resourceCosts)
    {
        for (int i = 0; i < resourceCosts.Length; i++)
        {
            if (resourceCosts[i] > resources[(ResourceTypes)i])
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveResources(int[] resourceCosts)
    {
        for (int i = 0; i < resourceCosts.Length; i++)
        {
            resources[(ResourceTypes)i] -= resourceCosts[i];
        }
        UpdateTexts();
    }

    private void UpdateTexts()
    {
        woodText.text = resources[ResourceTypes.wood] + "";
        stoneText.text = resources[ResourceTypes.stone] + "";
    }
}
