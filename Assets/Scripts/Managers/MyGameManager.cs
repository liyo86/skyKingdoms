using UnityEngine;
using UnityEngine.SceneManagement;

public class MyGameManager : MonoBehaviour
{
    public static MyGameManager Instance;

    public enum CollectibleTypes { NoType, GemBlue, GemPurple, Type3, Type4, Type5 };

    public GameObject GameOverCanvas;
    private GameObject GameCompleteCanvas;
    public bool gameOver;

    public bool blueGem;
    public bool purpleGem;
    public bool redGem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitMusic();
    }

    void InitMusic()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Cinematic":
                MyAudioManager.Instance.PlayMusic("Cinematic");
                break;
            case "Level1":
                MyAudioManager.Instance.PlayMusic("dayAmbient");
                break;
            case "Level2":
                MyAudioManager.Instance.PlayMusic("dungeon");
                break;
            case "Flight":
                MyAudioManager.Instance.PlayMusic("flight");
                break;
            case "BossBattle":
                MyAudioManager.Instance.PlayMusic("boss");
                break;
            case "Story_0":
                MyAudioManager.Instance.PlayMusic("dungeon");
                break;
            default:
                Debug.Log("No music assigned for scene: " + sceneName);
                break;
        }
    }

    public void CollectGem(SimpleCollectibleScript.CollectibleTypes gemType)
    {
        switch (gemType)
        {
            case SimpleCollectibleScript.CollectibleTypes.GemBlue:
                blueGem = true;
                BoyController.Instance.HasGemBlue = true;
                break;
            case SimpleCollectibleScript.CollectibleTypes.GemPurple:
                purpleGem = true;
                FlightLevel.Instance.LevelComplete();
                break;
            case SimpleCollectibleScript.CollectibleTypes.GemRed:
                redGem = true;
                MazeGenerator.Instance.LevelComplete();
                break;
        }
    }


    #region GAME OVER
    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            MyAudioManager.Instance.StopAny();
            MyAudioManager.Instance.PlaySfx("gameOverSFX");
            GameOverCanvas.SetActive(true);
        }
    }

    public void GameComplete()
    {
        if (!gameOver)
        {

        }   
    }
    #endregion

    #region PAUSE GAME
    public void PausePlayerMovement()
    {
        if (BoyController.Instance != null)
        {
            BoyController.Instance.BoyControl = false;
        }
    }

    public void ResumePlayerMovement()
    {
        if (BoyController.Instance != null)
        {
            BoyController.Instance.BoyControl = true;
        }
    }
    #endregion
}
