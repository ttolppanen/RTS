using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public float speed;
    List<Vector2Int> path = new List<Vector2Int>();
    bool moving = false;
    int i; //Missä kohdassa polkua ollaan menossa
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
            Vector2Int goal = new Vector2Int((int)mousePos.x, (int)mousePos.y);

            path.Clear();
            path.Add(new Vector2Int((int)transform.position.x, (int)transform.position.y));
            List<Vector2Int> checkedPoints = new List<Vector2Int>();
            FindPath(path, goal, checkedPoints);

            print(path.Count);
            if (path.Count > 1)
            {
                i = 0;
                moving = true;
                movingDirection = (path[1] - path[0]);
                rb.velocity = speed * movingDirection;
            }
        }
        else if(moving)
        {
            print(i);
            Vector2 vectorToNextPoint = path[i + 1] + new Vector2(0.5f, 0.5f) - (Vector2)transform.position;
            if (Vector2.Dot(vectorToNextPoint, movingDirection) <= 0)
            {
                transform.position = CordPosition();
                if (i + 1 == path.Count - 1)
                {
                    moving = false;
                    rb.velocity = Vector2.zero;
                }
                else
                {
                    i++;
                    movingDirection = (path[1 + i] - path[i]);
                    rb.velocity = speed * movingDirection;
                }
            }
        }
    }

    Vector2 CordPosition()
    {
        return new Vector2((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f);
    }

    void FindPath(List<Vector2Int> currentPath, Vector2Int goal, List<Vector2Int> checkedPoints) //path[0] on aloituspiste
    {
        if (path[path.Count - 1] == goal) //Onko valmis
        {
            return;
        }

        Vector2Int currentPoint = currentPath[currentPath.Count - 1];
        print(currentPoint);
        checkedPoints.Add(currentPoint);
        List<Vector2Int> possiblePoints = new List<Vector2Int>();
        for (int i = -1; i <= 1; i += 2)
        {
            Vector2Int pointWeAreChecking = new Vector2Int(currentPoint.x + i, currentPoint.y);
            if (pointWeAreChecking.x >= 0 && pointWeAreChecking.x < Map.ins.size.x && pointWeAreChecking.y >= 0 && pointWeAreChecking.y < Map.ins.size.y)
            {
                if ((Map.ins.mapData[pointWeAreChecking.x, pointWeAreChecking.y] == (int)LandTypes.grass || Map.ins.mapData[pointWeAreChecking.x, pointWeAreChecking.y] == (int)LandTypes.sand) && !checkedPoints.Contains(pointWeAreChecking)) //Jos mapissa on ruohoa ja ei olla jo katottu tätä pistettä
                {
                    possiblePoints.Add(pointWeAreChecking);
                }
            }
        }
        for (int i = -1; i <= 1; i += 2)
        {
            Vector2Int pointWeAreChecking = new Vector2Int(currentPoint.x, currentPoint.y + i);
            if (pointWeAreChecking.x >= 0 && pointWeAreChecking.x < Map.ins.size.x && pointWeAreChecking.y >= 0 && pointWeAreChecking.y < Map.ins.size.y)
            {
                if ((Map.ins.mapData[pointWeAreChecking.x, pointWeAreChecking.y] == (int)LandTypes.grass || Map.ins.mapData[pointWeAreChecking.x, pointWeAreChecking.y] == (int)LandTypes.sand) && !checkedPoints.Contains(pointWeAreChecking))//Jos mapissa on ruohoa ja ei olla jo katottu tätä pistettä
                {
                    possiblePoints.Add(pointWeAreChecking);
                }
            }
        }

        if (possiblePoints.Count != 0)
        {
            List<Vector2Int> arrangedList = ArrangeByLength(possiblePoints, goal);
            foreach (Vector2Int possiblePoint in arrangedList)
            {
                if (possiblePoint == goal)
                {
                    currentPath.Add(possiblePoint);
                    path = currentPath;
                    return;
                }
                else
                {
                    List<Vector2Int> newPath = new List<Vector2Int>(currentPath);
                    newPath.Add(possiblePoint);
                    checkedPoints.Add(possiblePoint);
                    FindPath(newPath, goal, checkedPoints);
                }
            }
        }
    }

    List<Vector2Int> ArrangeByLength(List<Vector2Int> a, Vector2Int b) //Lyhin etäisyys ekana...
    {
        List<Vector2Int> sortedList = new List<Vector2Int>();
        sortedList.Add(a[0]);
        for (int ia = 1; ia < a.Count; ia++)
        { 
            for (int i = 0; i < sortedList.Count; i++)
            {
                float aDist = (a[ia] - b).magnitude; //mahdollisista pisteistä otetun vectorin a etäisyys b
                float sDist = (sortedList[i] - b).magnitude; //...

                if (aDist < sDist)
                {
                    sortedList.Insert(i, a[ia]);
                    break;
                }
                else if (i == sortedList.Count - 1) 
                {
                    sortedList.Add(a[ia]);
                    break;
                }
            }
        }
        return sortedList;
    }
}