using UnityEngine;

public class AICharacterSpawner : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] GameObject characterGameObject;
    [SerializeField] GameObject instantiatedGameObject;

    private void Awake()
    {
    }
    private void Start()
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
            WorldAIManager.instance.AddCharacterToSpawnedCharactersList(instantiatedGameObject.GetComponent<AICharacterManager>()); 
        }
    }
}
