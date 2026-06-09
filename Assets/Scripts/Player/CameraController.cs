using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;      // プレイヤー
    public float distance = 4f;   // プレイヤーとの距離
    public float xSpeed = 180f;   // マウス横回転速度
    public float ySpeed = 120f;   // マウス縦回転速度
    public float yMin = -20f;     // 下向き制限
    public float yMax = 60f;      // 上向き制限

    float x = 0f;
    float y = 0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
        y = Mathf.Clamp(y, yMin, yMax);

        Quaternion rot = Quaternion.Euler(y, x, 0);
        Vector3 pos = rot * new Vector3(0, 0, -distance) + target.position;

        transform.rotation = rot;
        transform.position = pos;
    }
}
