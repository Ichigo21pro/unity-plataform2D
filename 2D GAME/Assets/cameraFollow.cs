using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform target; // El jugador que la cámara seguirá
    public Vector3 offset; // Desplazamiento de la cámara respecto al jugador
    public float smoothSpeed = 0.125f; // Velocidad de suavizado

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
