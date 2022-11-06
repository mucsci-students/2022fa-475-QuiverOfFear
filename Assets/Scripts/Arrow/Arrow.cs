using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{           
    GameObject[] points;                // Array of points to display prefab crosshair.

    private Animator anim;              // Get direction Player is facing and if shielding.
    private bool faceRight;             // Facing right?
    private AudioSource shotSFX;

    public Transform shotPoint;         // The transform that arows appear from on the Player.
    public GameObject arrow;            // Arrow GameObject in player.
    public GameObject player;           // Player.
    public GameObject crosshairPrefab;  // Visual of crosshair

    public int numberOfPoints;          // Amount of of points to display in the array.
    public float spaceBetweenPoints;    // Distance between points along the trajectory.
    public bool showTrajectory;         // Boolean to control the display of the crosshair.

    private bool canShootRight;         // Allowed to shoot right.
    private bool canShootLeft;          // Allowed to shoot left.
    public float shootCooldown = 0.5f;
    private float nextFireTime = 0f;
    public bool didShoot;
    public bool isPaused;
    
    private Vector3 mousePos;                               // Mouse position.
    private Camera mainCam;                                 // Camera position.
    [Header("Arrow Controls:")]
    private float shotPower;                                // Gets multiplied into velocity in Shoot();
    public float forcePower;                                // Force of arrow.
    public float maxForceHoldTime = .5f;                    // Max time to hold left click for max power.
    private float shootStartCounter;
    private float shootReleaseCounter;
    float holdTimeNormalized;


    private void Start() {
        showTrajectory = false;
        canShootLeft = false;
        canShootRight = false;
        didShoot = false;
        anim = transform.parent.parent.GetComponent<Animator>();
        points = new GameObject[numberOfPoints];
        shotSFX = GetComponent<AudioSource>();
        
        Debug.Log(player.name);
        ShowTrajectory();

    }

    // Update is called once per frame
    void Update()
    {
        faceRight = anim.GetBool("FacingRight");
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ShowTrajectory();
        }

        if(showTrajectory)
        {
            for(int i = 0; i < numberOfPoints; i++)
            {
                points[i].transform.position = PointPosition(i * spaceBetweenPoints);
            }
            
            if(Input.GetButtonDown("Fire1"))
            {
                
            }
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
        
        // Player facing left and mouse is in a 90 degree arc in front of him.
        if(!faceRight && mousePos.x < player.transform.position.x)
            {canShootLeft = true;}
        else
            {canShootLeft = false;}

        if(canShootLeft || canShootRight)
        {
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

        // transform.rotation = LookAtTarget(mousePos - transform.position);
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
        // GameObject pauseMenu = GameObject.Find("PauseMenu");
        // isPaused = pauseMenu.GetComponent<PauseMenu>().paused;
        if(!isPaused)
        {
            holdTimeNormalized = Mathf.Clamp01(shootReleaseCounter / maxForceHoldTime);
            //Debug.Log(holdTimeNormalized);
            mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

            Vector2 direction = mousePos - transform.position;
            Vector2 rotation = transform.position - mousePos;

            shotPower = holdTimeNormalized * forcePower;

            if(!faceRight)
            {
                GameObject newArrow = Instantiate(arrow, shotPoint.position, transform.rotation);
                newArrow.GetComponent<Rigidbody2D>().velocity = new Vector2 (direction.x, direction.y).normalized * shotPower;

                shotSFX.Play();
            }
            else
            {
                GameObject newArrow = Instantiate(arrow, shotPoint.position, transform.rotation);
                newArrow.GetComponent<Rigidbody2D>().velocity = new Vector2 (direction.x, direction.y).normalized * shotPower;
                shotSFX.Play();
            }
        }

    }
            
    // public Quaternion LookAtTarget(Vector2 r)
    //     {
    //         return Quaternion.Euler(0,0, Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
    //     }

    void ShowTrajectory()
    {
        // showTrajectory = !showTrajectory;
        //  for (int i = 0; i < numberOfPoints; i++)
        //  {
        //      points[i] = Instantiate(crosshairPrefab, -500* shotPoint.position, Quaternion.identity);
        //  }
    }

    // Where to place the points showing the trajectory.
    Vector2 PointPosition(float t)
    {
        Vector2 direction = mousePos - transform.position;
        Vector2 position = (Vector2)shotPoint.position + (direction.normalized * shotPower * t) + 0.5f * Physics2D.gravity * (t * t);
        // Vector2 position = (Vector2)shotPoint.position + (direction.normalized * launchForce * t) + 0.5f * Physics2D.gravity * (t * t);
        return position;
    }


    // Hides the shell after every shot.
    Vector2 HideTrajectory(float t)
    {
        t=0;
        Vector2 direction = mousePos - transform.position;
        Vector2 position = (Vector2)shotPoint.position* -500 + (0*direction.normalized * shotPower * t)*0f * Physics2D.gravity * (t * 0);
        return position;
    }
}