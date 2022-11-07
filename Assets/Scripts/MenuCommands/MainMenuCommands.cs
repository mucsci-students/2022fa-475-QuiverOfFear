using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCommands : MonoBehaviour
{
    private void Awake()
    {
        PlayerPrefs.DeleteAll();
    }

    public void StartGameWoods(int checkpoint)
    {
        PlayerPrefs.SetInt("level", checkpoint);
        SceneManager.LoadScene("CaveEntrance");
    }

    public void StartGameCaves(int checkpoint)
    {
        PlayerPrefs.SetInt("level", checkpoint);
        SceneManager.LoadScene("Cave");
    }

    public void QuitGame()
    {
        StartCoroutine(WaitForClose());
    }

    IEnumerator WaitForClose()
    {
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }
}
