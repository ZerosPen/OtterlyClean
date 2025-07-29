using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    private bool pasueIsOpen = false;

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

    IEnumerator DelayPause()
    {
        yield return new WaitForSeconds(0.75f);
        Time.timeScale = 0f;
    }
}
