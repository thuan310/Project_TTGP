using UnityEngine;
using UnityEngine.TextCore.Text;


public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    int horizontal;
    int vertical;
    
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }


    public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float horizontalAmount = horizontalMovement;
        float verticalAmount = verticalMovement;  
        //print("dangra");
        //Option 1 // theem 0.1f và time.deltaTime để làm chuyển động khi sang idle mượt hơn
        if (isSprinting)
        {
            verticalAmount = 2;
        }
        character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);

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
}
