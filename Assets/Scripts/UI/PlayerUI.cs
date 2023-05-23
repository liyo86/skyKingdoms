using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance;

    [SerializeField]
    private TextMeshProUGUI ringScoreTxt;
    
    
    [SerializeField]
    private TextMeshProUGUI timeLeftTxt;

    //Ring Score
    public int ringScore;

    private int minutes = 0;
    private void Awake()
    {
        Instance = this;
    }

    public void ScoreRingPoints()
    {
        ringScore += 1;
        ringScoreTxt.text = ringScore.ToString();
    }

    public void UpdateTimeLeft(int timeLeft)
    {
        
        if (timeLeft > 60)
        {
            minutes += 1;
        }

        if (timeLeft > 10)
        {
            timeLeftTxt.text = minutes.ToString() + ":" + timeLeft.ToString();
        } else if (timeLeft < 10)
        {
            timeLeftTxt.text = minutes.ToString() + ":0" + timeLeft.ToString();
        }
            
    }
}
