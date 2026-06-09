using UnityEngine;

public class RedEnemy : CharacterBase
{
    private static int m_RedMaxHp = 10;
    private static int m_RedMaxMp = 30;
    private static int m_RedAttackPercant = 30;

    private void Awake()
    {
        m_MaxHp = m_RedMaxHp;
        m_MaxMp = m_RedMaxMp;
        m_AttackPercent = m_RedAttackPercant;

        m_Hp = m_MaxHp;
        m_Mp = m_MaxMp;
        m_Power = 2;
        m_Magic = 13;
        m_Speed = 7;
        m_MagicType = MagicType.FIRE;
        m_Name = "RedEnemy";
        m_Acted = false;
    }
}
