using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDragonFollow : MonoBehaviour
{
    public Transform target; // Referencia al transform del dragón
    public float smoothSpeed = 0.125f; // Velocidad de suavizado de movimiento de la cámara
    private Vector3 offset; // Desplazamiento entre la cámara y el dragón

    private void Start()
    {
        offset = new Vector3(0f, 0f, -25f);
        transform.rotation = Quaternion.Euler(0f, 3f, 0f);
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
