using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviorLeft : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator m_Anim;
    private float arrowDeleteTimer = 0.1f;
    
    void Start()
    {
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
        Destroy(gameObject, arrowDeleteTimer);
    }
}
