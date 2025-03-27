using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{

    public static WorldSaveGameManager instance;

    public PlayerManager player;

    [Header("Save/Load")]
    [SerializeField] bool saveGame;
    [SerializeField] bool loadGame;

    [Header("World Scene Index")]
    [SerializeField] int worldSceneIndex = 2;

    [Header("Save Data Writer")]
    private SaveFileDataWriter saveFileDataWriter;

    [Header("Current Character Data")]
    public CharacterSlot currentCharaterSlotBeingUsed;
    public CharacterSaveData currentCharacterData;
    private string saveFileName;

    [Header("Character Slots")]
    public CharacterSaveData characterSlot01;
    public CharacterSaveData characterSlot02;
    public CharacterSaveData characterSlot03;
    public CharacterSaveData characterSlot04;
    public CharacterSaveData characterSlot05;
    public CharacterSaveData characterSlot06;
    public CharacterSaveData characterSlot07;
    public CharacterSaveData characterSlot08;
    public CharacterSaveData characterSlot09;
    public CharacterSaveData characterSlot10;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadAllChaaracterProfiles();
    }

    private void Update()
    {
        if (saveGame)
        {
            saveGame = false;
            SaveGame();

        }
        if (loadGame) 
        {
           loadGame = false;
            LoadGame();
        }
    }

    public string DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot characterSlot)
    {
        string fileName = "";

        switch (characterSlot)
        {
            case CharacterSlot.CharacterSlot_01 :
                fileName = "characterSlot_01";
                break;
            case CharacterSlot.CharacterSlot_02:
                fileName = "characterSlot_02";
                break;
            case CharacterSlot.CharacterSlot_03:
                fileName = "characterSlot_03";
                break;
            case CharacterSlot.CharacterSlot_04:
                fileName = "characterSlot_04";
                break;
            case CharacterSlot.CharacterSlot_05:
                fileName = "characterSlot_05";
                break;
            case CharacterSlot.CharacterSlot_06:
                fileName = "characterSlot_06";
                break;
            case CharacterSlot.CharacterSlot_07:
                fileName = "characterSlot_07";
                break;
            case CharacterSlot.CharacterSlot_08:
                fileName = "characterSlot_08";
                break;
            case CharacterSlot.CharacterSlot_09:
                fileName = "characterSlot_09";
                break;
            case CharacterSlot.CharacterSlot_10:
                fileName = "characterSlot_10";
                break;
            default:
                break;
        }
        return fileName;
    }

    public void AttemptToCreateNewGame()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        //Check to see if we can create a new save file (check for other existing files first)
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_01);

        if (!saveFileDataWriter.CheckToSeeFileExists())
        {
            //Debug.Log("1");
            //if this profile slot is not taken, make a new one using this slot
            currentCharaterSlotBeingUsed = CharacterSlot.CharacterSlot_01;

            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }

        //Check to see if we can create a new save file (check for other existing files first)
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_02);

        if (!saveFileDataWriter.CheckToSeeFileExists())
        {
            //Debug.Log("2");
            //if this profile slot is not taken, make a new one using this slot
            currentCharaterSlotBeingUsed = CharacterSlot.CharacterSlot_02;

            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }

        //Check to see if we can create a new save file (check for other existing files first)
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_03);

        if (!saveFileDataWriter.CheckToSeeFileExists())
        {
            //Debug.Log("2");
            //if this profile slot is not taken, make a new one using this slot
            currentCharaterSlotBeingUsed = CharacterSlot.CharacterSlot_03;

            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }


        //Check to see if we can create a new save file (check for other existing files first)
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_04);

        if (!saveFileDataWriter.CheckToSeeFileExists())
        {
            //Debug.Log("2");
            //if this profile slot is not taken, make a new one using this slot
            currentCharaterSlotBeingUsed = CharacterSlot.CharacterSlot_04;

            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }


        //Check to see if we can create a new save file (check for other existing files first)
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_05);

        if (!saveFileDataWriter.CheckToSeeFileExists())
        {
            //Debug.Log("2");
            //if this profile slot is not taken, make a new one using this slot
            currentCharaterSlotBeingUsed = CharacterSlot.CharacterSlot_05;

            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }


        //Check to see if we can create a new save file (check for other existing files first)
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_06);

        if (!saveFileDataWriter.CheckToSeeFileExists())
        {
            //Debug.Log("2");
            //if this profile slot is not taken, make a new one using this slot
            currentCharaterSlotBeingUsed = CharacterSlot.CharacterSlot_06;

            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }


        //Check to see if we can create a new save file (check for other existing files first)
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_07);

        if (!saveFileDataWriter.CheckToSeeFileExists())
        {
            //Debug.Log("2");
            //if this profile slot is not taken, make a new one using this slot
            currentCharaterSlotBeingUsed = CharacterSlot.CharacterSlot_07;

            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }


        //Check to see if we can create a new save file (check for other existing files first)
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_08);

        if (!saveFileDataWriter.CheckToSeeFileExists())
        {
            //Debug.Log("2");
            //if this profile slot is not taken, make a new one using this slot
            currentCharaterSlotBeingUsed = CharacterSlot.CharacterSlot_08;

            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }


        //Check to see if we can create a new save file (check for other existing files first)
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_09);

        if (!saveFileDataWriter.CheckToSeeFileExists())
        {
            //Debug.Log("2");
            //if this profile slot is not taken, make a new one using this slot
            currentCharaterSlotBeingUsed = CharacterSlot.CharacterSlot_09;

            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }

        //Check to see if we can create a new save file (check for other existing files first)
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_10);

        if (!saveFileDataWriter.CheckToSeeFileExists())
        {
            //Debug.Log("2");
            //if this profile slot is not taken, make a new one using this slot
            currentCharaterSlotBeingUsed = CharacterSlot.CharacterSlot_10;

            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }


        // If there are no free slots, notify the player
        TitleScreenManager.instance.DisplayNoFreeCharacterSlotsPopUp();

    }
    public void LoadGame()
    {
        // Load a previous file, with a file name depending on which slot we are using
        saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(currentCharaterSlotBeingUsed);

        saveFileDataWriter = new SaveFileDataWriter();
        //generally works on multiple machine type(Application.PresitentDataPath)
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;
        currentCharacterData = saveFileDataWriter.LoadSaveFile();

        StartCoroutine(LoadWorldScene());

    }
    public void SaveGame()
    {
        // save the current file under a file name depending on which slot we are using
        saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(currentCharaterSlotBeingUsed);

        saveFileDataWriter = new SaveFileDataWriter();
        //generally works on multiple machine type(Application.PresitentDataPath)
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;

        // Pass the players info, from game, to their save file
        player.SaveGameDataFromCurrentCharacterData(ref currentCharacterData);
        //write that info onto a json file, saved to this machine

        saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);

    }

    public void DeleteGame(CharacterSlot characterSlot)
    {
        // Choose file based on name
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(characterSlot);

        saveFileDataWriter.DeleteSaveFile();

    }

    // Load all character profiles on device when  starting game
    private void LoadAllChaaracterProfiles()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_01);
        characterSlot01 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_02);
        characterSlot02 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_03);
        characterSlot03 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_04);
        characterSlot04 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_05);
        characterSlot05 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_06);
        characterSlot06 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_07);
        characterSlot07 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_08);
        characterSlot08 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_09);
        characterSlot09 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(CharacterSlot.CharacterSlot_10);
        characterSlot10 = saveFileDataWriter.LoadSaveFile();
    }

    public IEnumerator LoadWorldScene()
    {
        //print("what the1");
        // if you just want 1 world scene use this
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
        //print(currentCharacterData.characterName);

        // if you want to use different scenes for levels in your project use this
        //AsyncOperation loadOperation = SceneManager.LoadSceneAsync(currentCharacterData.sceneIndex);
        player.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);

        yield return null;
    }
 
    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }



}
