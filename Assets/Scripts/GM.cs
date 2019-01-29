using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enum juttuja löytyy lopusta! Esim. TaskTypes ja LandTypes.

public class GM : MonoBehaviour
{
    public static IDictionary<TaskTypes, string> tasks = new Dictionary<TaskTypes, string>() //String on sen taskin scriptin nimi! Eli task on scripti mikä annetaan ukolle kun se on liikkunut maaliin.
    {
        { TaskTypes.idle, "idle"},
        { TaskTypes.collectResource, "CollectResourceTask" },
        { TaskTypes.bringBackResource, "ReturnResourceTask" }
    };
    public static List<LandTypes> allowedLand = new List<LandTypes> { LandTypes.grass, LandTypes.sand};
}

public enum LandTypes { sea, grass, tree, sand, building, lastNumber };
public enum TaskTypes { idle, collectResource, bringBackResource};
public enum ResourceTypes { wood, stone};