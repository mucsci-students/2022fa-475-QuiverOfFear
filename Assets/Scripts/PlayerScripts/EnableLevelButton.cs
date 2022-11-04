using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableLevelButton : MonoBehaviour
{
    public int checkNo;

    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("check") >= checkNo)
        {
            GetComponent<Button>().interactable = true;
        }
    }
}
