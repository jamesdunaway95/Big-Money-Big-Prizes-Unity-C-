using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform followTarget;

    private void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        transform.position = followTarget.position;
    }

}
