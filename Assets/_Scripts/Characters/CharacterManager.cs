using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : NetworkBehaviour
{
    public CharacterController characterController;
    CharacterNetworkManager characterNetworkManager;
    
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this); 

        characterController = GetComponent<CharacterController>();
        characterNetworkManager = GetComponent<CharacterNetworkManager>();
    }

    protected virtual void Update()
    {
        // if this character is being controlled from our side, then assign its network of position to the position of our transform
        if (IsOwner)
        {
            characterNetworkManager.netWorkPosition.Value = transform.position;
            characterNetworkManager.netWorkRotation.Value = transform.rotation;
        }
        // if this character is being controlled from else where, then assign its position here locally by the position of its network transform
        else
        {
            // position
            transform.position = Vector3.SmoothDamp(transform.position, 
                                                    characterNetworkManager.netWorkPosition.Value, 
                                                    ref characterNetworkManager.networkPositionVelocity, 
                                                    characterNetworkManager.netWorkPositionSmoothTime);

            // rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                characterNetworkManager.netWorkRotation.Value, 
                                                characterNetworkManager.netWorkRotationSmoothTime);
        }
    }

    protected virtual void LateUpdate()
    {
        
    }

}
