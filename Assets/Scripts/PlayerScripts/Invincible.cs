using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Invincible : MonoBehaviour
{
    private bool invincible = false;
    public float invincibilityTime = 2f;
    private Rigidbody2D m_rb;
    private GameObject feet;
    private float tempMass;
    private Transform center;

    [SerializeField] 
    private float knockbackStrength;

    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        center = GetComponent<Transform>();
    }
    
    void OnCollisionEnter2D(Collision2D collision) {
        if(!invincible) 
        {
            if(collision.gameObject.tag == "Enemies") 
            {
                StartCoroutine(Invulnerability(collision.gameObject));

                // Knock back player & enemy
                Vector2 playerDirection = (center.transform.position - collision.transform.position);
                Vector2 playerKnockback = playerDirection * knockbackStrength*0.9f;

                print("Player " + center.transform.position);
                print("Enemy " + collision.transform.position);
                
                // print(playerDirection);
                Vector2 enemyDirection = (collision.transform.position - transform.position).normalized;
                Vector2 enemyKnockback = enemyDirection * knockbackStrength;
                m_rb.gravityScale = 1;
                m_rb.mass = .5f;
                StartCoroutine(PlayerKnockBack(playerKnockback));
                StartCoroutine(KnockBack(playerKnockback, enemyKnockback));
                
            }
            else{
                print("No Collider");
            }
        }

         IEnumerator PlayerKnockBack(Vector2 playerKnockback)
        {
            Vector2 var = new Vector2 (.1f,.1f);
            playerKnockback = playerKnockback * var;
            m_rb.velocity = playerKnockback * 6f;
            yield return new WaitForSeconds(.09f);
            m_rb.velocity = Vector3.zero;
            m_rb.mass = 1f;
            m_rb.gravityScale = 3;

        }
        
        IEnumerator KnockBack(Vector2 playerKnockback, Vector2 enemyKnockback)
        {
            // m_rb.velocity = playerKnockback;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(enemyKnockback, ForceMode2D.Impulse);
            yield return new WaitForSeconds(1f);
            // m_rb.velocity = Vector3.zero;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        }
        
        IEnumerator Invulnerability(GameObject collider) 
        {
            print(collider.name);
            print(gameObject.name);
            invincible = true;

            Physics2D.IgnoreLayerCollision(6, 9, invincible);
            Physics2D.IgnoreLayerCollision(7, 9, invincible);

            yield return new WaitForSeconds(invincibilityTime);
        
            invincible = false;

            Physics2D.IgnoreLayerCollision(6, 9, invincible);
            Physics2D.IgnoreLayerCollision(7, 9, invincible);
        }

    }

}