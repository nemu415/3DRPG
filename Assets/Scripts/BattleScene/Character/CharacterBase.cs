using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ItemManager;
using static MagicSpawner;

public class CharacterBase : MonoBehaviour
{
    private MagicSpawner magicSpawner;

    private const float MOVE_SPEED = 5f;
    private const float ATTACK_DISTANCE_MIN = 2.5f;
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
    protected Vector3 m_DefaultPos;

    private bool m_IsSelectingItem = false;

    private Animator animator;

    protected virtual string MoveForwardAnimationName => "DefaultMove";
    protected virtual string MoveBackAnimationName => "DefaultMove";
    protected virtual string IdleAnimationName => "DefaultIdle";
    protected virtual string AttackAnimationName => "DefaultAttack";
    protected virtual string MagicAnimationName => "DefaultMagic";
    protected virtual string DamageAnimationName => "DefaultDamage";

    public bool IsAttacking { get; private set; } = false;
    public bool IsMagic { get; private set; } = false;

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

        m_DefaultPos = transform.position;
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

    public IEnumerator Damage(int damage)
    {
        m_Hp -= damage;

        if (m_Hp < 0)
        {
            m_Hp = 0;
        }
            
        animator.Play(DamageAnimationName, -1, 0f);

        yield return null;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitForSeconds(stateInfo.length);
    }

    public IEnumerator Attack(CharacterBase opponent)
    {
        IsAttacking = true;

        float currectY = this.transform.position.y;
        m_DefaultPos = this.transform.position;
        m_DefaultPos.y = currectY;
        Vector3 opponentPos = opponent.transform.position;
        opponentPos.y = currectY;

        animator.Play(MoveForwardAnimationName, -1, 0f);

        while (true)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, opponentPos, MOVE_SPEED * Time.deltaTime);

            if (Vector3.Distance(this.transform.position, opponentPos) <= ATTACK_DISTANCE_MIN)
            {
                break;
            }

            yield return null;
        }

        animator.Play(AttackAnimationName, -1, 0f);
        opponent.StartCoroutine(Damage(m_Power));
        TextManager.Instance.SetStatus();
        yield return null;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitForSeconds(stateInfo.length);

        animator.Play(MoveBackAnimationName, -1, 0f);

        while (true)
        {
            Vector3 thisPos = this.transform.position;
            float defaultDistance = Vector3.Distance(thisPos, m_DefaultPos);

            this.transform.position = Vector3.MoveTowards(this.transform.position, m_DefaultPos, MOVE_SPEED * Time.deltaTime);

            if (Vector3.Distance(this.transform.position, m_DefaultPos) <= 0.1f)
            {
                this.transform.position = m_DefaultPos;

                animator.Play(IdleAnimationName, -1, 0f);

                break;
            }

            yield return null;
        }
        
        IsAttacking = false;
    }

    public IEnumerator Magic(CharacterBase opponent)
    {
        IsMagic = true;

        m_Mp -= 5;
        animator.Play(MagicAnimationName, -1, 0f);

        if (magicSpawner == null)
        {
            magicSpawner = GameObject.FindAnyObjectByType<MagicSpawner>();
        }
        
        TextManager.Instance.SetStatus();
        yield return null;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitForSeconds(stateInfo.length);

        magicSpawner.PlayMagicEffect(opponent.transform.position);
        opponent.StartCoroutine(Damage(m_Magic));

        IsMagic = false;
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
                    StartCoroutine(Attack(targetCharacter));
                    TextManager.Instance.SetMessageText(this.GetName() + "の攻撃！"
                        + "\n" + targetCharacter.GetName() + "に" + this.GetPower() + "ダメージ！");
                }
                else if (this.gameObject.CompareTag("Player"))
                {
                    StartCoroutine(Attack(targetCharacter));
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
                    StartCoroutine(Magic(targetCharacter));
                    TextManager.Instance.SetMessageText(this.GetName() + "の魔法！"
                        + "\n" + targetCharacter.GetName() + "に" + this.GetMagic() + "ダメージ！");
                }
                else if (this.gameObject.CompareTag("Player"))
                {
                    StartCoroutine(Magic(targetCharacter));
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
