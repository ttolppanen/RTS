using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnWoodTask : MonoBehaviour
{
    GameObject woodPile;
    WoodPile wpScript;
    UnitResources resources;
    UnitMovement movScript;

    private void Start()
    {
        movScript = GetComponent<UnitMovement>();
        resources = GetComponent<UnitResources>();
        woodPile = movScript.currentTask.objective;
        wpScript = woodPile.GetComponent<WoodPile>();
        if (resources.carryingType == ResourceTypes.wood)
        {
            wpScript.StorageResources(resources.GiveResources());
        }
        movScript.Stop(); // Tähän että hakee seuraavan puun?
    }
}
