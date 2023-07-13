public static class Text_Story_0
{
    public static int GetMaxStep() => 7;

    public static string Text(int step)
    {
        string text = "";

        switch (step)
        {
            case 1:
                text = "MORTEM*Si quieres un trabajo bien hecho, debes hacerlo tu mismo.";
                break;
            case 2:
                text = "?*¡Alto!";
                break;
            case 3:
                text = "AMELIA*Sera mejor que no toques esa gema.";
                break;
            case 4:
                text = "MORTEM*Pero si la princesita ha venido al rescate.";
                break;
            case 5:
                text = "MORTEM*Ja, ja, ja... .";
                break;
            case 6:
                text = "AMELIA*¡No Sueltame! ¡Socorrooo!";
                break;
        }
        
        return text.ToUpper();
    }
}

