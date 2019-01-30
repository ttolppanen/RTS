using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectResourceTask : MonoBehaviour
{
    GameObject resourceObject;
    ResourceNode resourceScript;
    ResourceTypes resourceType;
    float collectTime;
    int resourceCollectAmount;
    UnitResources resources;
    UnitMovement movScript;
    float time;

    private void Start()
    {
        resources = GetComponent<UnitResources>();
        movScript = GetComponent<UnitMovement>();
        resourceObject = movScript.currentTask.objectives[0];
        if (resourceObject == null)//Jos on käynyt tosi huono tuuri ja juuri viimeframen aikana puu on tuhoutunut? Vaikka liikkumisen aikana tai jtn emt.
        {
            Reset();
            return;
        }
        resourceScript = resourceObject.GetComponent<ResourceNode>();
        resourceType = resourceScript.type;
        collectTime = 0.3f;
        resourceCollectAmount = 30;
    }

    private void Update()
    {
        if (time >= collectTime)
        {
            if (resourceObject == null || resourceScript.isDead)
            {
                Reset();
            }
            else
            {
                CollectResource();
            }
            time = 0;
        }
        time += Time.deltaTime;
    }

    void CollectResource()
    {
        resources.UpdateCarryingType(resourceType);
        int amount = Mathf.Min(resourceCollectAmount, resources.maxResourceAmount - resources.carryingAmount); //Hakataanko koko määrä vai vaan niin paljon kuin mahtuu taskuun... 
        amount = resourceScript.CollectResource(amount);
        bool isCarryingMax = resources.Storage(amount);
        if (isCarryingMax)
        {
            BringBackResource();
        }
        else if (resourceScript.isDead)
        {
            Reset();
        }
    }

    public static GameObject FindANewNode(List<Vector2Int> alreadyChecked, Vector2Int position, LandTypes type)
    {
        Vector2Int newNodeLocation = Map.ins.FindClosestLand(position, type, 200, alreadyChecked);
        if (newNodeLocation == new Vector2Int(-999, -999))
        {
            return null;
        }
        GameObject newNode = UF.FetchGameObject(newNodeLocation + new Vector2(0.5f, 0.5f)); //0.5 että menee keskelle ruutua/nodea.
        if (newNode.GetComponent<ResourceNode>().isDead)
        {
            alreadyChecked.Add(newNodeLocation);
            return FindANewNode(alreadyChecked, position, type);
        }
        else
        {
            return newNode;
        }
    }

    void Reset()
    {
        LandTypes landType = UF.ResourceTypeToLandType(resourceType);
        GameObject newNode = FindANewNode(new List<Vector2Int>(), UF.CoordinatePosition(transform.position), landType);
        if (newNode == null)
        {
            Destroy(this);
            return;
        }
        Vector2Int newNodeLocation = UF.CoordinatePosition(newNode.transform.position);
        Vector2Int nodeSize = newNode.GetComponent<BuildingStatus>().size;
        List<Vector2Int> path = Map.ins.CorrectPathToBuilding(UF.CoordinatePosition(transform.position), newNodeLocation, newNodeLocation, nodeSize);
        Task task = new Task(GM.tasks[TaskTypes.collectResource], new List<GameObject> { newNode });
        if (!(path.Count != 0 && path[0] == new Vector2Int(-999, -999)))
        {
            movScript.Move(path, task);
        }
        Destroy(this);
    }
    
    void BringBackResource()
    {
        if (Resources.ins.storagePoints[resourceType].Count != 0)
        {
            GameObject closestResourcePile = Resources.ins.ClosestStoragePoint(transform.position, resourceType);
            Vector2Int buildingSize = closestResourcePile.GetComponent<BuildingStatus>().size;
            Vector2Int buildingPoint = UF.CoordinatePosition(closestResourcePile.transform.position);
            List<Vector2Int> path = Map.ins.CorrectPathToBuilding(UF.CoordinatePosition(transform.position), buildingPoint, buildingPoint, buildingSize);
            List<GameObject> objectives = new List<GameObject> { closestResourcePile };
            objectives.Add(resourceObject);
            Task task = new Task(GM.tasks[TaskTypes.bringBackResource], objectives);
            movScript.Move(path, task);
        }
        else
        {
            movScript.Stop();
        }
    }
}
