﻿using System.Collections;
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

    public static LandTypes ResourceTypeToLandType(ResourceTypes type)
    {
        if (type == ResourceTypes.stone)
        {
            return LandTypes.stone;
        }
        else
        {
            return LandTypes.tree;
        }
    }

    public static Quaternion TurnUnit(Vector2 lookingDirection, float phaseShift)
    {
        float angle = Mathf.Atan2(lookingDirection.y, lookingDirection.x);
        return Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg + phaseShift);
    }

    public static float DistanceBetween2Units(Vector3 posA, Vector3 posB)
    {
        return ((Vector2)(posA) - (Vector2)(posB)).magnitude;
    }
}