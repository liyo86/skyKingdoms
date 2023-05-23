using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputsUI : MonoBehaviour
{
    [Header("Configuration")]
    public InputActionAsset inputActions;
    public LoadScreenManager sceneManager;
    public List<GameObject> selectList = new List<GameObject>();
    public GameObject menuCanvas;

    protected InputAction movementAction;
    protected InputAction submitAction;
    protected int actualOption = 0;
    protected int maxOptions;

    private void Awake()
    {
        movementAction = inputActions.FindActionMap("Player UI").FindAction("Direction");
        submitAction = inputActions.FindActionMap("Player UI").FindAction("Submit");
        
        maxOptions = selectList.Count;
    }

    private void OnEnable()
    {
        movementAction.performed += OnMovementPerformed;
        movementAction.Enable();

        submitAction.performed += OnSubmit;
        submitAction.Enable();
    }

    private void OnDisable()
    {
        movementAction.performed -= OnMovementPerformed;
        movementAction.Disable();

        submitAction.performed -= OnSubmit;
        submitAction.Disable();
    }

    protected virtual void OnMovementPerformed(InputAction.CallbackContext context)
    {
        float verticalInput = context.ReadValue<Vector2>().y;

        if (verticalInput > 0.1f)
        {
            actualOption--;
            if (actualOption < 0)
            {
                actualOption = maxOptions - 1;
            }
        }
        else if (verticalInput < -0.1f)
        {
            actualOption++;
            if (actualOption >= maxOptions)
            {
                actualOption = 0;
            }
        }

        ShowIcon(actualOption);
    }

    protected virtual void OnSubmit(InputAction.CallbackContext context)
    {
        menuCanvas.SetActive(false);
    }

    protected void ShowIcon(int option)
    {
        for (int i = 0; i < selectList.Count; i++)
        {
            selectList[i].SetActive(i == option);
        }
    }
}
