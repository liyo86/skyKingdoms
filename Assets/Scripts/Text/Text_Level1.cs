public class Text_Level1
{
    public static int GetMaxStep() => 1;

    public static string Text(int step)
    {
        string text = "";

        switch (step)
        {
            case 1:
                text = "Derrota a todas las flroes para conseguir la gema.";
                break;
        }
        
        return text.ToUpper();
    }
}
