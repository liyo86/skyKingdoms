using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragonControllerPlayer : MonoBehaviour
{
    public float velocidad;
    public float impulsoVelocidad;
    public float inclinacionMaxima;

    public GameObject FlameThrower;
    public Transform FirePoint;
    public Transform RotationPoint;

    private float inputVertical = 0.0f;
    private bool boostActivated = false;

    // New Input System
    private InputAction movementActionDragon;
    private InputAction shootActionDragon;
    private InputAction impulseActionDragon;
    public InputActionAsset inputActions;

    private void Awake()
    {
        movementActionDragon = inputActions.FindActionMap("Dragon").FindAction("Movement");
        shootActionDragon = inputActions.FindActionMap("Dragon").FindAction("Shoot");
        impulseActionDragon = inputActions.FindActionMap("Dragon").FindAction("Boost");
    }

    private void OnEnable()
    {
        movementActionDragon.performed += OnMovementPerformed;
        movementActionDragon.Enable();

        shootActionDragon.performed += ShootFire;
        shootActionDragon.Enable();

        impulseActionDragon.started += OnBoostStarted;
        impulseActionDragon.Enable();
    }

    private void OnDisable()
    {
        movementActionDragon.performed -= OnMovementPerformed;
        movementActionDragon.Disable();

        shootActionDragon.performed -= ShootFire;
        shootActionDragon.Disable();

        impulseActionDragon.started -= OnBoostStarted;
        impulseActionDragon.Disable();
    }

    private void Update()
    {
        // Calcular la velocidad actualizada
        float currentSpeed = boostActivated ? velocidad + impulsoVelocidad : velocidad;

        // Calcular el movimiento horizontal
        float horizontalMove = currentSpeed * Time.deltaTime;
        Vector3 horizontalMovement = new Vector3(horizontalMove, 0.0f, 0.0f);
        transform.position += horizontalMovement;

        // Calcular el movimiento vertical según el eje Y
        float verticalMove = inputVertical * currentSpeed * Time.deltaTime;
        Vector3 verticalMovement = new Vector3(0.0f, verticalMove, 0.0f);
        transform.position += verticalMovement;

        // Limitar la velocidad vertical
        float currentYVelocity = Mathf.Clamp(verticalMove, -currentSpeed, currentSpeed);
        transform.position = new Vector3(transform.position.x, transform.position.y + currentYVelocity, transform.position.z);

        // Calcular la inclinación del dragón hacia arriba o hacia abajo
        float targetInclination = -inputVertical * inclinacionMaxima;
        Quaternion targetRotation = Quaternion.Euler(targetInclination, 90f, 0.0f);
        RotationPoint.localRotation = Quaternion.Lerp(RotationPoint.localRotation, targetRotation, Time.deltaTime * 10f);
    }

    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        inputVertical = context.ReadValue<Vector2>().y;
    }

    private void ShootFire(InputAction.CallbackContext context)
    {
        Instantiate(FlameThrower, FirePoint);
    }

    private void OnBoostStarted(InputAction.CallbackContext context)
    {
        StartCoroutine(BoostCoroutine());
    }

    private IEnumerator BoostCoroutine()
    {
        boostActivated = true;

        yield return new WaitForSeconds(1.0f);

        boostActivated = false;
    }
}
