using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutWoodTask : MonoBehaviour
{
    GameObject tree;
    BasicTree treeScript;
    float cutTime;
    int treeCollectAmount;
    UnitStatus status;
    UnitMovement movScript;
    float time;

    private void Start()
    {
        status = GetComponent<UnitStatus>();
        movScript = GetComponent<UnitMovement>();
        tree = movScript.currentTask.objective;
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
        int amount = Mathf.Min(treeCollectAmount, status.maxResourceAmount - status.resources[ResourceTypes.wood]);
        amount = treeScript.CutWood(amount);
        status.StorageWood(amount, ResourceTypes.wood);
        if (treeScript.isDead)
        {
            Reset();
        }
    }

    GameObject FindANewTree(List<Vector2Int> alreadyChecked)
    {
        Vector2Int newTreeLocation = Map.ins.FindClosestLand(UsefullFunctions.CoordinatePosition(transform.position), LandTypes.tree, 200, alreadyChecked);
        if (newTreeLocation == new Vector2Int(-999, -999))
        {
            return null;
        }
        tree = UsefullFunctions.FetchGameObject(newTreeLocation + new Vector2(0.5f, 0.5f)); //0.5 että menee keskelle ruutua/nodea.
        if (tree.GetComponent<BasicTree>().isDead)
        {
            alreadyChecked.Add(newTreeLocation);
            return FindANewTree(alreadyChecked);
        }
        else
        {
            return tree;
        }
    }

    void Reset()
    {
        GameObject tree = FindANewTree(new List<Vector2Int>());
        if (tree == null)
        {
            Destroy(this);
            return;
        }
        Vector2Int newTreeLocation = UsefullFunctions.CoordinatePosition(tree.transform.position);
        List<Vector2Int> path = Map.ins.CorrectPathToBuilding(UsefullFunctions.CoordinatePosition(transform.position), newTreeLocation, newTreeLocation, new Vector2Int(1, 1));
        Task task = new Task(GM.tasks[TaskTypes.cutWood], tree, null);
        if (!(path.Count != 0 && path[0] == new Vector2Int(-999, -999)))
        {
            movScript.Move(path, task);
        }
        Destroy(this);
    }
}
