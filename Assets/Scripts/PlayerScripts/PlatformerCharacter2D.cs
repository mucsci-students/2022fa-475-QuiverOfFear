using System;
using UnityEngine; 
// using UnityEngine.InputSystem;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 40f;                  // Amount of force added when the player jumps.
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Animator m_Anim;                // Reference to the player's animator.
        private Rigidbody2D m_Rigidbody2D;      // Reference to the player's Rigidbody.
        private Transform m_GroundCheck;        // A position marking where to check if the player is grounded.
        private Transform m_CeilingCheck;       // A position marking where to check for ceilings.
        public ArrowBehaviorLeft arrowLeft;     // Starting point for arrow when facing left.
        public ArrowBehaviorRight arrowRight;   // Starting point for arrow when facing right.
        public Transform arrowOffset;
        private GameObject grapple;
        private SpriteRenderer grappleSprite;

        const float k_GroundedRadius = .2f;     // Radius of the overlap circle to determine if grounded
        const float k_CeilingRadius = .01f;     // Radius of the overlap circle to determine if the player can stand up
        const float m_SneakSpeed = 0f;         // Movement speed when sneaking.
        private bool m_Grounded;                // Whether or not the player is grounded.
        public bool m_FacingRight = true;       // For determining which way the player is currently facing.
        private bool isJumping;                 // Whether or not the player is in the air with jumpHoldDuration.
        private bool isAttacking;

        public float jumpHoldDuration = 0.25f;  // Max duration to hold space and gain velocity.
        public float jumpHoldCounter;           // Control for jumpHoldDuration

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            grapple = GameObject.Find("GrappleGun");
            grappleSprite = grapple.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        }

        private void FixedUpdate()
        {
            m_Grounded = false;
            isAttacking = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
            
            // Set Ground for animation
            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
            m_Anim.SetBool("FacingRight", m_FacingRight);
            m_Anim.SetBool("Attacking", isAttacking);

            // Debug.Log(m_Grounded);
        }

        public void Move(float move, bool fire, bool jump, bool jump_2, bool sneak)
        {
 
            // Setting variables in the animator.
            m_Anim.SetBool("Attacking", fire);
            m_Anim.SetFloat("yPos", m_Rigidbody2D.position.y);
            m_Anim.SetBool("Sneak", sneak);
        

            /* 
                Shooting left and right is a giant pain in the ass. 
                For the time being, different prefabs for each side.
            */
            if(fire)
            {
                if(!m_FacingRight)
                {
                    Instantiate(arrowLeft, arrowOffset.position, transform.rotation);
                }
                else {
                    Instantiate(arrowRight, arrowOffset.position, transform.rotation);
                }
            }


            // Only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // if(sneak){
                //     move = (sneak ? move * m_SneakSpeed: move);
                //     m_Anim.SetBool("Ground",false);
                // }
                
                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

                //If moving right and facing left:
                if (move > 0 && !m_FacingRight)
                {   
                    Flip();
                }
                // If moving left and facing right:
                else if (move < 0 && m_FacingRight)
                {
                    Flip();
                }

            }
              
            /*
                Dynamic jump height, could turn into double jump if we desire.
            */
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                
                // While in air, change states
                m_Grounded = false;
                isJumping = true;

                // Set state for animator
                m_Anim.SetBool("Ground", false);

                // Initialize jump interval
                jumpHoldCounter = jumpHoldDuration;
                // Juuump!
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce + 250f));
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
            // Switch the way the player is labelled as facing.       
            m_FacingRight = !m_FacingRight;
            


            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;

            // Flip grappling hook sprite.
            grappleSprite.flipY = !grappleSprite.flipY;

            /*
                Flip grappling hook again so it's not broken. Find better way to do this
                If character gets new child object before (2), it's broken.
            */
            // Debug.Log(this.gameObject.transform.GetChild(2).name);
            
            this.gameObject.transform.GetChild(2).localScale = theScale;
        }
    }
}
