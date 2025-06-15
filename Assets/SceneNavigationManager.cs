using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigationManager : MonoBehaviour
{
    public static SceneNavigationManager instance;

    public int nextSceneIndexToLoad =1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneByIndex(int index)
    {
        StartCoroutine(LoadAsync(index));
    }

    public void LoadNextScene()
    {
        nextSceneIndexToLoad++;
        StartCoroutine(LoadAsync(nextSceneIndexToLoad));
    }
    IEnumerator LoadAsync(int sceneIndex)
    {
        //loadingScreen.SetActive(true);
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex);
        //loadOperation.allowSceneActivation = false; // Optional: Manual control

        //while (!loadOperation.isDone)
        //{
        //    float progress = loadOperation.progress / 0.9f; // Unity stops at 0.9 until activation
        //    Debug.Log("Progress: " + (progress * 100) + "%");

        //    if (progress >= 1f)
        //    {
        //        loadOperation.allowSceneActivation = true; // Finally switch
        //    }
        yield return null;
        //}
    }
}
