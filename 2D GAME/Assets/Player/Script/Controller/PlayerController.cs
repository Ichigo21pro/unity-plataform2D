using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento del personaje
    public float jumpForce = 7f; // Fuerza del salto
    public float maxJumpTime = 0.5f; // Tiempo máximo de salto mantenido
    private float jumpTimeCounter; // Contador del tiempo de salto
    private Rigidbody2D rb; // Referencia al componente Rigidbody2D del personaje
    private Animator animator; // Referencia al componente Animator del personaje
    private Vector2 movement; // Vector para almacenar el movimiento del personaje
    private bool isGrounded; // Para comprobar si el personaje está en el suelo
    public Transform groundCheck; // Transform para comprobar si el personaje toca el suelo
    public float groundCheckRadius = 0.2f; // Radio para comprobar el suelo
    public LayerMask groundLayer; // Capa del suelo
    private bool isJumping; // Para controlar si el personaje está saltando
    private bool canDoubleJump; // Para controlar si el personaje puede hacer un doble salto
   

    public bool PlayerCanDoDobleJump; // Para controlar desde fuera si puede hacer doble salto

  
    void Start()
    {      
  
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
       
    }

    void Update()
    {
        // Capturar la entrada del jugador solo para movimiento horizontal
        movement.x = Input.GetAxisRaw("Horizontal"); // GetAxisRaw permite movimientos instantáneos sin suavizado
        movement.y = 0; // Asegurarse de que no haya movimiento vertical

        // Comprobar si el personaje está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Lógica de animación de correr
        if (movement.x != 0 && isGrounded)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        // Lógica de salto
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                isJumping = true;
                jumpTimeCounter = maxJumpTime;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetBool("isJumping", true);
                animator.SetBool("isFalling", false);
                if (PlayerCanDoDobleJump)
                {
                    canDoubleJump = true; // Permitir el doble salto después del primer salto
                }
            }
            else if (canDoubleJump)
            {
                isJumping = true;
                jumpTimeCounter = maxJumpTime;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetBool("isDoubleJumping", true);
                canDoubleJump = false; // Consumir el doble salto
            }
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        if (rb.velocity.y < 0)
        {
            animator.SetBool("isFalling", true);
        }
        else if (rb.velocity.y > 0)
        {
            animator.SetBool("isFalling", false);
        }

        if (isGrounded && rb.velocity.y == 0)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            animator.SetBool("isDoubleJumping", false); // Resetear cuando aterriza
        }

       
        // Girar el personaje según la dirección de movimiento
        if (movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (movement.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
       

    }

 
    void FixedUpdate()
    {
        // Aplicar el movimiento al personaje
        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
    }

    private void OnDrawGizmos()
    {
        // Dibujar el círculo de comprobación del suelo en el editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
}
