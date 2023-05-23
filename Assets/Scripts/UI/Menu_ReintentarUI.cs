using System.Collections;
using UnityEngine;
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
                StartCoroutine(nameof(WaitUntilRestart));
                break;
            case 1:
                sceneManager.LoadScene("Menu_game");
                break;
        }
    }

    private IEnumerator WaitUntilRestart()
    {
        MyGameManager.Instance.RestartLevel();
        
        yield return new WaitForSeconds(1f);
        
        menuCanvas.SetActive(false);
            
        sceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
