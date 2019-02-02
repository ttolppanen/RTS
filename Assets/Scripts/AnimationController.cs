using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public bool action; //Kun tehdään animaatiossa jotain juttua, niin tämä tulee true kun pitäisi tehä se myös koodissa, siis: Hakataan puuta -> Tehdään puun lyönti animaatiota, jossain vaiheessa animaatiota action = true ja sitten puun hakkuu scripti lukee sen ja hakkaa puun
    UnitStatus unitStatus;
    Animator anim;
    string currentAnimName = "";
    string lastAnimName = "";

    void Start()
    {
        anim = GetComponent<Animator>();
        unitStatus = GetComponent<UnitStatus>();
    }

    void Update()
    {
        currentAnimName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (currentAnimName != lastAnimName) //Jos animaatio on vaihtunut niin nollataan action...
        {
            action = false;
        }
        lastAnimName = currentAnimName;
    }

    public void SetActionTrue()
    {
        action = true;
    }
}
