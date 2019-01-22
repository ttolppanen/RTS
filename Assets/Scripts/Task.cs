using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public string taskName;
    public GameObject objective; //Peli objeckti mille taski tehdään, esim. puu?
    public MonoBehaviour taskScriptInstance; //Sitten kun taski on luotu niin pietetään tässä tallessa se scripti.

    public Task(string taskName, GameObject objective, MonoBehaviour taskScriptInstance)
    {
        this.taskName = taskName;
        this.objective = objective;
        this.taskScriptInstance = taskScriptInstance;
    }
}
