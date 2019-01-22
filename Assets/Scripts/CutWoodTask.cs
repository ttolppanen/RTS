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

    private void Start()
    {
        status = GetComponent<UnitStatus>();
        tree = GetComponent<UnitMovement>().currentTask.objective;
        treeScript = tree.GetComponent<BasicTree>();
        StartCoroutine(CuttingWood());
        cutTime = 1f;
        treeCollectAmount = 15;
    }

    IEnumerator CuttingWood()
    {
        yield return new WaitForSeconds(cutTime);
        int amount = Mathf.Min(treeCollectAmount, status.maxResourceAmount - status.resources[ResourceTypes.wood]);
        amount = treeScript.CutWood(amount);
        status.StorageWood(amount, ResourceTypes.wood);
        StartCoroutine(CuttingWood());
    }
}
