using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHealth : MonoBehaviour
{
    private AudioSource hitSFX;

    public int health;
    public int maxHealth;
    public ParticleSystem damageEffect;


    SpriteRenderer spriteRenderer;
    Color defaultColor;
    private float colorChangeTimer;

    [SerializeField]
    private float invincibilityDeltaTime = 0.2f;
    [SerializeField]
    private float invincibilityDurationSeconds = 1.0f;
    private bool isInvincible;



    void Start()
    {
        health = maxHealth;
        hitSFX = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        colorChangeTimer = invincibilityDurationSeconds;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If an arrow comes in contact, deal a point of damage
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            UpdateHealth();

        }
        else if(collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth health))
        {
            health.UpdateHealth(-1);
        }
    }

    public void UpdateHealth() 
    {
        if (isInvincible) return;
        --health;
        hitSFX.Play();
        Instantiate(damageEffect, transform.position, Quaternion.identity);

        // Dead
        if(health == 0)
        {
            gameObject.SetActive(false);
        }
        if (health > 0)
        {
            StartCoroutine(BecomeTemporarilyInvincible());
        }
    }

        private IEnumerator BecomeTemporarilyInvincible()
        {
            Debug.Log("Enemy turned invincible!");
            isInvincible = true;

            for (float i = 0; i < invincibilityDurationSeconds; i += invincibilityDeltaTime) {
                
                // It make it look flashy.
                float emission = Mathf.PingPong(2 * Time.time, 1f);
                
                // Color we're changing to
                Color baseColor = Color.red;
                // It make it look flashy 2.
                Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
                
                // Set new color.
                spriteRenderer.color = finalColor;
                
                yield return new WaitForSeconds(invincibilityDeltaTime);
            }
            
            Debug.Log("Enemy is no longer invincible!");
            
            // Change him back to his normal color
            spriteRenderer.color = defaultColor;

            isInvincible = false;
    }

}
