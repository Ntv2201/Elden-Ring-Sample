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

    protected override void LateUpdate()
    {
        if(!IsOwner) return;

        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraGameActions();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // if this player object owned by this client
        if (IsOwner)
        {
            PlayerCamera.instance.player = this;
        }
    }

}
