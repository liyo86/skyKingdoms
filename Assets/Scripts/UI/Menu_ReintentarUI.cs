using Managers;
using Service;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI
{
    public class Menu_ReintentarUI : InputsUI
    {
        protected override void OnSubmit(InputAction.CallbackContext context)
        {
            base.OnSubmit(context);

            switch (ActualOption)
            {
                case 0:
                    RestartLevel();
                    break;
                case 1:
                    ServiceLocator.GetService<LoadScreenManager>().LoadScene("Menu_game");
                    break;
            }
        }
    
        private void RestartLevel()
        {
            ServiceLocator.GetService<MyGameManager>().RestartLevel();
            Invoke(nameof(RestartScene), 1f);
        }

        private void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
