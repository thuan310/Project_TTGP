using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    PlayerManager player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }
    private void OnAnimatorMove()
    {   
        if(TimelineManager.instance == null)
        {
            return;
        }
        if (UnityEditor.EditorApplication.isPlaying && TimelineManager.instance.IsTimeLinePlaying())
        {
            player.transform.position += player.animator.deltaPosition;
            player.transform.rotation *= player.animator.deltaRotation;
        }
        else if (player.applyRootMotion)
        {
            Vector3 velocity = player.animator.deltaPosition;
            player.characterController.Move(velocity);
            player.transform.rotation *= player.animator.deltaRotation;
        }
    }

    // animation event calls
    public override void EnableCanDoCombo()
    {
        if (player.isUsingRightHand.Value)
        {
            player.playerCombatManager.canComboWithMainHandWeapon = true;
        }
        else
        {
            // envale off hand combo
        }
    }

    public override void DisableCanDoCombo()
    {
        player.playerCombatManager.canComboWithMainHandWeapon = false;
        //
    }
}
