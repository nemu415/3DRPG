using UnityEngine;

public class Cactus : CharacterBase
{
    private static int m_YellowMaxHp = 5;
    private static int m_YellowMaxMp = 10;
    private static int m_YellowAttackPercant = 0;

    protected override string MoveForwardAnimationName => "Cactus_RunFWD";
    protected override string MoveBackAnimationName => "Cactus_WalkBWD";
    protected override string IdleAnimationName => "Cactus_IdleNormal";
    protected override string AttackAnimationName => "Cactus_Attack01";
    protected override string MagicAnimationName => "Cactus_Attack02";
    protected override string DamageAnimationName => "Cactus_GetHit";
    protected override string DieAnimationName => "Cactus_Die";

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
        m_Name = "ƒTƒ{ƒeƒ“";
    }
}
