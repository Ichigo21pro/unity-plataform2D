using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightPlayer : MonoBehaviour
{
    public GameObject punchCollider; // Referencia al GameObject del colisionador de puñetazo
    public GameObject kickCollider; // Referencia al GameObject del colisionador de patada
    public bool PlayerCanFightPunch;
    public bool PlayerCanFightKikcs;

    private int punchComboStep; // Paso actual en el combo de puñetazos
    private float punchComboResetTime = 1f; // Tiempo para reiniciar el combo de puñetazos
    private float lastPunchTime; // Última vez que se presionó la tecla de puñetazo

    private int kickComboStep; // Paso actual en el combo de patadas
    private float kickComboResetTime = 0.5f; // Tiempo para reiniciar el combo de patadas
    private float lastKickTime; // Última vez que se presionó la tecla de patada
    private Animator animator; // Referencia al componente Animator del personaje
    void Start()
    {
        animator = GetComponent<Animator>();
        // Desactivar los colisionadores al principio
        punchCollider.SetActive(false);
        kickCollider.SetActive(false);
        punchComboStep = 0; // Inicialmente el combo de puñetazos está en el primer paso
        kickComboStep = 0; // Inicialmente el combo de patadas está en el primer paso
        // Desactivar los colisionadores al principio

    }

    // Update is called once per frame
    void Update()
    {
        // Lógica de combo de puñetazos
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
                    // Activar el colisionador de puñetazo
                    punchCollider.SetActive(true);

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

                punchComboStep = (punchComboStep + 1) % 4; // Avanzar al siguiente paso del combo y volver a 0 después del último paso
                lastPunchTime = Time.time; // Actualizar el tiempo del último ataque
                EndAttack();

            }
        }
        if (PlayerCanFightKikcs)
        {
            // Lógica de combo de patadas
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
                }

                kickComboStep = (kickComboStep + 1) % 2; // Avanzar al siguiente paso del combo y volver a 0 después del último paso
                lastKickTime = Time.time; // Actualizar el tiempo del último ataque
                EndAttack();


            }
        }
    }
    void EndAttack()
    {
        // Lógica para desactivar los colisionadores al final del ataque
        punchCollider.SetActive(false);
        kickCollider.SetActive(false);
    }
}
