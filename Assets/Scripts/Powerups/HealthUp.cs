using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void increaseHealth(float healthIncrease)
    {
        Debug.Log("Max health before: " + maxHealth);
        maxHealth += healthIncrease;
        Debug.Log("Max health after: " + maxHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If player picks up a health powerup, add a new permanent health point.
        if(collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth maxHealth))
        {
            print("Quiver gained a healthpoint!");
            increaseHealth();
            Destroy (gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

