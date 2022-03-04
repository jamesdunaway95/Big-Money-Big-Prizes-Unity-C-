using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class InputManager : MonoBehaviour
    {
        // Components
        private PlayerControls playerControls;

        // Variables
        public Vector2 movementInput;
        public Vector2 lookInput;
        public bool sprintInput;
        public bool jumpInput;

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.Gameplay.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.Gameplay.Look.performed += i => lookInput = i.ReadValue<Vector2>();

                playerControls.Gameplay.Jump.performed += i => jumpInput = true;
                playerControls.Gameplay.Jump.canceled += i => jumpInput = false;

                playerControls.Gameplay.Sprint.performed += i => sprintInput = true;
                playerControls.Gameplay.Sprint.canceled += i => sprintInput = false;
            }

            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
    }
}
