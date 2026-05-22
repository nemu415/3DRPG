using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private Transform cameraPivot;
    [SerializeField] private GameCamera cameraController;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 25f;
    [SerializeField] private float jumpPower = 5f;


    private Vector3 moveInput;
    private float jumpRequest;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
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

        moveInput = moveInput.normalized * moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
            jumpRequest = jumpPower;
    }


    void FixedUpdate()
    {
        // --- ƒJƒƒ‰‚ª“®‚¢‚Ä‚¢‚È‚¢‚È‚ç‰ñ“]‚µ‚È‚¢ ---
        if (Mathf.Abs(cameraController.RawMouseX) > 0.01f)
        {
            Vector3 camForward = cameraPivot.forward;
            camForward.y = 0;
            camForward.Normalize();

            Quaternion targetRot = Quaternion.LookRotation(camForward);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.fixedDeltaTime
            );
        }


        // --- Movement ---
        float y = rb.linearVelocity.y;

        if (jumpRequest != 0)
        {
            y = jumpRequest;
            jumpRequest = 0;
        }

        rb.linearVelocity = new Vector3(moveInput.x, y, moveInput.z);
    }


}
