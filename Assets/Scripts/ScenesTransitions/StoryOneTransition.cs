using Managers;
using UI;
using Unity.VisualScripting;
using UnityEngine;

public class StoryOneTransition : MonoBehaviour
{
    public static StoryOneTransition Instance;
    private bool textShowed;
    public DialogueOptions dialogueOptions;
    public bool CanCheck;

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(Constants.PLAYER) || textShowed || !CanCheck) return;
        textShowed = true;
        MyDialogueManager.Instance.NewOptionText(Text_Story_1.OptionText, Constants.DRAGON);
        dialogueOptions.ShowOptions();
    }
    
    

    private void OnTriggerExit(Collider other)
    {
        textShowed = false;
    }

}
