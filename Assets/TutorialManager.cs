using Ink.Parsed;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    [Header("Tutorial info")]
    [SerializeField] string tutorialNameDisplaying;
    [SerializeField] TextMeshProUGUI tutorialStringDisplaying;
    [SerializeField] TextMeshProUGUI numberOfTutorialDisplaying;
    [SerializeField] Image tutorialImageDisplaying;
    [SerializeField] GameObject tutorialUI;

    [SerializeField] List<Tutorial> packOfTutorials;
    [SerializeField] Tutorial currentTutorial;
    public int currentTutorialIdx=-1;
    public int currentTutorialPieceIdx=-1;

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
        //this.enabled = false;
    }

    private void OnEnable()
    {
        try{
            SceneNavigationManager.instance.currentSceneIndex.OnValueChanged += AutomateLoadPacksOfTutorials;
        }
        catch
        {
            //Debug.Log("Co ve ko gan duoc delegate vao currentSceneIdex roi");
        }
    }

    private void OnDisable()
    {
        SceneNavigationManager.instance.currentSceneIndex.OnValueChanged -= AutomateLoadPacksOfTutorials;
    }

    public void AutomateLoadPacksOfTutorials(int oldIdx, int newIdx)
    {
        packOfTutorials = WorldTutorialsManager.instance.LoadPacksOfTutorial(newIdx);
        currentTutorialIdx = -1;
        currentTutorialPieceIdx = -1;
    }

    public void LoadNextTutorials()
    {
        currentTutorialIdx++;
        currentTutorialPieceIdx = -1;
        //load the pack of tutorial
        currentTutorial = packOfTutorials[currentTutorialIdx];

        tutorialNameDisplaying = currentTutorial.tutorialName;
        LoadNextPieceOfTutorial();
        //Open Ui
        OpenTutorialUI();
    }

    public void LoadNextPieceOfTutorial()
    {
        if (currentTutorialPieceIdx +1  >= currentTutorial.tutorialStrings.Count)
        {
            if (currentTutorial.tutorialStrings.Count == 0)
            {
                Debug.Log("there are no tutorials in this pack");
                return;
            }
            Debug.Log("End of content to Load");
            return;
        }
        currentTutorialPieceIdx++;
        //load content
        tutorialStringDisplaying.text = currentTutorial.tutorialStrings[currentTutorialPieceIdx];
        tutorialImageDisplaying.sprite = currentTutorial.tutorialImages[currentTutorialPieceIdx];
        UpdateTheNumberOfTutorialsUI();
    }

    public void LoadPreviousPieceOfTutorial()
    {
        if (currentTutorialPieceIdx <=0)
        {
            return;
        }
        currentTutorialPieceIdx--;
        tutorialStringDisplaying.text = currentTutorial.tutorialStrings[currentTutorialPieceIdx];
        tutorialImageDisplaying.sprite = currentTutorial.tutorialImages[currentTutorialPieceIdx];
        UpdateTheNumberOfTutorialsUI();
    }

    void UpdateTheNumberOfTutorialsUI()
    {
        numberOfTutorialDisplaying.text = $"{currentTutorialPieceIdx+1}/{currentTutorial.tutorialStrings.Count}";
    }
    public void OpenTutorialUI()
    {
        tutorialUI.SetActive(true);
        SceneNavigationManager.instance.PauseScene();
    }
    public void CloseTutorialUI()
    {
        tutorialUI.SetActive(false);
        SceneNavigationManager.instance.ResumeScene();
    }

    // Debug

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    SceneNavigationManager.instance.currentSceneIndex.Value++;
        //}
    }
}
