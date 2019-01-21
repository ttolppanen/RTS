using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControll : MonoBehaviour
{
    public List<GameObject> chosenUnits = new List<GameObject>(); //Valitut ukot...

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = UsefullFunctions.GetMousePos();
            Collider2D unitCollider = Physics2D.OverlapCircle(mousePos, 0.1f);
            if (unitCollider == null || unitCollider.tag != "Unit")
            {
                chosenUnits.Clear();
            }
            else if (!chosenUnits.Contains(unitCollider.gameObject))
            {
                chosenUnits.Add(unitCollider.gameObject);
            }

            print(chosenUnits.Count);

        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (chosenUnits.Count != 0)
            {
                foreach (GameObject unit in chosenUnits)
                {
                    Vector2 mousePos = UsefullFunctions.GetMousePos();
                    unit.GetComponent<UnitMovement>().Move(new Vector2Int((int)mousePos.x, (int)mousePos.y));
                }
            }
        }
    }
}
