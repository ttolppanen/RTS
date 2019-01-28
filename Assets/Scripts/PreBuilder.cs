using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreBuilder : MonoBehaviour
{
    public GameObject buildingToSpawn;
    Vector2Int size;
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
        size = buildingToSpawn.GetComponent<BuildingStatus>().size;
    }

    private void Update()
    {
        Vector2Int point = UF.GetMousePosCoordinated();
        transform.position = (Vector2)point;
        if (Map.ins.CanBeBuilt(point, size))
        {
            bwMaterial.SetColor("_Color", canBuildColor);
            if (Input.GetMouseButtonDown(0))
            {
                Map.ins.AddBuildingToMap(point, size, buildingToSpawn);
                MouseControl.ins.mouseState = MouseStates.idle;
                Destroy(gameObject); //Sitten tuhotaan tämä koko hommaaa.
            }
        }
        else
        {
            bwMaterial.SetColor("_Color", cannotBuildColor);
        }
    }
}
