using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeoHouseTransition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene("LeoHouse");
    }
}
