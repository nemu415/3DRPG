using UnityEngine;

public class Player : MonoBehaviour
{
    public int m_Hp;
    private static int m_MaxHp = 100;
    public int m_Mp;
    private static int m_MaxMp = 100;
    private int m_Power;
    private int m_Magic;
    private int m_Speed;

    [SerializeField]
    private Enemy m_Enemy;

    private void Start()
    {
        m_Hp = m_MaxHp;
        m_Mp = m_MaxMp;
        m_Power = 5;
        m_Magic = 10;
        //m_Speed = 50;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Attack();
            Debug.Log("攻撃");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Magic();
            Debug.Log("魔法");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("アイテム");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("逃げる");
        }
    }

    public void Damage(int  damage)
    {
        m_Hp -= damage;
    }

    private void Attack()
    {
        m_Enemy.Damage(m_Power);
    }

    private void Magic()
    {
        m_Enemy.Damage(m_Magic);
        m_Mp -= 5;
    }
}
