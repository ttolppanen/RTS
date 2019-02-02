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

        target.GetComponent<UnitStatus>().resurrect();

        GetComponent<UnitMovement>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
