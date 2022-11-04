using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHealth : MonoBehaviour
{
    private AudioSource hitSFX;

    public int health;
    public ParticleSystem damageEffect;

    void Start()
    {
        hitSFX = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If an arrow comes in contact, deal a point of damage
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            --health;
            hitSFX.Play();
            Instantiate(damageEffect, transform.position, Quaternion.identity);

            //Destroy if health is equal to 0
            if(health == 0)
            {
                Destroy(gameObject);
            }
        }
        else if(collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth health)){
            health.UpdateHealth(-1);
        }
    }
}
