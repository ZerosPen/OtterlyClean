using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField]
    private MusicLibrary musicLibrary;
    [SerializeField]
    private AudioSource musicSourec;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayMusic(string nameTrack, float fadeDuration = 0.2f)
    {
        StartCoroutine(AnimatedMusicCrossFade(musicLibrary.GetClipFromName(nameTrack), fadeDuration));
    }

    IEnumerator AnimatedMusicCrossFade(AudioClip nextTrack, float fadeDuration = 0.5f)
    {
        float percent = 0f;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSourec.volume = Mathf.Lerp(1f, 0, percent);
            yield return null;
        }

        musicSourec.clip = nextTrack;
        musicSourec.Play();

        percent = 0f;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSourec.volume = Mathf.Lerp(0, 1f, percent);
            yield return null;
        }
    }
}
