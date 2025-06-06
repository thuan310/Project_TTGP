using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] GameObject gameeObject;
    [SerializeField] GameObject instantiatedGameObject;

    private void Awake()
    {
    }
    private void Start()
    {
        WorldObjectManager.instance.SpawnObject(this);
        gameObject.SetActive(false);
    }

    public void AttemptToSpawnCharacter()
    {
        if (gameeObject != null)
        {
            instantiatedGameObject = Instantiate(gameeObject);
            instantiatedGameObject.transform.position = transform.position;
            instantiatedGameObject.transform.rotation = transform.rotation;
        }
    }
}
