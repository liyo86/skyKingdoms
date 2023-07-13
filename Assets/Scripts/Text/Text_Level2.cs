public class Text_Level2
{
    public static int GetMaxStep() => 1;

    public static string Text(int step)
    {
        string text = "";

        switch (step)
        {
            case 1:
                text = "MISION*Encuentra la gema sin ser alcanzado por el fantasma.";
                break;
        }
        
        return text.ToUpper();
    }
}
