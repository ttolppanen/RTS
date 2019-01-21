using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsefullFunctions : MonoBehaviour
{
    public static Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
