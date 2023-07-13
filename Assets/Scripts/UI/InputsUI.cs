using System;
using System.Collections.Generic;
using Managers;
using Service;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace UI
{
    public class InputsUI : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> optionList = new List<GameObject>();

        private int _maxOptions;
        
        protected int ActualOption = 0;
        
        private void OnEnable()
        {
            ServiceLocator.GetService<MyInputManager>().uiMovementAction.performed += OnMovementPerformed;
            ServiceLocator.GetService<MyInputManager>().submitAction.performed += OnSubmit;
            ServiceLocator.GetService<MyInputManager>().anyAction.performed += OnAny;
        }

        private void OnDisable()
        {
            ServiceLocator.GetService<MyInputManager>().uiMovementAction.performed -= OnMovementPerformed;
            ServiceLocator.GetService<MyInputManager>().submitAction.performed -= OnSubmit;
            ServiceLocator.GetService<MyInputManager>().anyAction.performed -= OnAny;
        }

        private void Awake()
        {
            ServiceLocator.GetService<MyInputManager>().UIInputs();
        }

        private void Start()
        {
            _maxOptions = optionList.Count;
        }

        private void OnMovementPerformed(InputAction.CallbackContext obj)
        {
            float verticalInput = obj.ReadValue<Vector2>().y;
            float horizontalInput = obj.ReadValue<Vector2>().x;
        
            if (verticalInput > 0.1f || horizontalInput > 0.1f)
            {
                ActualOption--;
                if (ActualOption < 0)
                {
                    ActualOption = _maxOptions - 1;
                }
            }
            else if (verticalInput < -0.1f || horizontalInput < 0.1f)
            {
                ActualOption++;
                if (ActualOption >= _maxOptions)
                {
                    ActualOption = 0;
                }
            }

            ShowIcon(ActualOption);
        }
        
        protected virtual void OnSubmit(InputAction.CallbackContext context)
        {
       
        }
        
        private void ShowIcon(int option)
        {
            for (int i = 0; i < optionList.Count; i++)
            {
                optionList[i].SetActive(i == option);
            }
        }
    
        protected virtual void OnAny(InputAction.CallbackContext context)
        {
          
        }
    
    }
}
