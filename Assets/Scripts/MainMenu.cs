using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider musicSlider;
    public Slider soundSlider;

    private void Start()
    {
        loadSettings();
        MusicManager.instance.PlayMusic("BGM");
    }

    public void Play()
    {
        SceneManager.LoadScene("MainGame");
        MusicManager.instance.PlayMusic("BGM2");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SfxVolume", volume);
    }

    public void SaveSettings()
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        audioMixer.GetFloat("SfxVolume", out float soundVolume);
        PlayerPrefs.SetFloat("SfxVolume", soundVolume);
    }

    public void loadSettings()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        soundSlider.value = PlayerPrefs.GetFloat("SfxVolume");
    }

}
