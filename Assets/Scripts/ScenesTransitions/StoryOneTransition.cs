using System;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryOneTransition : MonoBehaviour
{
    public static StoryOneTransition Instance;
    private bool textShowed;
    private bool CanCheck;

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (gameObject.name)
        {
            case "Story_1Transition":
                MyLevelManager.Instance.backToScene = true;
                SceneManager.LoadScene("Story_1");
                break;
            case "DragonTransition":
                if (!other.CompareTag(Constants.PLAYER) || textShowed || !CanCheck) return;
                textShowed = true;
    
                MyDialogueManager.Instance.NewOptionText(Text_Story_1.OptionText, Constants.DRAGON, "", "", true);
                break;
        }
    }
    
    

    private void OnTriggerExit(Collider other)
    {
        textShowed = false;
    }

    public void CanCheckDialogueOptions()
    {
        CanCheck = true;
    }
}
