using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class GroundDetection : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float playerHeight = 2f;
        [SerializeField] private float groundDetectionDistance = 0.4f;
        public bool isGrounded;

        private RaycastHit slopeHit;

        public RaycastHit SlopeHit()
        {
            return slopeHit;
        }

        public bool OnSlope()
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

        private void Update()
        {
            HandleGroundDetection();
        }

        private void HandleGroundDetection()
        {
            isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), groundDetectionDistance, groundLayer);
        }

    }
}