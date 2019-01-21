using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public float speed;
    List<Vector2Int> path = new List<Vector2Int>();
    bool moving = false;
    int i; //Missä kohdassa path polkua ollaan menossa, huomioitava asia on että path tulee väärässä järjestyksesä. siis path[0] on maali
    Rigidbody2D rb;
    Vector2 movingDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = CordPosition();
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
            Vector2 vectorToNextPoint = path[i - 1] + new Vector2(0.5f, 0.5f) - (Vector2)transform.position;
            if (Vector2.Dot(vectorToNextPoint, movingDirection) <= 0)
            {
                i--;
                if (i == 0)
                {
                    moving = false;
                    rb.velocity = Vector2.zero;
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

    public void Move(Vector2Int goal)
    {
        path.Clear();
        Vector2Int start = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        path = Map.ins.AStarPathFinding(start, goal);
        print(path.Count);

        if (path.Count > 0)
        {
            i = path.Count - 1;
            moving = true;
            movingDirection = (path[i - 1] - path[i]);
            print(movingDirection);
            rb.velocity = speed * movingDirection.normalized;
        }
    }
}