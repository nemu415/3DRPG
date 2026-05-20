using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private readonly Vector3 m_StartPosition = new(0.0f, 2.0f, 0.0f);
    private readonly Vector3 m_StartRotation = new(5.0f, 90.0f, 0.0f);

    private readonly Vector3 m_BasePosition = new(-5.0f, 3.0f, -2.5f);
    private readonly Vector3 m_BaseRotation = new(20.0f, 55.0f, 0.0f);

    private void Start()
    {
        this.transform.position = m_StartPosition;
        this.transform.rotation = Quaternion.Euler(m_StartRotation);
    }

    public void BattleStart()
    {
        this.transform.position = m_BasePosition;
        this.transform.rotation = Quaternion.Euler(m_BaseRotation);
    }
}
