using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragonController : MonoBehaviour
{
    public float velocidad;
    public float impulsoVelocidad;
    public float inclinacionMaxima;
    public float alabeoMaximo;

    public GameObject FlameThrower;
    public Transform FirePoint;
    public Transform RotationPoint;

    private float inputVertical = 0.0f;
    private float inputHorizontal = 0.0f;

    [HideInInspector]
    public float limiteHorizontal;

    [HideInInspector]
    public float limiteVertical;

    private float flightStartX = 0f;
    private float flightStartY = 10f;
    private float flightStartZ = -100f;

    private bool boostActivated = false;
    
    // New Input System
    private InputAction movementActionDragon;
    private InputAction shootActionDragon;
    private InputAction impulseActionDragon;
    public InputActionAsset inputActions;

    public static DragonController Instance;

    private void Awake()
    {
        Instance = this;
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

        impulseActionDragon.performed += OnBoost;
        impulseActionDragon.Enable();
    }

    private void OnDisable()
    {
        movementActionDragon.performed -= OnMovementPerformed;
        movementActionDragon.Disable();
        
        shootActionDragon.performed -= ShootFire;
        shootActionDragon.Disable();
        
        impulseActionDragon.performed -= OnBoost;
        impulseActionDragon.Disable();
    }

    private void Start()
    {
        transform.position = new Vector3(flightStartX, flightStartY, flightStartZ);
    }

    void Update()
    {
        Controls();
    }

    private void Controls()
    {
        float alabeo = Mathf.Clamp(inputHorizontal * alabeoMaximo, -alabeoMaximo, alabeoMaximo);

        float inclinacion;

        inclinacion = Mathf.Clamp(inputVertical * inclinacionMaxima, -inclinacionMaxima, inclinacionMaxima);

        Quaternion targetRotation = Quaternion.Euler(inclinacion * -1, transform.rotation.eulerAngles.y + (alabeo / 10), alabeo * -1);
        
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

        if (boostActivated)
        {
            Quaternion zRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + (760f * Time.deltaTime));
            RotationPoint.transform.rotation = Quaternion.Lerp(transform.rotation, zRotation, Time.deltaTime * 20f);
        }

        float currentSpeed = boostActivated ? velocidad + impulsoVelocidad : velocidad;
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        velocidad = Mathf.Clamp(velocidad, 0.0f, 20f);
    }


    
    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        inputVertical = context.ReadValue<Vector2>().y;
        inputHorizontal = context.ReadValue<Vector2>().x;
    }

    private void OnBoost(InputAction.CallbackContext context)
    {
        StartCoroutine(BoostCoroutine());
    }

    private IEnumerator BoostCoroutine()
    {
        boostActivated = true;

        yield return new WaitForSeconds(1.0f);
        
        boostActivated = false;
    }

    private void ShootFire(InputAction.CallbackContext context)
    {
        Instantiate(FlameThrower, FirePoint);
    }
    
}
