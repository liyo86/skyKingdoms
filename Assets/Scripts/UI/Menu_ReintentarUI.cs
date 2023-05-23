using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menu_ReintentarUI : InputsUI
{
    protected override void OnSubmit(InputAction.CallbackContext context)
    {
        base.OnSubmit(context);

        switch (actualOption)
        {
            case 0:
                sceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case 1:
                sceneManager.LoadScene("Menu_game");
                break;
        }
    }
}
