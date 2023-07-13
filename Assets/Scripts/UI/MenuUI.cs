using System.Collections.Generic;
using Managers;
using Service;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuUI : MonoBehaviour
{
    [Header("Configuration")]
    public List<GameObject> selectList = new List<GameObject>();
    public GameObject menuCanvas;
    private int _actualOption = 0;
    private int _maxOptions;
    
    private void OnEnable()
    {
        ServiceLocator.GetService<MyInputManager>().uiMovementAction.performed += OnMovementPerformed;
        ServiceLocator.GetService<MyInputManager>().submitAction.performed += OnSubmit;
    }

    private void OnDisable()
    {
        ServiceLocator.GetService<MyInputManager>().uiMovementAction.performed -= OnMovementPerformed;
        ServiceLocator.GetService<MyInputManager>().submitAction.performed -= OnSubmit;
    }

    private void OnSubmit(InputAction.CallbackContext obj)
    {
        switch (_actualOption)
        {
            case 0:
                menuCanvas.SetActive(false);
                ServiceLocator.GetService<LoadScreenManager>().LoadScene("ChooseCharacter");
                break;
            case 1:
                menuCanvas.SetActive(false);
                ServiceLocator.GetService<LoadScreenManager>().LoadScene("Controls");
                break;
        }
    }

    private void OnMovementPerformed(InputAction.CallbackContext obj)
    {
        float verticalInput = obj.ReadValue<Vector2>().y;
        
        if (verticalInput > 0.1f)
        {
            _actualOption--;
            if (_actualOption < 0)
            {
                _actualOption = _maxOptions - 1;
            }
        }
        else if (verticalInput < -0.1f)
        {
            _actualOption++;
            if (_actualOption >= _maxOptions)
            {
                _actualOption = 0;
            }
        }

        ShowIcon(_actualOption);
    }

    private void Start()
    {
        _maxOptions = selectList.Count;
        ServiceLocator.GetService<MyInputManager>().UIInputs();
    }
    
    private void ShowIcon(int option)
    {
        for (int i = 0; i < selectList.Count; i++)
        {
            selectList[i].SetActive(i == option);
        }
    }
}
