using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public int checkpointNo;
    public GameObject ui;

    private void OnEnable()
    {
        // Disables this object if they spawn directly into it
        if (PlayerPrefs.GetInt("level") != 0 && PlayerPrefs.GetInt("check") >= checkpointNo)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Player is at the next level
            PlayerPrefs.SetInt("level", checkpointNo);

            // Check if checkpoint num is lower than greatest checkpoint
            if (PlayerPrefs.GetInt("check") < checkpointNo)
                PlayerPrefs.SetInt("check", checkpointNo);

            // Show the checkpoint text
            ui.GetComponent<ImageFade>().ShowCheckpointText();

            // Disable object
            gameObject.SetActive(false);
        }
    }
}
