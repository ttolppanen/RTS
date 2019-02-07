using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealSoul : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> targets = GetComponent<UnitMovement>().currentTask.objectives;

        GameObject target = targets[0];
        int souls = target.GetComponent<UnitStatus>().soulsToSteal();
        target.GetComponent<UnitStatus>().stealSouls();

        Resources.ins.AddResource(new Resource(ResourceTypes.soul, souls));

        GetComponent<UnitMovement>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
