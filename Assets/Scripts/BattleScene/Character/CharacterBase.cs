using System.ComponentModel;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    protected int m_MaxHp;
    protected int m_MaxMp;

    protected int m_Hp;
    protected int m_Mp;
    protected int m_Power;
    protected int m_Magic;
    protected int m_Speed;
    protected string m_Name;
    protected bool m_Acted;

    public enum MagicType
    {
        FIRE,
        WATER,
        THUNDER,
        HEAL,
    }

    protected MagicType m_MagicType;


    private void Start()
    {

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

    public void Damage(int damage)
    {
        m_Hp -= damage;

        if (m_Hp < 0)
        {
            m_Hp = 0;
        }
    }

    public void Attack(CharacterBase opponent)
    {
        opponent.Damage(m_Power);
    }

    public void Magic(CharacterBase opponent)
    {
        opponent.Damage(m_Magic);
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
