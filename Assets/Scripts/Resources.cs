using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour
{
    public static Resources ins;
    public Text woodText;
    
    public IDictionary<ResourceTypes, int> resources = new Dictionary<ResourceTypes, int>()
    {
        { ResourceTypes.wood, 0},
        { ResourceTypes.stone, 0},
    };

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
    }

    public void AddResource(int amount, ResourceTypes type)
    {
        resources[type] += amount;
        woodText.text = "Wood: " + resources[type];
    }
}
