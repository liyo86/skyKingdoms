using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyGameManager : MonoBehaviour
{
    public static MyGameManager Instance;
    
    public GameObject GameOverCanvas;
    public GameObject animationGameOver;
    public GameObject menuGameOver;
    public Animator playerCanvasAnimator;
    public bool gameOver;

    #region UNITY METHODS
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
    #endregion
    
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
                BoyController.Instance.HasGemBlue = true; //TODO cambiar por un manager del nivel
                break;
            case SimpleCollectibleScript.CollectibleTypes.GemPurple:
                FlightLevel.Instance.LevelComplete();
                break;
            case SimpleCollectibleScript.CollectibleTypes.GemRed:
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
            StartCoroutine(nameof(GameOverAnimation));
        }
    }

    IEnumerator GameOverAnimation()
    {
        animationGameOver.SetActive(true);
        
        playerCanvasAnimator.SetTrigger("dead");

        yield return new WaitForSeconds(3f);

        menuGameOver.SetActive(true);
    }

    public void RestartLevel()
    {
        // TODO control de vidas?
        playerCanvasAnimator.SetTrigger("continue");
    }
    
    public void LevelComplete()
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
            BoyController.Instance.CanMove = false;
        }
    }

    public void ResumePlayerMovement()
    {
        if (BoyController.Instance != null)
        {
            BoyController.Instance.CanMove = true;
        }
    }
    #endregion
}
