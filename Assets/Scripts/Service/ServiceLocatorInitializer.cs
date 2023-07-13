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

            var myGameManager = FindObjectOfType<MyGameManager>();
            ServiceLocator.AddService(myGameManager);

            var myInputManager = FindObjectOfType<MyInputManager>();
            ServiceLocator.AddService(myInputManager);

            var myAudioManager = FindObjectOfType<MyAudioManager>();
            ServiceLocator.AddService(myAudioManager);

            var myLevelManager = FindObjectOfType<MyLevelManager>();
            ServiceLocator.AddService(myLevelManager);

            var myDialogueManager = FindObjectOfType<MyDialogueManager>();
            ServiceLocator.AddService(myDialogueManager);
        }
    }
}