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

        public UnityEvent jumpInput;

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.Gameplay.Movement.performed += i => playerMovement.movementInput = i.ReadValue<Vector2>();
                playerControls.Gameplay.Look.performed += i => playerLook.lookInput = i.ReadValue<Vector2>();
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
            HandleJumpInput();
            HandleDashInput();
        }

        private void HandleJumpInput()
        {
            if (playerControls.Gameplay.Jump.triggered)
            {
                jumpInput.Invoke();
            }
        }

        private void HandleDashInput()
        {
            if (playerControls.Gameplay.Dash.triggered)
            {
                playerMovement.Dash();
            }
        }

    }
}
