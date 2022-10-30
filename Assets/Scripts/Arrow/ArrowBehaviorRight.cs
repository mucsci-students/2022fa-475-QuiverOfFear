using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviorRight : MonoBehaviour
{
    Rigidbody2D rb;
    private Animator m_Anim;
    public float force;
    private float arrowDeleteTimer = 0.1f;
    private Vector3 mousePos;
    private Camera mainCam;
    private Vector3 movePos;

    void Awake()
    {
        m_Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
    }

    void Update(){
        transform.rotation = LookAtTarget(mousePos - transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        m_Anim.SetTrigger("Hit");
        rb.gravityScale = 10f;
        if (collision.gameObject.tag != "enemy")
        {
            Destroy(gameObject, arrowDeleteTimer);
        }
    }
        public static Quaternion LookAtTarget(Vector2 r)
        {
            return Quaternion.Euler(0, 0, Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
        }

}

