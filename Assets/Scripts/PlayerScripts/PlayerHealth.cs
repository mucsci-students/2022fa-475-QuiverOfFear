using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int health;
    SpriteRenderer spriteRenderer;
    Color defaultColor;
    bool isHit = false;
    public float colorChangeTimer;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }

    // Other enemies call this function
    public void UpdateHealth(int mod)
    {
        // If health is going down:
        if(health + mod <= health)
        {
            if(!isHit)
            {
                isHit = true;
                // Idk how to explain Coroutine. But we can run a function as if it were in Update()
                StartCoroutine("Flash");
            }
        }
        health += mod;

        if (health > maxHealth)
        {
            health = maxHealth;
        } else if (health <= 0)
        {
            health = 0;
            PlayerDied();
        }
    }
    
    private void PlayerDied()
    {
        gameObject.SetActive(false);
    }

    IEnumerator Flash(){
        
        // Duration to flash
        float duration = 0.5f;
        while(duration > 0)
        {
            //bAsE cAsE fOr nO iNFinIte L00p$
            duration -= Time.deltaTime;

            // It make it look flashy.
            float emission = Mathf.PingPong(2 * Time.time, 1f);
            
            // Color we're changing to
            Color baseColor = Color.red;
            // It make it look flashy 2.
            Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
            
            // Set new color.
            spriteRenderer.color = finalColor;

            // Coroutine uses this yield return thing. I understand my code.
            yield return new WaitForSeconds(colorChangeTimer);
            // Change him back to his normal color
            spriteRenderer.color = defaultColor;
            isHit = false;
        }
    }
}