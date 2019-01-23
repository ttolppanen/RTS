﻿using System.Collections;
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
    public float acceleration;
    public float topSpeed;
    public float repulsionStrength;

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
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (path.Count > 0)
        {
            rb.AddForce(rb.mass * movingDirection.normalized * acceleration * rb.drag);
        }
        if (rb.velocity.magnitude > topSpeed)
        {
            rb.velocity = rb.velocity.normalized * topSpeed;
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
        if (path.Count > 1)
        {
            i = 0;
            movingDirection = (path[1] - path[0]);
        }
        else
        {
            StartTask();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Unit")
        {
            Vector2 awayFromOther = transform.position - collision.transform.position;
            rb.AddForce(awayFromOther.normalized * Time.deltaTime * repulsionStrength * rb.mass * rb.drag);
        }
    }
}