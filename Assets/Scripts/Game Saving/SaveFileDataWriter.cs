using UnityEngine;
using System;
using System.IO;
using System.Linq.Expressions;

[System.Serializable]
//Since we want to reference this data for every save file, this script is not a monobehaviour and is instead serializable
public class SaveFileDataWriter
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";

    //Before we create a new save file, we must check to see if one of this character slot already exists(Max 10 character slots)
    public bool CheckToSeeFileExists()
    {
        if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    //used to delete character slots
    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
    }

    // Used to create a save file upon strating a new game
    public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
    {
        // make a path to save the file (A location on the machine)
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

        try
        {
            // create the directory the file will be ritten to, if it does not already exist
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("Crating save file at save path:" + savePath);

            // Serialize the C# game data object into Json
            string dataToScore = JsonUtility.ToJson(characterData, true);

            //write the file to our system
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.Write(dataToScore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error while trying to save character data, game not saved" + savePath + "\n" + e);
        }
    }

    //used to load a save file upon loading a previous game
    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterData = null;
        // make a path to save the file (A location on the machine)
        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if (File.Exists(loadPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                // Deserialize the data from Json back to unity c#
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            }
            catch (Exception e)
            {
                
            }
        }

        return characterData;
        
    }



}

