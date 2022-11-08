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
    public bool toggleTrajectory;         // Boolean to control the display of the crosshair.

    private bool canAttack;
    private bool isDead;
    private bool isCharging;
    private bool canShootRight;         // Allowed to shoot right.
    private bool canShootLeft;          // Allowed to shoot left.
    public float shootCooldown = 0.25f;
    private float nextFireTime = 0f;
    public bool didShoot;
    public bool isPaused;
    public float spriteTimer;

    public Image coolDownImage;
    private bool validShot;
    private Vector3 mousePos;                               // Mouse position.
    private Camera mainCam;                                 // Camera position.
    [Header("Arrow Controls:")]
    private float shotPower;                                // Gets multiplied into velocity in Shoot();
    public float forcePower;                                // Force of arrow.
    public float maxForceHoldTime = 1.0f;                    // Max time to hold left click for max power.
    private float shootStartCounter;
    private float shootReleaseCounter;
    float holdTimeNormalized;

    private void Start() {
        toggleTrajectory = false;
        canShootLeft = false;
        canShootRight = false;
        didShoot = false;
        anim = transform.parent.parent.GetComponent<Animator>();
        points = new GameObject[numberOfPoints];
        shotSFX = GetComponent<AudioSource>();
        spriteTimer = shootCooldown;
        coolDownImage.GetComponent<Image>();
        
        Debug.Log(player.name);
    
        // ShowTrajectory();
        // validShot = false;
    }

    // Update is called once per frame
    void Update()
    {
        canAttack = anim.GetBool("canAttack");
        isDead = anim.GetBool("isDead");
        isCharging = anim.GetBool("isCharging");
        spriteTimer = shootCooldown;

        faceRight = anim.GetBool("FacingRight");
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // if (Input.GetKeyDown(KeyCode.LeftShift))
        // {
        //     ToggleTrajectory();
        // }

        // if(toggleTrajectory)
        // {
        //     for(int i = 0; i < numberOfPoints; i++)
        //     {
        //         points[i].transform.position = PointPosition(i * spaceBetweenPoints);
        //     }
        // }

         if(faceRight && mousePos.x > player.transform.position.x)
            {canShootRight = true;}
        else
            {canShootRight = false;}
        
        // Player facing left and mouse is in a 90 degree arc in front of him.
        if(!faceRight && mousePos.x < player.transform.position.x)
            {canShootLeft = true;}
        else
            {canShootLeft = false;}

        if(Time.time >= nextFireTime)
        {
            anim.SetBool("canAttack", true);
        }
        else{
            anim.SetBool("canAttack", false);
        }

        if(canAttack && Input.GetMouseButtonDown(0)) {
            validShot = true;
            anim.SetBool("isCharging", true);
            shootStartCounter = Time.time;
        }
        
        if(Time.time > nextFireTime && canShootLeft || canShootRight && validShot )
        {
            // If left click is released
            if(Input.GetMouseButtonUp(0))
            {
                shootReleaseCounter = Time.time - shootStartCounter;
                Shoot();
                anim.SetBool("isCharging", false);
                nextFireTime = Time.time + shootCooldown;
                coolDownImage.fillAmount = 0;
            }
        }


        // for (int i = 0; i < numberOfPoints; i++)
        // {
        //     points[i] = Instantiate(crosshairPrefab, shotPoint.position, Quaternion.identity);
        // }

        // float holdTimeNormalized = Mathf.Clamp01(shootReleaseCounter / maxForceHoldTime);
        // StartCoroutine(UpdateCrosshair(holdTimeNormalized));
       
        if(didShoot){
            coolDownImage.fillAmount += 1.0f / spriteTimer * Time.deltaTime;

            if(coolDownImage.fillAmount >= 1)
            {
                print(coolDownImage.name);
                coolDownImage.fillAmount = 1.0f;
                didShoot = false;
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
        GameObject pauseMenu = GameObject.Find("PauseMenu");
        isPaused = pauseMenu.GetComponent<PauseMenu>().paused;
        if(!isPaused && !isDead)
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
            didShoot = true;
            
        }
        validShot = false;
    }
            
    // public Quaternion LookAtTarget(Vector2 r)
    //     {
    //         return Quaternion.Euler(0,0, Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
    //     }

    // void ToggleTrajectory()
    // {
    //     toggleTrajectory = !toggleTrajectory;
    //      for (int i = 0; i < numberOfPoints; i++)
    //      {
    //          points[i] = Instantiate(crosshairPrefab, -500* shotPoint.position, Quaternion.identity);
    //      }
    // }
    
    
    // public IEnumerator UpdateCrosshair(float holdTimeNormalized)
    // {
        
    //     Camera mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    //     Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
    //     Vector2 direction = mousePos - transform.position;
    //     Vector2 rotation = transform.position - mousePos;
    //     float shotPower = holdTimeNormalized * 40f;
        

    //     for(int i = 0; i < numberOfPoints; i++)
    //     {
    //         new Vector2 (direction.x, direction.y).normalized * shotPower;
            
    //         points[i].transform.position = PointPosition(i * spaceBetweenPoints);

    //     }
        // Vector2 position = (Vector2)shotPoint.position + (direction.normalized * launchForce * t) + 0.5f * Physics2D.gravity * (t * t);
        
        // for (int i = 0; i < numberOfPoints; i++)
    //         {
    //             points[i] = Instantiate(crosshairPrefab, shotPoint.position, Quaternion.identity);
    //         }

        // yield return new WaitForSeconds(0f);
        
    // }

    // void ShowTrajectory()
    // {
    //     if(toggleTrajectory){

    //         for (int i = 0; i < numberOfPoints; i++)
    //         {
    //             points[i] = Instantiate(crosshairPrefab, shotPoint.position, Quaternion.identity);
    //         }
    //      }
    // }

    
    
    // Where to place the points showing the trajectory.
    // Vector2 PointPosition(float t)
    //{
       // Vector2 direction = mousePos - transform.position;
        //Vector2 position = (Vector2)shotPoint.position + (direction.normalized * shotPower * t) + 0.5f * Physics2D.gravity * (t * t);
        // Vector2 position = (Vector2)shotPoint.position + (direction.normalized * launchForce * t) + 0.5f * Physics2D.gravity * (t * t);




    //     return position;
    // }


    // Hides the shell after every shot.
    // Vector2 HideTrajectory(float t)
    // {
    //     t=0;
    //     Vector2 direction = mousePos - transform.position;
    //     Vector2 position = (Vector2)shotPoint.position* -500 + (0*direction.normalized * shotPower * t)*0f * Physics2D.gravity * (t * 0);
    //     return position;
    // }
}