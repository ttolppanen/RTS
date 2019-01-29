using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UF
{
    public static Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public static Vector2Int GetMousePosCoordinated()
    {
        return CoordinatePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public static RaycastHit2D MouseCast()
    {
        Vector2 mousePos = GetMousePos();
        return Physics2D.Raycast(mousePos, Vector2.zero);
    }

    public static GameObject FetchGameObject(Vector2 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.collider == null)
        {
            return null;
        }
        else
        {
            return hit.collider.gameObject;
        }
    }

    public static Vector2Int CoordinatePosition(Vector3 pos)
    {
        return new Vector2Int((int)pos.x, (int)pos.y);
    }

    public static bool IsOnUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public static LandTypes ResourceTypeToLandType(ResourceTypes type)//PITÄÄ LAITTAA KIVI TÄNNE
    {
        if (type == ResourceTypes.stone)
        {
            return LandTypes.tree;
        }
        else
        {
            return LandTypes.tree;
        }
    }
}