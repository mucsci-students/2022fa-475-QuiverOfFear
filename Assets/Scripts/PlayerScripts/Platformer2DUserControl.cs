using UnityEngine;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump_button;
        private bool m_Jump_down;
                        
        private bool fire;
        

        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
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
                fire = true;
            }    
        }

        private void FixedUpdate()
        {
            // Read the inputs.
            bool sneak = Input.GetKey(KeyCode.S);
            float move = Input.GetAxis("Horizontal");
                     
            // Pass all parameters to the character control script.
            m_Character.Move(move, fire, m_Jump_down, m_Jump_button, sneak);

            // Reset
            fire = false;
            m_Jump_down = false;
            m_Jump_button = false;
            sneak = false;
        }
    }
}
