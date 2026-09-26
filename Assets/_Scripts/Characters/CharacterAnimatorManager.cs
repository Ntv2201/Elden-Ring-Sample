using Unity.Netcode;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    readonly int HORIZONTAL = Animator.StringToHash("Horizontal");
    readonly int VERTICAL = Animator.StringToHash("Vertical");
    CharacterManager character;
    int horizontal;
    int vertical;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        
    }

    public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float horizontalAmount = horizontalMovement;
        float verticalAmount = verticalMovement;

        if (isSprinting)
        {
            verticalAmount = 2;
        }

        character.animator.SetFloat(HORIZONTAL, horizontalAmount, .1f, Time.deltaTime);
        character.animator.SetFloat(VERTICAL, verticalAmount, .1f, Time.deltaTime);

    }

    public virtual void PlayerTargetActionAnimation(
        string targetAnimation, 
        bool isPerformingAction = false,
        bool applyRootMotion = false,
        bool canRotate = false, 
        bool canMove = false)
    {
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, .2f);


        // can be used to stop character from attempting new actions
        // for example: if u get damaged and begin to perform a damage action
        // this flag is turn true if you're stunned
        // we can then check for this before attempting new actions
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;

        // tell the server/host we played an animation and play that animation for everyone else present
        character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }
}
