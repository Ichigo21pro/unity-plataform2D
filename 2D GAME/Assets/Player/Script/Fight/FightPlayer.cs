using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightPlayer : MonoBehaviour
{
    public Animator animator;
    public PlayerController playerController;

    public Transform PunchPoint;
    public Transform KickPoint;
    public LayerMask enemyLayer;

    public float attackPunchRange = 0.5f;
    public float attackKickRange = 0.5f;
    public int attackPunchDamage = 40;
    public int attackKickDamage = 50;

    int currentPunchComboStep = 0; // Paso actual del combo de puñetazos
    int currentKickComboStep = 0; // Paso actual del combo de patadas
    float lastPunchAttackTime = 0f; // Tiempo del último puñetazo
    float lastKickAttackTime = 0f; // Tiempo de la última patada

    public float comboWindow = 0.5f; // Ventana de tiempo para realizar el siguiente ataque en el combo
    public float timeBetweenAttacks = 0.3f; // Tiempo entre ataques
    public float comboCooldown = 1.0f; // Tiempo de espera después de un combo completo

    bool canAttack = true; // Indica si el jugador puede atacar

    void Update()
    {
        if (canAttack && Input.GetKeyDown(KeyCode.J) && playerController.IsGrounded())
        {
            if (Time.time - lastPunchAttackTime >= timeBetweenAttacks)
            {
                AttackPunch();
            }
        }

        if (canAttack && Input.GetKeyDown(KeyCode.K) && playerController.IsGrounded())
        {
            if (Time.time - lastKickAttackTime >= timeBetweenAttacks)
            {
                AttackKick();
            }
        }
    }

    void AttackPunch()
    {
        // Deshabilitar el movimiento del jugador
        playerController.enabled = false;

        // Detectar si se presiona la tecla de ataque y si estamos dentro de la ventana de tiempo del combo
        if (Time.time - lastPunchAttackTime < comboWindow)
        {
            // Avanzar al siguiente paso del combo
            currentPunchComboStep++;

            // Ejecutar el ataque correspondiente al paso del combo
            switch (currentPunchComboStep)
            {
                case 1:
                    animator.SetTrigger("Punch1");
                    break;
                case 2:
                    animator.SetTrigger("Punch2");
                    break;
                case 3:
                    animator.SetTrigger("Punch3");
                    StartCoroutine(ComboCooldown());
                    break;
                default:
                    // Si ya se ha completado el combo, reiniciar desde el primer paso
                    currentPunchComboStep = 1;
                    animator.SetTrigger("Punch1");
                    break;
            }

            // Actualizar el tiempo del último ataque
            lastPunchAttackTime = Time.time;
        }
        else
        {
            // Si se presiona la tecla de ataque fuera de la ventana de tiempo del combo, iniciar nuevamente el combo
            currentPunchComboStep = 1;
            animator.SetTrigger("Punch1");
            lastPunchAttackTime = Time.time;
        }

        // Detectar enemigos
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(PunchPoint.position, attackPunchRange, enemyLayer);

        // Dañar a los enemigos
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<enemie>().TakeDamage(attackPunchDamage, enemie.AttackType.Punch);
            Debug.Log("We hit " + enemy.name);
        }

        // Habilitar el movimiento del jugador después de un pequeño retraso
        StartCoroutine(ReenableMovement());
    }

    void AttackKick()
    {
        // Deshabilitar el movimiento del jugador
        playerController.enabled = false;

        // Detectar si se presiona la tecla de ataque y si estamos dentro de la ventana de tiempo del combo
        if (Time.time - lastKickAttackTime < comboWindow)
        {
            // Avanzar al siguiente paso del combo
            currentKickComboStep++;

            // Ejecutar el ataque correspondiente al paso del combo
            switch (currentKickComboStep)
            {
                case 1:
                    animator.SetTrigger("Kick1");
                    break;
                case 2:
                    animator.SetTrigger("Kick2");
                    StartCoroutine(ComboCooldown());
                    break;
                default:
                    // Si ya se ha completado el combo, reiniciar desde el primer paso
                    currentKickComboStep = 1;
                    animator.SetTrigger("Kick1");
                    break;
            }

            // Actualizar el tiempo del último ataque
            lastKickAttackTime = Time.time;
        }
        else
        {
            // Si se presiona la tecla de ataque fuera de la ventana de tiempo del combo, iniciar nuevamente el combo
            currentKickComboStep = 1;
            animator.SetTrigger("Kick1");
            lastKickAttackTime = Time.time;
        }

        // Detectar enemigos
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(KickPoint.position, attackKickRange, enemyLayer);

        // Dañar a los enemigos
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<enemie>().TakeDamage(attackKickDamage, enemie.AttackType.Kick);
            Debug.Log("We hit " + enemy.name);
        }

        // Habilitar el movimiento del jugador después de un pequeño retraso
        StartCoroutine(ReenableMovement());
    }

    private IEnumerator ReenableMovement()
    {
        // Esperar un tiempo antes de habilitar el movimiento
        yield return new WaitForSeconds(timeBetweenAttacks);

        // Habilitar el movimiento del jugador
        playerController.enabled = true;
    }

    private IEnumerator ComboCooldown()
    {
        // Deshabilitar ataques
        canAttack = false;

        // Esperar un tiempo después del último ataque del combo
        yield return new WaitForSeconds(comboCooldown);

        // Habilitar ataques nuevamente
        canAttack = true;

        // Reiniciar el combo
        currentPunchComboStep = 0;
        currentKickComboStep = 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (PunchPoint != null)
        {
            Gizmos.DrawWireSphere(PunchPoint.position, attackPunchRange);
        }

        if (KickPoint != null)
        {
            Gizmos.DrawWireSphere(KickPoint.position, attackKickRange);
        }
    }
}
