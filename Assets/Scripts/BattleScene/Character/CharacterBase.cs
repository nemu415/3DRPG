using System.Collections.Generic;
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
    protected bool m_IsPlayer;

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

    public bool IsPlayer() { return m_IsPlayer; }

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

   public virtual void UseItem()
    {

    }

    public void SetStatusText(StatusText m_StatusText)
    {
        m_StatusText.SetStatus(m_Hp, m_Mp, m_Name);
    }

    public void Action(CharacterManager.ActionType type, List<CharacterBase> characterList)
    {
        switch (type)
        {
            case CharacterManager.ActionType.ATTACK:
                if (this.gameObject.CompareTag("Enemy"))
                {
                    Attack(characterList[0]);
                    TextManager.Instance.SetMessageText(this.GetName() + "の攻撃！"
                        + "\n" + characterList[0].GetName() + "に" + this.GetPower() + "ダメージ！");
                }
                else if (this.gameObject.CompareTag("Player"))
                {
                    Attack(characterList[1]);
                    TextManager.Instance.SetMessageText(this.GetName() + "の攻撃！"
                        + "\n" + characterList[1].GetName() + "に" + this.GetPower() + "ダメージ！");
                }

                break;

            case CharacterManager.ActionType.MAGIC:
                if (this.gameObject.CompareTag("Enemy"))
                {
                    Magic(characterList[0]);
                    TextManager.Instance.SetMessageText(this.GetName() + "の魔法！"
                        + "\n" + characterList[0].GetName() + "に" + this.GetMagic() + "ダメージ！");
                }
                else if (this.gameObject.CompareTag("Player"))
                {
                    Magic(characterList[1]);
                    TextManager.Instance.SetMessageText(this.GetName() + "の魔法！"
                        + "\n" + characterList[1].GetName() + "に" + this.GetMagic() + "ダメージ！");
                }

                break;

            case CharacterManager.ActionType.ITEM:
                TextManager.Instance.CreateText(TextManager.TextType.ITEM_TEXT);
                TextManager.Instance.SetItemText();
                break;

            case CharacterManager.ActionType.ESCAPE:


                break;

            default: break;
        }
    }
}
