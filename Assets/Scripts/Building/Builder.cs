using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public enum BuildingTypes{WoodsPile,StonesPile};
    public GameObject preBuilder;
    public List<GameObject> buildings;
    public void makeBuilding(BuildingTypes building)
    {
        GameObject o = Instantiate(preBuilder);
        switch (building)
        {
            case BuildingTypes.WoodsPile:
                o.GetComponent<PreBuilder>().buildingToSpawn = buildings[0];
            break;

            case BuildingTypes.StonesPile:
                o.GetComponent<PreBuilder>().buildingToSpawn = buildings[1];
            break;
        }

    }

    public void buildWoodPile()
    {
        makeBuilding(BuildingTypes.WoodsPile);
    }

    public void buildStonePile()
    {
        makeBuilding(BuildingTypes.StonesPile);
    }

}
