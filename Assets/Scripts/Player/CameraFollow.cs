using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smooth = 10f;

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            target.position,
            smooth * Time.deltaTime
        );
    }
}
