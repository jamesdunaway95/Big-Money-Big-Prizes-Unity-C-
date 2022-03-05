using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class MoveCamera : MonoBehaviour
    {
        [SerializeField] private Transform cameraFollow;

        private void Update()
        {
            transform.position = cameraFollow.position;
        }
    }
}