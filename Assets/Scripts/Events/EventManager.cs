using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance { get; private set; }

    public QuestEvents questEvents;
    public InputEvents inputEvents;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        instance = this;


        questEvents = new QuestEvents();
        inputEvents = new InputEvents();
    }

}
