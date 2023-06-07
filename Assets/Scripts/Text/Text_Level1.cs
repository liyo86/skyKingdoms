public class Text_Level1
{
    public static int GetMaxStep() => 1;

    public static string Text(int step)
    {
        string text = "";

        switch (step)
        {
            case 1:
                text = "MISION*Derrota a todas las flores para conseguir la gema.";
                break;
        }
        
        return text.ToUpper();
    }
}
