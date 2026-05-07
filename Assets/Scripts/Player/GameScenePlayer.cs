using UnityEngine;

public class GameScenePlayer : MonoBehaviour
{
    private Rigidbody m_Rigidbody;

    // --- Player用 ---
    [SerializeField] private float m_MoveSpeed = 5f;
    [SerializeField] private float m_RotationSpeed = 5f;
    [SerializeField] private float m_JumpPow = 5f;

    // --- カメラ用 ---
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

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // --- カメラ回転 ---
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        cameraPivot.Rotate(0, mouseX, 0);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);
        playerCamera.transform.localEulerAngles = new Vector3(cameraPitch, 0, 0);

        // --- ズーム ---
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cameraDistance -= scroll * zoomSpeed;
        cameraDistance = Mathf.Clamp(cameraDistance, minDistance, maxDistance);

        // --- 移動入力 ---
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
        // プレイヤーの向きを移動方向に合わせる
        if (moveInput.sqrMagnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.fixedDeltaTime * m_RotationSpeed);
        }

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
        // Pivotのローカル後方にカメラを配置
        Vector3 localOffset = new Vector3(0, 0, -cameraDistance);

        // Pivotの回転を適用してワールド座標へ
        playerCamera.transform.position = cameraPivot.TransformPoint(localOffset);

        // カメラの向きはPivotに合わせる
        playerCamera.transform.rotation = cameraPivot.rotation;
    }

}

