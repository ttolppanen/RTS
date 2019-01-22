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
            Collider2D unitCollider = UsefullFunctions.MouseCast().collider;
            if (unitCollider == null || unitCollider.tag != "Unit")
            {
                chosenUnits.Clear();
            }
            else if (!chosenUnits.Contains(unitCollider.gameObject))
            {
                chosenUnits.Add(unitCollider.gameObject);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (chosenUnits.Count != 0)
            {
                foreach (GameObject unit in chosenUnits)
                {
                    Task task = new Task(GM.tasks[TaskTypes.idle], null, null);
                    List<Vector2Int> path = new List<Vector2Int>();

                    Vector2Int mousePos = UsefullFunctions.GetMousePosCoordinated();
                    RaycastHit2D objectUnderMouse = UsefullFunctions.MouseCast();
                    Vector2Int unitPos = UsefullFunctions.CoordinatePosition(unit.transform.position);

                    if (objectUnderMouse.collider != null && objectUnderMouse.collider.tag == "Tree")
                    {
                        GameObject tree = objectUnderMouse.collider.gameObject;
                        path = Map.ins.CorrectPathToBuilding(unitPos, mousePos, UsefullFunctions.CoordinatePosition(tree.transform.position), new Vector2Int(1, 1));
                        task = new Task(GM.tasks[TaskTypes.cutWood], tree, null);
                    }
                    else
                    {
                        path = Map.ins.AStarPathFinding(unitPos, mousePos);
                    }
                    if (!(path.Count == 1 && path[0] == new Vector2Int(-999, -999)))//jos path[0] = -999,-999 niin ei toimi...
                    {
                        unit.GetComponent<UnitMovement>().Move(path, task);
                    }
                }
            }
        }
    }
}
