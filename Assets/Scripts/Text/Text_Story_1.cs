public static class Text_Story_1
{
    public static int GetMaxStep() => 14;
    public static string OptionText = "¿Estás seguro?";

    public static string Text(int step)
    {
        string text = step switch
        {
            1 => "LEO*Algo está sucediendo en el castillo...",
            2 => "LEO*Ese es Vrazabard, el dragón de la princesa.",
            3 => "VRAZABARD*Leo, tenemos un problema serio.",
            4 => "LEO*¿Qué ocurre, Vrazabard? Parece que algo no va bien.",
            5 => "VRAZABARD*Un goblin malvado llamado Mortem ha irrumpido en el castillo y ha robado la valiosa gema verde.",
            6 => "LEO*¡No puedo creerlo! Debemos detenerlo y rescatar a la princesa.",
            7 => "VRAZABARD*Exacto, Leo. Desconocemos sus intenciones y el peligro que representa.",
            8 => "LEO*Si Mortem llega a obtener todas las gemas... nuestro Reino estará en grave peligro.",
            9 => "VRAZABARD*Se rumorea que ha establecido su guarida en las Montañas Sombrías, un lugar peligroso lleno de criaturas temibles.",
            10 => "LEO*Entonces, ese será nuestro próximo destino. Nos enfrentaremos a esta aventura juntos.",
            11 => "VRAZABARD*Pero antes, debemos encontrar las otras tres gemas de poder. Mortem ya se ha adelantado en recuperarlas.",
            12 => "LEO*¡Está un paso adelante! Con la gema verde y las demás en su posesión, ¿qué planea hacer?",
            13 => "VRAZABARD*No importa. No permitiremos que logre sus objetivos. Nuestro Reino necesita héroes como nosotros.",
            14 => "VRAZABARD*Ven a verme cuando estés preparado, Leo. Juntos, haremos historia.",
            _ => ""
        };

        return text.ToUpper();
    }
}