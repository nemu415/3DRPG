using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Camera playerCamera;

    [SerializeField] private float mouseSmoothSpeed = 10f;
    private float smoothedMouseX;

    public float SmoothedMouseX => smoothedMouseX;

    private float cameraPitch = 0f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 45f;

    [SerializeField] private float cameraDistance = 5f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 8f;
    [SerializeField] private float zoomSpeed = 2f;

    [SerializeField] private float cameraRotateSpeed = 120f;
    [SerializeField] private float mouseSensitivity = 1.0f;
    public float RawMouseX { get; private set; }

    void Update()
    {
        RawMouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // --- 水平回転（一定速度） ---
        if (Mathf.Abs(RawMouseX) > 0.01f)
        {
            float yaw = RawMouseX > 0 ? 1f : -1f;
            cameraPivot.Rotate(0, yaw * cameraRotateSpeed * Time.deltaTime, 0);
        }

        // 垂直回転
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);
        playerCamera.transform.localEulerAngles = new Vector3(cameraPitch, 0, 0);

        // ズーム
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cameraDistance -= scroll * zoomSpeed;
        cameraDistance = Mathf.Clamp(cameraDistance, minDistance, maxDistance);
    }



    void LateUpdate()
    {
        Vector3 offset = new Vector3(0, 0, -cameraDistance);
        playerCamera.transform.position = cameraPivot.TransformPoint(offset);
        playerCamera.transform.rotation = cameraPivot.rotation;
    }
}
