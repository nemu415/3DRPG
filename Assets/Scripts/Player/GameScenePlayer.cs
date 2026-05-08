using UnityEngine;

public class GameScenePlayer : MonoBehaviour
{
    private Rigidbody m_Rigidbody;

    // --- Player ---
    [SerializeField] private float m_MoveSpeed = 5f;
    [SerializeField] private float m_RotationSpeed = 10f; // 基本回転速度
    [SerializeField] private float m_MouseRotMultiplier = 3f; // マウス動き量で回転速度を増加
    [SerializeField] private float m_JumpPow = 5f;

    // --- Camera ---
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float cameraDistance = 5f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 8f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 45f;

    private float cameraPitch = 0f;
    private Vector3 moveInput = Vector3.zero;
    private float jumpRequest = 0f;

    // --- Smooth rotation ---
    private float smoothedMouseX = 0f;
    private float mouseSmoothSpeed = 10f; // マウス回転のスムージング速度

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // --- Camera rotation ---
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // マウスの動きをスムージング
        smoothedMouseX = Mathf.Lerp(smoothedMouseX, mouseX, Time.deltaTime * mouseSmoothSpeed);

        // カメラ水平回転
        cameraPivot.Rotate(0, smoothedMouseX, 0);

        // カメラ上下回転
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);
        playerCamera.transform.localEulerAngles = new Vector3(cameraPitch, 0, 0);

        // --- Zoom ---
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cameraDistance -= scroll * zoomSpeed;
        cameraDistance = Mathf.Clamp(cameraDistance, minDistance, maxDistance);

        // --- Movement input ---
        moveInput = Vector3.zero;

        Vector3 camForward = cameraPivot.forward;
        Vector3 camRight = cameraPivot.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        if (Input.GetKey(KeyCode.W)) moveInput += camForward;
        if (Input.GetKey(KeyCode.S)) moveInput -= camForward;
        if (Input.GetKey(KeyCode.D)) moveInput += camRight;
        if (Input.GetKey(KeyCode.A)) moveInput -= camRight;

        moveInput = moveInput.normalized * m_MoveSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
            jumpRequest = m_JumpPow;
    }

    void FixedUpdate()
    {
        // --- プレイヤー回転（スムーズ + マウス量で速度変化） ---
        Vector3 camForward = cameraPivot.forward;
        camForward.y = 0;
        camForward.Normalize();

        // マウスの動き量に応じて回転速度を変化
        float dynamicRotSpeed = m_RotationSpeed + Mathf.Abs(smoothedMouseX) * m_MouseRotMultiplier;

        Quaternion targetRot = Quaternion.LookRotation(camForward);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            Time.fixedDeltaTime * dynamicRotSpeed
        );

        // --- Movement ---
        float y = m_Rigidbody.linearVelocity.y;

        if (jumpRequest != 0)
        {
            y = jumpRequest;
            jumpRequest = 0;
        }

        m_Rigidbody.linearVelocity = new Vector3(moveInput.x, y, moveInput.z);
    }

    void LateUpdate()
    {
        // Camera position
        Vector3 localOffset = new Vector3(0, 0, -cameraDistance);
        playerCamera.transform.position = cameraPivot.TransformPoint(localOffset);
        playerCamera.transform.rotation = cameraPivot.rotation;
    }
}
