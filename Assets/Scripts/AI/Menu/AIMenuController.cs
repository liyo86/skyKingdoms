using System.Collections;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AI.Menu
{
    internal class AIMenuController : MonoBehaviour
    {
        public TextMeshProUGUI countdownText;
        private float timer = 10f;

        private void Start()
        {
            StartCoroutine(StartCountdown());
        }

        private IEnumerator StartCountdown()
        {
            while (timer > 0f)
            {
                countdownText.text = "Demo en " + timer.ToString("F0");
                yield return new WaitForSeconds(1f);
                timer--;
            }
            
            MyGameManager.Instance.AIDemoControl = true;
            SceneManager.LoadScene("Level1");
        }
    }
}
