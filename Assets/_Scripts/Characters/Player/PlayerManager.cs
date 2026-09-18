using UnityEngine;

public class PlayerManager : CharacterManager
{
    private PlayerLocomotionManager playerLocomotionManager;

    protected override void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }

    protected override void Update()
    {
        base.Update();
        
        // Handle movement
        playerLocomotionManager.HandleAllMovement();
    }
}
