using UnityEngine;

public class AICharacterSpawner : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] public GameObject characterGameObject;
    [SerializeField] GameObject instantiatedGameObject;
    [SerializeField] string idleAction;

    private void Awake()
    {
    }
    private void OnEnable()
    {
        WorldAIManager.instance.SpawnCharacters(this);
        gameObject.SetActive(false);
    }

    public void AttemptToSpawnCharacter()
    {
        if (characterGameObject != null)
        {
            instantiatedGameObject = Instantiate(characterGameObject);
            instantiatedGameObject.transform.position = transform.position;
            instantiatedGameObject.transform.rotation = transform.rotation;
            instantiatedGameObject.gameObject.GetComponent<AICharacterManager>().idleAction = idleAction;
            WorldAIManager.instance.AddCharacterToSpawnedCharactersList(instantiatedGameObject.GetComponent<AICharacterManager>()); 
        }
    }
}
