using Managers;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyInputManager : MonoBehaviour
{
    public static MyInputManager Instance;

    public InputActionAsset playerInput;
    private InputAction movementAction;
    private InputAction shootAction;
    private InputAction jumpAction;
    private InputAction defenseAction;
    private InputAction specialAttackAction;

    public InputAction uiMovementAction;
    public InputAction submitAction;
    public InputAction anyAction;

    public bool UIControls;
    public bool PlayerControls;
    
    public bool AnyBtnPressed;
   
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        Initialize();
    }

    private void Start()
    {
        UIInputs();
    }

    private void Initialize()
    {
        // PLAYER
        InputActionMap playerActionMap = playerInput.FindActionMap("Player");
        movementAction = playerActionMap.FindAction("Movement");
        shootAction = playerActionMap.FindAction("Shoot");
        jumpAction = playerActionMap.FindAction("Jump");
        defenseAction = playerActionMap.FindAction("Defense");
        specialAttackAction = playerActionMap.FindAction("Special Attack");
        
        InputActionMap uiActionMap = playerInput.FindActionMap("Player UI");
        uiMovementAction = uiActionMap.FindAction("Direction");
        submitAction = uiActionMap.FindAction("Submit");
        anyAction = uiActionMap.FindAction("Any");
    }

    #region ENABLE / DISABLE
    private void OnEnable()
    {
        movementAction.performed += OnMovementPerformed;
        movementAction.canceled += OnMovementCanceled;
        movementAction.Enable();
        
        shootAction.performed += OnShootPerformed;
        shootAction.Enable();
        
        jumpAction.performed += OnJumpPerformed;
        jumpAction.Enable();
        
        defenseAction.performed += OnDefensePerformed;
        defenseAction.Enable();
        
        specialAttackAction.started += OnSpecialAttackStarted;
        specialAttackAction.canceled += OnSpecialAttackCanceled;
        specialAttackAction.Enable();

        // UI
        uiMovementAction.performed += OnUIMovementPerformed;
        uiMovementAction.Enable();

        submitAction.performed += OnSubmit;
        submitAction.Enable();

        anyAction.performed += OnAny;
        anyAction.canceled += OnAnyCanceled;
        anyAction.Enable();
    }
    
    private void OnDisable()
    {
        movementAction.performed -= OnMovementPerformed;
        movementAction.canceled -= OnMovementCanceled;
        movementAction.Disable();
        
        shootAction.performed -= OnShootPerformed;
        shootAction.Disable();
        
        jumpAction.performed -= OnJumpPerformed;
        jumpAction.Disable();
        
        defenseAction.performed -= OnDefensePerformed;
        defenseAction.Disable();
        
        specialAttackAction.started -= OnSpecialAttackStarted;
        specialAttackAction.canceled -= OnSpecialAttackCanceled;
        specialAttackAction.Disable();

        uiMovementAction.performed -= OnUIMovementPerformed;
        uiMovementAction.Disable();

        submitAction.performed -= OnSubmit;
        submitAction.Disable();

        anyAction.performed -= OnAny;
        anyAction.canceled -= OnAnyCanceled;
        anyAction.Disable();
    }
    
    public void PlayerInputs()
    {
        uiMovementAction.Disable();
        submitAction.Disable();
        anyAction.Disable();
        
        movementAction.Enable();
        shootAction.Enable();
        jumpAction.Enable();
        defenseAction.Enable();
        specialAttackAction.Enable();
    }

    public void UIInputs()
    {
        movementAction.Disable();
        shootAction.Disable();
        jumpAction.Disable();
        defenseAction.Disable();
        specialAttackAction.Disable();
        
        uiMovementAction.Enable();
        submitAction.Enable();
        anyAction.Enable();
    }
    #endregion
    
    #region PLAYER INPUTS
    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        BoyController.Instance.SetMovementPerformed(context.ReadValue<Vector2>());
    }

    private void OnMovementCanceled(InputAction.CallbackContext context)
    {
        BoyController.Instance.SetMovementPerformed(Vector3.zero); 
    }

    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        BoyController.Instance.SetShootPermormed();
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        
    }

    private void OnDefensePerformed(InputAction.CallbackContext context)
    {
        BoyController.Instance.SetDefensePerformed();
    }

    private void OnSpecialAttackStarted(InputAction.CallbackContext context)
    {
        
    }

    private void OnSpecialAttackCanceled(InputAction.CallbackContext context)
    {
        
    }
    #endregion

    #region UI INPUTS
    private void OnUIMovementPerformed(InputAction.CallbackContext context)
    {
        // Lógica para el movimiento de la interfaz de usuario
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
       MyDialogueManager.Instance.OnBtnSubmit();
    }

    private void OnAny(InputAction.CallbackContext context)
    {
        AnyBtnPressed = true;
    }
    
    private void OnAnyCanceled(InputAction.CallbackContext context)
    {
        AnyBtnPressed = false;
    }
    #endregion
}
