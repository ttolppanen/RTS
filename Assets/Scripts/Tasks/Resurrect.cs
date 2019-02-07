using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resurrect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> targets =  GetComponent<UnitMovement>().currentTask.objectives;

        GameObject target = targets[0];
        int souls = target.GetComponent<UnitStatus>().howManySouls();
        Resource soulResource = new Resource(ResourceTypes.soul, souls);
        if(Resources.ins.IsEnoughResources(soulResource))
        {
            Resources.ins.RemoveResources(soulResource);
            target.GetComponent<UnitStatus>().resurrect();
        }
        else
        {

        }
        GetComponent<UnitMovement>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
