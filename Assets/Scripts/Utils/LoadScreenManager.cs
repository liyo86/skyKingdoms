﻿using System.Collections;
using Managers;
using Service;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadScreenManager : MonoBehaviour
{
    public GameObject loadingScreenPanel;
    public Image loadedImg;
    public TextMeshProUGUI loadedText;
    public string sceneName;
    public float minLoadTime;
    private bool minTimeElapsed;
    private AsyncOperation async;
    private bool isLoading;

    private void Start()
    {
        loadingScreenPanel.SetActive(false);
        
        loadedImg.fillAmount = 0f;
        loadedText.text = "0%";

         if (SceneManager.GetActiveScene().name == "FirstLoad")
         {
             LoadScene();
         }
         else
         {
             ServiceLocator.GetService<MyGameManager>().Init();
         }
 
         ServiceLocator.GetService<MyGameManager>().isLoading = false;
 
         SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadScene(string name = "null")
    {
        ServiceLocator.GetService<MyGameManager>().isLoading = true;

        if (name != "null")
            sceneName = name;
        
        if (!isLoading)
        {
            isLoading = true;
            StartCoroutine(LoadScreenCoroutine());
            Invoke(nameof(TimeElapsed), minLoadTime);
        }
    }

    private IEnumerator LoadScreenCoroutine()
    {
        loadingScreenPanel.SetActive(true);
        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        while(!async.isDone)
        {
            loadedImg.fillAmount = async.progress / 0.9f;
            loadedText.text = $"{(int)(loadedImg.fillAmount * 100f)}%";
            if(async.progress >= 0.9f && minTimeElapsed)
            {
                loadedImg.fillAmount = 1f;
                loadedText.text = "100%";
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private void TimeElapsed()
    {
        minTimeElapsed = true;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isLoading = false;
        loadingScreenPanel.SetActive(false);
        ServiceLocator.GetService<MyGameManager>().Init();
    }
}
