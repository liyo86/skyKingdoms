using Service;
using UnityEngine.InputSystem;

namespace UI
{
    public class MenuUI : InputsUI
    {
        protected override void OnSubmit(InputAction.CallbackContext obj)
        {
            base.OnSubmit(obj);
        
            switch (ActualOption)
            {
                case 0:
                    ServiceLocator.GetService<LoadScreenManager>().LoadScene("ChooseCharacter");
                    break;
                case 1:
                    ServiceLocator.GetService<LoadScreenManager>().LoadScene("Controls");
                    break;
            }
        }
    }
}
