using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnResourceTask : MonoBehaviour
{
    GameObject resourcePile;
    GameObject previousNode;
    ResourcePile rpScript;
    UnitResources resources;
    UnitMovement movScript;
    ResourceTypes returningType;

    private void Start()
    {
        movScript = GetComponent<UnitMovement>();
        resources = GetComponent<UnitResources>();
        resourcePile = movScript.currentTask.objectives[0];
        previousNode = movScript.currentTask.objectives[1];
        returningType = ((ResourceCollection)movScript.currentTask).collectingType;
        rpScript = resourcePile.GetComponent<ResourcePile>();
        if (resources.carryingType == returningType)
        {
            rpScript.StorageResources(resources.GiveResources());
        }
        if (previousNode == null)
        {
            LandTypes landType = UF.ResourceTypeToLandType(returningType);
            GameObject newNode = CollectResourceTask.FindANewNode(new List<Vector2Int>(), UF.CoordinatePosition(transform.position), landType);
            if (newNode == null)
            {
                movScript.Stop();
                return;
            }
            else
            {
                previousNode = newNode;
            }
        }
        Task newTask = new ResourceCollection(GM.tasks[TaskTypes.collectResource], new List<GameObject> { previousNode }, returningType);
        Vector2Int nodePoint = UF.CoordinatePosition(previousNode.transform.position);
        Vector2Int nodeSize = previousNode.GetComponent<BuildingStatus>().size;
        List<Vector2Int> path = Map.ins.CorrectPathToBuilding(UF.CoordinatePosition(transform.position), nodePoint, nodePoint, nodeSize);
        movScript.Move(path, newTask);
    }
}
