using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateUnitButton : MonoBehaviour
{
    public GameObject unit;

    void SpawnUnit()
    {
        GameObject building = UIChooser.ins.whoseUI;
        Resource[] resourceCosts = unit.GetComponent<UnitStatus>().resourceCosts;
        if (Resources.ins.IsEnoughResources(resourceCosts))
        {
            Resources.ins.RemoveResources(resourceCosts);
            List<Vector2Int> possibleSpawningPoints = Map.ins.PointsAroundBuilding(UF.CoordinatePosition(building.transform.position), building.GetComponent<BuildingStatus>().size);
            Instantiate(unit, possibleSpawningPoints[0] + new Vector2(0.5f, 0.5f), unit.transform.rotation);
        }
    }
}
