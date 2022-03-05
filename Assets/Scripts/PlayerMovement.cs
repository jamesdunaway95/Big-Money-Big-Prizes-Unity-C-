using System;
using UnityEngine;

namespace NoStackDev.BigMoney
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(InputManager))]
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody rb;
        private InputManager input;

        [Header("Movement")]
        [SerializeField] private Transform orientation;
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float moveMultiplier = 10f;
        [SerializeField] private float airMultiplier = 10f;
        [SerializeField] private float jumpForce = 5f;

        [Header("Ground Detection")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float playerHeight = 2f;
        [SerializeField] private float groundDetectionDistance = 0.4f;
        [SerializeField] private bool isGrounded;
        private RaycastHit slopeHit;

        [Header("Physics")]
        [SerializeField] private float groundDrag = 6f;
        [SerializeField] private float airDrag = 1f;

        private float horizontalMovement;
        private float verticalMovement;

        private Vector3 moveDirection;
        private Vector3 slopeMoveDirection;

        private bool OnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.5f))
            {
                if (slopeHit.normal != Vector3.up)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            input = GetComponent<InputManager>();
        }

        private void Start()
        {
            rb.freezeRotation = true;
        }

        private void Update()
        {
            HandleGroundDetection();
            HandleInput();
            HandleDrag();

            if (input.jumpInput && isGrounded)
            {
                HandleJump();
            }

            slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
        }

        private void HandleInput()
        {
            horizontalMovement = input.movementInput.x;
            verticalMovement = input.movementInput.y;

            moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
        }

        private void HandleDrag()
        {
            if (isGrounded) 
            {
                rb.drag = groundDrag;
            }
            else
            {
                rb.drag = airDrag;
            }
        }

        private void HandleGroundDetection()
        {
            isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), groundDetectionDistance, groundLayer);
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
            if (isGrounded && !OnSlope())
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
            }
            else if (isGrounded && OnSlope())
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