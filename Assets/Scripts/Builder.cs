using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public enum BuildingTypes{WoodsPile};
    public GameObject preBuilder;
    public List<GameObject> buildings;
    public void makeBuilding(BuildingTypes building)
    {
        switch(building)
        {
            case BuildingTypes.WoodsPile:
                GameObject o = Instantiate(preBuilder);
                o.GetComponent<PreBuilder>().buildingToSpawn = buildings[0];
            break;
        }

    }

    public void buildWoodPile()
    {
        makeBuilding(BuildingTypes.WoodsPile);
    }
}
