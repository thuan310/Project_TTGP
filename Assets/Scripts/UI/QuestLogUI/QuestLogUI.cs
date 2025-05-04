using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestLogUI : MonoBehaviour {
    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private QuestLogList scrollingList;
    [SerializeField] private TextMeshProUGUI questDisplayNameText;
    [SerializeField] private TextMeshProUGUI questStatusText;
    [SerializeField] private TextMeshProUGUI questRequirementsText;

    private Button firstSelectedButton;

    private void OnEnable()
    {
        EventManager.instance.inputEvents.onToggleQuestPressed += QuestLogTogglePressed;
        EventManager.instance.questEvents.onQuestStateChange += QuestStateChange;
    }

    private void OnDisable()
    {
        EventManager.instance.inputEvents.onToggleQuestPressed -= QuestLogTogglePressed;
        EventManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
    }

    private void QuestLogTogglePressed()
    {
        if (contentParent.activeInHierarchy)
        {
            HideUI();
        }
        else
        {
            ShowUI();
        }
    }

    private void ShowUI()
    {
        contentParent.SetActive(true);
        //EventManager.instance.playerEvents.DisablePlayerMovement();

        if (firstSelectedButton != null)
        {
            firstSelectedButton.Select();
        }
    }

    private void HideUI()
    {
        contentParent.SetActive(false);
        //EventManager.instance.playerEvents.EnablePlayerMovement();
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void QuestStateChange(Quest quest)
    {
        QuestLogButton questLogButton = scrollingList.CreateButtonIfNotExists(quest, () => {
            SetQuestLogInfo(quest);
        });

        if (firstSelectedButton == null)
        {
            firstSelectedButton = questLogButton.button;
            firstSelectedButton.Select();

        }



        // set the button color based on quest state
        questLogButton.SetState(quest.state);
    }

    private void SetQuestLogInfo(Quest quest)
    {
        // quest name
        questDisplayNameText.text = quest.info.displayName;

        // status
        questStatusText.text = quest.GetFullStatusText();

        // requirements
        questRequirementsText.text = "";
        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            questRequirementsText.text += prerequisiteQuestInfo.displayName + "\n";
        }


    }
}
