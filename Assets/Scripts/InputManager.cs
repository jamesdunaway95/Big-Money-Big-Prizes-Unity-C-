using UnityEngine;
using UnityEngine.Events;

namespace NoStackDev.BigMoney
{
    public class InputManager : MonoBehaviour
    {
        // Components
        private PlayerControls playerControls;
        private PlayerMovement playerMovement;
        private PlayerLook playerLook;

        public bool sprintInput;
        public bool jumpInput;
        public bool wallJumpInput;
        public bool crouchInput;

        public UnityEvent slideInput;

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.Gameplay.Movement.performed += i => playerMovement.movementInput = i.ReadValue<Vector2>();
                playerControls.Gameplay.Look.performed += i => playerLook.lookInput = i.ReadValue<Vector2>();

                playerControls.Gameplay.Sprint.performed += i => sprintInput = true;
                playerControls.Gameplay.Sprint.canceled += i => sprintInput = false;

                playerControls.Gameplay.Jump.performed += i => jumpInput = true;
                playerControls.Gameplay.Jump.canceled += i => jumpInput = false;

                playerControls.Gameplay.Crouch.performed += i => crouchInput = true;
                playerControls.Gameplay.Crouch.canceled += i => crouchInput = false;
            }

            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            playerLook = GetComponent<PlayerLook>();
        }

        private void Update()
        {
            HandleSlideInput();
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

        // FIXME: This should be changed back to old method, like the jump, invoking is limiting.
        private void HandleSlideInput()
        {
            if (playerControls.Gameplay.Slide.triggered)
            {
                slideInput.Invoke();
            }
        }
    }
}
