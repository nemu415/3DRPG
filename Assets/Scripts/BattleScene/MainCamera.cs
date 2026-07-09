using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private readonly Vector3 m_StartPosition = new(-2.27f, 2.2f, -1.5f);
    private readonly Vector3 m_StartRotation = new(5.0f, 90.0f, 0.0f);

    private readonly Vector3 m_BasePosition = new(-5.5f, 3.8f, -3.5f);
    private readonly Vector3 m_BaseRotation = new(20.0f, 70.0f, 0.0f);

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
