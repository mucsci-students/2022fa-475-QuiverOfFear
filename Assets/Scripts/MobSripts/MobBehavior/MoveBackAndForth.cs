using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackAndForth : MonoBehaviour
{
    public float speed = 3;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetBool("Walk", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(new Vector3(-speed * Time.deltaTime, 0f, 0f));
    }
}
