using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class GroundDetection : MonoBehaviour
    {
        [Header("Ground Detection")]
        [SerializeField] private Transform groundDetection;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float playerHeight = 2f;
        [SerializeField] private float groundDetectionDistance = 0.3f;
        public bool isGrounded;

        [Header("Slopes")]
        [SerializeField] private float maxSlopeAngle;
        [HideInInspector] public RaycastHit slopeHit;

        public bool OnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, (playerHeight * 0.5f) + 0.5f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

                return angle < maxSlopeAngle && angle != 0;
            }

            return false;
        }

        public Vector3 GetSlopeMoveDirection(Vector3 direction)
        {
            return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
        }

        private void Update()
        {
            HandleGroundDetection();
        }

        private void HandleGroundDetection()
        {
            isGrounded = Physics.CheckSphere(groundDetection.position, groundDetectionDistance, groundLayer);
        }

    }
}