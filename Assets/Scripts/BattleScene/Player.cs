using UnityEngine;

public class Player : MonoBehaviour
{
    private int m_Hp;
    private static int m_MaxHp = 100;
    private int m_Mp;
    private static int m_MaxMp = 80;
    private int m_Power;
    private int m_Magic;
    private int m_Speed;
    private string m_Name;
    private bool m_Acted;

    public enum PlayerMagicType
    {
        âä,
        êÖ,
        óã,
        âÒïú,
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
        m_Speed = 10;
        m_Type = PlayerMagicType.âä;
        m_Name = "Player";
        m_Acted = false;
    }

    private void Update()
    {

    }

    public int GetHP() { return m_Hp; }

    public int GetMP() { return m_Mp; }

    public int GetPower() { return m_Power; }
    public int GetMagic() { return m_Magic; }

    public int GetSpeed() { return m_Speed; }

    public string GetName() { return m_Name; }

    public bool IsActed() { return m_Acted; }

    public void Act() { m_Acted = true; }

    public void ActedReset() { m_Acted = false; }

    public void Damage(int  damage)
    {
        m_Hp -= damage;

        if (m_Hp < 0)
        {
            m_Hp = 0;
        }
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

    public void HPHeal(int heal)
    {
        m_Hp += heal;

        if (m_Hp > m_MaxHp)
        {
            m_Hp = m_MaxHp;
        }
    }

    public void MPHeal(int heal)
    {
        m_Mp += heal;

        if (m_Mp > m_MaxMp)
        {
            m_Mp = m_MaxMp;
        }
    }
}
