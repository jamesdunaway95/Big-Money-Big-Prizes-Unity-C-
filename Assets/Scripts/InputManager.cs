using UnityEngine;

namespace DunawayDevelopment.BigMoney
{
    public class InputManager : MonoBehaviour
    {
        private PlayerControls playerControls;

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
            }

            playerControls.Enable();
        }

        public Vector2 GetMovementInput()
        {
            return playerControls.Gameplay.Movement.ReadValue<Vector2>();
        }

        public Vector2 GetLookInput()
        {
            return playerControls.Gameplay.Look.ReadValue<Vector2>();
        }

        public bool GetJumpInput()
        {
            return playerControls.Gameplay.Jump.triggered;
        }

        public bool GetSprintInput()
        {
            return playerControls.Gameplay.Sprint.ReadValue<float>() > 0.05f ? true : false;
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
    }
}
