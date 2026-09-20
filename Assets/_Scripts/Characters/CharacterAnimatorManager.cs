using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    readonly int HORIZONTAL = Animator.StringToHash("Horizontal");
    readonly int VERTICAL = Animator.StringToHash("Vertical");
    CharacterManager character;
    float horizontal;
    float vertical;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValues)
    {
        // option 1
        character.animator.SetFloat(HORIZONTAL, horizontalValue, .1f, Time.deltaTime);
        character.animator.SetFloat(VERTICAL, verticalValues, .1f, Time.deltaTime);
    }
}
