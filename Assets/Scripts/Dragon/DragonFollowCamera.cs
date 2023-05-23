using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFollowCamera : MonoBehaviour
{
    private Transform target;
    private float smoothSpeed = 0.125f;
    private Vector3 offset;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("DragonCameraTarget").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
