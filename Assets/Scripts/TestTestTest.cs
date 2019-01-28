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
            Vector2 pos = UsefullFunctions.GetMousePosCoordinated();
            GameObject ins = Instantiate(preBuilder, pos, Quaternion.identity);
            PreBuilder script = ins.GetComponent<PreBuilder>();
            script.buildingToSpawn = BuildingToInstantiate;
        }       
    }
}
