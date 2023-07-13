using Service;
using UnityEngine.InputSystem;

namespace UI
{
    public class ControlsUI : InputsUI
    {
        protected override void OnAny(InputAction.CallbackContext context)
        {
            base.OnAny(context);
            ServiceLocator.GetService<LoadScreenManager>().LoadScene("Menu_game");
        }
    }
}
