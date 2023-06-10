using System.Collections;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MyLevelManager : MonoBehaviour
    {
        public static MyLevelManager Instance;
        public bool canStart;
        public bool canCheck;
        public int enemyCount;
        
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

        private void Start()
        {
            if(SceneManager.GetActiveScene().name == "BossBattle")
                Level3();
        }

        private void Update()
        {
            if (canStart)
            {
                if (actualLevel == "level1")
                {
                    canStart = false;
                    MyDialogueManager.Instance.TextLevel("Level1");
                    Gem_Level1.SetActive(false);
                    canCheck = true;
                }
                else if (actualLevel == "level2")
                {
                    canStart = false;
                    MyDialogueManager.Instance.TextLevel("Level2");
                }
                else if (actualLevel == "level3") //Boss
                {
                    canStart = false;
                    MyDialogueManager.Instance.TextLevel("Level3");
                    Gem_Level_Boss.SetActive(false);
                    canCheck = true;
                }
            }

            if (canCheck)
            {
                if (actualLevel == "level1")
                {
                    CheckEnemyCounter();
                }
                else if (actualLevel == "level3")
                {
                    if (!flowerDefeated)
                        CheckFlowerBossLife();
                    else if (flowerDefeated && !goblinDefeated)
                        CheckGoblinBossLife();
                }
            }
        }

        public void Level1()
        {
            actualLevel = "level1";
        }
        
        public void Level2()
        {
            actualLevel = "level2";
        }
        
        //Boss
        public void Level3()
        {
            actualLevel = "level3";
            canStart = true;
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
