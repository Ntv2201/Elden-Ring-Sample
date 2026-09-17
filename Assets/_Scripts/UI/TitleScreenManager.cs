using JetBrains.Annotations;
using Unity.Netcode;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public void StartNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }
}
