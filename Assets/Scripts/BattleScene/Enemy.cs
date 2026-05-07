using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int m_Hp;
    private static int m_MaxHp =70;
    public int m_Mp;
    private static int m_MaxMp = 60;
    private int m_Power;
    private int m_Magic;
    private int m_Speed;

    private void Start()
    {
        m_Hp = m_MaxHp;
        m_Mp = m_MaxMp;
        /* m_Power = 30;
         m_Magic = 20;
         m_Speed = 50;*/
    }

    private void Update()
    {
        
    }

    public void Damage(int damage)
    {
        m_Hp -= damage;
    }
}
