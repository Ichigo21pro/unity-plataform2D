using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemie : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    int currentHealth;

    // Enum para definir los tipos de ataque
    public enum AttackType
    {
        Punch,
        Kick
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, AttackType attackType)
    {
        if (currentHealth == 999)
        {
            return; // No hacer nada si la salud es 999
        }
        else
        {
            currentHealth -= damage;
        }

        // Ejecutar la animación correspondiente según el tipo de ataque
        switch (attackType)
        {
            case AttackType.Punch:
                animator.SetTrigger("PunchAttack");
                break;
            case AttackType.Kick:
                animator.SetTrigger("KickAttack");
                break;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("ENEMY DIE");
        // Desactivar el colisionador y este script
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

}
