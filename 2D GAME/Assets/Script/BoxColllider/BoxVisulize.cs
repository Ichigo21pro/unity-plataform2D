using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxVisulize : MonoBehaviour
{
    private BoxCollider boxCollider;

    void OnDrawGizmos()
    {
        boxCollider = GetComponent<BoxCollider>();

        if (boxCollider != null && boxCollider.enabled)
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }
}
