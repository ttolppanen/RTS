using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTree : MonoBehaviour
{
    public int wood;

    public int CutWood(int amount)
    {
        wood -= amount;
        if (wood <= 0)
        {
            Destroy(gameObject, 0.1f);
            return wood + amount;
        }
        return amount;
    }
}