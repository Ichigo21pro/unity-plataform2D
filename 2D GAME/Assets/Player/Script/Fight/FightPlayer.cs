using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightPlayer : MonoBehaviour
{

    public GameObject punchCollider; // Referencia al GameObject del colisionador de pu�etazo
    public GameObject kickCollider; // Referencia al GameObject del colisionador de patada
    public bool PlayerCanFightPunch;
    public bool PlayerCanFightKikcs;

    private int punchComboStep; // Paso actual en el combo de pu�etazos
    private float punchComboResetTime = 1f; // Tiempo para reiniciar el combo de pu�etazos
    private float lastPunchTime; // �ltima vez que se presion� la tecla de pu�etazo

    private int kickComboStep; // Paso actual en el combo de patadas
    private float kickComboResetTime = 0.5f; // Tiempo para reiniciar el combo de patadas
    private float lastKickTime; // �ltima vez que se presion� la tecla de patada
    private Animator animator; // Referencia al componente Animator del personaje

    void Start()
    {
        animator = GetComponent<Animator>();
        // Desactivar los colisionadores al principio
        punchCollider.SetActive(false);
        kickCollider.SetActive(false);
        punchComboStep = 0; // Inicialmente el combo de pu�etazos est� en el primer paso
        kickComboStep = 0; // Inicialmente el combo de patadas est� en el primer paso
    }

    void Update()
    {
        // L�gica de combo de pu�etazos
        if (PlayerCanFightPunch)
        {
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
                    // Activar el colisionador de pu�etazo
                    punchCollider.SetActive(true);
                }
                else if (punchComboStep == 1)
                {
                    animator.SetTrigger("Punch2");
                    punchCollider.SetActive(true);
                }
                else if (punchComboStep == 2)
                {
                    animator.SetTrigger("Punch3");
                    punchCollider.SetActive(true);
                }
                else if (punchComboStep == 3)
                {
                    animator.SetTrigger("Punch4");
                    punchCollider.SetActive(true);
                }

                punchComboStep = (punchComboStep + 1) % 4; // Avanzar al siguiente paso del combo y volver a 0 despu�s del �ltimo paso
                lastPunchTime = Time.time; // Actualizar el tiempo del �ltimo ataque

                // Desactivar el colisionador despu�s de un breve tiempo para asegurarse de que el ataque se ha procesado
                StartCoroutine(DeactivateColliderAfterTime(punchCollider, 0.5f));
            }
        }

        if (PlayerCanFightKikcs)
        {
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
                    // Activar el colisionador de patada
                    kickCollider.SetActive(true);
                }
                else if (kickComboStep == 1)
                {
                    animator.SetTrigger("Kick2");
                    punchCollider.SetActive(true);
                }

                kickComboStep = (kickComboStep + 1) % 2; // Avanzar al siguiente paso del combo y volver a 0 despu�s del �ltimo paso
                lastKickTime = Time.time; // Actualizar el tiempo del �ltimo ataque

                // Desactivar el colisionador despu�s de un breve tiempo para asegurarse de que el ataque se ha procesado
                StartCoroutine(DeactivateColliderAfterTime(kickCollider, 0.5f));
            }
        }
    }

    private IEnumerator DeactivateColliderAfterTime(GameObject collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.SetActive(false);
    }

    void EndAttack()
    {
        // L�gica para desactivar los colisionadores al final del ataque
        punchCollider.SetActive(false);
        kickCollider.SetActive(false);
    }
}
