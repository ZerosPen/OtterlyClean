using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    private bool pasueIsOpen = false;
    public AudioMixer audioMixer;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape pressed");
            if (!pasueIsOpen)
            {
                Pause();
                pasueIsOpen = true;
            }
            else
            {
                Continue();
            }
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        DialogueManager.instance.GamePause();
        StartCoroutine(DelayPause());
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        DialogueManager.instance.GameContinue();
        pasueIsOpen = false;
    }

    public void changetoMainMenu()
    {
        pausePanel.SetActive(false);
        GameManager.Instance.GameOff();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
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

    IEnumerator DelayPause()
    {
        yield return new WaitForSeconds(0.75f);
        Time.timeScale = 0f;
    }
}
