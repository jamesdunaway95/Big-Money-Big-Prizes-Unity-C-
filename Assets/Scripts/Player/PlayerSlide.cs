using System.Collections;
using UnityEngine;

namespace NoStackDev.BigMoney
{
<<<<<<< HEAD
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(GroundDetection))]
=======
>>>>>>> 2babb41109e75060e8ad637adf3539a9536d8b04
    public class PlayerSlide : MonoBehaviour
    {
        private Rigidbody rb;
        private CapsuleCollider cCollider;
<<<<<<< HEAD
        private InputManager inputManager; // Testing
        private PlayerMovement playerMovement;
        private GroundDetection groundDetection;
        [SerializeField] private Transform orientation;


        private float originalHeight;
        [SerializeField] private float reducedHeight;
        [SerializeField] private float slideForce = 100f;
        [SerializeField] private float slideCooldown = 1f;
=======
        [SerializeField] private Transform orientation;

        private float originalHeight;
        [SerializeField] private float reducedHeight;
        [SerializeField] private float slideSpeed = 10f;
        [SerializeField] private float slideCooldown = 1f;
        public bool isSliding = false;
>>>>>>> 2babb41109e75060e8ad637adf3539a9536d8b04
        private bool canSlide = true;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            cCollider = GetComponentInChildren<CapsuleCollider>();
<<<<<<< HEAD
            inputManager = GetComponent<InputManager>();
            playerMovement = GetComponent<PlayerMovement>();
            groundDetection = GetComponent<GroundDetection>();

=======
>>>>>>> 2babb41109e75060e8ad637adf3539a9536d8b04
            originalHeight = cCollider.height;
        }
        private IEnumerator SlideCoolDown()
        {
            yield return new WaitForSeconds(slideCooldown);

            canSlide = true;
        }


        private void Update()
        {
<<<<<<< HEAD
            if (playerMovement.isSliding && rb.velocity.magnitude < 2f)
=======
            if (isSliding && rb.velocity.magnitude < 2f)
>>>>>>> 2babb41109e75060e8ad637adf3539a9536d8b04
            {
                StopSlide();
            }
        }

        public void Slide()
        {
<<<<<<< HEAD
            if (playerMovement.isSliding)
=======
            if (isSliding)
>>>>>>> 2babb41109e75060e8ad637adf3539a9536d8b04
            {
                StopSlide();
            }

<<<<<<< HEAD
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
=======
            if (rb.velocity.magnitude < 2f || !canSlide) return;

            isSliding = true;
            canSlide = false;

            cCollider.height = reducedHeight;
            rb.AddForce(orientation.forward * slideSpeed, ForceMode.VelocityChange);
>>>>>>> 2babb41109e75060e8ad637adf3539a9536d8b04
        }

        private void StopSlide()
        {
<<<<<<< HEAD
            playerMovement.isSliding = false;

            StartCoroutine(SlideCoolDown());

            cCollider.height = originalHeight;
            transform.localScale = new Vector3(transform.localScale.x, originalHeight * 0.5f, transform.localScale.z); // Only temporary until a model and animation is added.
=======
            cCollider.height = originalHeight;
            isSliding = false;
            StartCoroutine(SlideCoolDown());
>>>>>>> 2babb41109e75060e8ad637adf3539a9536d8b04
        }
    }
}