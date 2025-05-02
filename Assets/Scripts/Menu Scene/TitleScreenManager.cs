using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TitleScreenManager : MonoBehaviour
{
    [Header("Prefabs Player")]
    [SerializeField] private GameObject playerPrefab ;

    public static TitleScreenManager instance;
    [Header("Menus")]
    [SerializeField] GameObject titleScreenMainMenu;
    [SerializeField] GameObject titledScreenLoadMenu;

    [Header("Buttons")]
    [SerializeField] Button loadMenuReturnButton;
    [SerializeField] Button mainMenunewGameButton;
    [SerializeField] Button mainMenuLoadGameButton;
    [SerializeField] Button deleteCharacterPopUpConfirmButton;

    [Header("Pop Ups")]
    [SerializeField] GameObject noCharacterSlotsPopUp;
    [SerializeField] Button noCharacterSlotsOkayButton;
    [SerializeField] GameObject deleteCharacterSlotPopUP;

    [Header("Character Slots")]
    public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreatePlayer()
    {
        Instantiate(playerPrefab, new Vector3 (0,2,0), Quaternion.identity);
    }

    public void StartNewGame()
    {
        WorldSaveGameManager.instance.AttemptToCreateNewGame();
    }

    public void OpenLoadGameMenu()
    {
        //Close main menu
        titleScreenMainMenu.SetActive(false);

        // Open Load menu
        titledScreenLoadMenu.SetActive(true);

        // select the return button first
        loadMenuReturnButton.Select();
        // 
    }

    public void CloseLoadGame()
    {
        // Close Load menu
        titledScreenLoadMenu.SetActive(false);

        // Open main menu
        titleScreenMainMenu.SetActive(true);

        // select the load button
        mainMenuLoadGameButton.Select();
        // 
    }

    public void DisplayNoFreeCharacterSlotsPopUp()
    {
        noCharacterSlotsPopUp.SetActive(true);
        noCharacterSlotsOkayButton.Select();
    }

    public void CloseNoFreeCharacterSlotsPopUp()
    {
        //print("toi da ra");
        noCharacterSlotsPopUp.SetActive(false);
        mainMenunewGameButton.Select();
    }

    // Character SLots

    public void SelectCharacterSlot(CharacterSlot characterSlot)
    {
        currentSelectedSlot = characterSlot;
    }

    public void SelectNoSlot()
    {
      currentSelectedSlot = CharacterSlot.NO_SLOT;
    }

    public void AttemptToDeleteCharacterSlot()
    {
        if (currentSelectedSlot != CharacterSlot.NO_SLOT)
        {
            deleteCharacterSlotPopUP.SetActive(true);
            deleteCharacterPopUpConfirmButton.Select();
        }
    }

    public void DeleteCharacterSlot()
    {
        deleteCharacterSlotPopUP.SetActive(false);
        WorldSaveGameManager.instance.DeleteGame(currentSelectedSlot);

        // we disable and the enable the load menu, to refresh the slots( the deleted slots will now become inactive)
        titledScreenLoadMenu.SetActive(false );
        titledScreenLoadMenu.SetActive(true);
        loadMenuReturnButton.Select();
    }
    public void CloseDeleteCharacterPopUP()
    {
        deleteCharacterSlotPopUP.SetActive(false);
        loadMenuReturnButton.Select();
    }
}
