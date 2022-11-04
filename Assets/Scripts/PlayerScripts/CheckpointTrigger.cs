using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public int checkpointNo;
    public GameObject ui;

    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("check") >= checkpointNo)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("check", checkpointNo);
            ui.GetComponent<ImageFade>().ShowCheckpointText();
            gameObject.SetActive(false);
        }
    }
}
