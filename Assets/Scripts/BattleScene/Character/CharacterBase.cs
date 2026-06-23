using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ItemManager;

public class CharacterBase : MonoBehaviour
{
    protected int m_MaxHp;
    protected int m_MaxMp;
    protected int m_AttackPercent;

    protected int m_Hp;
    protected int m_Mp;
    protected int m_Power;
    protected int m_Magic;
    protected int m_Speed;
    protected string m_Name;
    protected bool m_Acted;
    protected bool m_IsPlayer;

    private bool m_IsSelectingItem = false;

    private Animator animator;

    private bool isPlaying = false;

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
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!m_IsSelectingItem) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectItemEnd(ItemType.HP_HEAL);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectItemEnd(ItemType.MP_HEAL);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectItemEnd(ItemType.ESCAPE);
        }

        if (isPlaying)
        {
            transform.localPosition = Vector3.zero;

            isPlaying = false;
        }
    }

    public int GetHP() { return m_Hp; }
    public int GetMP() { return m_Mp; }
    public int GetMaxHP() { return m_MaxHp; }
    public int GetMaxMP() { return m_MaxMp; }

    public int GetPower() { return m_Power; }
    public int GetMagic() { return m_Magic; }

    public int GetSpeed() { return m_Speed; }

    public string GetName() { return m_Name; }

    public bool IsActed() { return m_Acted; }

    public bool IsPlayer() { return m_IsPlayer; }

    public int GetAttackPercent()
    {
        return m_AttackPercent;
    }

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
        isPlaying = true;
        animator.Play("AttackAnim", -1, 0f);
    }

    public void Magic(CharacterBase opponent)
    {
        opponent.Damage(m_Magic);
        m_Mp -= 5;
        animator.Play("PlayerMagic", -1, 0f);
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

    public void Escape()
    {
        //Debug.Log("escape");

        int rand = Random.Range(1, 100);

        int playerSpeed = 0;
        int enemySumSpeed = 0;

        List<CharacterBase> characterList = new List<CharacterBase>();

        characterList = CharacterManager.Instance.GetCharacterList();

        for (int i = 0; i < characterList.Count; i++)
        {
            if (characterList[i] != null) continue;

            if (characterList[i].IsPlayer())
            {
                playerSpeed = characterList[i].GetSpeed();
            }
            else
            {
                enemySumSpeed += characterList[i].GetSpeed();
            }
        }

        int enemyAvarageSpeed = enemySumSpeed / (characterList.Count - 1);

        int border = 50;
        border -= (playerSpeed - enemyAvarageSpeed);

        if (rand > border)
        {
            TextManager.Instance.SetMessageText("逃げ切れた！");

            SceneManager.LoadScene("SoshiKurosawa");
        }
        else
        {
            TextManager.Instance.SetMessageText("逃げられなかった");
        }
    }

   public void UseItem(ItemType type)
   {
        switch (type)
        {
            case ItemType.HP_HEAL:
                HPHeal(10);
                TextManager.Instance.SetMessageText("HP回復");
                break;
            case ItemType.MP_HEAL:
                TextManager.Instance.SetMessageText("MP回復");
                MPHeal(10);
                break;
            case ItemType.ESCAPE:
                TextManager.Instance.SetMessageText("逃走");
                Escape();
                break;
            default:
                break;
        }

        ItemManager.Instance.SpendItem(type);
   }

    public void SetStatusText(StatusText m_StatusText)
    {
        m_StatusText.SetStatus(m_Hp, m_Mp, m_Name);
    }

    public void Action(ActionType type, CharacterBase targetCharacter)
    {
        switch (type)
        {
            case ActionType.ATTACK:
                if (!m_IsPlayer)
                {
                    Attack(targetCharacter);
                    TextManager.Instance.SetMessageText(this.GetName() + "の攻撃！"
                        + "\n" + targetCharacter.GetName() + "に" + this.GetPower() + "ダメージ！");
                }
                else if (this.gameObject.CompareTag("Player"))
                {
                    Attack(targetCharacter);
                    TextManager.Instance.SetMessageText(this.GetName() + "の攻撃！"
                        + "\n" + targetCharacter.GetName() + "に" + this.GetPower() + "ダメージ！");
                }

                if (targetCharacter.GetHP() <= 0)
                {
                    TextManager.Instance.AddMessageText("\n" + targetCharacter.GetName() + "は倒れた");
                }

                break;

            case ActionType.MAGIC:
                if (!m_IsPlayer)
                {
                    Magic(targetCharacter);
                    TextManager.Instance.SetMessageText(this.GetName() + "の魔法！"
                        + "\n" + targetCharacter.GetName() + "に" + this.GetMagic() + "ダメージ！");
                }
                else if (this.gameObject.CompareTag("Player"))
                {
                    Magic(targetCharacter);
                    TextManager.Instance.SetMessageText(this.GetName() + "の魔法！"
                        + "\n" + targetCharacter.GetName() + "に" + this.GetMagic() + "ダメージ！");
                }

                if (targetCharacter.GetHP() <= 0)
                {
                    TextManager.Instance.AddMessageText("\n" + targetCharacter.GetName() + "は倒れた");
                }

                break;

            case ActionType.ITEM:
                if (m_IsPlayer)
                {
                    TextManager.Instance.CreateText(TextType.ITEM_TEXT);
                    TextManager.Instance.SetItemText();

                    m_IsSelectingItem = true;
                }
                else
                {

                }
                    break;

            case ActionType.ESCAPE:
                Debug.Log("escape");
                Escape();
                break;

            default: break;
        }
    }

    private void SelectItemEnd(ItemType type)
    {
        m_IsSelectingItem = false;
        UseItem(type);
        TextManager.Instance.DeleteText(TextType.ITEM_TEXT);
    }

    public void Die()
    {
        Destroy(gameObject);

    }
    
}
