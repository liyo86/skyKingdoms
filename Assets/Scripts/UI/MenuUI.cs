using UnityEngine.InputSystem;

public class MenuUI : InputsUI
{
    protected override void OnSubmit(InputAction.CallbackContext context)
    {
        base.OnSubmit(context);

        switch (actualOption)
        {
            case 0:
                menuCanvas.SetActive(false);
                sceneManager.LoadScene("Story_0");
                break;
            case 1:
                menuCanvas.SetActive(false);
                sceneManager.LoadScene("Controls");
                break;
        }
    }
}
