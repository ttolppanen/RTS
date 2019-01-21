using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public float speed;
    List<Vector2Int> path = new List<Vector2Int>();
    bool moving = false;
    int i; //Missä kohdassa path polkua ollaan menossa
    Rigidbody2D rb;
    Vector2 movingDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!moving && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Move(new Vector2Int((int)mousePos.x, (int)mousePos.y));
        }
        if(moving)
        {
            Vector2 vectorToNextPoint = path[i] + new Vector2(0.5f, 0.5f) - (Vector2)transform.position;
            if (Vector2.Dot(vectorToNextPoint, movingDirection) <= 0)
            {
                transform.position = CordPosition();
                if (i == path.Count - 1)
                {
                    moving = false;
                    rb.velocity = Vector2.zero;
                }
                else
                {
                    i++;
                    movingDirection = (path[i] - path[i - 1]);
                    rb.velocity = speed * movingDirection;
                }
            }
        }
    }

    Vector2 CordPosition()
    {
        return new Vector2((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f);
    }

    public void Move(Vector2Int goal)
    {
        path.Clear();
        path.Add(new Vector2Int((int)transform.position.x, (int)transform.position.y));
        List<Vector2Int> checkedPoints = new List<Vector2Int>();
        List<Vector2Int> beginningPath = new List<Vector2Int>();
        beginningPath.Add(new Vector2Int((int)transform.position.x, (int)transform.position.y));
        Map.ins.FindPath(goal, checkedPoints, beginningPath, path);

        if (path.Count > 0)
        {
            i = 0;
            moving = true;
            movingDirection = (path[1] - path[0]);
            rb.velocity = speed * movingDirection;
        }
    }
}