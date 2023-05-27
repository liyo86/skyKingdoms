using JetBrains.Annotations;
using UnityEngine;

namespace Managers
{
    public class MyLevelManager : MonoBehaviour
    {
        public static MyLevelManager Instance;
        public bool canStart;
        public bool canCheck;
        public int enemyCount;
        private string actualLevel = "";

        [CanBeNull]
        public GameObject Gem_Level1;
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
                } else if (actualLevel == "level2")
                {
                    canStart = false;
                    MyDialogueManager.Instance.TextLevel("Level2"); 
                }
            }

            if (canCheck)
            {
                if (actualLevel == "level1")
                {
                    CheckEnemyCounter();
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

        private void CheckEnemyCounter()
        {
            if (enemyCount >= 5)
            {
                canCheck = false;
                Gem_Level1.SetActive(true);
            }
        }

    }
}
