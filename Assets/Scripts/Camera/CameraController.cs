using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineFreeLook freeLookCamera;

    [Tooltip("La velocidad de rotación de la cámara.")]
    [SerializeField]
    private float rotationSpeed = 2.0f;

    [Tooltip("La referencia al Input Action Asset.")]
    [SerializeField]
    private InputActionAsset inputActions;

    [Tooltip("La acción para controlar la rotación de la cámara.")]
    [SerializeField]
    private InputAction cameraRotationAction;

    private void Awake()
    {
        cameraRotationAction = inputActions.FindActionMap("Player").FindAction("Camera");
        freeLookCamera = GetComponent<CinemachineFreeLook>();
    }

    private void OnEnable()
    {
        cameraRotationAction.performed += OnCameraRotationPerformed;
        cameraRotationAction.Enable();
    }

    private void OnDisable()
    {
        cameraRotationAction.performed -= OnCameraRotationPerformed;
        cameraRotationAction.Disable();
    }

    private void OnCameraRotationPerformed(InputAction.CallbackContext context)
    {
        Vector2 rawRotation = context.ReadValue<Vector2>();

        // Actualiza la rotación de la cámara en función de las entradas del ratón y del mando
        freeLookCamera.m_XAxis.m_InputAxisValue += rawRotation.x * rotationSpeed;
        freeLookCamera.m_YAxis.m_InputAxisValue -= rawRotation.y * rotationSpeed;
    }
}