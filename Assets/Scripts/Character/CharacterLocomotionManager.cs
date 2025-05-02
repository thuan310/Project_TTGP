using UnityEditor;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Ground Check & Jumping")]
    [SerializeField] protected float gravityForce = -5.55f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckSphereRaidus = 1;
    [SerializeField] protected Vector3 yVelocity; // the force at which our character is pulled up or down (Jumping or falling)
    [SerializeField] protected float groundedYVelocity = -20f; // the force at which our is sticking to the ground whilst they are grounded
    [SerializeField] protected float fallStartYVelocity = -5f; // the force at which our character begins to fall when they become ungrounded (rises as they fall longer)
    protected bool fallingVelocityHasBeenSet = false;
    protected float inAirTimer = 0;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    protected virtual void Update()
    {
        HandleGroundCheck();

        if (character.isGrounded)
        {
            // if we are not attempting to jump or move upward
            if (yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallingVelocityHasBeenSet = false;
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
            // if we are not jumping, and our falling velocity has not been set
            if (!character.isJumping && !fallingVelocityHasBeenSet)
            {
                fallingVelocityHasBeenSet = true;
                yVelocity.y = fallStartYVelocity;
            }

            inAirTimer = inAirTimer+ Time.deltaTime;
            character.animator.SetFloat("InAirTimer", inAirTimer);

            yVelocity.y += gravityForce * Time.deltaTime;
        }

        //there should always be some force applied to the y velocity
        character.characterController.Move(yVelocity * Time.deltaTime);
    }

    protected void HandleGroundCheck()
    {
        character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRaidus, groundLayer);
    }

    // draws our ground check sphere in scene view 
    protected void OnDrawGizmosSelected()
    {
        if (EditorApplication.isPlaying)
        {
            Gizmos.DrawSphere(character.transform.position, groundCheckSphereRaidus);
        }
        
    }
}
