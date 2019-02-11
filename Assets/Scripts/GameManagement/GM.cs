using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enum juttuja löytyy lopusta! Esim. TaskTypes ja LandTypes.

public class GM : MonoBehaviour
{
    public static IDictionary<TaskTypes, string> tasks = new Dictionary<TaskTypes, string>() //String on sen taskin scriptin nimi! Eli task on scripti mikä annetaan ukolle kun se on liikkunut maaliin.
    {
        { TaskTypes.idle, "idle"},
        { TaskTypes.attack, "AttackTask"},
        { TaskTypes.bringBackResource, "ReturnResourceTask" },
        { TaskTypes.collectResource, "CollectResourceTask" },
        { TaskTypes.resurrect, "Resurrect" },
        { TaskTypes.stealSoul, "StealSoul" }
    };
    public static List<LandTypes> allowedLand = new List<LandTypes> { LandTypes.grass, LandTypes.sand};

    public static float fixedAcceleration = 30;
    public static float unitRepulsionStrenght = 50;
}

public enum LandTypes { sea, grass, tree, sand, building, stone, lastNumber };
public enum ResourceTypes { wood, stone, soul };
public enum TaskTypes { idle, attack, bringBackResource, collectResource, resurrect, stealSoul};
public enum UnitStates { idle, casting, moving, attacking, building};
public enum UnitTypes { wizard, worker, enemy, body};