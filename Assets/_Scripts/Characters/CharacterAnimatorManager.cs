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

    public virtual void PlayerTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true)
    {
        character.animator.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, .2f);

        // can be used to stop character from attempting new actions
        // for example: if u get damaged and begin to perform a damage action
        // this flag is turn true if you're stunned
        // we can then check for this before attempting new actions
        
    }
}
