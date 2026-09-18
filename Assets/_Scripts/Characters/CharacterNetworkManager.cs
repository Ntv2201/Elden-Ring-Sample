using Unity.Netcode;
using UnityEngine;

public class CharacterNetworkManager : NetworkBehaviour
{
    [Header("Position")]
    public NetworkVariable<Vector3> netWorkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> netWorkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public Vector3 networkPositionVelocity;
    public float netWorkPositionSmoothTime = 0.1f;
    public float netWorkRotationSmoothTime = 0.1f;

}
