using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputsUI : MonoBehaviour
{
    [Header("Configuration")]
    public List<GameObject> selectList = new List<GameObject>();
    public GameObject menuCanvas;
    private int maxOptions;
    protected int actualOption = 0;

    private void Awake()
    {
        maxOptions = selectList.Count;
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
       
    }
    
    protected virtual void OnAny(InputAction.CallbackContext context)
    {
       
    }

    protected void ShowIcon(int option)
    {
        for (int i = 0; i < selectList.Count; i++)
        {
            selectList[i].SetActive(i == option);
        }
    }
}
