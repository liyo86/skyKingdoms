using System.Collections;
using AI.Player_Controller;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MyGameManager : MonoBehaviour
    {
        #region VARIABLES

        public static MyGameManager Instance;

        public GameObject GameOverCanvas;
        public GameObject animationGameOver;
        public GameObject menuGameOver;
        public Animator playerCanvasAnimator;
        public bool gameOver;
        public bool isLoading;
        public bool AIDemoControl;

        #endregion
    
        #region UNITY METHODS
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion
    
        #region INIT CONFIGURATION
        public void Init()
        {
            var sceneName = SceneManager.GetActiveScene().name;

            switch (sceneName)
            {
                case "Cinematic":
                    MyAudioManager.Instance.PlayMusic("Cinematic");
                    break;
                case "Level1":
                    MyAudioManager.Instance.PlayMusic("dayAmbient");
                    MyLevelManager.Instance.Level("level1");
               
                    GameObject Player = GameObject.FindWithTag(Constants.PLAYER);
                    if (Player == null) return;
        
                    if (AIDemoControl)
                    {
                        Player.GetComponent<BoyController>().enabled = false;
                        Player.GetComponent<AIController>().enabled = true;
                    }
                    break;
                case "Level2":
                    MyAudioManager.Instance.PlayMusic("dungeon");
                    MyLevelManager.Instance.Level("level2", true);
                    break;
                case "Flight":
                    MyAudioManager.Instance.PlayMusic("flight");
                    break;
                case "BossBattle":
                    MyAudioManager.Instance.PlayMusic("boss");
                    MyLevelManager.Instance.Level("level3", true);
                    break;
                case "TheEnd":
                    MyAudioManager.Instance.PlayMusic("theEnd");
                    break;
                case "Story_0":
                    MyAudioManager.Instance.PlayMusic("dungeon");
                    break;
                case "Story_1":
                    if (MyLevelManager.Instance.backToScene) return;
                    MyAudioManager.Instance.PlayMusic("town");
                    break;
                default:
                    Debug.Log("No music assigned for scene: " + sceneName);
                    ResumePlayerMovement();
                    break;
            }
        }
        #endregion
    
        #region COLLECTIBLES
        public void CollectGem(Collectible.CollectibleTypes gemType)
        {
            switch (gemType)
            {
                case Collectible.CollectibleTypes.GemBlue:
                    Debug.Log("Azul conseguida");
                    break;
                case Collectible.CollectibleTypes.GemPurple:
                    FlightLevel.Instance.LevelComplete();
                    break;
                case Collectible.CollectibleTypes.GemRed:
                    Debug.Log("Roja conseguida");
                    break;
                case Collectible.CollectibleTypes.GemGreen:
                    Debug.Log("Verde conseguida");
                    break;
            }
        }
        #endregion
    
        #region GAME OVER
        public void GameOver()
        {
            if (isLoading) return; //TODO meter pequeño delay
        
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
            playerCanvasAnimator.SetTrigger("continue");
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

        public static void ResumePlayerMovement()
        {
            if (BoyController.Instance != null)
            {
                BoyController.Instance.CanMove = true;
            }
        }
        #endregion
    }
}
