using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleGunPowerup : MonoBehaviour
{
    private Camera cam;
    public GameObject playerGrappleGun;

    private void Start()
    {
        cam = Camera.main;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("got gun!");
        if (collision.gameObject.CompareTag("Player"))
        {
            EnableGun();
            var camAudio = cam.gameObject.GetComponent<MusicStartToLoop>();

            // Next music track
            camAudio.startClip = camAudio.level3StartClip;
            camAudio.loopClip = camAudio.level3LoopClip;
            camAudio.SwitchBGM();
        }
    }

    public void EnableGun()
    {
        playerGrappleGun.GetComponent<GrappleGun>().enabled = true;
        playerGrappleGun.GetComponent<SpriteRenderer>().enabled = true;
        playerGrappleGun.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
