using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChooser : MonoBehaviour
{
    public GameObject[] UIs;
    public static UIChooser ins;
    public List<Transform> freeSlots;

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


    public void activateUI(UITypes type, List<GameObject> freeButtons)
    {
        deActivateAll();
        switch(type)
        {
            case UITypes.Worker:
                UIs[0].SetActive(true);
            break;

            case UITypes.Building:
                UIs[1].SetActive(true);
            break;

            case UITypes.Attacker:
                UIs[2].SetActive(true);
            break;
        }
    }

    public void deActivateAll()
    {
        foreach (GameObject UI in UIs)
        {
            UI.SetActive(false);
        }

        foreach (Transform slot in freeSlots)
        {
            if(slot.childCount>0)
            {
                Destroy(slot.GetChild(0));
            }
            else
            {
                break;
            }
        }
    }

    public void stopUnits()
    {
        List<GameObject> units = MouseControl.ins.giveChosenUnits();
        print("Click");

        foreach(GameObject unit in units)
        {
            unit.GetComponent<UnitMovement>().Stop();
        }

    }
}
