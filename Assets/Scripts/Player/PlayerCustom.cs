using Managers;
using Service;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCustom : MonoBehaviour
    {
        [SerializeField] GameObject Boy;
        [SerializeField] GameObject Girl;

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

        void Start()
        {
            ServiceLocator.GetService<MyInputManager>().UIInputs();
            
            Girl.transform.rotation = Quaternion.Euler(0f, 00f, 0f);
            Boy.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            ServiceLocator.GetService<MyDialogueManager>().NewOptionText("Elige un personaje", "", "Leo", "Violet", true);
        }

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            Debug.Log("Entro en monivimiento");
            var horizontalInput = context.ReadValue<Vector2>().x;
            
            Debug.Log(horizontalInput);

            switch (horizontalInput)
            {
                case > 0.1f:
                {
                    
                    Girl.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    Boy.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    break;
                }
                case < -0.1f:
                {
                    Girl.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    Boy.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    break;
                }
            }
        }
        
        private void OnSubmit(InputAction.CallbackContext context)
        {
            if ( ServiceLocator.GetService<MyLevelManager>().ActualDialogueResponse == 1)
                ServiceLocator.GetService<MyGameManager>().Character = "M";
            else 
                ServiceLocator.GetService<MyGameManager>().Character = "F";
            
            ServiceLocator.GetService<LoadScreenManager>().LoadScene("Story_0");
        }
    }
}
