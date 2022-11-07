using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPower : MonoBehaviour
{
    public int world;
    public int powerNo;

    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("powerup" + world + powerNo) == 1)
        {
           gameObject.SetActive(false);
        }
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.CompareTag("Player"))
        {
            PlayerHealth healthStats = obj.GetComponent<PlayerHealth>();
            ++healthStats.maxHealth;
            healthStats.health = healthStats.maxHealth;
            PlayerPrefs.SetInt("health", healthStats.maxHealth);
            PlayerPrefs.SetInt("powerup" + world + powerNo, 1);
            gameObject.SetActive(false);
        }
    }
}
