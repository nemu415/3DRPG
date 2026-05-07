using UnityEngine;
using static Player;

public class Enemy : MonoBehaviour
{
    public int m_Hp;
    private static int m_MaxHp =70;
    public int m_Mp;
    private static int m_MaxMp = 60;
    private int m_Power;
    private int m_Magic;
    private int m_Speed;
    private static string m_Name;

    public enum EnemyMagicType
    {
        âä,
        êÖ,
        óã,
        âÒïú,
        MAGIC_TYPE_MAX,
        MAGIC_TYPE_NONE = -1,
    }

    public EnemyMagicType m_Type;

    [SerializeField]
    private Player m_Player;

    private void Start()
    {
        m_Hp = m_MaxHp;
        m_Mp = m_MaxMp;
        m_Power = 7;
        m_Magic = 2;
        // m_Speed = 30;
        m_Name = "BlueEnemy";
        m_Type = EnemyMagicType.êÖ;
    }

    private void Update()
    {
        
    }

    public int GetPower() { return m_Power; }
    public int GetMagic() { return m_Magic; }

    public string GetName() { return m_Name; }

    public void Damage(int damage)
    {
        m_Hp -= damage;
    }

    public void Attack()
    {
        m_Player.Damage(m_Power);
    }

    public void Magic()
    {
        m_Player.Damage(m_Magic);
        m_Mp -= 5;
    }
}
