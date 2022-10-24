using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBackAndForth : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private bool jumping = false;

    [SerializeField] GameObject player;
    public float speed = 1f;
    public float jumpHeight = 2000f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 heading = player.transform.position - transform.position;
        speed = AngleDir(transform.forward, heading, transform.up);
    }

    void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            if (!jumping)
            {
                rb.AddForce(new Vector2(speed * 1000, jumpHeight));
                jumping = true;
                speed *= -1f;
            }
        }
        else
            jumping = false;
    }

    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y);
            return Math.Abs(speed);
        }
        else
        {
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y);
            return -Math.Abs(speed);
        }
    }

    //void JumpLoop()
    //{
    //    for (int i = 0; i < 2; ++i)
    //    {
    //        StartCoroutine(WaitForAnimation());
    //    }
    //    animator.SetBool("Walk", true);
    //    StartCoroutine(WaitForAnimation());
    //    animator.SetBool("Walk", false);
    //}

    //IEnumerator WaitForAnimation()
    //{
    //    yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    //}
}
