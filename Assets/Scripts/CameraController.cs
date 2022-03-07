using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraFollow;

        [Header("FOV")]
        private float baseFov;
        private float wallRunFov;

        private void Update()
        {
            FollowPlayer();
        }

        private void FollowPlayer()
        {
            transform.position = cameraFollow.position;
        }
    }
}