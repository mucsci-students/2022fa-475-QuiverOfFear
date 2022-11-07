using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableCounter : MonoBehaviour
{
    private int collected = 0;
    public int world;
    [SerializeField] Sprite[] digits;

    private void OnEnable()
    {
        CalculateCurrentCollectables();
    }

    private void OnDisable()
    {
        collected = 0;
    }

    void CalculateCurrentCollectables()
    {
        for (int i = 0; i < 6; ++i)
        {
            if (PlayerPrefs.GetInt("powerup" + world + i) == 1)
                ++collected;
        }

        transform.Find("Collected").Find("Tens").GetComponent<Image>().sprite = digits[collected / 10];
        transform.Find("Collected").Find("Ones").GetComponent<Image>().sprite = digits[collected % 10];
    }
}
