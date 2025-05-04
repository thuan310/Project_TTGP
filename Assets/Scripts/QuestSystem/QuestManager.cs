using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {
    [Header("Config")]
    [SerializeField] private bool loadQuestState = true;

    private Dictionary<string, Quest> questMap;




    private void OnEnable()
    {
        EventManager.instance.questEvents.onStartQuest += StartQuest;
        EventManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        EventManager.instance.questEvents.onFinishQuest += FinishQuest;
        EventManager.instance.questEvents.onQuestStepStateChange += QuestStepStateChange;
    }

    private void OnDisable()
    {
        EventManager.instance.questEvents.onStartQuest -= StartQuest;
        EventManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        EventManager.instance.questEvents.onFinishQuest -= FinishQuest;
        EventManager.instance.questEvents.onQuestStepStateChange -= QuestStepStateChange;

    }

    private void Awake()
    {
        questMap = CreateQuestMap();

    }

    private void Start()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.IN_PROGRESS)
            {
                quest.InstantiateQuestStep(this.transform);
            }
            EventManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    private void Update()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

        foreach (QuestInfoSO questinfo in allQuests)
        {

            if (idToQuestMap.ContainsKey(questinfo.id))
            {
                Debug.Log("Duplicate ID Found");
            }
            idToQuestMap.Add(questinfo.id, LoadQuest(questinfo));

            Debug.Log($"Loaded Quest: ID = {questinfo.id}, Name = {questinfo.name}");

        }

        return idToQuestMap;
    }



    private bool CheckRequirementMet(Quest quest)
    {
        bool requirementsMet = true;



        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            if (GetQuestByID(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                requirementsMet = false;
                break;
            }
        }

        return requirementsMet;
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestByID(id);
        quest.state = state;
        EventManager.instance.questEvents.QuestStateChange(quest);
    }

    private Quest GetQuestByID(string id)
    {
        Quest quest = questMap[id];
        if (quest != null)
        {
            Debug.Log("ID not found");
        }
        return quest;
    }

    private void StartQuest(string id)
    {
        Quest quest = GetQuestByID(id);
        quest.InstantiateQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestByID(id);

        quest.MoveToNextStep();

        if (quest.CurrentStepExists())
        {
            quest.InstantiateQuestStep(this.transform);
        }
        else
        {
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestByID(id);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);

    }


    private void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        Quest quest = GetQuestByID(id);
        quest.StoreQuestStepState(questStepState, stepIndex);
        ChangeQuestState(id, quest.state);
    }

    private void OnApplicationQuit()
    {
        foreach (Quest quest in questMap.Values)
        {
            SaveQuest(quest);
        }
    }

    private void SaveQuest(Quest quest)
    {
        try
        {
            QuestData questData = quest.GetQuestData();
            string serializeData = JsonUtility.ToJson(questData);
            PlayerPrefs.SetString(quest.info.id, serializeData);
            Debug.Log(serializeData);
        }
        catch (System.Exception e)
        {
            Debug.Log("Failed to save quest with id: " + quest.info.id + ": " + e);
        }
    }

    private Quest LoadQuest(QuestInfoSO questInfo)
    {
        Quest quest = null;
        try
        {
            if (PlayerPrefs.HasKey(questInfo.id) && loadQuestState)
            {
                string serializedData = PlayerPrefs.GetString(questInfo.id);
                QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
                quest = new Quest(questInfo, questData.state, questData.questStepIndex, questData.questStepStates);
            }
            else
            {
                quest = new Quest(questInfo);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load quest with id " + quest.info.id + ": " + e);
        }
        return quest;
    }

}
