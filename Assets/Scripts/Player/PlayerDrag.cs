using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class PlayerDrag : MonoBehaviour
    {
        private Rigidbody rb;
        private GroundDetection groundDetection;

        [SerializeField] private float groundDrag = 6f;
        [SerializeField] private float airDrag = 1f;
        [SerializeField] private float slideDrag = 3f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            groundDetection = GetComponent<GroundDetection>();
        }

        private void Update()
        {
            CalculateDrag();
        }

        private void CalculateDrag()
        {
            if (groundDetection.isGrounded)
            {
                rb.drag = groundDrag;
            }
            else if (GetComponent<PlayerMovement>().isSliding && groundDetection.isGrounded)
            {
                rb.drag = slideDrag;
            }
            else
            {
                rb.drag = airDrag;
            }
        }
    }
}