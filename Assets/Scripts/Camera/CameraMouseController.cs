using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMouseController : MonoBehaviour
{
    private CinemachineFreeLook freeLookCamera;
    private Vector2 rotationInput;
    
    private InputAction rotateAction;
    public InputActionAsset inputActions;
    
    public float smoothTime = 0.2f;  // Duración del suavizado
    private float smoothVelocityX;  // Velocidad suavizada en el eje X
    private float smoothVelocityY;  // Velocidad suavizada en el eje Y


    private void Awake()
    {
        rotateAction = inputActions.FindActionMap("Player").FindAction("Camera Mouse");
        freeLookCamera = GetComponent<CinemachineFreeLook>();
    }

    private void OnEnable()
    {
        rotateAction.performed += OnRotate;
        rotateAction.Enable();
    }

    private void OnDisable()
    {
        rotateAction.performed -= OnRotate;
        rotateAction.Disable();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        rotationInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        float mouseX = rotationInput.x;
        float mouseY = rotationInput.y;

        // Aplicar la interpolación suave
        float smoothX = Mathf.SmoothDamp(freeLookCamera.m_XAxis.Value, freeLookCamera.m_XAxis.Value + mouseX, ref smoothVelocityX, smoothTime);
        float smoothY = Mathf.SmoothDamp(freeLookCamera.m_YAxis.Value, freeLookCamera.m_YAxis.Value + mouseY, ref smoothVelocityY, smoothTime);

        freeLookCamera.m_XAxis.Value = smoothX;
        freeLookCamera.m_YAxis.Value = smoothY;
    }

}
