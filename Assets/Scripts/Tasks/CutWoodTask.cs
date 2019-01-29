using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutWoodTask : MonoBehaviour
{
    GameObject tree;
    BasicTree treeScript;
    float cutTime;
    int treeCollectAmount;
    UnitResources resources;
    UnitMovement movScript;
    float time;

    private void Start()
    {
        resources = GetComponent<UnitResources>();
        movScript = GetComponent<UnitMovement>();
        tree = movScript.currentTask.objectives[0];
        if (tree == null)//Jos on käynyt tosi huono tuuri ja juuri viimeframen aikana puu on tuhoutunut? Vaikka liikkumisen aikana tai jtn emt.
        {
            Reset();
            return;
        }
        treeScript = tree.GetComponent<BasicTree>();
        cutTime = 0.3f;
        treeCollectAmount = 30;
    }

    private void Update()
    {
        if (time >= cutTime)
        {
            if (tree == null || treeScript.isDead)
            {
                Reset();
            }
            else
            {
                CutWood();
            }
            time = 0;
        }
        time += Time.deltaTime;
    }

    void CutWood()
    {
        resources.UpdateCarryingType(ResourceTypes.wood);
        int amount = Mathf.Min(treeCollectAmount, resources.maxResourceAmount - resources.carryingAmount); //Hakataanko koko määrä vai vaan niin paljon kuin mahtuu taskuun... 
        amount = treeScript.CutWood(amount);
        bool isCarryingMax = resources.Storage(amount);
        if (isCarryingMax)
        {
            BringBackWood();
        }
        else if (treeScript.isDead)
        {
            Reset();
        }
    }

    public static GameObject FindANewTree(List<Vector2Int> alreadyChecked, Vector2Int position)
    {
        Vector2Int newTreeLocation = Map.ins.FindClosestLand(position, LandTypes.tree, 200, alreadyChecked);
        if (newTreeLocation == new Vector2Int(-999, -999))
        {
            return null;
        }
        GameObject newTree = UF.FetchGameObject(newTreeLocation + new Vector2(0.5f, 0.5f)); //0.5 että menee keskelle ruutua/nodea.
        if (newTree.GetComponent<BasicTree>().isDead)
        {
            alreadyChecked.Add(newTreeLocation);
            return FindANewTree(alreadyChecked, position);
        }
        else
        {
            return newTree;
        }
    }

    void Reset()
    {
        GameObject tree = FindANewTree(new List<Vector2Int>(), UF.CoordinatePosition(transform.position));
        if (tree == null)
        {
            Destroy(this);
            return;
        }
        Vector2Int newTreeLocation = UF.CoordinatePosition(tree.transform.position);
        List<Vector2Int> path = Map.ins.CorrectPathToBuilding(UF.CoordinatePosition(transform.position), newTreeLocation, newTreeLocation, new Vector2Int(1, 1));
        Task task = new ResourceCollectionTask(GM.tasks[TaskTypes.cutWood], new List<GameObject> { tree }, ResourceTypes.wood);
        if (!(path.Count != 0 && path[0] == new Vector2Int(-999, -999)))
        {
            movScript.Move(path, task);
        }
        Destroy(this);
    }
    
    void BringBackWood()
    {
        if (Resources.ins.storagePoints[ResourceTypes.wood].Count != 0)
        {
            GameObject closestWoodPile = Resources.ins.ClosestStoragePoint(transform.position, ResourceTypes.wood);
            Vector2Int buildingSize = closestWoodPile.GetComponent<BuildingStatus>().size;
            Vector2Int buildingPoint = UF.CoordinatePosition(closestWoodPile.transform.position);
            List<Vector2Int> path = Map.ins.CorrectPathToBuilding(UF.CoordinatePosition(transform.position), buildingPoint, buildingPoint, buildingSize);
            List<GameObject> objectives = new List<GameObject> { closestWoodPile };
            objectives.Add(tree);
            Task task = new Task(GM.tasks[TaskTypes.bringBackWood], objectives);
            movScript.Move(path, task);
        }
        else
        {
            movScript.Stop();
        }
    }
}
