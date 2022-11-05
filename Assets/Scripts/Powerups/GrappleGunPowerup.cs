using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleGunPowerup : MonoBehaviour
{
    private Camera cam;
    public GameObject playerGrappleGun;
    public AudioClip startClip;
    public AudioClip loopClip;

    private void Start()
    {
        cam = Camera.main;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("got gun!");
        if (collision.gameObject.CompareTag("Player"))
        {
            playerGrappleGun.GetComponent<GrappleGun>().enabled = true;
            playerGrappleGun.GetComponent<SpriteRenderer>().enabled = true;
            playerGrappleGun.transform.GetChild(0).gameObject.SetActive(true);

            // Next music track
            cam.gameObject.GetComponent<MusicStartToLoop>().startClip = startClip;
            cam.gameObject.GetComponent<MusicStartToLoop>().loopClip = loopClip;
            cam.gameObject.GetComponent<MusicStartToLoop>().SwitchBGM();
            Destroy(gameObject);
        }
    }
}
