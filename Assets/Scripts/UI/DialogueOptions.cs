using System.Collections.Generic;
using Managers;
using Service;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class DialogueOptions : MonoBehaviour
    {
        [Header("Configuration")]
        public List<GameObject> selectList = new List<GameObject>();
        
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
            ServiceLocator.GetService<MyInputManager>().uiMovementAction.performed += OnMovementPerformed;
            ServiceLocator.GetService<MyInputManager>().submitAction.performed += OnSubmit;
        }

        private void OnDisable()
        {
            ServiceLocator.GetService<MyInputManager>().uiMovementAction.performed -= OnMovementPerformed;
            ServiceLocator.GetService<MyInputManager>().submitAction.performed -= OnSubmit;
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
            Options.SetActive(false);
            ServiceLocator.GetService<MyDialogueManager>().HideDialogBox();
            ServiceLocator.GetService<MyLevelManager>().DialogOptionResponse(actualOption);
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
