using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/ TEst Action")]
public class WeaponItemAction : ScriptableObject
{
    public int actionID;

    public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        // Waht does every weapon action have in common?
        // 1. we should always keep trach of which is currently being used
        playerPerformingAction.currentWeaponBeingUsed.Value = weaponPerformingAction.itemID;

        //Debug.Log("The Action Has Fired");
    }
}

