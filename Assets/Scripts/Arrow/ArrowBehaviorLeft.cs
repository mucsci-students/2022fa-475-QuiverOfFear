using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviorLeft : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator m_Anim;
    public float force;
    private float arrowDeleteTimer = 0.1f;
    private SpriteRenderer sp;
    public Ray2D ray;
    private GameObject arrow;
    public Vector2 direction;
    public Vector2 rotation;
    
    void Start()
    {
        //startRotation = new Vector2 (direction.x, direction.y).normalized;
        //transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(startRotation.y, startRotation.x) * Mathf.Rad2Deg);
        // transform.localScale *= -1;
        // sp.flipX = !sp.flipX;
        // Ray2D ray = new Ray2D (this.transform.position, this.transform.position + this.transform.right) ;
        m_Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }


    void Update() 
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) *Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {       
        m_Anim.SetTrigger("Hit");
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        Debug.Log(collision.collider.gameObject.name);

        if (collision.gameObject.tag != "enemy")
        {
            Destroy(gameObject, arrowDeleteTimer);
        }

        // else if(collision.gameObject.TryGetComponent<enemyHealth>(out enemyHealth Health))
        // {
        //     Health.TakeDamage(1);
        // }
    }
}
