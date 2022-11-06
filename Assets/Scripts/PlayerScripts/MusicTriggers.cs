using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggers : MonoBehaviour
{
    private Camera cam;
    public bool fadeOut;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            if (fadeOut)
            {
                print("fading out...");
                StartCoroutine(cam.GetComponent<MusicFades>().FadeOut());
            }
            else
            {
                print("fading in...");
                StartCoroutine(cam.GetComponent<MusicFades>().FadeIn());
            }
        }
    }
}
