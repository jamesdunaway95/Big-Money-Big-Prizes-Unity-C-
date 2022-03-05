using System;
using UnityEngine;

namespace NoStackDev.BigMoney
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(InputManager))]
    [RequireComponent(typeof(GroundDetection))]
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody rb;
        private InputManager input;
        private GroundDetection groundDetection;

        [Header("Movement")]
        [SerializeField] private Transform orientation;
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float moveMultiplier = 10f;
        [SerializeField] private float airMultiplier = 10f;
        [SerializeField] private float jumpForce = 5f;

        [Header("Physics")]
        [SerializeField] private float groundDrag = 6f;
        [SerializeField] private float airDrag = 1f;

        private float horizontalMovement;
        private float verticalMovement;

        private Vector3 moveDirection;
        private Vector3 slopeMoveDirection;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            input = GetComponent<InputManager>();
            groundDetection = GetComponent<GroundDetection>();
        }

        private void Start()
        {
            rb.freezeRotation = true;
        }

        private void Update()
        {
            HandleInput();
            HandleDrag();

            if (input.jumpInput && groundDetection.isGrounded)
            {
                HandleJump();
            }

            slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, groundDetection.SlopeHit().normal);
        }

        private void HandleInput()
        {
            horizontalMovement = input.movementInput.x;
            verticalMovement = input.movementInput.y;

            moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
        }

        private void HandleDrag()
        {
            if (groundDetection.isGrounded) 
            {
                rb.drag = groundDrag;
            }
            else
            {
                rb.drag = airDrag;
            }
        }

        private void HandleJump()
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }


        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            if (groundDetection.isGrounded && !groundDetection.OnSlope())
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
            }
            else if (groundDetection.isGrounded && groundDetection.OnSlope())
            {
                rb.AddForce(slopeMoveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Acceleration);
            }
        }
    }
}