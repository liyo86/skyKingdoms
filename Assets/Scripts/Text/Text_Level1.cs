using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_Level1
{
    public static int GetMaxStep() => 2;

    public static string Text(int step)
    {
        string text = "";

        switch (step)
        {
            case 1:
                text = "Dragon: Aun no podemos emprender el viaje...";
                break;
            case 2:
                text = "Dragon: Encuentra la gema primero.";
                break;
        }
        
        return text.ToUpper();
    }
}
