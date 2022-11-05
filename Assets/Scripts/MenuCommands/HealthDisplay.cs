using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public Sprite[] nums;
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        UpdateNumber(transform.Find("CurrentHealth"), player.GetComponent<PlayerHealth>().health);
        UpdateNumber(transform.Find("TotalHealth"), player.GetComponent<PlayerHealth>().maxHealth);
    }

    void UpdateNumber(Transform display, int num)
    {
        UpdateDigit(display.Find("Tens"), num / 10);
        UpdateDigit(display.Find("Ones"), num % 10);
    }

    void UpdateDigit(Transform display, int num)
    {
        display.gameObject.GetComponent<Image>().sprite = nums[num];
    }
}
