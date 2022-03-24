using System.Collections;
using UnityEngine;

namespace NoStackDev.BigMoney
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(GroundDetection))]
    public class PlayerSlide : MonoBehaviour
    {
        private Rigidbody rb;
        private CapsuleCollider cCollider;
        private InputManager inputManager; // Testing
        private PlayerMovement playerMovement;
        private GroundDetection groundDetection;
        [SerializeField] private Transform orientation;


        [Header("Sliding")]
        [SerializeField] private float maxSlideTime;
        [SerializeField] private float slideForce = 100f;
        [SerializeField] private float slideCooldown = 2f;
        private float slideTimer;
        private bool readyToSlide = true;

        private float originalHeight;
        private float reducedHeight = 1f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            cCollider = GetComponentInChildren<CapsuleCollider>();
            inputManager = GetComponent<InputManager>();
            playerMovement = GetComponent<PlayerMovement>();
            groundDetection = GetComponent<GroundDetection>();

            originalHeight = cCollider.height;
        }

        private void Update()
        {
            if (inputManager.slideInput && inputManager.movementInput != Vector2.zero 
                && readyToSlide && !playerMovement.isSliding && groundDetection.isGrounded)
            {
                StartSlide();
            }

            if (playerMovement.isSliding && !inputManager.slideInput)
            {
                StopSlide();
            }
        }

        private void FixedUpdate()
        {
            if (playerMovement.isSliding)
            {
                SlidingMovement();
            }
        }

        private void StartSlide()
        {
            playerMovement.isSliding = true;

            cCollider.height = reducedHeight;
            transform.localScale = new Vector3(transform.localScale.x, reducedHeight * 0.5f, transform.localScale.z); // Only temporary until a model and animation is added.

            rb.AddRelativeForce(Vector3.down * 2f, ForceMode.Impulse);

            slideTimer = maxSlideTime;
        }

        private void SlidingMovement()
        {
            Vector3 inputDirection = orientation.forward * inputManager.movementInput.y + orientation.right * inputManager.movementInput.x;

            if (!groundDetection.OnSlope() || rb.velocity.y > -0.1f)
            {
                rb.AddRelativeForce(inputDirection.normalized * slideForce * (rb.velocity.magnitude * 0.35f), ForceMode.Force);

                slideTimer -= Time.deltaTime;
            }
            else
            {
                rb.AddRelativeForce(groundDetection.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
            }

            if (slideTimer <= 0)
            {
                StopSlide();
            }
        }

        private void StopSlide()
        {
            playerMovement.isSliding = false;

            cCollider.height = originalHeight;
            transform.localScale = new Vector3(transform.localScale.x, originalHeight * 0.5f, transform.localScale.z); // Only temporary until a model and animation is added.

            readyToSlide = false;

            Invoke(nameof(ResetSlide), slideCooldown);
        }

        private void ResetSlide()
        {
            readyToSlide = true;
        }
    }
}