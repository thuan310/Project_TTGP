using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkExternalFunctions : MonoBehaviour {
    public void Bind(Story story)
    {
        story.BindExternalFunction("StartQuest", (string questId) => StartQuest(questId));
        story.BindExternalFunction("AdvanceQuest", (string questId) => AdvanceQuest(questId));
        story.BindExternalFunction("FinishQuest", (string questId) => FinishQuest(questId));
        story.BindExternalFunction("FinishQuestStep", (string questId) => FinishQuestStep(questId) );
    }

    public void Unbind(Story story)
    {
        story.UnbindExternalFunction("StartQuest");
        story.UnbindExternalFunction("AdvanceQuest");
        story.UnbindExternalFunction("FinishQuest");
        story.UnbindExternalFunction("FinishQuestStep");
    }

    private void StartQuest(string questId)
    {
        EventManager.instance.questEvents.StartQuest(questId);
    }

    private void AdvanceQuest(string questId)
    {
        EventManager.instance.questEvents.AdvanceQuest(questId);
    }

    private void FinishQuest(string questId)
    {
        EventManager.instance.questEvents.FinishQuest(questId);
    }

    private void FinishQuestStep(string questId)
    {
        EventManager.instance.questEvents.FinishQuestStep(questId);
    }
}
