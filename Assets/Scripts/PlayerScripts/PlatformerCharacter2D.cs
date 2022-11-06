using System;
using UnityEngine; 

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        /* --- ONE SCRIPT FOR EVERYTHING WOO!! --- */           // Please don't show Dr. Zoppetti this, he'd be so disappointed. :(
        [SerializeField] private float m_MaxSpeed = 10f;        // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 40f;       // Amount of force added when the player jumps.
        [SerializeField] private bool m_AirControl = false;     // Whether or not a player can steer while jumping.
        [SerializeField] private LayerMask m_WhatIsGround;      // A mask determining what is ground to the character.

        private Animator m_Anim;                                // Reference to the player's animator.
        private Animator g_Anim;                                // Reference to center's animator.
        private Rigidbody2D m_Rigidbody2D;                      // Reference to the player's Rigidbody.
        private Transform m_GroundCheck;                        // A position marking where to check if the player is grounded.
        private Transform m_CeilingCheck;                       // A position marking where to check for ceilings.
        private Transform m_Center; 
        private GameObject grapple;                             // Reference to grapple gameObject.

        const float k_GroundedRadius = .2f;                     // Radius of the overlap circle to determine if grounded.
        const float k_CeilingRadius = .01f;                     // Radius of the overlap circle to determine if the player can stand up.
        const float m_SneakSpeed = 0f;                          // Movement speed when sneaking.
        public bool m_FacingRight = true;                       // For determining which way the player is currently facing.
        private bool isJumping;                                 // Whether or not the player is in the air with jumpHoldDuration.
        private bool isCharging;                                // Set Charging animation in animator.
        private bool m_Grounded;                                // Whether or not the player is grounded.
        
        [Header("Jump Controls:")]
        public float jumpHoldDuration = 0.25f;  // Max duration to hold space and gain velocity.
        public float jumpHoldCounter;           // Control for jumpHoldDuration

        private AudioSource jumpSFX;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_Center = transform.Find("Center");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            grapple = GameObject.Find("GrappleGun");
            jumpSFX = GetComponent<AudioSource>();
        }

        private void Start()
        {
            g_Anim = m_Center.GetComponent<Animator>();
        }

        
        private void FixedUpdate()
        {
            m_Grounded = false;
            g_Anim.SetBool("Landed", false);
            isCharging = m_Anim.GetBool("isCharging");

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
                    g_Anim.SetBool("Landed", true);
            }
            
            // Set Ground for animation
            m_Anim.SetBool("Ground", m_Grounded);
            g_Anim.SetBool("Jumping", false);
            m_Anim.SetBool("canAttack", false);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
            m_Anim.SetBool("FacingRight", m_FacingRight);
            m_Anim.SetBool("isCharging", isCharging);

        }

        public void Move(float move, bool jump, bool jump_2, bool sneak)
        {
 
            // Setting variables in the animator.
            m_Anim.SetFloat("yPos", m_Rigidbody2D.position.y);
            m_Anim.SetBool("Sneak", sneak);

            // Only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // if(sneak){
                //     move = (sneak ? move * m_SneakSpeed: move);
                //     m_Anim.SetBool("Ground",false);
                // }
                
                // The Speed animator parameter is set to the absolute value of the horizontal input.
                if(!isCharging || (isCharging && !m_Grounded) ){
                    m_Anim.SetFloat("Speed", Mathf.Abs(move));

                    // Move the character
                    m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);
                }

                //If moving right and facing left:
                if (move > 0 && !m_FacingRight)
                {   g_Anim.SetBool("TurnRight",true);
                    Flip();
                }
                // If moving left and facing right:
                else if (move < 0 && m_FacingRight)
                {
                    g_Anim.SetBool("TurnLeft", true);
                    Flip();
                }
                else
                {
                    g_Anim.SetBool("TurnRight",false);
                    g_Anim.SetBool("TurnLeft",false);
                }

            }

            //print("GROUNDED: " + m_Grounded + ". JUMP: " + jump + ". GROUND" + m_Anim.GetBool("Ground"));
              
            /*
                Dynamic jump height, could turn into double jump if we desire.
            */
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                
                // While in air, change states
                m_Grounded = false;
                isJumping = true;

                g_Anim.SetBool("Jumping", true);

                // Set state for animator
                m_Anim.SetBool("Ground", false);
                g_Anim.SetBool("Landed", false);

                // Initialize jump interval
                jumpHoldCounter = jumpHoldDuration;
                // Juuump!
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce + 250f));
                jumpSFX.Play();
            }

            // If HOLDING jump && in the air:
            if (jump_2 && isJumping)
            {
                if (jumpHoldCounter > 0)
                {
                    // Reduce timer and jump
                    jumpHoldCounter -= Time.deltaTime;
                    m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce *.95f));
                }
                // No more jumping for you.
                else { 
                    isJumping = false;
                    jumpHoldCounter = 0;
                }
            }
            // No more jumping for you x2.
            if(!jump_2) 
            { 
                isJumping = false; 
                jumpHoldCounter = 0;
            }
            
        }

        // Flip player depending on the way they are / should be facing.
        private void Flip()
        {
            SpriteRenderer grappleSprite = grapple.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            // Switch the way the player is labelled as facing.       
            m_FacingRight = !m_FacingRight;
            
            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;

            // Flip grappling hook sprite when player turns.
            grappleSprite.flipY = !grappleSprite.flipY;
            grappleSprite.transform.parent.localScale = theScale;
        }
    }
}
