using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public UITypes UI;
    public int hierachy;
    public List<GameObject> unitButtons;

    public List<GameObject> giveButtons()
    {
        return unitButtons;
    }

    public UITypes giveUI()
    {
        return UI;
    }

}
public enum UITypes { Worker, Building, Attacker, Structure };
