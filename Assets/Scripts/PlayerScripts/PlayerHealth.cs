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
    }

    // Other enemies call this function
    public void UpdateHealth(int mod) {

        if (isInvincible) return;
        --health; 

        // Dead
        if (health <= 0) {
            health = 0;
            m_anim.SetBool("isDead", true);
            m_rb.gravityScale = 20f;
            m_anim.SetTrigger("die");
            StartCoroutine(PlayerDied());
        }
        
        if(health > maxHealth) {
            // Don't over heal.
            health = maxHealth;
        }

        if (health > 0)
        {
            StartCoroutine(BecomeTemporarilyInvincible());
        }
    }

    private IEnumerator PlayerDied()
    {
        

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