using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{           
    // Vector2 direction;                  // Direction of shot.
    GameObject[] points;                // Array of points to display prefab crosshair.

    private Animator anim;              // Get direction Bobbert is facing and if shielding.
    private bool faceRight;             // Facing right?

    public Transform shotPoint;         // The transform that shells appear from on Bobbert.
    public GameObject shot;             // SlingAmmo prefab.
    public GameObject player;           // Bobbert.
    public GameObject point;            // Where the shot will land.

    public int numberOfPoints;          // Amount of of points to display in the array.
    public float spaceBetweenPoints;    // Distance between points along the trajectory.
    public bool showTrajectory;         // Boolean to control the display of the crosshair.

    private bool canShootRight;         // Allowed to shoot right.
    private bool canShootLeft;          // Allowed to shoot left.
    public float shootCooldown = 0.5f;
    private float nextFireTime = 0f;
    public bool didShoot;

    public ArrowBehaviorLeft arrowLeft;                     // Starting point for arrow when facing left.
    public ArrowBehaviorRight arrowRight;
    public Transform arrowOffset;                           // Location arrow spawns from.
    private Vector3 mousePos;                               // Mouse position.
    private Camera mainCam;                                 // Camera position.
    [Header("Arrow Controls:")]
    private float shotPower;                                // Gets multiplied into velocity in Shoot();
    public float forcePower = 500f;                         // If using ForceShot.
    public float impulsePower = 20f;                        // If using ImpulseShot.
    public float maxForceHoldTime = .5f;                    // Max time to hold left click for max power.
    private float shootStartCounter;
    private float shootReleaseCounter;
    float holdTimeNormalized;
    public enum ArrowShotType                               // Used in editor
    {
        ForceShot,
        ImpulseShot
    }

    [SerializeField] public ArrowShotType arrowType = ArrowShotType.ForceShot;

    // Generates the prefab of opaque turtle shell a bunch of times.
    private void Start() {
        showTrajectory = true;
        canShootLeft = false;
        canShootRight = false;
        didShoot = false;
        anim = transform.parent.GetComponent<Animator>();
        points = new GameObject[numberOfPoints];
        
        Debug.Log(player.name);

        // for (int i = 0; i < numberOfPoints; i++)
        // {
        //     points[i] = Instantiate(point, -500* shotPoint.position, Quaternion.identity);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        faceRight = anim.GetBool("FacingRight");
        //Shoot();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            showTrajectory = !showTrajectory;
        }

        if(Input.GetButtonDown("Fire1"))
        {
            shootStartCounter = Time.time;
        }

        if(Input.GetButtonUp("Fire1"))
        {
            shootReleaseCounter = Time.time - shootStartCounter;
        }
  
        // Character facing right && mouse is in a 90 degree arc in front of him.
        if(faceRight && mousePos.x > player.transform.position.x)
            {canShootRight = true;}
        else
            {canShootRight = false;}
        
        // Bobbert facing left and mouse is in a 90 degree arc in front of him.
        if(!faceRight && mousePos.x < player.transform.position.x)
            {canShootLeft = true;}
        else
            {canShootLeft = false;}

        // Debug.Log("left " + canShootLeft);
        // Debug.Log("Right " + canShootRight);

        if(canShootLeft || canShootRight)
        {
            Debug.Log(showTrajectory);
            // If left click is held down
            if(showTrajectory)
            {
                // Display crosshair
                // for(int i = 0; i < numberOfPoints; i++)
                // {
                //     points[i].transform.position = PointPosition(i * spaceBetweenPoints);
                // }
            }
        
            if(Time.time > nextFireTime)
            {
                // If left click is released
                if(Input.GetMouseButtonUp(0))
                {
                    Shoot();
                    nextFireTime = Time.time + shootCooldown;
                }
            }
        }

        // If mouse moves out of shooting position
        // if(Input.GetMouseButtonUp(0))
        // {
        //     for(int i = 0; i < numberOfPoints; i++)
        //     {
        //         points[i].transform.position = HideTrajectory(i * 0);
        //     }  
        // }

    }

    // Pew pew time.
    public void Shoot() 
    {
        holdTimeNormalized = Mathf.Clamp01(shootReleaseCounter / maxForceHoldTime);
        Debug.Log(holdTimeNormalized);
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mousePos - transform.position;
        // Vector2 direction = mousePos - ;
        Vector2 rotation = transform.position - mousePos;
        
        // If ForceShot is selected.
        if (arrowType == ArrowShotType.ForceShot)
        {
            shotPower = holdTimeNormalized * forcePower;
        }
        else
        {
            shotPower = holdTimeNormalized * impulsePower;
            
        }
        if(!faceRight)
        {
            ArrowBehaviorLeft spawnArrowLeft = Instantiate(arrowLeft, arrowOffset.position, transform.rotation);
            spawnArrowLeft.GetComponent<Rigidbody2D>().AddForce(new Vector2 (direction.x, direction.y).normalized * shotPower, ForceMode2D.Force);
        }
        else
        {
            ArrowBehaviorRight spawnArrowRight = Instantiate(arrowRight, arrowOffset.position, transform.rotation);
            spawnArrowRight.GetComponent<Rigidbody2D>().AddForce(new Vector2 (direction.x, direction.y).normalized * shotPower, ForceMode2D.Force);
        }

    }

    // Where to place the points showing the trajectory.
    // Vector2 PointPosition(float t)
    // {
        // Vector2 direction = mousePos - transform.position;
        // Vector2 position = (Vector2)arrowOffset.position + (direction.normalized * shotPower * t) + 0.5f * Physics2D.gravity * (t * t);
        // // Vector2 position = (Vector2)shotPoint.position + (direction.normalized * launchForce * t) + 0.5f * Physics2D.gravity * (t * t);
        // return position;
    // }

    // Hides the shell after every shot.
    Vector2 HideTrajectory(float t)
    {
        t=0;
        Vector2 direction = mousePos - transform.position;
        Vector2 position = (Vector2)shotPoint.position* -500 + (0*direction.normalized * shotPower * t)*0f * Physics2D.gravity * (t * 0);
        return position;
    }
}