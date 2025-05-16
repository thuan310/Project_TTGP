    using UnityEngine;
[System.Serializable]
public class CharacterSaveData
{
    [Header("Scene Index")]
    public int sceneIndex = 1;

    [Header("Character Name")]
    public string characterName = "ThangDepTrai";

    [Header("Time Played")]
    public float secondsPlayed;

    //Question: why not use a Vector3
    //Answer: we can only save data fron "Basic" Variable types(float, int, string, bool, etc)
    [Header("World Coordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;

    [Header("Stats")]
    public int currentHealth;
    public float currentStamina;

    [Header("Stats")]
    public int vitality;
    public int endurance;

    [Header("Bosses")]
    public SerializableDictionary<int, bool> bossesAwakened; // the int is the boss I.D, the bool is the awakened status //hashmap
    public SerializableDictionary<int, bool> bossesDefeated; // the int is the boss I.D, the bool is the awakened status

    public CharacterSaveData()
    {
        bossesAwakened = new SerializableDictionary<int, bool>();
        bossesDefeated = new SerializableDictionary<int, bool>();
    }
}
