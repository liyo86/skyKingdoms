using System.Collections;
using DG.Tweening;
using JetBrains.Annotations;
using Player;
using Service;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MyLevelManager : MonoBehaviour
    {
        #region VARIABLES
        public static MyLevelManager Instance;

        public bool canStart { get; set; }

        private bool canCheck { get; set; }

        public int enemyCount { get; set; }

        public bool backToScene;

        public int ActualDialogueResponse = -1;

        private string actualLevel = "";
        private bool flowerDefeated;
        private bool goblinDefeated;

        [CanBeNull]
        public GameObject Gem_Level1;
        
        [CanBeNull]
        public GameObject Gem_Level_Boss;
        
        [CanBeNull]
        public GameObject Goblin;
        
        [CanBeNull]
        public GameObject GoblinWeapon;
        #endregion

        private void Update() //Configuraciones que necesita cada escena
        {
            if (canStart)
            {
                switch (actualLevel)
                {
                    case "level1":
                        canStart = false;
                        MyDialogueManager.Instance.TextLevel("Level1");
                        Gem_Level1.SetActive(false);
                        canCheck = true;
                        break;
                    case "level2":
                        canStart = false;
                        MyDialogueManager.Instance.TextLevel("Level2");
                        break;
                    //Boss
                    case "level3":
                        canStart = false;
                        MyDialogueManager.Instance.TextLevel("Level3");
                        Gem_Level_Boss.SetActive(false);
                        canCheck = true;
                        break;
                    case "Story_1":
                        canStart = false;
                        if (backToScene)
                        {
                            backToScene = false;
                            var position = GameObject.FindWithTag("BackPosition").GetComponent<Transform>()
                                .position;
                            BoyController.Instance.SetPosition(position);
                        }

                        //TODO null reference
                        StoryOneTransition.Instance.CanCheckDialogueOptions();
                        MyGameManager.ResumePlayerMovement();
                        break;
                }
            }

            if (!canCheck) return;
            
            switch (actualLevel)
            {
                case "level1":
                    CheckEnemyCounter();
                    break;
                case "level3" when !flowerDefeated:
                    CheckFlowerBossLife();
                    break;
                case "level3":
                {
                    if (flowerDefeated && !goblinDefeated)
                        CheckGoblinBossLife();
                    break;
                }
            }
        }
        
        public void Level(string level, bool start = false)
        {
            actualLevel = level;
            canStart = start;
        }

        public void StartLevel()
        {
            ServiceLocator.GetService<PlayerData>().PlayerInstantation();
        }
        
        private void CheckEnemyCounter()
        {
            if (enemyCount >= 5)
            {
                canCheck = false;
                Gem_Level1.SetActive(true);
            }
        }
        
        private void CheckFlowerBossLife()
        {
            if (FlowerBossHealth.Instance == null) return;
            
            if (FlowerBossHealth.Instance.CurrentHealth <= 0f)
            {
                flowerDefeated = true;
                StartCoroutine(StartGoblinIA());
            }
        }

        private void CheckGoblinBossLife()
        {
            if (GoblinBossHealth.Instance == null) return;
            
            if (GoblinBossHealth.Instance.CurrentHealth <= 0f)
            {
                goblinDefeated = true;
                Gem_Level_Boss.SetActive(true);
                Gem_Level_Boss.transform.DOMoveY(transform.position.y, 3).SetEase(Ease.Linear).Play();
            }
        }

        private IEnumerator StartGoblinIA()
        {
            yield return new WaitForSeconds(2f);
            Goblin.SetActive(true);
            GoblinWeapon.SetActive(true);
        }

    }
}
