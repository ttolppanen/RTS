using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlls : MonoBehaviour
{
    public Vector2 scrollBoxSize;
    public float speed;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos.x *= 1f / Screen.width;
        mousePos.y *= 1f / Screen.height;
        mousePos -= new Vector2(0.5f, 0.5f); //Tehdään origi keskelle näyttöä. Näiden jälkeen siis 0,0 on keskellä näyttöä, 0.5, 0.5 on oikea yläreuna

        if (!(Mathf.Abs(mousePos.x) <= scrollBoxSize.x / 2f && Mathf.Abs(mousePos.y) <= scrollBoxSize.y / 2f)) //Jos ei olla boxin sisällä niin pitää siirtää kameraa...
        {
            rb.velocity = mousePos.normalized * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
