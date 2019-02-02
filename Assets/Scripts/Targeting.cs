using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    public static Targeting ins;
    private List<UnitTypes> targetTypes;
    public Task task;


    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
        else
        {
            Destroy(this);
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
            else
            {
                doAbilityNoTarget();
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            MouseControl.ins.mouseState = MouseStates.idle;
            Destroy(this);
        }
    }

    public void receiveTargetTypes(List<UnitTypes> yeet)
    {
        targetTypes = yeet;
        MouseControl.ins.mouseState = MouseStates.targeting;

    }

    private void doAbility(GameObject target)
    {
        Task k = new Task(GM.tasks[TaskTypes.resurrect], new List<GameObject> { target }, 50);
    }

    private void doAbilityNoTarget()
    {

    }
}
