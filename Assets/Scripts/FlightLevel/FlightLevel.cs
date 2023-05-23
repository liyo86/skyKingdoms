using UnityEngine;

public class FlightLevel : MonoBehaviour
{
    public static FlightLevel Instance;

    public GameObject GemPurple;

    public GameObject ScoreCanvas;
    public GameObject ScoreCanvas3D;

    public LoadScreenManager sceneManager;
    
    [HideInInspector]
    public int ringDone = 0;
    
    #region FlightScene

    private float _secondsLeft;

    private bool gemVisible;
    
    private Vector3 gemPosition;
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _secondsLeft = 30f;
    }

    void Update()
    {
        StartFlightScene();
        
        if (ringDone == 10 && !gemVisible)
        {
            gemVisible = true;
            GameObject Gem = Instantiate(GemPurple, gemPosition, Quaternion.identity);
            Gem.transform.localScale = new Vector3(20f, 20f, 20f);
        }
    }
    
    public void SetGemPosition(Vector3 position)
    {
        gemPosition = position;
    }
    
    public void StartFlightScene()
    {
        if (_secondsLeft <= 0f)
        {
            _secondsLeft = 0f;
            PlayerUI.Instance.UpdateTimeLeft(0);
            ScoreCanvas.SetActive(false);
            ScoreCanvas3D.SetActive(false);
            MyGameManager.Instance.GameOver();
        }
        else
        {
            _secondsLeft -= Time.deltaTime;

            int secondsToShow = (int)_secondsLeft;
        
            PlayerUI.Instance.UpdateTimeLeft(secondsToShow);
        }
    }
    
    public void AddSecond()
    {
        _secondsLeft += 1f;
        int secondsToShow = (int)_secondsLeft;
        PlayerUI.Instance.UpdateTimeLeft(secondsToShow);
    }

    public void LevelComplete()
    {
        sceneManager.LoadScene();
    }
}
