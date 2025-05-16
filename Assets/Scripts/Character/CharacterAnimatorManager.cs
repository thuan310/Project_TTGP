using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;


public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    int horizontal;
    int vertical;

    [Header("Damage Animations")]
    public string lastDamagAnimationPlayed;

    [SerializeField] string hit_Forward_Medium_01 = "hit_Forward_Medium_01";
    [SerializeField] string hit_Forward_Medium_02= "hit_Forward_Medium_02";

    [SerializeField] string hit_Backward_Medium_01 = "hit_Backward_Medium_01";
    [SerializeField] string hit_Backward_Medium_02 = "hit_Backward_Medium_02";

    [SerializeField] string hit_Left_Medium_01 = "hit_Left_Medium_01";
    [SerializeField] string hit_Left_Medium_02 = "hit_Left_Medium_02";

    [SerializeField] string hit_Right_Medium_01 = "hit_Right_Medium_01";
    [SerializeField] string hit_Right_Medium_02 = "hit_Right_Medium_02";

    public List<string> forward_Medium_Damage = new List<string>();
    public List<string> backward_Medium_Damage = new List<string>();
    public List<string> left_Medium_Damage = new List<string>();
    public List<string> right_Medium_Damage = new List<string>();

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    protected virtual void Start()
    {
        forward_Medium_Damage .Add(hit_Forward_Medium_01);
        forward_Medium_Damage.Add(hit_Forward_Medium_02);

        backward_Medium_Damage.Add(hit_Backward_Medium_01);
        backward_Medium_Damage.Add(hit_Backward_Medium_02);

        left_Medium_Damage.Add(hit_Left_Medium_01);
        left_Medium_Damage.Add(hit_Left_Medium_02);

        right_Medium_Damage.Add(hit_Right_Medium_01);
        right_Medium_Damage.Add(hit_Right_Medium_02);

        //Debug.Log(character.characterAnimatorManager.forward_Medium_Damage.Count);
        //Debug.Log(character.characterAnimatorManager.backward_Medium_Damage.Count);
        //Debug.Log(character.characterAnimatorManager.left_Medium_Damage.Count);
        //Debug.Log(character.characterAnimatorManager.right_Medium_Damage.Count);
    }

    public string GetRandomAnimationFromList(List<string> animationList)
    {
        List<string> finalList = new List<string>();
        //print(animationList.Count);

        foreach (var item in animationList)
        {
            finalList.Add(item);
        }

        //print(finalList.Count);
        // check if we have already played this damage animation so it doesnt repeat
        finalList.Remove(lastDamagAnimationPlayed);

        // Check the list for null entries, and remove them
        for(int i  = finalList.Count - 1; i > -1; i --)
        {
            if (finalList[i] == null)
            {
                finalList.RemoveAt(i);
            }
        }

        int randomValue = Random.Range(0, finalList.Count);
        return finalList[randomValue];
    }

    public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float snappedHorizontalAmount;
        float snappedVerticalAmount;  
        //print("dangra");
        //Option 1 // theem 0.1f và time.deltaTime để làm chuyển động khi sang idle mượt hơn

        // this if chain will round the horizontal movement to -1, -0,5 , 0, 0,5 or 1

        if(horizontalMovement > 0 && horizontalMovement <= 0.5f)
            snappedHorizontalAmount = 0.5f;
        else if (horizontalMovement > 0.5f && horizontalMovement <= 1f)
            snappedHorizontalAmount = 1f;
        else if(horizontalMovement < 0 && horizontalMovement >= -0.5f)
            snappedHorizontalAmount = -0.5f;
        else if(horizontalMovement < -0.5f && horizontalMovement >= -1f)
            snappedHorizontalAmount = -1f;
        else 
            snappedHorizontalAmount = 0;

        // this if chain will round the vertical movement to -1, -0,5 , 0, 0,5 or 1

        if (verticalMovement > 0 && verticalMovement <= 0.5f)
            snappedVerticalAmount = 0.5f;
        else if (verticalMovement > 0.5f && verticalMovement <= 1f)
            snappedVerticalAmount = 1f;
        else if (verticalMovement < 0 && verticalMovement >= -0.5f)
            snappedVerticalAmount = -0.5f;
        else if (verticalMovement < -0.5f && verticalMovement >= -1f)
            snappedVerticalAmount = -1f;
        else
            snappedVerticalAmount = 0f;


        if (isSprinting)
        {
            snappedVerticalAmount = 2;
        }
        character.animator.SetFloat(horizontal, snappedHorizontalAmount, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, snappedVerticalAmount, 0.1f, Time.deltaTime);

        #region dùng khi cần thêm modify
        ////Option 2
        //float snappedHorizontal = 0;
        //float snappedVertical = 0;

        //#region Horizontal
        //// this if chain will round the horizontal movement to -1, -0.5, 0, 0.5 or 1

        //if (horizontalMovement > 0 && horizontalMovement <= 0.5f)
        //{
        //    snappedHorizontal = 0.5f;
        //}
        //else if(horizontalMovement > 0.5f && horizontalMovement <= 1f)
        //{
        //    snappedHorizontal = 1f;
        //}
        //else if(horizontalMovement < 0 && horizontalMovement >= -0.5f)
        //{
        //    snappedHorizontal = -0.5f;
        //}
        //else if(horizontalMovement < -0.5f && horizontalMovement >= -1f)
        //{
        //    snappedHorizontal = -1f;
        //}
        //else
        //{
        //    snappedHorizontal = 0;
        //}
        //#endregion

        //#region Vertical
        //// this if chain will round the horizontal movement to -1, -0.5, 0, 0.5 or 1

        //if (verticalMovement > 0 && verticalMovement <= 0.5f)
        //{
        //    snappedVertical = 0.5f;
        //}
        //else if (verticalMovement > 0.5f && verticalMovement <= 1f)
        //{
        //    snappedVertical = 1f;
        //}
        //else if (verticalMovement < 0 && verticalMovement >= -0.5f)
        //{
        //    snappedVertical = -0.5f;
        //}
        //else if (verticalMovement < -0.5f && verticalMovement >= -1f)
        //{
        //    snappedVertical = -1f;
        //}
        //else
        //{
        //    snappedVertical = 0;
        //}

        //character.animator.SetFloat("Horizontal", snappedHorizontal);
        //character.animator.SetFloat("Vertical", snappedVertical);
        //#endregion
        #endregion
    }
    public virtual void PlayTargetActionAnimation(
       string targetAnimation,
       bool isPerformingAction,
       bool applyRootMotion = true,
       bool canRotate = false,
       bool canMove = false)
    {
        //Debug.Log("play Animation" + targetAnimation);
        character.applyRootMotion = applyRootMotion;
        //print(character.applyRootMotion);
        character.animator.CrossFade(targetAnimation, 0.2f);
        // Canbe used to stop character from attempting new actions
        // For example, if you get damaged, and begin performing a damage animation
        // This flag will turn true if you are stunned
        // We can then check for this before attempting new actions
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;

    }

    public virtual void PlayTargetAttackActionAnimation( AttackType attackType,
   string targetAnimation,
   bool isPerformingAction,
   bool applyRootMotion = true,
   bool canRotate = false,
   bool canMove = false)
    {
        // Keep track of last attack performed ( For Combos)
        // Keep track of currnt attack type (light, heavy, ect)
        // update aniamtion set to current weapons animations
        // decide if our attack can be parried
        
        character.characterCombatManager.currentAttackType = attackType;
        character.characterCombatManager.lastAttackAnimationPerformed = targetAnimation;
        character.applyRootMotion = applyRootMotion;
        //print(character.applyRootMotion);
        character.animator.CrossFade(targetAnimation, 0.2f);
        // Canbe used to stop character from attempting new actions
        // For example, if you get damaged, and begin performing a damage animation
        // This flag will turn true if you are stunned
        // We can then check for this before attempting new actions
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;

    }

    public virtual void EnableCanDoCombo()
    {
    }

    public virtual void DisableCanDoCombo()
    {
        //
    }
}
