using System;
using UnityEngine;

namespace NoStackDev.BigMoney
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        // Components
        private Rigidbody rb;
        private InputManager inputManager;

        // Variables
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 7f;
        [SerializeField] private float sprintModifier = 1.7f;

        [Header("Ground Check")]
        [Tooltip("This should be an empty gameObject placed around the player's feet.")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance = 0.4f;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private bool isGrounded;

        // Hidden Variables
        private float targetSpeed;
        private Vector3 direction = Vector3.zero;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            inputManager = GetComponent<InputManager>();
        }

        // TODO: May look into a better method, seems messy to get input from input class then send it where it needs to go.
        // Input should be read in update
        private void Update()
        {
            float _x = inputManager.movementInput.x;
            float _z = inputManager.movementInput.y;
            direction = new Vector3(_x, 0f, _z);

            HandleSpeed(_z);
        }

        // Rigidbodies and physics should be updated in Fixed Update
        void FixedUpdate()
        {
            HandleMovement();
            HandleGroundDetection();
            HandleJump();
        }
        private void HandleMovement()
        {
            Vector3 _targetVelocity = direction.normalized * targetSpeed * Time.fixedDeltaTime;
            _targetVelocity.y = rb.velocity.y;
            rb.MovePosition(transform.position + transform.TransformDirection(_targetVelocity));
        }

        private void HandleGroundDetection()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }

        // FIXME: Jump feels way to quick, hard to adjust using this method. Will look into other rigidbody methods or changing unities default gravity value.
        private void HandleJump()
        {
            if (inputManager.jumpInput && isGrounded)
            {
                rb.AddForce(new Vector3(0, 100, 0), ForceMode.Impulse);
            }
        }

        private void HandleSpeed(float zMove)
        {
            targetSpeed = moveSpeed;

            if (inputManager.sprintInput && zMove > 0)
            {
                targetSpeed *= sprintModifier;
            }
        }
    }
}