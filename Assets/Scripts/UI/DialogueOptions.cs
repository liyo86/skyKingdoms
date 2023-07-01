using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class DialogueOptions : MonoBehaviour
    {
        [Header("Configuration")]
        //public InputActionAsset inputActions;
        public List<GameObject> selectList = new List<GameObject>();

        public InputAction movementAction;
        public InputAction submitAction;
        private int actualOption = 0;
        private int maxOptions;
    
        public GameObject Options;
        public GameObject OptionsCharacter;

        private void Awake()
        {
            maxOptions = selectList.Count;
        }

        private void OnEnable()
        {
            MyInputManager.Instance.uiMovementAction.performed += OnMovementPerformed;
            MyInputManager.Instance.submitAction.performed += OnSubmit;
        }

        private void OnDisable()
        {
            MyInputManager.Instance.uiMovementAction.performed -= OnMovementPerformed;
            MyInputManager.Instance.submitAction.performed -= OnSubmit;
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
                    MyLevelManager.Instance.DialogOptionResponse(actualOption);
                    break;
                case 1:
                    Options.SetActive(false);
                    MyDialogueManager.Instance.HideDialogBox();
                    MyLevelManager.Instance.DialogOptionResponse(actualOption);
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

        public void ShowOptions(bool showCharacter = false)
        {
            Options.SetActive(true);
            OptionsCharacter.SetActive(showCharacter);
        }
    }
}
