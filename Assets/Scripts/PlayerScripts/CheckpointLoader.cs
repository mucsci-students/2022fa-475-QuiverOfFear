using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointLoader : MonoBehaviour
{
    void OnEnable()
    {
        int checkpoint = PlayerPrefs.GetInt("level");

        switch(checkpoint)
        {
            case 1:
                transform.position = new Vector2(130f, 6f);
                break;
            default:
                break;
        }
    }
}
