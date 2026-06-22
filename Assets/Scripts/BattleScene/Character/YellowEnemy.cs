using UnityEngine;

public class YellowEnemy : CharacterBase
{
    private static int m_YellowMaxHp = 60;
    private static int m_YellowMaxMp = 20;
    private static int m_YellowAttackPercant = 30;

    private void Awake()
    {
        m_MaxHp = m_YellowMaxHp;
        m_MaxMp = m_YellowMaxMp;
        m_AttackPercent = m_YellowAttackPercant;

        m_Hp = m_MaxHp;
        m_Mp = m_MaxMp;
        m_Power = 5;
        m_Magic = 10;
        m_Speed = 10;
        m_MagicType = MagicType.THUNDER;
        m_Name = "YellowEnemy";
        m_Acted = false;
    }
}
