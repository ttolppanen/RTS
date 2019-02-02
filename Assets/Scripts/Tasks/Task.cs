using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public string taskName;
    public List<GameObject> objectives; //Peli objeckti mille taski tehdään, esim. puu?
    public MonoBehaviour taskScriptInstance; //Sitten kun taski on luotu niin pietetään tässä tallessa se scripti.
    public float taskRange; //Jos taskia voi suorittaa kaukaa

    public Task(string taskName, List<GameObject> objectives)
    {
        this.taskName = taskName;
        this.objectives = objectives;
        taskScriptInstance = null;
        taskRange = 0;
    }

    public Task(string taskName, List<GameObject> objectives, float taskRange)
    {
        this.taskName = taskName;
        this.objectives = objectives;
        taskScriptInstance = null;
        this.taskRange = taskRange;
    }
}