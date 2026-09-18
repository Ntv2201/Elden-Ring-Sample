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

        // if we dont own this object, we dont control it
        if(!IsOwner) return;
        
        // Handle movement
        playerLocomotionManager.HandleAllMovement();
    }
}
