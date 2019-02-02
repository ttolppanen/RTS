using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RezButton : MonoBehaviour
{
    public List<UnitTypes> targetTypes;
    public GameObject targeting;

    public void startTargeting()
    {
        if(Targeting.ins != null)
        {
            Destroy(Targeting.ins);
        }
        GameObject target = Instantiate(targeting);
         
        target.GetComponent<Targeting>().receiveTargetTypes(targetTypes);
    }
}
