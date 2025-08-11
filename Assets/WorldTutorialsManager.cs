using Ink.Parsed;
using System.Collections.Generic;
using UnityEngine;

public class WorldTutorialsManager : MonoBehaviour
{
    public static WorldTutorialsManager instance;
    [Header("Tutorial Thu Vien")]
    [SerializeField] List<Tutorial> tutorialThuVien;
    [Header("Tutorial Village")]
    [SerializeField] List<Tutorial> tutorialVillage;
    [Header("Tutorial Camp")]
    [SerializeField] List<Tutorial> tutorialCamp;

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
    }

    public List<Tutorial> LoadPacksOfTutorial(int idx)
    {
        switch(idx)
        {
            case 1:
                return tutorialThuVien;
            case 2:
                return tutorialVillage;
            case 3:
                return tutorialCamp;
            default:
                Debug.Log("idx out of boundaries, so cann't load the tutorials");
                return null;
        }
        return null;

    }
}
