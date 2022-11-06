using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int health;

    public float colorChangeTimer;
    SpriteRenderer spriteRenderer;
    Color defaultColor;
    
    private Rigidbody2D m_rb;
    private Animator m_anim;

    [SerializeField]
    private float invincibilityDeltaTime;
    [SerializeField]
    private float invincibilityDurationSeconds;
    private bool isInvincible;
    private CheckpointLoader respawn;

    // Start is called before the first frame update
    void Start()
    {
        // health = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        m_rb = GetComponent<Rigidbody2D>();
        respawn = GetComponent<CheckpointLoader>();
        m_anim = GetComponent<Animator>();

        // Makes sure player health is never under the max health
        if (PlayerPrefs.GetInt("health") < maxHealth)
            PlayerPrefs.SetInt("health", maxHealth);

        maxHealth = PlayerPrefs.GetInt("health");
    }

    // Other enemies call this function for damage.
    public void UpdateHealth(int mod) {

        if (isInvincible) return;

        // print(mod);
        if(health + mod < health);
        {
            health+=mod;
        
            // Dead
            if (health <= 0) {
                health = 0;
                // Set animator settings to play death animation.
                m_anim.SetBool("isDead", true);
                m_anim.SetTrigger("die");

                // Make player fall down haha.
                m_rb.gravityScale = 20f;
                StartCoroutine(PlayerDied());
            }
            
            // Don't want player invincible at 0 hp
            if (health > 0)
            {
                StartCoroutine(BecomeTemporarilyInvincible());
            }
        }
        if(health + mod > health) {
        
            health +=mod;
            
            // Don't wanna overheal.
            if(health > maxHealth) {
                health = maxHealth;
            }
        }
    }

    private IEnumerator PlayerDied()
    {
        // Need the timer so we can play animations...
        yield return new WaitForSeconds(2f);
        
        gameObject.SetActive(false);
        m_anim.ResetTrigger("die");
        m_rb.gravityScale = 3f;

        m_anim.SetBool("isDead", false);
        m_anim.SetTrigger("respawn");
        m_anim.ResetTrigger("respawn");
        respawn.Respawn(true);
    }          

    private IEnumerator BecomeTemporarilyInvincible()
    {
        Debug.Log("Player turned invincible!");
        isInvincible = true;

        for (float i = 0; i < invincibilityDurationSeconds; i += invincibilityDeltaTime) {
            
            // It make it look flashy.
            float emission = Mathf.PingPong(2 * Time.time, 1f);
            
            // Color we're changing to.
            Color baseColor = Color.red;
            // It make it look flashy 2.
            Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
            // Set new color.
            spriteRenderer.color = finalColor;
            
            yield return new WaitForSeconds(invincibilityDeltaTime);
        }
        
        Debug.Log("Player is no longer invincible!");
        
        // Change him back to his normal color
        spriteRenderer.color = defaultColor;

        isInvincible = false;
    }
}