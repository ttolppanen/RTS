using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnWoodTask : MonoBehaviour
{
    GameObject woodPile;
    GameObject previousTree;
    WoodPile wpScript;
    UnitResources resources;
    UnitMovement movScript;

    private void Start()
    {
        movScript = GetComponent<UnitMovement>();
        resources = GetComponent<UnitResources>();
        woodPile = movScript.currentTask.objectives[0];
        previousTree = movScript.currentTask.objectives[1];
        wpScript = woodPile.GetComponent<WoodPile>();
        if (resources.carryingType == ResourceTypes.wood)
        {
            wpScript.StorageResources(resources.GiveResources());
        }
        if (previousTree == null)
        {
            GameObject newTree = CutWoodTask.FindANewTree(new List<Vector2Int>(), UF.CoordinatePosition(transform.position));
            if (newTree == null)
            {
                movScript.Stop();
                return;
            }
            else
            {
                previousTree = newTree;
            }
        }
        Task newTask = new ResourceCollectionTask(GM.tasks[TaskTypes.cutWood], new List<GameObject> { previousTree }, ResourceTypes.wood);
        Vector2Int treePoint = UF.CoordinatePosition(previousTree.transform.position);
        Vector2Int treeSize = previousTree.GetComponent<BuildingStatus>().size;
        List<Vector2Int> path = Map.ins.CorrectPathToBuilding(UF.CoordinatePosition(transform.position), treePoint, treePoint, treeSize);
        movScript.Move(path, newTask);
    }
}
