using System.Collections.Generic;
using UnityEngine;

public class TalkToNPCQuestStep : QuestStep
{
    public string status;

    private int NPCsToTalkTo = 3;

    private HashSet<int> talkedNPCIds = new HashSet<int>();

    private void Start()
    {
        UpdateState();
        foreach(int id in talkedNPCIds)
        {
            EventManager.instance.talkToNPCEvents.ActivateNPC(id);
        }

    }


    protected override void OnEnable()
    {
        EventManager.instance.talkToNPCEvents.onTalkToNPC += NPCTalked;
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        EventManager.instance.talkToNPCEvents.onTalkToNPC -= NPCTalked;
        base.OnDisable();
    }

    private void NPCTalked(int id)
    {
        if (!talkedNPCIds.Contains(id))
        {
            talkedNPCIds.Add(id);
            UpdateState();

            if (talkedNPCIds.Count >= NPCsToTalkTo)
            {
                FinishQuestStep();
            }
        }
    }

    private void UpdateState()
    {
        string state = string.Join(",", talkedNPCIds);
        string status = "Talk to villagers " + talkedNPCIds.Count + "/" + NPCsToTalkTo + ".";
        ChangeState(state, status);
    }

    protected override void SetQuestStepState(string state)
    {
        talkedNPCIds.Clear();

        foreach (var idStr in state.Split(','))
        {
            if (int.TryParse(idStr, out int id))
            {
                talkedNPCIds.Add(id);
            }
        }

        UpdateState();
    }
}
