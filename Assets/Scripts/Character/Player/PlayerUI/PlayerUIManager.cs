using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PlayerUIManager : MonoBehaviour
{
    #region UI knowledge
    //1. Diegetic UI
    //Visible inside the game world, the character can "see" it too.

    //It's part of the game environment and feels immersive.

    //✅ Examples:

    //A hologram map your character projects in a sci-fi game.

    //Ammo counter shown on a gun (like in Dead Space).

    //🧭 2. Non-Diegetic UI
    //Traditional HUD elements, not part of the game world.

    //The player sees it, but characters don’t.

    //✅ Examples:

    //Health bar, minimap, XP bar

    //Quest objectives shown on screen

    //Inventory shown in a floating menu

    //🎮 3. Spatial UI
    //UI that's anchored in the game world, but characters might not directly interact with it.

    //Usually appears near an object, NPC, or area.

    //✅ Examples:

    //A floating name tag or health bar above an enemy’s head

    //Interaction prompts like “Press E to open door”

    //Dialog choices appearing above a character

    //🧠 4. Meta UI
    //UI that reflects the character’s state or abstract information.

    //May be stylized or affect the game screen directly.

    //✅ Examples:

    //Screen turns red when you’re low on health

    //Blurred vision when stunned or poisoned

    //Audio distortion when near danger

    //      Static HUD              Always visible                      Health bar in FPS
    //      Dynamic HUD             Appears only when needed            Ammo count only when shooting
    //      Context-sensitive UI    Changes depending on what's around	Button prompts that change based on interactables

    #endregion

    public static PlayerUIManager instance;

    [HideInInspector] public PlayerUIHUDManager playerUIHUDManager;
    [HideInInspector] public PlayerUIDynamicHUDManager playerUIDynamicHUDManager;
    [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;

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

        playerUIHUDManager = GetComponentInChildren<PlayerUIHUDManager>();
        playerUIDynamicHUDManager = GetComponentInChildren<PlayerUIDynamicHUDManager>();
        playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        playerUIDynamicHUDManager.ControlUI();
    }

}
