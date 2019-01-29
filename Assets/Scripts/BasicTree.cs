using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTree : MonoBehaviour
{
    public int wood;
    public bool isDead = false;

    public int CutWood(int amount)
    {
        wood -= amount;
        if (wood <= 0)
        {
            Destroy(gameObject);
            isDead = true;
            Vector2Int treePos = UF.CoordinatePosition(transform.position);
            Map.ins.mapData[treePos.x, treePos.y] = (int)LandTypes.grass;
            return wood + amount;
        }
        return amount;
    }
}