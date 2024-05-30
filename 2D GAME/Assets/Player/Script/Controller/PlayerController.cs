using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento del personaje
    public float jumpForce = 7f; // Fuerza del salto
    public float maxJumpTime = 0.5f; // Tiempo m�ximo de salto mantenido
    private float jumpTimeCounter; // Contador del tiempo de salto
    private Rigidbody2D rb; // Referencia al componente Rigidbody2D del personaje
    private Animator animator; // Referencia al componente Animator del personaje
    private Vector2 movement; // Vector para almacenar el movimiento del personaje
    private bool isGrounded; // Para comprobar si el personaje est� en el suelo
    public Transform groundCheck; // Transform para comprobar si el personaje toca el suelo
    public float groundCheckRadius = 0.2f; // Radio para comprobar el suelo
    public LayerMask groundLayer; // Capa del suelo
    private bool isJumping; // Para controlar si el personaje est� saltando
    private bool canDoubleJump; // Para controlar si el personaje puede hacer un doble salto
    public bool PlayerCanDoDobleJump; // Para controlar desde fuera si puede hacer doble salto
    public bool PlayerCanFightPunch; // Para controlar desde fuera si puede hacer doble salto
    public bool PlayerCanFightKikcs; // Para controlar desde fuera si puede hacer doble salto

    private int punchComboStep; // Paso actual en el combo de pu�etazos
    private float punchComboResetTime = 1f; // Tiempo para reiniciar el combo de pu�etazos
    private float lastPunchTime; // �ltima vez que se presion� la tecla de pu�etazo

    private int kickComboStep; // Paso actual en el combo de patadas
    private float kickComboResetTime = 0.5f; // Tiempo para reiniciar el combo de patadas
    private float lastKickTime; // �ltima vez que se presion� la tecla de patada

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        punchComboStep = 0; // Inicialmente el combo de pu�etazos est� en el primer paso
        kickComboStep = 0; // Inicialmente el combo de patadas est� en el primer paso
    }

    void Update()
    {
        // Capturar la entrada del jugador solo para movimiento horizontal
        movement.x = Input.GetAxisRaw("Horizontal"); // GetAxisRaw permite movimientos instant�neos sin suavizado
        movement.y = 0; // Asegurarse de que no haya movimiento vertical

        // Comprobar si el personaje est� en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // L�gica de animaci�n de correr
        if (movement.x != 0 && isGrounded)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        // L�gica de salto
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
                    canDoubleJump = true; // Permitir el doble salto despu�s del primer salto
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

        // L�gica de combo de pu�etazos
        if (PlayerCanFightPunch) { 
        if (Input.GetKeyDown(KeyCode.J))
        {
            float timeSinceLastPunch = Time.time - lastPunchTime;

            if (timeSinceLastPunch > punchComboResetTime)
            {
                punchComboStep = 0; // Reiniciar el combo si el tiempo entre ataques es demasiado largo
            }

            if (punchComboStep == 0)
            {
                animator.SetTrigger("Punch1");
            }
            else if (punchComboStep == 1)
            {
                animator.SetTrigger("Punch2");
            }
            else if (punchComboStep == 2)
            {
                animator.SetTrigger("Punch3");
            }
            else if (punchComboStep == 3)
            {
                animator.SetTrigger("Punch4");
            }

            punchComboStep = (punchComboStep + 1) % 4; // Avanzar al siguiente paso del combo y volver a 0 despu�s del �ltimo paso
            lastPunchTime = Time.time; // Actualizar el tiempo del �ltimo ataque
        }
        }
        if(PlayerCanFightKikcs) { 
        // L�gica de combo de patadas
        if (Input.GetKeyDown(KeyCode.K))
        {
            float timeSinceLastKick = Time.time - lastKickTime;

            if (timeSinceLastKick > kickComboResetTime)
            {
                kickComboStep = 0; // Reiniciar el combo si el tiempo entre ataques es demasiado largo
            }

            if (kickComboStep == 0)
            {
                animator.SetTrigger("Kick1");
            }
            else if (kickComboStep == 1)
            {
                animator.SetTrigger("Kick2");
            }

            kickComboStep = (kickComboStep + 1) % 2; // Avanzar al siguiente paso del combo y volver a 0 despu�s del �ltimo paso
            lastKickTime = Time.time; // Actualizar el tiempo del �ltimo ataque
        }
        }
        // Girar el personaje seg�n la direcci�n de movimiento
        if (movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (movement.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (PlayerCanFightKikcs || PlayerCanFightPunch) { 
        // Reiniciar animaciones de ataque cuando terminan
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                if (movement.x == 0)
                {
                    animator.SetBool("isRunning", false);
                }
                else
                {
                    animator.SetBool("isRunning", true);
                }
            }
        }
        }
    }

    void FixedUpdate()
    {
        // Aplicar el movimiento al personaje
        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
    }

    private void OnDrawGizmos()
    {
        // Dibujar el c�rculo de comprobaci�n del suelo en el editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
