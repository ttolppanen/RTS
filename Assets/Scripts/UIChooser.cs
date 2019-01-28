using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChooser : MonoBehaviour
{
    public GameObject[] UIs;
    public static UIChooser ins;
    

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


    public void activateUI(UITypes type)
    {
        switch(type)
        {
            case UITypes.Worker:
                UIs[0].SetActive(true);
            break;
        }
    }

    public void deActivateAll()
    {
        foreach (GameObject UI in UIs)
        {
            UI.SetActive(false);
        }
    }

    public void stopUnits()
    {
        List<GameObject> units = UnitControll.ins.giveChosenUnits();

        foreach(GameObject unit in units)
        {
            unit.GetComponent<UnitMovement>().Stop();
        }

    }
}
