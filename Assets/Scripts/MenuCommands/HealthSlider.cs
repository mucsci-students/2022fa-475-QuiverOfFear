using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    private PlayerHealth playerHealth;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Slider>().value = playerHealth.health * 1.0f / playerHealth.maxHealth;
    }
}
