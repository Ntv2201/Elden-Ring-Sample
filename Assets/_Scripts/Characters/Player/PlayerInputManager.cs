using System;
using System.ComponentModel;
using System.Security;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    [Header("Camera movement input")]
    [SerializeField] private Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;
    PlayerControls playerControls;

    [Header("Player Movement Input")]
    [SerializeField] private Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("Player Actions Input")]
    [SerializeField] bool dodgeInput = false;   



    public static PlayerInputManager instance;
    public PlayerManager player;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject); 
        
        // when the scene changes, run this logic
        SceneManager.activeSceneChanged += OnSceneChange;
    
        instance.enabled = false;
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        // if we're loading into our world scene, enable players controls
        if(newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;
        }
        // otherwise, disable our player controls
        //
        else
        {
            instance.enabled = false;
        }
    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;

        }
        playerControls.Enable();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    // if window is minimized or lowered, stop adjusting inputs
    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            if (focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    private void Update()
    {
        HandleAllInput();
    }

    private void HandleAllInput()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
    }

    // MOVEMENT
    private void HandlePlayerMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        // return the absolute number
        moveAmount = Mathf.Clamp01(MathF.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        // clamp the value, so they are 0, 0.5 or 1 (optional)
        if(moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if(moveAmount > .5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }

        if(player == null) return;

        // why 0 on the horizontal? cuz we only want non-strafe movement
        // if not locked on, only use move amount
        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
        // if locked on, pass the horizontal as well

    }

    private void HandleCameraMovementInput()
    {
        cameraHorizontalInput = cameraInput.x;
        cameraVerticalInput = cameraInput.y;
    }

    // ACTIONS
    private void HandleDodgeInput()
    {
        if (dodgeInput)
        {
            dodgeInput = false;

            player.playerLocomotionManager.AttemptToPerformedDodge();
        }
    }
}
