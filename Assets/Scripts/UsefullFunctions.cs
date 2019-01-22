using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsefullFunctions
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

    public static Vector2Int CoordinatePosition(Vector3 pos)
    {
        return new Vector2Int((int)pos.x, (int)pos.y);
    }
}