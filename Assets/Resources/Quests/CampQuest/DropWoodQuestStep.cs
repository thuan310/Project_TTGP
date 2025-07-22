using UnityEngine;

public class DropWoodQuestStep : QuestStep
{
    public string status;
    public GameObject icon;
    private int treesToDrop = 3;
    private int treesDropped = 0;

    private void Start()
    {
        UpdateState();


    }


    protected override void OnEnable()
    {
        EventManager.instance.chopWoodMinigameEvents.onDropWood += DropWood;
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        EventManager.instance.chopWoodMinigameEvents.onDropWood -= DropWood;
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

    private void DropWood()
    {
        treesDropped++;
        UpdateState();

        if (treesDropped >= treesToDrop)
        {
            FinishQuestStep();
        }

    }

    private void UpdateState()
    {
        string state = treesDropped.ToString();
        string status = "Place wood in the cart " + treesDropped + "/" + treesToDrop + ".";
        ChangeState(state, status);
    }

    protected override void SetQuestStepState(string state)
    {
        treesDropped = int.Parse(state);

        UpdateState();
    }
}
