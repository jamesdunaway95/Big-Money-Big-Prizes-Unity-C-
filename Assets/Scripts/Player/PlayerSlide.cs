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


        private float originalHeight;
        [SerializeField] private float reducedHeight;
        [SerializeField] private float slideForce = 100f;
        [SerializeField] private float slideCooldown = 1f;
        private bool canSlide = true;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            cCollider = GetComponentInChildren<CapsuleCollider>();
            inputManager = GetComponent<InputManager>();
            playerMovement = GetComponent<PlayerMovement>();
            groundDetection = GetComponent<GroundDetection>();

            originalHeight = cCollider.height;
        }
        private IEnumerator SlideCoolDown()
        {
            yield return new WaitForSeconds(slideCooldown);

            canSlide = true;
        }


        private void Update()
        {
            if (playerMovement.isSliding && rb.velocity.magnitude < 2f)
            {
                StopSlide();
            }
        }

        public void Slide()
        {
            if (playerMovement.isSliding)
            {
                StopSlide();
            }

            if (rb.velocity.magnitude < 2f || !canSlide || playerMovement.movementInput.y <= 0) return;


            playerMovement.isSliding = true;
            canSlide = false;

            cCollider.height = reducedHeight;
            transform.localScale = new Vector3(transform.localScale.x, reducedHeight * 0.5f, transform.localScale.z); // Only temporary until a model and animation is added.

            rb.AddRelativeForce(Vector3.down * 5f, ForceMode.Force);


            if (!groundDetection.OnSlope() || rb.velocity.y > -0.1f) // Normal slide
            {
                rb.AddRelativeForce(orientation.forward * slideForce * rb.velocity.magnitude, ForceMode.Force);
            }
            else // Down slope slide
            {
                rb.AddRelativeForce(groundDetection.GetSlopeMoveDirection(orientation.forward) * slideForce * rb.velocity.magnitude, ForceMode.Force);
            }
        }

        private void StopSlide()
        {
            playerMovement.isSliding = false;

            StartCoroutine(SlideCoolDown());

            cCollider.height = originalHeight;
            transform.localScale = new Vector3(transform.localScale.x, originalHeight * 0.5f, transform.localScale.z); // Only temporary until a model and animation is added.
        }
    }
}