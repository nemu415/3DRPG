using System.Xml.Linq;
using UnityEngine;
using static CharacterBase;

public class Slime : CharacterBase
{
    private static int m_BlueMaxHp = 5;
    private static int m_BlueMaxMp = 50;
    private static int m_BlueAttackPercant = 0;

    protected override string MoveForwardAnimationName => "RunFWD";
    protected override string MoveBackAnimationName => "WalkBWD";
    protected override string IdleAnimationName => "IdleBattle";
    protected override string AttackAnimationName => "Attack01";
    protected override string MagicAnimationName => "Attack02";
    protected override string DamageAnimationName => "GetHit";
    protected override string DieAnimationName => "Die";

    private void Awake()
    {
        m_MaxHp = m_BlueMaxHp;
        m_MaxMp = m_BlueMaxMp;
        m_AttackPercent = m_BlueAttackPercant;

        m_Hp = m_MaxHp;
        m_Mp = m_MaxMp;
        m_Power = 4;
        m_Magic = 10;
        m_Speed = 12;
        m_MagicType = MagicType.FIRE;
        m_Name = "ƒXƒ‰ƒCƒ€";
        m_Acted = false;
    }
}
