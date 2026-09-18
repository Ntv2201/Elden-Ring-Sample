using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;
    public float verticalMovement;
    public float horizontalMovement;
    public float moveAmount;
    private Vector3 moveDirection;

    [SerializeField] private float walkingSpeed = 2f;
    [SerializeField] private float runningSpeed = 5f;


    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    public void HandleAllMovement()
    {
        HandleGroundedMovement();
    }

    private void GetVerticalAndHorizontalInputs()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;

        // clamp the movements

    }

    private void HandleGroundedMovement()
    {
        GetVerticalAndHorizontalInputs();
        // move direction is based on camera perspective & and our movement input
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

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
