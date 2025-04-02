using System;
using UnityEngine;

public class QuestEvents 
{
    public event Action<string> onStartQuest;
    public void StartQuest(string id)
    {
        onStartQuest?.Invoke(id);
    }

    public event Action<string> onAdvanceQuest;

    public void AdvanceQuest(string id)
    {
        onAdvanceQuest?.Invoke(id);
    }

    public event Action<string> onFinishQuest;

    public void FinishQuest(string id)
    {

        onFinishQuest?.Invoke(id);
        
    }

    public event Action<Quest> onQuestStateChange;
    public void QuestStateChange(Quest quest)
    {

        onQuestStateChange?.Invoke(quest);
  
    }

    public event Action<string, int, QuestStepState> onQuestStepStateChange;
    public void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {

        onQuestStepStateChange?.Invoke(id, stepIndex, questStepState);
        
    }

    public event Action<string> onFinishQuestStep;
    public void FinishQuestStep(string id)
    {
        onFinishQuestStep?.Invoke(id);
    }
}
