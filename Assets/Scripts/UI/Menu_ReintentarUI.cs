using Managers;
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
                RestartLevel();
                break;
            case 1:
                menuCanvas.SetActive(false);
                SceneManager.LoadScene("Menu_game");
                break;
        }
    }

    private void RestartLevel()
    {
        MyGameManager.Instance.RestartLevel();   
        Invoke(nameof(RestartScene), 1f);
    }

    private void RestartScene()
    {
        menuCanvas.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
