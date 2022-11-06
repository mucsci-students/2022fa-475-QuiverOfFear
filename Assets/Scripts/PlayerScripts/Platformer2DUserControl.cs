using UnityEngine;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private Animator m_anim;
        private bool m_Jump_button;
        private bool m_Jump_down;
        private bool isDead;
        private float shootStartCounter;
        private float shootReleaseCounter;        
        private bool fire;
        

        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
            m_anim = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!m_Jump_down)
            {
                // m_Jump_down when jump is pressed
                m_Jump_down = Input.GetButtonDown("Jump");

                // m_Jump_button DURATION jump is pressed
                m_Jump_button = Input.GetButton("Jump");
            }

            if(Input.GetButtonDown("Fire1"))
            {
                shootStartCounter = Time.time;
            }

            if(Input.GetButtonUp("Fire1"))
            {
                shootReleaseCounter = Time.time - shootStartCounter;
                fire = true;
            }  
        }

        private void FixedUpdate()
        {
            isDead = m_anim.GetBool("isDead");
            print("dead " + isDead);
            // Read the inputs.
            bool sneak = Input.GetKey(KeyCode.S);
            float move = Input.GetAxis("Horizontal");
                     
            // Pass all parameters to the character control script.
            if(!isDead){
                m_Character.Move(move, fire, m_Jump_down, m_Jump_button, sneak, shootReleaseCounter);
            }

            // Reset
            fire = false;
            m_Jump_down = false;
            m_Jump_button = false;
            sneak = false;
        }
    }
}
