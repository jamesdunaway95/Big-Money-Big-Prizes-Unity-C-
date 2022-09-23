using UnityEngine;

namespace NoStackDev.BigMoney
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(GroundDetection))]
    public class PlayerSlide : MonoBehaviour
    {
        private Rigidbody rb;
        private CapsuleCollider capsuleCollider;
        private InputManager inputManager;
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
            capsuleCollider = GetComponent<CapsuleCollider>();
            inputManager = GetComponent<InputManager>();
            playerMovement = GetComponent<PlayerMovement>();
            groundDetection = GetComponent<GroundDetection>();

            originalHeight = capsuleCollider.height;
        }

        private void Update()
        {
            // Crouching
            if (inputManager.crouchInput && !playerMovement.isSliding && groundDetection.isGrounded)
            {
                Crouch();
            }

            if (!inputManager.crouchInput && !inputManager.slideInput && playerMovement.isCrouching) StopCrouch();

            // Sliding
            if (inputManager.slideInput && inputManager.movementInput != Vector2.zero 
                && readyToSlide && !playerMovement.isSliding && !playerMovement.isCrouching && groundDetection.isGrounded)
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

        #region Sliding
        private void StartSlide()
        {
            playerMovement.isSliding = true;

            capsuleCollider.height = reducedHeight;
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
            if (inputManager.slideInput)
            {
                playerMovement.isSliding = false;

                readyToSlide = false;

                Invoke(nameof(ResetSlide), slideCooldown);

                Crouch();

                return;
            }

            playerMovement.isSliding = false;

            capsuleCollider.height = originalHeight;
            transform.localScale = new Vector3(transform.localScale.x, originalHeight * 0.5f, transform.localScale.z); // Only temporary until a model and animation is added.

            readyToSlide = false;

            Invoke(nameof(ResetSlide), slideCooldown);
        }

        private void ResetSlide()
        {
            readyToSlide = true;
        }
        #endregion

        #region Crouching
        private void Crouch()
        {
            playerMovement.isCrouching = true;

            capsuleCollider.height = reducedHeight;
            transform.localScale = new Vector3(transform.localScale.x, reducedHeight * 0.5f, transform.localScale.z); // Only temporary until a model and animation is added.

            rb.AddRelativeForce(Vector3.down * 5f, ForceMode.Force);
        }

        private void StopCrouch()
        {
            playerMovement.isCrouching = false;

            capsuleCollider.height = originalHeight;
            transform.localScale = new Vector3(transform.localScale.x, originalHeight * 0.5f, transform.localScale.z); // Only temporary until a model and animation is added.
        }
        #endregion
    }
}