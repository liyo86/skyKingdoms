using System.Collections.Generic;
using Doublsb.Dialog;
using Managers;
using UnityEngine;

public class StoryOneTransition : MonoBehaviour
{
    public DialogManager DialogManager;
    private bool textShowed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.PLAYER) && !textShowed)
        {
            textShowed = true;
            var dialogTexts = new List<DialogData>();
            var Text1 = new DialogData("¿Estás preparado?");
            Text1.SelectList.Add("Yes", "Sí");
            Text1.SelectList.Add("No", "No");

            Text1.Callback = () => Check_Correct();

            dialogTexts.Add(Text1);

            DialogManager.Show(dialogTexts);
         
        }
    }
    
    private void Check_Correct()
    {
        if(DialogManager.Result == "Yes")
        {
            var dialogTexts = new List<DialogData>();

            dialogTexts.Add(new DialogData("Nos vamos"));

            DialogManager.Show(dialogTexts);
        }
        else if (DialogManager.Result == "No")
        {
            var dialogTexts = new List<DialogData>();

            dialogTexts.Add(new DialogData("Date prisa"));

            DialogManager.Show(dialogTexts);
        }
    }
}
