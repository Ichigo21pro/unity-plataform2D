using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallJump : MonoBehaviour
{
   public float wallSlideSpeed = 2f; // Velocidad de deslizamiento por la pared
    public float wallJumpForce = 5f; // Fuerza del salto en la pared
    public Transform wallCheck; // Punto para verificar si está en contacto con la pared
    public float wallCheckRadius = 0.2f; // Radio de verificación
    public LayerMask wallLayer; // Capa de la pared

    private PlayerController playerController;
    private Rigidbody2D rb;
    private Animator animator;

    private bool isTouchingWall;
    public bool isWallSliding;
    private bool isWallJumping;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);

        if (isTouchingWall && !playerController.IsGrounded() && rb.velocity.y < 0)
        {
            StartWallSlide();
        }
        else
        {
            StopWallSlide();
        }

        if (isWallSliding && Input.GetButtonDown("Jump"))
        {
            WallJumpAction();
        }

        animator.SetBool("isWallSliding", isWallSliding);
    }

    void StartWallSlide()
    {
        isWallSliding = true;
        rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
    }

    void StopWallSlide()
    {
        isWallSliding = false;
    }

    void WallJumpAction()
    {
        isWallJumping = true;

        // Calcula la dirección del salto basándote en la posición de la pared
        float wallJumpDirectionX = isTouchingWall ? (transform.position.x - wallCheck.position.x) : 0f;
        wallJumpDirectionX = Mathf.Sign(wallJumpDirectionX); // Ajusta el signo para que salte hacia la dirección opuesta

        // Calcula la dirección del salto diagonalmente
        Vector2 wallJumpDirection = new Vector2(wallJumpDirectionX, 1).normalized;

        // Agrega una fuerza horizontal para el salto
        Vector2 jumpForce = wallJumpDirection * wallJumpForce;
        jumpForce.x *= 1.5f; // Ajusta este valor según sea necesario

        rb.velocity = jumpForce;

        Invoke("SetWallJumpingToFalse", 0.1f); // Previene múltiples saltos en la pared
    }

    void SetWallJumpingToFalse()
    {
        isWallJumping = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
    }
}
