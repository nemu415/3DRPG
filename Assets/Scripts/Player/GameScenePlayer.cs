using UnityEngine;

public class GameScenePlayer : MonoBehaviour
{
    private Rigidbody m_Rigidbody = null;

    [SerializeField]
    private float m_MoveSpeed = 15.0f;

    [SerializeField]
    private float m_RotationSpeed = 3.0f;

    [SerializeField]
    private float m_JumpPow = 5.0f;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 moveXZ = Vector3.zero;

        float mouseX = Input.GetAxis("Mouse X") * m_RotationSpeed;

        transform.Rotate(0, mouseX, 0);

        if (Input.GetKeyDown(KeyCode.W))
        {
            moveXZ = transform.forward * m_MoveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            moveXZ = -transform.forward * m_MoveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            moveXZ += transform.right * m_MoveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            moveXZ -= transform.right * m_MoveSpeed;
        }
    }
}
