using Unity.Netcode;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;
    [Header("NETWORK JOIN")]
    [SerializeField] bool startGameAsClient;

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
    }

    private void Update()
    {
        if (startGameAsClient)
        {
            startGameAsClient = false;
            // SHUTDOWN FIRST, BECAUSE WE STARTED A HOST DURING THE TITLE SCREEN
            NetworkManager.Singleton.Shutdown();
            // THEN START THE NETWORK
            NetworkManager.Singleton.StartClient();
        }
    }
}
