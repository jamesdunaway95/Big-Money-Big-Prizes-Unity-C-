using System.Collections;
using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class PlayerDash : MonoBehaviour
    {
        private Rigidbody rb;
        private PlayerMovement playerMovement;

        [SerializeField] private float dashCooldown = 2f;
        [SerializeField] private float distance = 5.0f;
        private bool canDash = true;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        private IEnumerator DashCooldown()
        {
            yield return new WaitForSeconds(dashCooldown);

            canDash = true;
        }

        // THIS IS BROKEN
        // WHY IS THIS MY LIFE
        public void Dash()
        {
            if (!canDash) return;
            if (playerMovement.moveDirection == Vector3.zero) return;

            RaycastHit hit;
            Vector3 direction = new Vector3(playerMovement.moveDirection.x, 0f, playerMovement.moveDirection.y);
            Vector3 destination = rb.position + direction * distance;

            // If the player is going to collide with a wall, dash to just before it.
            if (Physics.Linecast(rb.position, destination, out hit))
            {
                destination = rb.position + direction * (hit.distance - 1f);
            }

            // If there is no collision, execute the full dash.
            if (Physics.Raycast(destination, -Vector3.up, out hit))
            {
                destination = hit.point;
                destination.y = rb.position.y;
                rb.MovePosition(destination);
                canDash = false;
                StartCoroutine(DashCooldown());
            }
        }
    }
}