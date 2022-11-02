using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    private float length, startPos;
    public GameObject cam;
    public float parallaxSpeed;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x + parallaxSpeed * Time.deltaTime, transform.position.y, transform.position.z);

        if(transform.position.x > startPos + length)
            transform.position = new Vector3(startPos, transform.position.y, transform.position.z);
    }
}
