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
    private string m_Name;

    public enum PlayerMagicType
    {
        âä,
        êÖ,
        óã,
        âÒïú,
        MAGIC_TYPE_MAX,
        MAGIC_TYPE_NONE = -1,
    }

    public PlayerMagicType m_Type;

    [SerializeField]
    private Enemy m_Enemy;

    private void Start()
    {
        m_Hp = m_MaxHp;
        m_Mp = m_MaxMp;
        m_Power = 5;
        m_Magic = 10;
        //m_Speed = 50;
        m_Type = PlayerMagicType.âä;
        m_Name = "Player";
    }

    private void Update()
    {

    }

    public int GetPower() { return m_Power; }
    public int GetMagic() { return m_Magic; }

    public string GetName() { return m_Name; }

    public void Damage(int  damage)
    {
        m_Hp -= damage;
    }

    public void Attack()
    {
        m_Enemy.Damage(m_Power);
    }

    public void Magic()
    {
        m_Enemy.Damage(m_Magic);
        m_Mp -= 5;
    }
}
