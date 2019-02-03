using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    public static Targeting ins;
    private List<UnitTypes> targetTypes;
    private TaskTypes taskType;
    public Task task;
    private int taskRange;

    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            RaycastHit2D hit = UF.MouseCast();

            if(hit.collider != null & targetTypes.Count != 0)
            {
                if(hit.transform.tag == "Body")
                {
                    doAbility(hit.transform.gameObject);
                }
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            MouseControl.ins.mouseState = MouseStates.idle;
            Destroy(gameObject);
        }
    }

    public void receiveTargetTypes(List<UnitTypes> yeet, TaskTypes type, int range)
    {
        taskType = type;
        targetTypes = yeet;
        taskRange = range;
        MouseControl.ins.mouseState = MouseStates.targeting;

    }

    private void doAbility(GameObject target)
    {
        Task k = new Task(GM.tasks[taskType], new List<GameObject> { target }, taskRange);
        Vector2Int mousePos = UF.GetMousePosCoordinated();
        Vector2Int unitPos = UF.CoordinatePosition(UIChooser.ins.whoseUI.transform.position);
        List<Vector2Int> path = Map.ins.AStarPathFinding(unitPos, mousePos);
        UIChooser.ins.whoseUI.GetComponent<UnitMovement>().Move(path, k);
        MouseControl.ins.mouseState = MouseStates.idle;
        Destroy(gameObject);
    }

}
