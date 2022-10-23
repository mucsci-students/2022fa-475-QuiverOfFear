using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBackAndForth : MonoBehaviour
{
    private Animator animator;
    private bool jumping = false;
    
    public float speed = 3f;
    public float jumpHeight = 2000f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            if (!jumping)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(-speed * 1000, jumpHeight));
                jumping = true;
            }
            //transform.Translate(new Vector3(-speed * Time.deltaTime, 0f, 0f));
        }
        else
            jumping = false;        
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
