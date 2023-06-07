using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyCreditsManager : MonoBehaviour
{
    public GameObject credits;

    private bool exit;

    public void SubmitEvent()
    {
        if (!credits.gameObject.activeSelf && !exit)
        {
            exit = true;
            credits.SetActive(true);
            StartCoroutine(nameof(WaitForSubmit));
        }
       
        if (credits.gameObject.activeSelf && !exit)
        {
            SceneManager.LoadScene("Menu_game");
        }
           
    }

    private IEnumerator WaitForSubmit()
    {
        yield return new WaitForSeconds(2f);

        exit = false;
    }
}
