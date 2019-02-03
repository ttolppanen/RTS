using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseStates { idle, choosingUnits, building, targeting};

public class MouseControl : MonoBehaviour
{
    public static MouseControl ins;

    public MouseStates mouseState = MouseStates.idle;
    public List<GameObject> chosenUnits = new List<GameObject>(); //Valitut ukot...
    Vector2 boxMouseStart;
    Vector2 boxMouseEnd;
    Vector2 boxSize;
    public GameObject selectingBoxGraphic;

    public List<GameObject> giveChosenUnits()
    {
        return chosenUnits;
    }

    public void chooseUI()
    {
        if (chosenUnits.Count != 0)
        {
            int highestHierachy = -1;
            GameObject showThisUnitsUI = chosenUnits[0];
            foreach (GameObject unit in chosenUnits)
            {
                if (highestHierachy < unit.GetComponent<Buttons>().hierachy)
                {
                    showThisUnitsUI = unit;
                    highestHierachy = unit.GetComponent<Buttons>().hierachy;
                }
            }

            UITypes UIType = showThisUnitsUI.GetComponent<Buttons>().giveUI();
            List<GameObject> freeSlot = showThisUnitsUI.GetComponent<Buttons>().giveButtons();

            UIChooser.ins.activateUI(UIType,freeSlot, showThisUnitsUI);
        }
        else
        {
            UIChooser.ins.deActivateAll();
        }
        
    }

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
        if (UF.IsOnUI())
        {
            return;
        }
        switch(mouseState)
        {
            case MouseStates.idle:
                if (Input.GetMouseButtonDown(0))
                {
                    boxMouseStart = UF.GetMousePos();
                    mouseState = MouseStates.choosingUnits;
                }

                //Right Click
                if (Input.GetMouseButtonDown(1))
                {
                    if (chosenUnits.Count != 0)
                    {
                        if (chosenUnits[0].tag == "Unit")
                        {
                            foreach (GameObject unit in chosenUnits)
                            {
                                Task task = new Task(GM.tasks[TaskTypes.idle], null);
                                List<Vector2Int> path = new List<Vector2Int>();

                                Vector2Int mousePos = UF.GetMousePosCoordinated();
                                RaycastHit2D objectUnderMouse = UF.MouseCast();
                                Vector2Int unitPos = UF.CoordinatePosition(unit.transform.position);

                                if (objectUnderMouse.collider != null && objectUnderMouse.collider.GetComponent<Interactable>() != null) //On painettu objectia ja sille voi tehdä jotain/Siltä löytyy interctable scripti
                                {
                                    GameObject taskObject = objectUnderMouse.collider.gameObject;
                                    TaskTypes taskType = taskObject.GetComponent<Interactable>().taskType;
                                    task = new Task(GM.tasks[taskType], new List<GameObject> { taskObject });
                                    if (taskObject.GetComponent<BuildingStatus>() != null) //On jonkin kokoinen
                                    {
                                        Vector2Int size = taskObject.GetComponent<BuildingStatus>().size;
                                        path = Map.ins.CorrectPathToBuilding(unitPos, mousePos, UF.CoordinatePosition(taskObject.transform.position), size);
                                    }
                                    else if (taskObject.tag == "Enemy")
                                    {
                                        path = Map.ins.CorrectPathToBuilding(unitPos, mousePos, UF.CoordinatePosition(taskObject.transform.position), new Vector2Int(1, 1)); //Vihollisilla on vakio koko 1x1
                                    }
                                    else
                                    {
                                        path = Map.ins.AStarPathFinding(unitPos, mousePos);
                                    }
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

                break;

            case MouseStates.choosingUnits:
                if (Input.GetMouseButton(0))
                {
                    boxMouseEnd = UF.GetMousePos();
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
                else if (Input.GetMouseButtonUp(0))
                {
                    RaycastHit2D[] hits = new RaycastHit2D[1];
                    if (boxMouseEnd == boxMouseStart)
                    {
                        hits[0] = UF.MouseCast();
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

                    chooseUI();

                    mouseState = MouseStates.idle;
                }

                if (Input.GetMouseButtonDown(1))
                {
                    selectingBoxGraphic.SetActive(false);
                    mouseState = MouseStates.idle;
                }
                break;
        }
        
        
    }
}
