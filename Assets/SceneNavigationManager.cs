using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigationManager : MonoBehaviour
{
    public static SceneNavigationManager instance;

    public PlayerManager player;

    public int nextSceneIndex =1;

    public int previousSceneIndex;

    public Vector3 playerPreviousPosition;

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
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex;
        nextSceneIndex= previousSceneIndex++;
        StartCoroutine(LoadAsync(nextSceneIndex));
        print("da CHuyen Scene");
    }
    public void BringBackToPreviousPlace()
    {
        print("wth");
        player.gameObject.transform.position = playerPreviousPosition;
        // Scene cũ vẫn còn và không bị reset

    }

    public void GotoAreana1()
    {
        ArenaManager.instance.FightVillager();
        playerPreviousPosition = player.gameObject.transform.position;
        player.gameObject.transform.position = ArenaManager.instance.playerSpawnPosition.position;
    }

    public void GotoAreana2()
    {
        ArenaManager.instance.FightNgoQuyen();
        playerPreviousPosition = player.gameObject.transform.position;
        player.gameObject.transform.position = ArenaManager.instance.playerSpawnPosition.position;
        // Optionally: tắt các đối tượng trong scene cũ
        //int previousSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //SceneManager.LoadScene("Scene_World_04_Arena2");
    }

    public void Update()
    {
        //print(player.gameObject.transform.position);
        if (Input.GetKeyDown(KeyCode.O))
        {
            GotoAreana2();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            GotoAreana1();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            BringBackToPreviousPlace();
        }
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

    private void SetSceneObjectsActive(int sceneIdx, bool isActive)
    {
        Scene scene = SceneManager.GetSceneAt(sceneIdx);
        foreach (GameObject go in scene.GetRootGameObjects())
        {
            go.SetActive(isActive);
        }
    }
}
