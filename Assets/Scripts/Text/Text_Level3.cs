public class Text_Level3
{
    public static int GetMaxStep() => 1;

    public static string Text(int step)
    {
        string text = "";

        switch (step)
        {
            case 1:
                text = "MISION*Derrota a la flor para conseguir la gema.";
                break;
        }
        
        return text.ToUpper();
    }
}
