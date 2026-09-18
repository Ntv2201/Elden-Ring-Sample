using System;
using System.ComponentModel;
using System.Security;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    [SerializeField] private Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;
    PlayerControls playerControls;

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
        }
        playerControls.Enable();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void Update()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
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
    }

}
