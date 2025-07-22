using UnityEngine;

public class SharpenWoodQuestStep : QuestStep
{
    public string status;
    public GameObject icon;
    private int treesToSharpen = 3;
    private int treesSharpened = 0;

    private void Start()
    {
        UpdateState();


    }


    protected override void OnEnable()
    {
        EventManager.instance.chopWoodMinigameEvents.onSharpenWood += SharpenWood;
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        EventManager.instance.chopWoodMinigameEvents.onSharpenWood -= SharpenWood;
        base.OnDisable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            icon.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            icon.SetActive(true);
        }
    }

    private void SharpenWood()
    {
        treesSharpened++;
        UpdateState();

        if (treesSharpened >= treesToSharpen)
        {
            FinishQuestStep();
        }

    }

    private void UpdateState()
    {
        string state = treesSharpened.ToString();
        string status = "Sharpen wood " + treesSharpened + "/" + treesToSharpen + ".";
        ChangeState(state, status);
    }

    protected override void SetQuestStepState(string state)
    {
        treesSharpened = int.Parse(state);

        UpdateState();
    }
}
