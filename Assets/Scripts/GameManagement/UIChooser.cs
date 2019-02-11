using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChooser : MonoBehaviour
{
    public GameObject[] UIs;
    public static UIChooser ins;
    public List<Transform> freeSlots;
    public GameObject whoseUI;


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


    public void activateUI(UITypes type, List<GameObject> freeButtons, GameObject UIWho)
    {
        deActivateAll();
        whoseUI = UIWho;
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

            case UITypes.Structure:
                print("Muistutus, että UIChooserissa structure ui tyyppi on auki.");
            break;
        }

        for(int i = 0; i<freeButtons.Count; i++)
        {
            GameObject button = Instantiate(freeButtons[i],freeSlots[i]);
            button.SetActive(true);
        }
    }

    public void deActivateAll()
    {
        whoseUI = null;
        foreach (GameObject UI in UIs)
        {
            UI.SetActive(false);
        }

        foreach (Transform slot in freeSlots)
        {
            if(slot.childCount > 0)
            {
                Destroy(slot.GetChild(0).gameObject);
            }
        }
    }

    public void stopUnits()
    {
        List<GameObject> units = MouseControl.ins.giveChosenUnits();
        foreach(GameObject unit in units)
        {
            unit.GetComponent<UnitMovement>().Stop();
        }
    }
}
