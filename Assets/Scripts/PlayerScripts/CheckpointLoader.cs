using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointLoader : MonoBehaviour
{
    private Camera cam;
    public GameObject gun;
    public Transform mobs;

    void OnEnable()
    {
        cam = Camera.main;
        Respawn(false);
    }

    public void Respawn(bool reset)
    {
        // Calculates the player's latest checkpoint
        int checkpoint = PlayerPrefs.GetInt("level");
        GetComponent<PlayerHealth>().health = GetComponent<PlayerHealth>().maxHealth;
        gameObject.SetActive(true);
        cam.GetComponent<AudioSource>().volume = 1;

        switch (checkpoint)
        {
            // Back to checkpoint 1
            case 1:
                transform.position = new Vector2(130f, 6f);
                break;
            // Back to checkpoint 2
            case 2:
                transform.position = new Vector2(-12f, 12f);
                gun.GetComponent<GrappleGunPowerup>().EnableGun();
                break;
            // Back to the beginning
            default:
                transform.position = new Vector2(-5f, 0f);
                break;
        }

        foreach(Transform mob in mobs)
        {
            mob.gameObject.SetActive(true);
            mob.GetComponent<MobHealth>().health = mob.GetComponent<MobHealth>().maxHealth;

            if (reset)
                mob.transform.position = mob.GetComponent<MobRender>().startingPosition;
        }
    }
}
