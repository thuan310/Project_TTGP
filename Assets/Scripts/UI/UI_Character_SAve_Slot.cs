using UnityEngine;
using TMPro;
public class UI_Character_SAve_Slot : MonoBehaviour
{
    SaveFileDataWriter saveFileWriter;

    [Header("Game Slot")]
    public CharacterSlot characterSlot;

    [Header("Character Info")]
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI timedPLayed;

    private void OnEnable()
    {
        LoadSaveSlot();
    }

    private void LoadSaveSlot()
    {
        saveFileWriter = new SaveFileDataWriter();
        saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;

        //save slot 01
        if (characterSlot == CharacterSlot.CharacterSlot_01)
        { 
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(characterSlot);

            //if the file exists, get information from it
            if (saveFileWriter.CheckToSeeFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
            }
            // if it does not, disable this gameobject
            else
            {
                gameObject.SetActive(false);
            }
        }
        // save slot 02
        else if (characterSlot == CharacterSlot.CharacterSlot_02)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(characterSlot);

            //if the file exists, get information from it
            if (saveFileWriter.CheckToSeeFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot02.characterName;
            }
            // if it does not, disable this gameobject
            else
            {
                gameObject.SetActive(false);
            }
        }
        // save slot 03
        else if (characterSlot == CharacterSlot.CharacterSlot_03)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(characterSlot);

            //if the file exists, get information from it
            if (saveFileWriter.CheckToSeeFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot03.characterName;
            }
            // if it does not, disable this gameobject
            else
            {
                gameObject.SetActive(false);
            }
        }
        // save slot 04
        else if (characterSlot == CharacterSlot.CharacterSlot_04)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(characterSlot);

            //if the file exists, get information from it
            if (saveFileWriter.CheckToSeeFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot04.characterName;
            }
            // if it does not, disable this gameobject
            else
            {
                gameObject.SetActive(false);
            }
        }
        // save slot 05
        else if (characterSlot == CharacterSlot.CharacterSlot_05)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(characterSlot);

            //if the file exists, get information from it
            if (saveFileWriter.CheckToSeeFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot05.characterName;
            }
            // if it does not, disable this gameobject
            else
            {
                gameObject.SetActive(false);
            }
        }
        // save slot 06
        else if (characterSlot == CharacterSlot.CharacterSlot_06)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(characterSlot);

            //if the file exists, get information from it
            if (saveFileWriter.CheckToSeeFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot06.characterName;
            }
            // if it does not, disable this gameobject
            else
            {
                gameObject.SetActive(false);
            }
        }
        // save slot 07
        else if (characterSlot == CharacterSlot.CharacterSlot_07)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(characterSlot);

            //if the file exists, get information from it
            if (saveFileWriter.CheckToSeeFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot07.characterName;
            }
            // if it does not, disable this gameobject
            else
            {
                gameObject.SetActive(false);
            }
        }
        // save slot 08
        else if (characterSlot == CharacterSlot.CharacterSlot_08)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(characterSlot);

            //if the file exists, get information from it
            if (saveFileWriter.CheckToSeeFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot08.characterName;
            }
            // if it does not, disable this gameobject
            else
            {
                gameObject.SetActive(false);
            }
        }
        // save slot 09
        else if (characterSlot == CharacterSlot.CharacterSlot_09)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(characterSlot);

            //if the file exists, get information from it
            if (saveFileWriter.CheckToSeeFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot09.characterName;
            }
            // if it does not, disable this gameobject
            else
            {
                gameObject.SetActive(false);
            }
        }
        // save slot 10
        else if (characterSlot == CharacterSlot.CharacterSlot_10)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharactersSlotBeingUsed(characterSlot);

            //if the file exists, get information from it
            if (saveFileWriter.CheckToSeeFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot10.characterName;
            }
            // if it does not, disable this gameobject
            else
            {
                gameObject.SetActive(false);
            }
        }

    }

    public void LoadGameFromCharacterSlot()
    {
        WorldSaveGameManager.instance.currentCharaterSlotBeingUsed = characterSlot;
        WorldSaveGameManager.instance.LoadGame();
    }

    public void SelectCurrentSlot()
    {
        TitleScreenManager.instance.SelectCharacterSlot(characterSlot);
    }
}
