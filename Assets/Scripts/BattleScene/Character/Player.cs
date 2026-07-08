using System.Threading.Tasks;
using UnityEngine;
using static BattleManager;

public class Player : CharacterBase
{
    private static int m_PlayerMaxHp = 5;
    private static int m_PlayerMaxMp = 80;

    protected override string MoveForwardAnimationName => "MoveFWD_Battle_InPlace_SwordAndShield";
    protected override string MoveBackAnimationName => "MoveBWD_Battle_InPlace_SwordAndShield";
    protected override string IdleAnimationName => "Idle_Battle_SwordAndShield";
    protected override string AttackAnimationName => "Attack01_SwordAndShiled";
    protected override string MagicAnimationName => "Attack04_Spinning_SwordAndShield";
    protected override string DamageAnimationName => "GetHit01_SwordAndShield";
    protected override string DieAnimationName => "Die01_SwordAndShield";

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
}
