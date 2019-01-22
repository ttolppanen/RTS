using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enum juttuja löytyy lopusta! Esim. TaskTypes ja LandTypes.

public class GM : MonoBehaviour
{
    public static IDictionary<int, string> tasks = new Dictionary<int, string>() //String on sen taskin scriptin nimi! Eli task on scripti mikä annetaan ukolle kun se on liikkunut maaliin.
    {
        { (int)TaskTypes.idle, "idle"},
        { (int)TaskTypes.cutWood, "idle" },
        { (int)TaskTypes.bringBackWood, "idle" }
    };
}

public enum LandTypes { sea, grass, tree, sand, building, lastNumber };
public enum TaskTypes { idle, cutWood, bringBackWood};