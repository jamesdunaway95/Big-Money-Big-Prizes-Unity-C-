using System;
using UnityEngine;

namespace DunawayDevelopment.BigMoney
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        // Components
        private Rigidbody rb;
        private InputManager inputManager;

        // Variables
        [SerializeField] private float moveSpeed = 7f;
        [SerializeField] private float sprintModifier = 1.7f;
        [SerializeField] private bool isSprinting;

        // Hidden Variables
        private float targetSpeed;
        private Vector3 direction = Vector3.zero;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            inputManager = GetComponent<InputManager>();
        }

        private void Update()
        {
            float _x = inputManager.GetMovementInput().x;
            float _z = inputManager.GetMovementInput().y;
            direction = new Vector3(_x, 0f, _z);

            HandleSpeed(_z);
        }

        void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            direction = direction.normalized * targetSpeed * Time.fixedDeltaTime;
            rb.MovePosition(transform.position + transform.TransformDirection(direction));
        }

        private void HandleSpeed(float zMove)
        {
            targetSpeed = moveSpeed;
            isSprinting = (inputManager.GetSprintInput() && zMove > 0);

            if (isSprinting)
            {
                targetSpeed *= sprintModifier;
            }
        }
    }
}