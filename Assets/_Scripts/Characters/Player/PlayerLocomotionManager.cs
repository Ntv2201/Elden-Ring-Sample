using System.Security;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;
    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;

    [Header("Movement Settings")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] private float walkingSpeed = 2f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float sprintingSpeed = 7f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Dodge")]
    private Vector3 rollDirection;


    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (player.IsOwner)
        {
            player.characterNetworkManager.verticalMovement.Value = verticalMovement;
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.moveAmount.Value = moveAmount;
        }
        else
        {
            verticalMovement = player.characterNetworkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
            moveAmount = player.characterNetworkManager.moveAmount.Value;

            //if not locked on, pass move amount
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);

            // if locked on, pass both
        }
    }

    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        HandleRotation();
    }

    private void GetMovementValues()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;

        // clamp the movements

    }

    private void HandleGroundedMovement()
    {
        if(!player.canMove) return;
        
        GetMovementValues();
        // move direction is based on camera perspective & and our movement input
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (player.playerNetworkManager.isSprinting.Value)
        {
            player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
        }
        else
        {
            if(PlayerInputManager.instance.moveAmount > 0.5f)
            {
                // move at a running speed
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            else if(PlayerInputManager.instance.moveAmount <= 0.5f)
            {
                // move at a walking speed
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
        }

        
    }

    private void HandleRotation()
    {
        if(!player.canRotate) return;
        targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
        targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;

        if(targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }
        
        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }

    public void AttemptToPerformedDodge()
    {
        if(player.isPerformingAction) return;

        // if we're moving while attempting a dodge
        if(PlayerInputManager.instance.moveAmount > 0)
        {
            rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
            
            rollDirection.y = 0;
            rollDirection.Normalize();

            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;

            player.playerAnimatorManager.PlayerTargetActionAnimation("roll_forward", true, true);

            // perform a roll
        }
        else
        {
            // perform a backstep
            player.playerAnimatorManager.PlayerTargetActionAnimation("back_step", true, true);
        }
    }

    public void HandleSprinting()
    {
        if (player.isPerformingAction)
        {
            // set sprint to false
            player.playerNetworkManager.isSprinting.Value = false;
        }

        // if we're out of stamina, set sprint to false

        // if we're moving, sprint is true
        if(moveAmount >= .5f)
        {
            player.playerNetworkManager.isSprinting.Value = true;
        }

        // if not, sprint is false
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }

        // if we're moving set sprint to true

        //if we stationary set sprint to false
    }

}
