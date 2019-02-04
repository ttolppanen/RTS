using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectResourceTask : MonoBehaviour
{
    GameObject resourceObject;
    ResourceNode resourceScript;
    ResourceTypes resourceType;
    int resourceCollectAmount;
    UnitResources resources;
    UnitMovement movScript;
    Animator anim;
    AnimatorController animControl;

    private void Start()
    {
        resources = GetComponent<UnitResources>();
        movScript = GetComponent<UnitMovement>();
        anim = GetComponent<Animator>();
        animControl = GetComponent<AnimatorController>();
        resourceObject = movScript.currentTask.objectives[0];
        if (resourceObject == null)//Jos on käynyt tosi huono tuuri ja juuri viimeframen aikana puu on tuhoutunut? Vaikka liikkumisen aikana tai jtn emt.
        {
            Reset();
            return;
        }
        resourceScript = resourceObject.GetComponent<ResourceNode>();
        resourceType = resourceScript.type;
        resourceCollectAmount = 30;
    }

    private void Update()
    {
        if (resourceObject == null || resourceScript.isDead)
        {
            Reset();
        }
        else
        {
            if (animControl.allowedToDo)
            {
                switch (resourceType)
                {
                    case ResourceTypes.wood:
                        anim.SetBool("CuttingWood", true);
                        break;
                    case ResourceTypes.stone:
                        print("Kivelle ei ole vielä animaatiota, muista tehdä tänne se animaatio ja silleen");
                        break;
                }
            }
        }
        if (animControl.action)
        {
            animControl.action = false;
            CollectResource();
        }
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
            movScript.Stop();
            return;
        }
        Vector2Int newNodeLocation = UF.CoordinatePosition(newNode.transform.position);
        Vector2Int nodeSize = newNode.GetComponent<BuildingStatus>().size;
        Task task = new Task(GM.tasks[TaskTypes.collectResource], new List<GameObject> { newNode });
        movScript.GoDoATask(newNodeLocation, nodeSize, task);
    }
    
    void BringBackResource()
    {
        if (Resources.ins.storagePoints[resourceType].Count != 0)
        {
            GameObject closestResourcePile = Resources.ins.ClosestStoragePoint(transform.position, resourceType);
            Vector2Int buildingSize = closestResourcePile.GetComponent<BuildingStatus>().size;
            Vector2Int buildingPoint = UF.CoordinatePosition(closestResourcePile.transform.position);
            List<GameObject> objectives = new List<GameObject> { closestResourcePile, resourceObject };
            Task task = new Task(GM.tasks[TaskTypes.bringBackResource], objectives);
            movScript.GoDoATask(buildingPoint, buildingSize, task);
        }
        else
        {
            movScript.Stop();
        }
    }
}
