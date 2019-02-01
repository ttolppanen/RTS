using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreBuilder : MonoBehaviour
{
    public GameObject buildingToSpawn;
    Vector2Int size;
    int[] resourceCost;
    SpriteRenderer sr;
    Material bwMaterial; //BlackAndWhite material
    Color canBuildColor = new Color(1, 1.5f, 1);
    Color cannotBuildColor = new Color(1.5f, 1, 1);

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bwMaterial = sr.material;
        Sprite buildingSprite = buildingToSpawn.GetComponent<SpriteRenderer>().sprite;
        sr.sprite = buildingSprite;
        BuildingStatus bs = buildingToSpawn.GetComponent<BuildingStatus>();
        size = bs.size;
        resourceCost = bs.resourceCosts;
    }

    private void Update()
    {
        if (UF.IsOnUI())
        {
            return;
        }
        Vector2Int point = UF.GetMousePosCoordinated();
        transform.position = (Vector2)point;
        if (Map.ins.CanBeBuilt(point, size) && Resources.ins.IsEnoughResources(resourceCost))
        {
            bwMaterial.SetColor("_Color", canBuildColor);
            if (Input.GetMouseButtonDown(0))
            {
                Map.ins.AddBuildingToMap(point, size, buildingToSpawn);
                MouseControl.ins.mouseState = MouseStates.idle;
                MouseControl.ins.chooseUI();
                Resources.ins.RemoveResources(resourceCost);
                Destroy(gameObject); //Sitten tuhotaan tämä koko hommaaa.
            }
        }
        else
        {
            bwMaterial.SetColor("_Color", cannotBuildColor);
        }
        if (Input.GetMouseButtonDown(1))
        {
            MouseControl.ins.mouseState = MouseStates.idle;
            Destroy(gameObject);
        }
    }
}
