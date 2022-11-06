using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Invincible : MonoBehaviour
{
    private Rigidbody2D m_rb;

    [SerializeField] 
    private float knockbackStrength;

    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }
    
    void OnCollisionEnter2D(Collision2D collision) {

        //Debug.Log(center.name);
        if(collision.gameObject.tag == "Enemies") 
        {
            Vector3 playerDirection;
            Vector3 playerKnockDistance;
            Vector3 playerKnockback;
            Vector3 enemyDirection;
            Vector3 enemyKnockDistance;
            Vector3 enemyKnockback;

            if(transform.position.x < collision.transform.position.x)
            {
                playerKnockDistance = new Vector3(-5.0f, 5.0f,0f);
                enemyKnockDistance = new Vector3(5.0f, 5.0f,0f);
            }
            else
            {
                playerKnockDistance = new Vector3(5.0f, 5.0f,0f);
                enemyKnockDistance = new Vector3(-5.0f, 5.0f,0f);
            }
            // Knock back player & enemy
            playerDirection = (playerKnockDistance - transform.position);
            playerKnockback = playerDirection * knockbackStrength;

            PlayerKnockBack(playerKnockback, knockbackStrength);
            print("Player " + transform.position);
            print("Enemy " + collision.transform.position);
                
            print(playerDirection);
            enemyDirection = (playerKnockDistance - collision.transform.position);
            enemyKnockback = enemyDirection * knockbackStrength;
            EnemyKnockBack(enemyKnockback, knockbackStrength);
                
        }

        void PlayerKnockBack(Vector2 playerKnockback, float knockbackStrength)
        {
            m_rb.velocity = new Vector2 (playerKnockback.x, playerKnockback.y).normalized * knockbackStrength;
        //    yield return new WaitForSeconds(.09f);
            m_rb.velocity = Vector3.zero;

        }
        
        void EnemyKnockBack(Vector2 enemyKnockback, float knockbackStrength)
        {
            // m_rb.velocity = playerKnockback;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (enemyKnockback.x, enemyKnockback.y).normalized * knockbackStrength;
            // yield return new WaitForSeconds(1f);
            // m_rb.velocity = Vector3.zero;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        }
        
    }

}