using UnityEngine;

public class BlueEnemy : CharacterBase
{
    private static int m_BlueMaxHp = 50;
    private static int m_BlueMaxMp = 50;
    private static int m_BlueAttackPercant = 0;

    protected override string MoveForwardAnimationName => "RunFWD";
    protected override string MoveBackAnimationName => "WalkBWD";
    protected override string IdleAnimationName => "IdleBattle";
    protected override string AttackAnimationName => "Attack01";
    protected override string MagicAnimationName => "Attack02";
    protected override string DamageAnimationName => "GetHit";

    private void Awake()
    {
        m_MaxHp = m_BlueMaxHp;
        m_MaxMp = m_BlueMaxMp;
        m_AttackPercent = m_BlueAttackPercant;

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
