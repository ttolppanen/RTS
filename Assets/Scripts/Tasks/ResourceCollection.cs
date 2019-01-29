using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollectionTask : Task
{
    public ResourceTypes collectingType;

    public ResourceCollectionTask(string taskName, List<GameObject> objectives, ResourceTypes collectingType) : base(taskName, objectives)
    {
        base.taskName = taskName;
        base.objectives = objectives;
        this.collectingType = collectingType;
        taskScriptInstance = null;
    }
}