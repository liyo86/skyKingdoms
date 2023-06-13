using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class DialogueOptions : MonoBehaviour
    {
        [Header("Configuration")]
        public InputActionAsset inputActions;
        public List<GameObject> selectList = new List<GameObject>();

        public InputAction movementAction;
        public InputAction submitAction;
        private int actualOption = 0;
        private int maxOptions;
    
        public GameObject Options;

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

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            var verticalInput = context.ReadValue<Vector2>().x;

            switch (verticalInput)
            {
                case > 0.1f:
                {
                    actualOption--;
                    if (actualOption < 0)
                    {
                        actualOption = maxOptions - 1;
                    }

                    break;
                }
                case < -0.1f:
                {
                    actualOption++;
                    if (actualOption >= maxOptions)
                    {
                        actualOption = 0;
                    }

                    break;
                }
            }

            ShowIcon(actualOption);
        }

        private void OnSubmit(InputAction.CallbackContext context)
        {
            switch (actualOption)
            {
                case 0:
                    Options.SetActive(false);
                    MyDialogueManager.Instance.HideDialogBox();
                    MyLevelManager.Instance.DialogOptionResponse();
                    break;
                case 1:
                    Options.SetActive(false);
                    MyDialogueManager.Instance.HideDialogBox();
                    break;
            }
        }

        private void ShowIcon(int option)
        {
            for (var i = 0; i < selectList.Count; i++)
            {
                selectList[i].SetActive(i == option);
            }
        }

        public void ShowOptions()
        {
            Options.SetActive(true);
        }
    }
}
