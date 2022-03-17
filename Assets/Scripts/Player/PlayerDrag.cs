<<<<<<< HEAD
=======
using System;
>>>>>>> 2babb41109e75060e8ad637adf3539a9536d8b04
using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class PlayerDrag : MonoBehaviour
    {
        private Rigidbody rb;
<<<<<<< HEAD
        private GroundDetection groundDetection;

        [SerializeField] private float groundDrag = 6f;
        [SerializeField] private float airDrag = 1f;
        [SerializeField] private float slideDrag = 3f;
=======
        private PlayerMovement playerMovement;
        private GroundDetection groundDetection;

        [SerializeField] private float moveDrag;
        [SerializeField] private float brakingDrag = 6f;
        [SerializeField] private float slopeDrag = 45f;
        [SerializeField] private float airDrag = 1f;
>>>>>>> 2babb41109e75060e8ad637adf3539a9536d8b04

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            groundDetection = GetComponent<GroundDetection>();
<<<<<<< HEAD
=======
            playerMovement = GetComponent<PlayerMovement>();
>>>>>>> 2babb41109e75060e8ad637adf3539a9536d8b04
        }

        private void Update()
        {
            CalculateDrag();
        }

        private void CalculateDrag()
        {
<<<<<<< HEAD
            if (groundDetection.isGrounded)
            {
                rb.drag = groundDrag;
            }
            else if (GetComponent<PlayerMovement>().isSliding && groundDetection.isGrounded)
            {
                rb.drag = slideDrag;
=======
            if (groundDetection.isGrounded && playerMovement.movementInput == Vector2.zero && !groundDetection.OnSlope())
            {
                rb.drag = brakingDrag;
            }
            else if (groundDetection.isGrounded && playerMovement.movementInput != Vector2.zero)
            {
                moveDrag = (playerMovement.sumOfAppliedAccelerations).magnitude /
                    (rb.mass * playerMovement.maxSpeed + (playerMovement.sumOfAppliedAccelerations).magnitude * Time.fixedDeltaTime);

                rb.drag = moveDrag;
            }
            else if (groundDetection.isGrounded && playerMovement.movementInput == Vector2.zero && groundDetection.OnSlope())
            {
                rb.drag = slopeDrag;
>>>>>>> 2babb41109e75060e8ad637adf3539a9536d8b04
            }
            else
            {
                rb.drag = airDrag;
            }
        }
    }
}