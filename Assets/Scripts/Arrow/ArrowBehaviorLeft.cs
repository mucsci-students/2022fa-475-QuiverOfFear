using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviorLeft : MonoBehaviour
{
    Rigidbody2D rb;
    private Animator m_Anim;
    public float force;
    private float arrowDeleteTimer = 0.1f;
    private Vector3 mousePos;
    private Camera mainCam;
    private Vector3 movePos;
    private SpriteRenderer sp;

    
    void Awake()
    {
        m_Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();    
    }

    void Start()
    {
        Debug.Log("spawn");
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        transform.localScale *= -1;
        // sp.flipY = !sp.flipY;

    }

    void Update()
    {
        transform.rotation = LookAtTarget(mousePos - transform.position);
        // transform.position = movePosition;
    }

    private void OnCollisionEnter2D(Collision2D collision) {       
        m_Anim.SetTrigger("Hit");
        rb.gravityScale = 10f;
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

    public static Quaternion LookAtTarget(Vector2 r)
    {

        return Quaternion.Euler(0, 0, Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
    }
}
