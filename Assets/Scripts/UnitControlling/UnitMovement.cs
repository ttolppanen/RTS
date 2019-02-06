using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitMovement : MonoBehaviour
{
    List<Vector2Int> path = new List<Vector2Int>();
    int i; //Missä kohdassa path polkua ollaan menossa.
    Rigidbody2D rb;
    Vector2 movingDirection;
    Animator anim;
    AnimatorController animControl;
    UnitStatus unitStatus;
    public Task currentTask;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = CordPosition();
        anim = GetComponent<Animator>();
        animControl = GetComponent<AnimatorController>();
        unitStatus = GetComponent<UnitStatus>();
    }

    private void Update()
    {
        if (path.Count != 0)
        {
            if (currentTask.taskRange != 0)
            {
                if (UF.DistanceBetween2Units(currentTask.objectives[0].transform.position, transform.position) <= currentTask.taskRange) //Jos etäisyys kohteeseen on vähemmän kuin taskiRange
                {
                    GoalReached();
                    return;
                }
            }
            if (UF.DistanceBetween2Units(path[i] + new Vector2(0.5f, 0.5f), transform.position) <= 0.2f)
            {
                i++;
                if (i == path.Count)
                {
                    GoalReached();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (path.Count > 0)
        {
            movingDirection = (path[i] + new Vector2(0.5f, 0.5f)) - (Vector2)(transform.position);
            transform.rotation = UF.TurnUnit(movingDirection, -90f);
            rb.AddForce(rb.mass * movingDirection.normalized * GM.fixedAcceleration);
        }
        if (rb.velocity.magnitude > unitStatus.maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * unitStatus.maxSpeed;
        }
    }

    void GoalReached()
    {
        rb.velocity = Vector2.zero;
        path.Clear();
        anim.SetBool("Running", false);
        StartTask();
    }

    void StartTask()
    {
        if (currentTask != null && currentTask.taskName != GM.tasks[TaskTypes.idle])
        {
            currentTask.taskScriptInstance = (MonoBehaviour)gameObject.AddComponent(Type.GetType(currentTask.taskName)); //Lisätään task nimellä löytyvä koodi jäbälle...
        }
    }

    Vector2 CordPosition()
    {
        return new Vector2((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f);
    }

    public void GoDoATask(Vector2Int goal, Task newTask) //Mennään vaan jonnekkin pisteeseen tekemään jtn
    {
        List<Vector2Int> newPath = Map.ins.AStarPathFinding(UF.CoordinatePosition(transform.position), goal);
        if (newPath.Count != 0 && newPath[0] == new Vector2Int(-999, -999)) //ei pitäisi oikeasti koskaan päässä tänne?
        {
            return;
        }
        ResetTaskAndPathAndOthers();
        currentTask = newTask;
        path = newPath;
        if (path.Count == 1)
        {
            GoalReached();
        }
        else
        {
            i = 1;
            anim.SetBool("Running", true);
            movingDirection = (path[1] + new Vector2(0.5f, 0.5f)) - (Vector2)(transform.position);
        }
    }

    public void GoDoATask(Vector2Int goal, Vector2Int buildingSize, Task newTask) //Mennään jollekkin rakennukselle, tai jollekkin millä on koko niin tekemään jtn
    {
        List<Vector2Int> newPath = Map.ins.CorrectPathToBuilding(UF.CoordinatePosition(transform.position), goal, goal, buildingSize);
        if (newPath.Count != 0 && newPath[0] == new Vector2Int(-999, -999)) //ei pitäisi oikeasti koskaan päässä tänne?
        {
            return;
        }
        ResetTaskAndPathAndOthers();
        currentTask = newTask;
        path = newPath;
        if (path.Count == 1)
        {
            GoalReached();
        }
        else
        {
            i = 1;
            anim.SetBool("Running", true);
            movingDirection = (path[1] + new Vector2(0.5f, 0.5f)) - (Vector2)(transform.position);
        }
    }

    public void GoDoATask(Task newTask)//Mennään tekemään jotain esim jollekkin ukolle, siis jollekkin mikä liikkuu. Tällöis task.objectives[0] tulee olla se kenen luokse ollaan menossa
    {
        List<Vector2Int> newPath = Map.ins.AStarPathFinding(UF.CoordinatePosition(transform.position), UF.CoordinatePosition(newTask.objectives[0].transform.position));
        if (newPath.Count != 0 && newPath[0] == new Vector2Int(-999, -999)) //ei pitäisi oikeasti koskaan päässä tänne?
        {
            return;
        }
        ResetTaskAndPathAndOthers();
        currentTask = newTask;
        path = newPath;
        if (path.Count == 1)
        {
            GoalReached();
        }
        else
        {
            i = 1;
            anim.SetBool("Running", true);
            movingDirection = (path[1] + new Vector2(0.5f, 0.5f)) - (Vector2)(transform.position);
            StartCoroutine(UpdatePath(newTask.objectives[0]));
        }
    }

    IEnumerator UpdatePath(GameObject goal)
    {
        if (goal == null)
        {
            Stop();
            yield break;
        }
        List<Vector2Int> newPath = Map.ins.AStarPathFinding(UF.CoordinatePosition(transform.position), UF.CoordinatePosition(goal.transform.position));
        if (newPath.Count != 0 && newPath[0] == new Vector2Int(-999, -999)) //ei pitäisi oikeasti koskaan päässä tänne?
        {
            Stop();
            yield break;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(UpdatePath(goal));
    }

   /* public void GoDoATask(List<Vector2Int> newPath, Task newTask)
    {
        if (newPath.Count != 0 && newPath[0] == new Vector2Int(-999, -999)) //ei pitäisi oikeasti koskaan päässä tänne?
        {
            return;
        }
        ResetTaskInstance();
        animControl.ResetAnimations();
        currentTask = newTask;
        path = newPath;
        if (path.Count > 1)
        {
            i = 0;
            anim.SetBool("Running", true);
            movingDirection = (path[1] - path[0]);
            transform.rotation = UF.TurnUnit(movingDirection, -90f);
        }
        else
        {
            path.Clear();
            StartTask();
        }
    }*/

    public void SetAndStartTask(Task newTask)
    {
        currentTask = newTask;
        StartTask();
    }

    public void Stop()
    {
        ResetTaskAndPathAndOthers();
        rb.velocity = Vector2.zero;
        currentTask = new Task(GM.tasks[TaskTypes.idle], null);
        unitStatus.currentState = UnitStates.idle;
    }

    void ResetTaskAndPathAndOthers()
    {
        path.Clear();
        ResetTaskInstance();
        animControl.ResetAnimations();
        StopCoroutine("UpdatePath");
    }

    void ResetTaskInstance()
    {
        if (currentTask != null && currentTask.taskScriptInstance != null)
        {
            Destroy(currentTask.taskScriptInstance);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Unit")
        {
            Vector2 awayFromOther = transform.position - collision.transform.position;
            rb.AddForce(awayFromOther.normalized * Time.deltaTime * GM.unitRepulsionStrenght * rb.mass * rb.drag);
        }
    }
}