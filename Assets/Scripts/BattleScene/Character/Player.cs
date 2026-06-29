using System.Threading.Tasks;
using UnityEngine;
using static BattleManager;

public class Player : CharacterBase
{
    private static int m_PlayerMaxHp = 300;
    private static int m_PlayerMaxMp = 80;

    private void Awake()
    {
        m_MaxHp = m_PlayerMaxHp;
        m_MaxMp = m_PlayerMaxMp;

        m_Hp = m_MaxHp;
        m_Mp = m_MaxMp;
        m_Power = 5;
        m_Magic = 10;
        m_Speed = 10;
        m_MagicType = MagicType.FIRE;
        m_Name = "Player";
        m_Acted = false;
        m_IsPlayer = true;
    }

    void LateUpdate()
    {
        if (transform.hasChanged)
        {
            Debug.Log($"{gameObject.name} Ç™ìÆÇ´Ç‹ÇµÇΩÅIåªç›ÇÃç¿ïW: {transform.position}", gameObject);
            transform.hasChanged = false;
        }
    }

}
