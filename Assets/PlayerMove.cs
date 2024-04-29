using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float moveSpeed = 5f; // Velocidad de movimiento horizontal
    public float jumpForce = 10f; // Fuerza de salto
    public bool canDoubleJump = false; // Habilitar/Deshabilitar doble salto

    private Rigidbody2D rb;

    private int jumpCount;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movimiento horizontal con teclas A y D
        float horizontal = Input.GetAxis("Horizontal");
        Vector2 velocity = rb.velocity;
        velocity.x = horizontal * moveSpeed;
        rb.velocity = velocity;

        // Salto con teclas W o Espacio
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpCount = 1; // Reseteamos el contador de saltos

            if (canDoubleJump && jumpCount < 2) // Doble salto
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jumpCount++;
            }
        }

        //se tendria que añadir :
        // se ha tocado el suelo?
        // resetear jumpCount = 0 cuando toca 0
    }


}
