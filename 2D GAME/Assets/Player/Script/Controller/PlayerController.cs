using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento del personaje
    public float jumpForce = 7f; // Fuerza del salto
    public float maxJumpTime = 0.5f; // Tiempo máximo de salto mantenido
    private float jumpTimeCounter; // Contador del tiempo de salto
    public Rigidbody2D rb; // Referencia al componente Rigidbody2D del personaje
    public Animator animator; // Referencia al componente Animator del personaje
    private Vector2 movement; // Vector para almacenar el movimiento del personaje
    private bool isGrounded; // Para comprobar si el personaje está en el suelo
    public Transform groundCheck; // Transform para comprobar si el personaje toca el suelo
    public float groundCheckRadius = 0.2f; // Radio para comprobar el suelo
    public LayerMask groundLayer; // Capa del suelo
    private bool isJumping; // Para controlar si el personaje está saltando
    private bool canDoubleJump; // Para controlar si el personaje puede hacer un doble salto
    public bool PlayerCanDoDobleJump; // Para controlar desde fuera si puede hacer doble salto
    private bool isCrouching; // Para controlar si el personaje está agachado

    private wallJump wallJump; // Referencia al script WallJump

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        wallJump = GetComponent<wallJump>();
    }

    void Update()
    {
        // Capturar la entrada del jugador solo para movimiento horizontal
        movement.x = Input.GetAxisRaw("Horizontal"); // GetAxisRaw permite movimientos instantáneos sin suavizado
        movement.y = 0; // Asegurarse de que no haya movimiento vertical

        // Comprobar si el personaje está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Lógica de animación de correr
        if (movement.x != 0 && isGrounded && !isCrouching)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        HandleJumping();
        HandleCrouching();

        // Actualizar isFalling según la velocidad vertical
        if (rb.velocity.y < 0)
        {
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isFalling", false);
        }

        if (isGrounded && rb.velocity.y == 0)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            animator.SetBool("isDoubleJumping", false);
        }

        // Girar el personaje según la dirección de movimiento
        if (!isCrouching && !wallJump.isWallSliding) // Solo girar si no está agachado o deslizando en la pared
        {
            if (movement.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (movement.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    void FixedUpdate()
    {
        // Aplicar el movimiento al personaje
        if (!isCrouching && !wallJump.isWallSliding)
        {
            rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
        }
        else if (isCrouching)
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Detener el movimiento horizontal cuando está agachado
        }
    }

    void HandleJumping()
    {
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
                    canDoubleJump = true;
                }
            }
            else if (canDoubleJump)
            {
                isJumping = true;
                jumpTimeCounter = maxJumpTime;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetBool("isDoubleJumping", true);
                canDoubleJump = false;
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
    }

    void HandleCrouching()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            isCrouching = true;
            animator.SetBool("isCrouching", true);
        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            isCrouching = false;
            animator.SetBool("isCrouching", false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public void CanDoubleJump() { canDoubleJump = true; }
}
