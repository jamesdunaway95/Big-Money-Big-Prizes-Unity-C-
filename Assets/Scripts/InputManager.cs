using UnityEngine;
using UnityEngine.InputSystem;

namespace NoStackDev.BigMoney
{
    public class InputManager : MonoBehaviour
    {
        [Header("References")]
        private PlayerControls playerControls;

        [Header("Vector Inputs")]
        [HideInInspector] public Vector2 movementInput;
        [HideInInspector] public Vector2 lookInput;

        [Header("Input Bools")]
        public bool sprintInput;
        public bool jumpInput;
        public bool wallJumpInput;
        public bool crouchInput;
        public bool slideInput;

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.Gameplay.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.Gameplay.Look.performed += i => lookInput = i.ReadValue<Vector2>();

                playerControls.Gameplay.Sprint.performed += i => sprintInput = true;
                playerControls.Gameplay.Sprint.canceled += i => sprintInput = false;

                playerControls.Gameplay.Jump.performed += i => jumpInput = true;
                playerControls.Gameplay.Jump.canceled += i => jumpInput = false;

                playerControls.Gameplay.Crouch.performed += i => crouchInput = true;
                playerControls.Gameplay.Crouch.canceled += i => crouchInput = false;

                playerControls.Gameplay.Slide.performed += i => slideInput = true;
                playerControls.Gameplay.Slide.canceled += i => slideInput = false;
            }

            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        private void Update()
        {
            HandleWallJumpInput();
        }

        // This is separate due to the two different ways the input should be read.
        private void HandleWallJumpInput()
        {
            if (playerControls.Gameplay.Jump.triggered)
            {
                wallJumpInput = true;
            }
            else
            {
                wallJumpInput = false;
            }
        }
    }
}