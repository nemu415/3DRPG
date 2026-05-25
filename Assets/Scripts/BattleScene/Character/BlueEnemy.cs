using UnityEngine;

public class BlueEnemy : CharacterBase
{
    private static int m_BlueMaxHp = 70;
    private static int m_BlueMaxMp = 50;

    private void Awake()
    {
        m_MaxHp = m_BlueMaxHp;
        m_MaxMp = m_BlueMaxMp;

        m_Hp = m_MaxHp;
        m_Mp = m_MaxMp;
        m_Power = 4;
        m_Magic = 8;
        m_Speed = 12;
        m_MagicType = MagicType.WATER;
        m_Name = "BlueEnemy";
        m_Acted = false;
    }
}
