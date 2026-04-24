using UnityEngine;

public class GameScenePlayer : MonoBehaviour
{
    private Rigidbody m_Rigidbody = null;

    [SerializeField]
    private float m_MoveSpeed = 15.0f;

    [SerializeField]
    private float m_RotationSpeed = 10.0f;

    [SerializeField]
    private float m_JumpPow = 5.0f;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {

        }
    }
}
