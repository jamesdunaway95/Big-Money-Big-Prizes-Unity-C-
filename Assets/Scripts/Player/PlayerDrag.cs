using System;
using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class PlayerDrag : MonoBehaviour
    {
        private Rigidbody rb;
        private PlayerMovement playerMovement;
        private GroundDetection groundDetection;

        [SerializeField] private float moveDrag;
        [SerializeField] private float brakingDrag = 6f;
        [SerializeField] private float slopeDrag = 45f;
        [SerializeField] private float airDrag = 1f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            groundDetection = GetComponent<GroundDetection>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            CalculateDrag();
        }

        private void CalculateDrag()
        {
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
            }
            else
            {
                rb.drag = airDrag;
            }
        }
    }
}