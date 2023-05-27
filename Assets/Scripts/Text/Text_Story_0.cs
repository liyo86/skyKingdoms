using System;
using UnityEngine;

public static class Text_Story_0
{
    public static int GetMaxStep() => 7;

    public static string Text(int step)
    {
        string text = "";

        switch (step)
        {
            case 1:
                text = "GOBLIN:\nSi quieres un trabajo bien hecho, debes hacerlo tu mismo.";
                break;
            case 2:
                text = "¿?:\n¡Alto!";
                break;
            case 3:
                text = "PRINCESA:\nSera mejor que no toques esa gema.";
                break;
            case 4:
                text = "GOBLIN:\nPero si la princesita ha venido al rescate.";
                break;
            case 5:
                text = "GOBLIN:\nJa, ja, ja... .";
                break;
            case 6:
                text = "PRINCESA:\n¡No Sueltame! ¡Socorrooo!";
                break;
        }
        
        return text.ToUpper();
    }
}

