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

    private void Start()
    {
        status = GetComponent<UnitStatus>();
        movScript = GetComponent<UnitMovement>();
        tree = movScript.currentTask.objective;
        treeScript = tree.GetComponent<BasicTree>();
        StartCoroutine(CuttingWood());
        cutTime = 0.3f;
        treeCollectAmount = 30;
    }

    IEnumerator CuttingWood()
    {
        if (tree == null || treeScript.isDead)
        {
            FindNewTreeAndReset();
            Destroy(this);
        }
        yield return new WaitForSeconds(cutTime);
        int amount = Mathf.Min(treeCollectAmount, status.maxResourceAmount - status.resources[ResourceTypes.wood]);
        amount = treeScript.CutWood(amount);
        status.StorageWood(amount, ResourceTypes.wood);
        if (treeScript.isDead)
        {
            FindNewTreeAndReset();
        }
        StartCoroutine(CuttingWood());
    }

    void FindNewTreeAndReset()
    {
        Vector2Int newTreeLocation = Map.ins.FindClosestLand(UsefullFunctions.CoordinatePosition(transform.position), LandTypes.tree, 100);
        tree = UsefullFunctions.FetchGameObject(newTreeLocation + new Vector2(0.5f, 0.5f)); //0.5 että menee keskelle ruutua/nodea.

        List<Vector2Int> path = Map.ins.CorrectPathToBuilding(UsefullFunctions.CoordinatePosition(transform.position), newTreeLocation, newTreeLocation, new Vector2Int(1, 1));
        Task task = new Task(GM.tasks[TaskTypes.cutWood], tree, null);
        movScript.Move(path, task);
        Destroy(this);
    }
}
