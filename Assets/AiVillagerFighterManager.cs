using System.Collections;
using UnityEngine;

public class AiVillagerFighterManager : AICharacterManager
{
    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        currentHealth.Value = 0;
        isDead.Value = true;

        // reset any flags here that need to be reset
        // nothing

        // if we are not grounded, play an aerial Death animation
        if (!manuallySelectDeathAnimation)
        {
            characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
        }

        SceneNavigationManager.instance.BringBackToPreviousPlace();

        yield return new WaitForSeconds(5);
    }

}
