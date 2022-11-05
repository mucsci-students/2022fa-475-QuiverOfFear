using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobRender : MonoBehaviour
{
    public Vector2 startingPosition;

    [SerializeField] GameObject player;
    [SerializeField] float renderDist = 20f;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        RenderCheck();
    }

    void RenderCheck()
    {
        // If the mob is further than the render distance to the player, move back to spawn
        if (Vector3.Distance(player.transform.position, transform.position) > renderDist)
        {
            transform.position = startingPosition;
        }
    }
}
