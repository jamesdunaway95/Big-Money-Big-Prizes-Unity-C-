using System;
using System.Collections;
using UnityEngine;

namespace NoStackDev.BigMoney
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(GroundDetection))]
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody rb;
        private GroundDetection groundDetection;

        [Header("Movement")]
        [SerializeField] private Transform orientation;
        [SerializeField] private float maxVelocity;
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float moveMultiplier = 10f;
        [SerializeField] private float airMultiplier = 10f;
        [SerializeField] private float jumpForce = 15f;

        [Header("Dash")]
        [SerializeField] private float dashCooldown = 1f;
        [SerializeField] private float dashForce = 0.7f;
        private bool canDash = true;

        [Header("Physics")]
        [SerializeField] private float groundDrag = 6f;
        [SerializeField] private float airDrag = 1f;


        [HideInInspector] public Vector2 movementInput;
        private Vector3 moveDirection;
        private Vector3 slopeMoveDirection;

        public float currentVelocity; // DEBUGGING

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            groundDetection = GetComponent<GroundDetection>();

            rb.freezeRotation = true;
        }

        private void Update()
        {
            moveDirection = orientation.forward * movementInput.y + orientation.right * movementInput.x;

            slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, groundDetection.SlopeHit().normal);

            currentVelocity = rb.velocity.magnitude; // DEBUGGING

            HandleDrag();
        }

        private IEnumerator DashCooldown()
        {
            //pauses execution of function for set amount of time
            yield return new WaitForSeconds(dashCooldown);

            canDash = true;
        }

        #region Movement
        private void HandleDrag()
        {
            // TODO: Will look into dynamically changing the drag as the player gets closer to maxSpeed. This should provide a more fluid movement.
            if (groundDetection.isGrounded)
            {
                rb.drag = groundDrag;
            }
            else
            {
                rb.drag = airDrag;
            }
        }

        private void FixedUpdate()
        {
            HandleMovement();
            HandleVelocity();
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

        private void HandleVelocity()
        {
            if (rb.velocity.magnitude > maxVelocity)
            {
                rb.velocity = rb.velocity.normalized * maxVelocity;
            }
        }
        #endregion

        #region Public Methods
        public void Jump()
        {
            if (!groundDetection.isGrounded) return;

            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        public void Dash()
        {
            if (!canDash) return;

            rb.AddForce(moveDirection * dashForce, ForceMode.Impulse);
            canDash = false;
            StartCoroutine(DashCooldown());
        }
        #endregion
    }
}