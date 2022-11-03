using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleGunPowerup : MonoBehaviour
{
    public GameObject playerGrappleGun;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("got gun!");
        if (collision.gameObject.CompareTag("Player"))
        {
            playerGrappleGun.GetComponent<GrappleGun>().enabled = true;
            playerGrappleGun.GetComponent<SpriteRenderer>().enabled = true;
            playerGrappleGun.transform.GetChild(0).gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
