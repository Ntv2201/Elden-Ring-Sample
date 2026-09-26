using Unity.VisualScripting;
using UnityEngine;

public class CharacterSFXManager : MonoBehaviour
{
    private AudioSource audioSource;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRollSFX()
    {
        audioSource.PlayOneShot(WorldSFXManager.instance.rollSFX);
    }
}
