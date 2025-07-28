using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    private SoundLIbrary sfxLibary; 
    [SerializeField]
    private AudioSource sfx2dLibary;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void playSound3D (AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint (clip, pos);
        }
    }

    public void PlaySound3D (string soundName, Vector3 pos)
    {
        playSound3D(sfxLibary.GetClipFromName(soundName), pos);
    }

    public void PlaySound2D (string soundName)
    {
        sfx2dLibary.PlayOneShot(sfxLibary.GetClipFromName (soundName));
    }
}
