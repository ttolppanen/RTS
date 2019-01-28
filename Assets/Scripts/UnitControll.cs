using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseStates { idle, choosingUnits, building};

public class UnitControll : MonoBehaviour
{
    public static UnitControll ins;

    public MouseStates mouseState = MouseStates.idle;
    public List<GameObject> chosenUnits = new List<GameObject>(); //Valitut ukot...
    Vector2 boxMouseStart;
    Vector2 boxMouseEnd;
    Vector2 boxSize;
    public GameObject selectingBoxGraphic;

    private void Awake()
    {
        {
        if (ins == null)
        {
            ins = this;
        }
        else
        {
            Destroy(this);
        }
        }//ins määrittely...
    }

    void Update()
    {
        if (!(mouseState == MouseStates.idle || mouseState == MouseStates.choosingUnits))//kun tehään jotain muuta niin pietetään valitut listat tyhjänä ja ei sallita nuita napin painalluksia
        {
            chosenUnits.Clear();
            return;
        }

        if (UsefullFunctions.IsOnUI())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            boxMouseStart = UsefullFunctions.GetMousePos();
            mouseState = MouseStates.choosingUnits;
        }
        else if (Input.GetMouseButton(0) && mouseState == MouseStates.choosingUnits)
        {
            boxMouseEnd = UsefullFunctions.GetMousePos();
            boxSize = boxMouseEnd - boxMouseStart;
            if (boxMouseEnd == boxMouseStart)
            {
                selectingBoxGraphic.SetActive(false);
            }
            else
            {
                selectingBoxGraphic.SetActive(true);
                selectingBoxGraphic.transform.localScale = boxSize;
                selectingBoxGraphic.transform.position = boxMouseStart;
            }
        }
        else if (Input.GetMouseButtonUp(0) && mouseState == MouseStates.choosingUnits)
        {
            RaycastHit2D[] hits = new RaycastHit2D[1];
            if (boxMouseEnd == boxMouseStart)
            {
                hits[0] = UsefullFunctions.MouseCast();
             }
            else
            {
                selectingBoxGraphic.transform.localScale = boxSize;
                boxSize.x = Mathf.Abs(boxSize.x);
                boxSize.y = Mathf.Abs(boxSize.y);
                hits = Physics2D.BoxCastAll((boxMouseStart + boxMouseEnd) / 2f, boxSize, 0f, Vector2.zero, 0f, LayerMask.GetMask("Unit"));
            }

            selectingBoxGraphic.SetActive(false);
            chosenUnits.Clear();
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    chosenUnits.Add(hit.transform.gameObject);
                }
            }
            mouseState = MouseStates.idle;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (chosenUnits.Count != 0)
            {
                foreach (GameObject unit in chosenUnits)
                {
                    Task task = new Task(GM.tasks[TaskTypes.idle], null);
                    List<Vector2Int> path = new List<Vector2Int>();

                    Vector2Int mousePos = UsefullFunctions.GetMousePosCoordinated();
                    RaycastHit2D objectUnderMouse = UsefullFunctions.MouseCast();
                    Vector2Int unitPos = UsefullFunctions.CoordinatePosition(unit.transform.position);

                    if (objectUnderMouse.collider != null && objectUnderMouse.collider.tag == "Tree")
                    {
                        GameObject tree = objectUnderMouse.collider.gameObject;
                        path = Map.ins.CorrectPathToBuilding(unitPos, mousePos, UsefullFunctions.CoordinatePosition(tree.transform.position), new Vector2Int(1, 1));
                        task = new Task(GM.tasks[TaskTypes.cutWood], tree);
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
