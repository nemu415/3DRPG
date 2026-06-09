using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 8f;
    public float gravity = -20f;

    CharacterController controller;
    Animator anim;

    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(h, 0, v).normalized;

        // カメラ方向に合わせて移動方向を変換
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camForward * input.z + camRight * input.x;

        // アニメーションに速度を渡す
        float speed = move.magnitude;

        // プレイヤーを移動方向に向ける
        if (move != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.15f);
        }

        // 接地判定
        if (controller.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -2f;

            // ジャンプ
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = jumpPower;
            }
        }

        // 重力
        velocity.y += gravity * Time.deltaTime;

        // 移動
        controller.Move((move * moveSpeed + velocity) * Time.deltaTime);

        anim.SetFloat("Speed", move.magnitude);
        anim.SetFloat("VerticalSpeed", velocity.y);
        anim.SetBool("IsGrounded", controller.isGrounded);

    }
}
