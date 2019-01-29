using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTestTest : MonoBehaviour
{

    public GameObject BuildingToInstantiate;
    public GameObject preBuilder;

    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            MouseControl.ins.mouseState = MouseStates.building;
            Vector2 pos = UF.GetMousePosCoordinated();
            GameObject ins = Instantiate(preBuilder, pos, Quaternion.identity);
            PreBuilder script = ins.GetComponent<PreBuilder>();
            script.buildingToSpawn = BuildingToInstantiate;
        }       
    }
}
