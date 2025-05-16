using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager instance;

    [Header("Characters")]
    [SerializeField] List<AICharacterSpawner> aiCharacterSpawners;
    [SerializeField] List<AICharacterManager> spawnedInCharacters;

    [Header("Bosses")]
    [SerializeField] List<AIBossCharacterManager> spawnedInBosses;

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

    public void SpawnCharacters(AICharacterSpawner aiCharacterSpawner)
    {
        aiCharacterSpawners.Add(aiCharacterSpawner);
        aiCharacterSpawner.AttemptToSpawnCharacter();
    }

    public void AddCharacterToSpawnedCharactersList(AICharacterManager character)
    {
        if (spawnedInCharacters.Contains(character))
            return;

        spawnedInCharacters.Add(character);

        AIBossCharacterManager aIBossCharacter = character as AIBossCharacterManager;

        if (aIBossCharacter !=null)
        {
            if (spawnedInBosses.Contains(aIBossCharacter))
                return;

            spawnedInBosses.Add(aIBossCharacter);
        }
    }

    public AIBossCharacterManager GetBossCharacterByID(int ID)
    {
        return spawnedInBosses.FirstOrDefault(boss => boss.bossID == ID);
    }

    private void DespawnAllCharacter()
    {
        foreach (var character in spawnedInCharacters)
        {
            Destroy(character);
        }
    }

    private void DesalbeAllCharacters()
    {
        // to do disable character gameobjects, sync disabled status on network
        // disable gameobjects for clients upon connecting, if disabled status is true
        // can be used to disable characters that are far from players to save memory
        // characters can be split into areas (area_00_, Area_01, Area_02) ect

    }

}

