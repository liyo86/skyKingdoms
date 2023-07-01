using Managers;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCustom : MonoBehaviour
    {
        [SerializeField] GameObject Boy;
        [SerializeField] GameObject Girl;
        [SerializeField] private DialogueOptions dialogueOptions;
        [SerializeField] private TextMeshProUGUI optionA_Text;
        [SerializeField] private TextMeshProUGUI optionB_Text;
        
        private void OnEnable()
        {
            MyInputManager.Instance.uiMovementAction.performed += OnMovementPerformed;
        }

        private void OnDisable()
        {
            MyInputManager.Instance.uiMovementAction.performed -= OnMovementPerformed;
        }

        void Start()
        {
            Girl.transform.rotation = Quaternion.Euler(0f, 00f, 0f);
            Boy.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            MyDialogueManager.Instance.NewOptionText("Elige un personaje", "");
            optionA_Text.text = "Leo";
            optionB_Text.text = "Elle";
            dialogueOptions.ShowOptions();
        }

        public void ChooseCharacter()
        {
            
        }
        
        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            var horizontalInput = context.ReadValue<Vector2>().x;

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
    }
}
