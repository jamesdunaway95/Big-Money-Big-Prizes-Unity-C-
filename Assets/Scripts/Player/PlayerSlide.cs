using System.Collections;
using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class PlayerSlide : MonoBehaviour
    {
        private Rigidbody rb;
        private CapsuleCollider cCollider;
        [SerializeField] private Transform orientation;

        private float originalHeight;
        [SerializeField] private float reducedHeight;
        [SerializeField] private float slideSpeed = 10f;
        [SerializeField] private float slideCooldown = 1f;
        public bool isSliding = false;
        private bool canSlide = true;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            cCollider = GetComponentInChildren<CapsuleCollider>();
            originalHeight = cCollider.height;
        }
        private IEnumerator SlideCoolDown()
        {
            yield return new WaitForSeconds(slideCooldown);

            canSlide = true;
        }


        private void Update()
        {
            if (isSliding && rb.velocity.magnitude < 2f)
            {
                StopSlide();
            }
        }

        public void Slide()
        {
            if (isSliding)
            {
                StopSlide();
            }

            if (rb.velocity.magnitude < 2f || !canSlide) return;

            isSliding = true;
            canSlide = false;

            cCollider.height = reducedHeight;
            rb.AddForce(orientation.forward * slideSpeed, ForceMode.VelocityChange);
        }

        private void StopSlide()
        {
            cCollider.height = originalHeight;
            isSliding = false;
            StartCoroutine(SlideCoolDown());
        }
    }
}