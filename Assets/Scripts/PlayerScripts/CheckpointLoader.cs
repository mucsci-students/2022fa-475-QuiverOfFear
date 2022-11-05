using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointLoader : MonoBehaviour
{
    public Transform mobs;

    void OnEnable()
    {
        Respawn(false);
    }

    public void Respawn(bool reset)
    {
        // Calculates the player's latest checkpoint
        int checkpoint = PlayerPrefs.GetInt("level");
        GetComponent<PlayerHealth>().health = GetComponent<PlayerHealth>().maxHealth;
        gameObject.SetActive(true);

        switch (checkpoint)
        {
            // Back to checkpoint 1
            case 1:
                transform.position = new Vector2(130f, 6f);
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
