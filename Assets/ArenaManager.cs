using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public static ArenaManager instance;

    public Transform playerSpawnPosition;

    public GameObject bossEventTrigger;

    public AICharacterSpawner spawner;

    [SerializeField] GameObject ngoQuyenPrefab;
    [SerializeField] GameObject villagerPrefab;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FightVillager()
    {
        spawner.characterGameObject = villagerPrefab;
        spawner.gameObject.SetActive(true);
        bossEventTrigger.SetActive(false);
    }

    public void FightNgoQuyen()
    {
        spawner.characterGameObject = ngoQuyenPrefab;
        spawner.gameObject.SetActive(true);
        bossEventTrigger.SetActive(true);
    }



    
}
