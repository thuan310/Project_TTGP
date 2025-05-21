using UnityEngine;

public class AICharacterAnimatorManager : CharacterAnimatorManager
{
    public float speed = 1f;
    AICharacterManager aiCharacter;

    protected override void Awake()
    {
        base.Awake();
        aiCharacter = GetComponent<AICharacterManager>();
    }
    private void OnAnimatorMove()
    {
        // đoạn này chưa hiểu rõ, cần đọc lại sau
        if(!aiCharacter.isGrounded) 
            return;

        Vector3 velocity = aiCharacter.animator.deltaPosition*speed;

        aiCharacter.characterController.Move(velocity);
        aiCharacter.transform.rotation *= aiCharacter.animator.deltaRotation;

    }
}
