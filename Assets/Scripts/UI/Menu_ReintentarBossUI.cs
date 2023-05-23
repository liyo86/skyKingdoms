using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menu_ReintentarBossUI : InputsUI
{
    protected override void OnSubmit(InputAction.CallbackContext context)
    {
        base.OnSubmit(context);

        switch (actualOption)
        {
            case 0:
                menuCanvas.SetActive(false);
                PlayerHealth.Instance.Reset();
                sceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case 1:
                menuCanvas.SetActive(false);
                sceneManager.LoadScene("Menu_game");
                break;
        }
    }
}
