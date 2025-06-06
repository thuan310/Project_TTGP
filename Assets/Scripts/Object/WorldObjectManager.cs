using System.Collections.Generic;
using UnityEngine;

public class WorldObjectManager : MonoBehaviour
{
    public static WorldObjectManager instance;

    [Header("Objects")]
    [SerializeField] List<ObjectSpawner> objectSpawners;
    [SerializeField] List<GameObject> spawnedCharacter;

    [Header("Fog Walss")]
    public List<FogWallInteractable> fogWalls;


    // 1. create an object script that will hold the logic for fog walls
    // 2, spawn in those fogwalls aas network objects during start of game(must have a spawner object)
    // 3. create general object spawner script and prefab
    // 4. when the fog walls are spawned, add them to the world fog wall list
    // 5. grab the correct forwall from the list on the boss manager when the bos is being initialized  
    private void Awake()
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

    public void SpawnObject(ObjectSpawner objectSpawner)
    {
        objectSpawners.Add(objectSpawner);
        objectSpawner.AttemptToSpawnCharacter();
    }

    public void AddFogWallToList(FogWallInteractable fogWall)
    {
        if(!fogWalls.Contains(fogWall))
        {
            fogWalls.Add(fogWall);
        }
    }

    public void RemoveFogWallToList(FogWallInteractable fogWall)
    {
        if (!fogWalls.Contains(fogWall))
        {
            fogWalls.Remove(fogWall);
        }
    }
}
