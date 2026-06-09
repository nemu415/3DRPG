using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform target;
    public float defaultDistance = 4f;
    public float minDistance = 0.5f;
    public float smooth = 10f;

    float currentDistance;

    void Start()
    {
        currentDistance = defaultDistance;
    }

    void LateUpdate()
    {
        Vector3 dir = (transform.position - target.position).normalized;

        if (Physics.Raycast(target.position, dir, out RaycastHit hit, defaultDistance))
        {
            currentDistance = Mathf.Clamp(hit.distance, minDistance, defaultDistance);
        }
        else
        {
            currentDistance = defaultDistance;
        }

        transform.position = target.position + dir * currentDistance;
    }
}
