using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beAttack : MonoBehaviour
{
    private Animator animator;

    // Tiempo despu�s del cual se restablece el estado de animaci�n
    public float resetTime = 0.5f; // Puedes ajustar este valor seg�n lo necesario

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // M�todo para detectar colisiones con los ataques del jugador
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerPunch"))
        {
            animator.SetBool("beAttack", true);
            StartCoroutine(ResetAnimationBool("beAttack", resetTime));
        }
        else if (collision.CompareTag("PlayerKick"))
        {
            animator.SetBool("beStrongAttack", true);
            StartCoroutine(ResetAnimationBool("beStrongAttack", resetTime));
        }
    }

    // Corrutina para restablecer el estado de animaci�n
    private IEnumerator ResetAnimationBool(string parameterName, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool(parameterName, false);
    }
}

