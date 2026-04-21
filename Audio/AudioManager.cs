using FMODUnity;
using System;
using UnityEngine;

[DisallowMultipleComponent]
public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    public void PlayOneShot(EventReference eventReference, Vector3 position)
    {
        RuntimeManager.PlayOneShot(eventReference, position);
    }
}
