using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigationManager : MonoBehaviour
{
    public static SceneNavigationManager instance;

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

    IEnumerator LoadAsync(int worldSceneIndex)
    {
        //loadingScreen.SetActive(true);
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
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
