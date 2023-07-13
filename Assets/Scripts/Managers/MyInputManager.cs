using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class MyInputManager : MonoBehaviour
    {
        public InputActionAsset playerInput;
        private InputAction movementAction;
        private InputAction shootAction;
        private InputAction jumpAction;
        private InputAction defenseAction;
        private InputAction specialAttackAction;

        public InputAction uiMovementAction;
        public InputAction submitAction;
        public InputAction anyAction;

        public bool AnyBtnPressed;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (playerInput == null)
            {
                Debug.LogError("Player Input Asset is not assigned in MyInputManager.");
                return;
            }

            // PLAYER
            InputActionMap playerActionMap = playerInput.FindActionMap("Player");
            if (playerActionMap != null)
            {
                movementAction = playerActionMap.FindAction("Movement");
                shootAction = playerActionMap.FindAction("Shoot");
                jumpAction = playerActionMap.FindAction("Jump");
                defenseAction = playerActionMap.FindAction("Defense");
                specialAttackAction = playerActionMap.FindAction("Special Attack");
            }
            else
            {
                Debug.LogError("Player action map not found in Player Input Asset.");
            }

            // UI
            InputActionMap uiActionMap = playerInput.FindActionMap("Player UI");
            if (uiActionMap != null)
            {
                uiMovementAction = uiActionMap.FindAction("Direction");
                submitAction = uiActionMap.FindAction("Submit");
                anyAction = uiActionMap.FindAction("Any");
            }
            else
            {
                Debug.LogError("UI action map not found in Player Input Asset.");
            }
        }

        #region ENABLE / DISABLE


        public void PlayerInputs()
        {
            if (uiMovementAction != null)
                uiMovementAction.Disable();

            if (submitAction != null)
                submitAction.Disable();

            if (anyAction != null)
                anyAction.Disable();

            if (movementAction != null)
                movementAction.Enable();

            if (shootAction != null)
                shootAction.Enable();

            if (jumpAction != null)
                jumpAction.Enable();

            if (defenseAction != null)
                defenseAction.Enable();

            if (specialAttackAction != null)
                specialAttackAction.Enable();
        }

        public void UIInputs()
        {
            if (movementAction != null)
                movementAction.Disable();

            if (shootAction != null)
                shootAction.Disable();

            if (jumpAction != null)
                jumpAction.Disable();

            if (defenseAction != null)
                defenseAction.Disable();

            if (specialAttackAction != null)
                specialAttackAction.Disable();

            if (uiMovementAction != null)
                uiMovementAction.Enable();

            if (submitAction != null)
                submitAction.Enable();

            if (anyAction != null)
                anyAction.Enable();
        }
        #endregion

        #region PLAYER INPUTS
        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            if (BoyController.Instance != null)
                BoyController.Instance.SetMovementPerformed(context.ReadValue<Vector2>());
        }

        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            if (BoyController.Instance != null)
                BoyController.Instance.SetMovementPerformed(Vector3.zero);
        }

        private void OnShootPerformed(InputAction.CallbackContext context)
        {
            if (BoyController.Instance != null)
                BoyController.Instance.SetShootPermormed();
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            // Lógica para el salto
        }

        private void OnDefensePerformed(InputAction.CallbackContext context)
        {
            if (BoyController.Instance != null)
                BoyController.Instance.SetDefensePerformed();
        }

        private void OnSpecialAttackStarted(InputAction.CallbackContext context)
        {
            // Lógica para el inicio del ataque especial
        }

        private void OnSpecialAttackCanceled(InputAction.CallbackContext context)
        {
            // Lógica para la cancelación del ataque especial
        }
        #endregion

        #region UI INPUTS
     
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
}
