using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincible : MonoBehaviour
{
    private bool invincible = false;
    public float invincibilityTime = 3f;
    
    void OnCollisionEnter2D(Collision2D collision) {
        if(!invincible) 
        {
            if(collision.gameObject.tag == "Enemies") 
            {
                StartCoroutine(Invulnerability(collision.gameObject));
                
            }
        }
    }
    
    IEnumerator Invulnerability(GameObject collide) 
    {
        print(collide.name);
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