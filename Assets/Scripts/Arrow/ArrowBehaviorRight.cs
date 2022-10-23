using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviorRight : MonoBehaviour
{
    Rigidbody2D rb;
    private Animator m_Anim;
    public float speed = 4.5f;
    public float arrowDeleteTimer = 0.5f;

    void Awake()
    {
        m_Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;   
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        speed = 0;
        m_Anim.SetTrigger("Hit");
        if (collision.gameObject.tag != "enemy"){
            Destroy(gameObject, arrowDeleteTimer);
        }
    }
}

