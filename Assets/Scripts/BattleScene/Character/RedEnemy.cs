using UnityEngine;

public class RedEnemy : CharacterBase
{
    private static int m_RedMaxHp = 5;
    private static int m_RedMaxMp = 30;
    private static int m_RedAttackPercant = 0;

    protected override string MoveForwardAnimationName => "Mushroom_runFWDSmile";
    protected override string MoveBackAnimationName => "Mushroom_walkBWDSmile";
    protected override string IdleAnimationName => "Mushroom_IdleNormalSmile";
    protected override string AttackAnimationName => "Mushroom_Attack01Smile";
    protected override string MagicAnimationName => "Mushroom_Attack02Smile";
    protected override string DamageAnimationName => "Mushroom_GetHitSmile";
    protected override string DieAnimationName => "Mushroom_DieSmile";


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
