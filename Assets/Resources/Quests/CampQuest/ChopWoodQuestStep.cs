using System.Collections.Generic;
using UnityEngine;

public class ChopWoodQuestStep : QuestStep
{
    public string status;

    private int treesToChop = 3;
    private int treeChopped = 0;

    private void Start()
    {
        UpdateState();

        EventManager.instance.chopWoodMinigameEvents.ActivateIcon();

    }


    protected override void OnEnable()
    {
        EventManager.instance.chopWoodMinigameEvents.onChopWood += WoodChopped;
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        EventManager.instance.chopWoodMinigameEvents.onChopWood -= WoodChopped;
        base.OnDisable();
    }

    private void WoodChopped()
    {
        treeChopped++;
        UpdateState();

        if (treeChopped >= treesToChop)
        {
            FinishQuestStep();
        }
        
    }

    private void UpdateState()
    {
        string state = treeChopped.ToString();
        string status = "Chop wood " + treeChopped + "/" + treesToChop + ".";
        ChangeState(state, status);
    }

    protected override void SetQuestStepState(string state)
    {
        treeChopped = int.Parse(state);

        UpdateState();
    }
}
