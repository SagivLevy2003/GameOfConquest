using FMODUnity;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public void PlayOneShotSound(EventReference sound, Vector3 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }
}
