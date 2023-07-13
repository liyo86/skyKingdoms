using Managers;
using UnityEngine;

namespace Service
{
    [DefaultExecutionOrder(-5)]
    public class ServiceLocatorInitializer : MonoBehaviour
    {
        private void Awake()
        {
            if (!ServiceLocator.IsInitialized)
            {
                AddServices();
                ServiceLocator.IsInitialized = true;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void AddServices()
        {
            var loadScreenManager = FindObjectOfType<LoadScreenManager>();
            ServiceLocator.AddService(loadScreenManager);
            DontDestroyOnLoad(loadScreenManager.gameObject);

            var myGameManager = FindObjectOfType<MyGameManager>();
            ServiceLocator.AddService(myGameManager);
            DontDestroyOnLoad(myGameManager.gameObject);

            var myInputManager = FindObjectOfType<MyInputManager>();
            ServiceLocator.AddService(myInputManager);
            DontDestroyOnLoad(myInputManager.gameObject);

            var myAudioManager = FindObjectOfType<MyAudioManager>();
            ServiceLocator.AddService(myAudioManager);
            DontDestroyOnLoad(myAudioManager.gameObject);

            var myLevelManager = FindObjectOfType<MyLevelManager>();
            ServiceLocator.AddService(myLevelManager);
            DontDestroyOnLoad(myLevelManager.gameObject);

            var myDialogueManager = FindObjectOfType<MyDialogueManager>();
            ServiceLocator.AddService(myDialogueManager);
            DontDestroyOnLoad(myDialogueManager.gameObject);
        }
    }
}