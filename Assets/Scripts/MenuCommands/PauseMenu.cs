using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool paused = false, homeOpen = false, pauseLastActive = false;
    private Canvas pauseCanvas;
    public Canvas ingameCanvas;
    public Canvas homeCanvas;
    public AudioSource pauseSFX;
    public AudioSource unpauseSFX;

    // Start is called before the first frame update
    void Start()
    {
        pauseCanvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!homeOpen) {
            if (paused)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;

            pauseCanvas.enabled = paused;
            ingameCanvas.enabled = !paused;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            homeCanvas.enabled = false;
            PauseUnpause();
        }
    }

    public void PauseUnpause() {
        paused = !paused;

        if (paused)
            pauseSFX.Play();
        else
            unpauseSFX.Play();

        homeOpen = false;
    }

    public void QuitToHome() {
        if (pauseCanvas.enabled)
            pauseLastActive = true;
        else
            pauseLastActive = false;

        paused = true;
        homeOpen = true;
        pauseCanvas.enabled = false;
        ingameCanvas.enabled = false;
        homeCanvas.enabled = true;
        Time.timeScale = 0;
    }

    public void CancelQuitToHome() {
        homeOpen = false;
        homeCanvas.enabled = false;

        if (pauseLastActive)
            pauseCanvas.enabled = true;
        else {
            paused = false;
            ingameCanvas.enabled = true;
        }
    }

    public void BackToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
