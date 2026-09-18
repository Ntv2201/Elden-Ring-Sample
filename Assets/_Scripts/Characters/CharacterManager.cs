using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : NetworkBehaviour
{
    public CharacterController characterController;
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this); 

        characterController = GetComponent<CharacterController>();
    }

    protected virtual void Update()
    {
        
    }
}
