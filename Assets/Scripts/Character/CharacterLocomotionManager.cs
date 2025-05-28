using UnityEditor;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour {
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

    [Header("Flags")]
    public bool isRolling = false;

    private Vector3 originPosition; 
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

            inAirTimer = inAirTimer + Time.deltaTime;
            character.animator.SetFloat("InAirTimer", inAirTimer);

            yVelocity.y += gravityForce * Time.deltaTime;
        }

        //there should always be some force applied to the y velocity
        //Debug.Log("Before Move: " + character.characterController.transform.position);
        character.characterController.Move(yVelocity * Time.deltaTime);
        //Debug.Log("After Move: " + character.characterController.transform.position);
        originPosition = character.transform.position;

    }


    public float sphereOffset = 2f;
    protected void HandleGroundCheck()
    {
        character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRaidus, groundLayer);
        //print(Physics.CheckSphere(character.transform.position, groundCheckSphereRaidus, groundLayer));

        //Vector3 origin = character.transform.position + Vector3.up * sphereOffset; // Start ray slightly above feet
        //character.isGrounded = Physics.CheckSphere(origin, groundCheckSphereRaidus, groundLayer);
        ////float rayDistance = 3.0f; // Adjust based on character height and tolerance

        ////character.isGrounded = Physics.Raycast(origin, Vector3.down, rayDistance, groundLayer);
        //Debug.Log(character.isGrounded);
        ////Debug.DrawRay(origin, Vector3.down * rayDistance, Color.red, 2f);
        ///


        //float sphereCastOffset = 1f;         // Start height above feet
        //float groundCheckSphereRadius = 1f;  // Radius of the sphere
        //float groundCheckDistance = 0.6f;
        //Vector3 origin = originPosition + Vector3.up * sphereCastOffset;
        //Vector3 direction = Vector3.down;

        //character.isGrounded = Physics.SphereCast(
        //    origin,
        //    groundCheckSphereRadius,
        //    direction,
        //    out RaycastHit hit,
        //    groundCheckDistance,
        //    groundLayer
        //);
        //Debug.DrawRay(origin, direction * groundCheckDistance, Color.black, 0.1f);

        //Debug.Log(origin);
        //Debug.Log(character.isGrounded);



    }


    // draws our ground check sphere in scene view 
    protected void OnDrawGizmosSelected()
    {
        if (EditorApplication.isPlaying)
        {
            Gizmos.DrawSphere(character.transform.position, groundCheckSphereRaidus);
        }
        
    }

    public void EnableCanRotate()
    {
        character.canRotate = true;
    }

    public void DisableCanRotate()
    {
        character.canRotate = false;
    }

}
