using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitMovement : MonoBehaviour
{
    public float speed;
    List<Vector2Int> path = new List<Vector2Int>();
    int i; //Missä kohdassa path polkua ollaan menossa.
    Rigidbody2D rb;
    Vector2 movingDirection;
    public Task currentTask;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = CordPosition();
    }

    private void Update()
    {
        if (path.Count != 0)
        {
            Vector2 vectorToNextPoint = path[i + 1] + new Vector2(0.5f, 0.5f) - (Vector2)transform.position;
            if (Vector2.Dot(vectorToNextPoint, movingDirection) <= 0)
            {
                i++;
                if (i == path.Count - 1)
                {
                    rb.velocity = Vector2.zero;
                    path.Clear();
                    StartTask();
                }
                else
                {
                    movingDirection = (path[i + 1] - path[i]);
                    rb.velocity = speed * movingDirection.normalized;
                }
            }
        }
    }

    void StartTask()
    {
        if (currentTask.taskName != GM.tasks[TaskTypes.idle])
        {
            currentTask.taskScriptInstance = (MonoBehaviour)gameObject.AddComponent(Type.GetType(currentTask.taskName)); //Lisätään task nimellä löytyvä koodi jäbälle...
        }
    }

    Vector2 CordPosition()
    {
        return new Vector2((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f);
    }

    public void Move(List<Vector2Int> newPath, Task newTask)
    {
        if (currentTask != null && currentTask.taskScriptInstance != null)
        {
            Destroy(currentTask.taskScriptInstance);
        }
        currentTask = newTask;
        path = newPath;
        print(path.Count);
        if (path.Count > 1)
        {
            i = 0;
            movingDirection = (path[1] - path[0]);
            print(movingDirection);
            rb.velocity = speed * movingDirection.normalized;
        }
        else
        {
            StartTask();
        }
    }
}