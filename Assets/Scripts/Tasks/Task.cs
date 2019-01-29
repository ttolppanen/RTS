using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public string taskName;
    public List<GameObject> objectives; //Peli objeckti mille taski tehdään, esim. puu?
    public MonoBehaviour taskScriptInstance; //Sitten kun taski on luotu niin pietetään tässä tallessa se scripti.

    public Task(string taskName, List<GameObject> objectives)
    {
        this.taskName = taskName;
        this.objectives = objectives;
        taskScriptInstance = null;
    }
}