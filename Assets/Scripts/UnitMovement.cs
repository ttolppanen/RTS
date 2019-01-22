using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitMovement : MonoBehaviour
{
    public float speed;
    List<Vector2Int> path = new List<Vector2Int>();
    int i; //Missä kohdassa path polkua ollaan menossa, huomioitava asia on että path tulee väärässä järjestyksesä. siis path[0] on maali
    Rigidbody2D rb;
    Vector2 movingDirection;
    Task currentTask;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = CordPosition();
    }

    private void Update()
    {
        if (path.Count != 0)
        {
            Vector2 vectorToNextPoint = path[i - 1] + new Vector2(0.5f, 0.5f) - (Vector2)transform.position;
            if (Vector2.Dot(vectorToNextPoint, movingDirection) <= 0)
            {
                i--;
                if (i == 0)
                {
                    rb.velocity = Vector2.zero;
                    path.Clear();
                    if (currentTask.taskName != GM.tasks[(int)TaskTypes.idle])
                    {
                        currentTask.taskScriptInstance = (MonoBehaviour)gameObject.AddComponent(Type.GetType(currentTask.taskName)); //Lisätään task nimellä löytyvä koodi jäbälle...
                    }
                }
                else
                {
                    movingDirection = (path[i - 1] - path[i]);
                    rb.velocity = speed * movingDirection.normalized;
                }
            }
        }
    }

    Vector2 CordPosition()
    {
        return new Vector2((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f);
    }

    public void Move(Vector2Int goal, Task newTask)
    {
        if (currentTask.taskScriptInstance != null)
        {
            Destroy(currentTask.taskScriptInstance);
        }
        currentTask = newTask;

        path.Clear();
        Vector2Int start = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        path = Map.ins.AStarPathFinding(start, goal);

        if (path.Count > 0)
        {
            i = path.Count - 1;
            movingDirection = (path[i - 1] - path[i]);
            print(movingDirection);
            rb.velocity = speed * movingDirection.normalized;
        }
    }
}