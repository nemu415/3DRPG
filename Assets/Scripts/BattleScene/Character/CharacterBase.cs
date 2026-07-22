using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static EffectSpawner;
using static ItemManager;
using static UnityEngine.GraphicsBuffer;

public class CharacterBase : MonoBehaviour
{
    private EffectSpawner effectSpawner;

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
    protected bool m_Alive;
    protected bool m_IsPoisoned;
    protected Vector3 m_DefaultPos;

    private bool m_IsSelectingItem = false;

    private Animator animator;

    protected virtual string MoveForwardAnimationName => "DefaultMove";
    protected virtual string MoveBackAnimationName => "DefaultMove";
    protected virtual string IdleAnimationName => "DefaultIdle";
    protected virtual string AttackAnimationName => "DefaultAttack";
    protected virtual string MagicAnimationName => "DefaultMagic";
    protected virtual string DamageAnimationName => "DefaultDamage";
    protected virtual string DieAnimationName => "DefaultDie";

    public bool IsAttacking { get; private set; } = false;
    public bool IsMagic { get; private set; } = false;

    public bool IsUsingItem { get; private set; } = false;

    public enum MagicType
    {
        FIRE,
        WATER,
        THUNDER,
        HEAL,
        MAGIC_TYPE_MAX,
    }

    protected MagicType m_MagicType;


    private void Start()
    {
        animator = GetComponent<Animator>();

        m_DefaultPos = transform.position;

        m_Alive = true;
        m_IsPoisoned = false;
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

    public bool IsPoisoned() { return m_IsPoisoned; }

    public int GetAttackPercent()
    {
        return m_AttackPercent;
    }

    public MagicType GetMagicType()
    {
        return m_MagicType;
    }

    public void Act() { m_Acted = true; }

    public void ActedReset() { m_Acted = false; }

    private IEnumerator WaitForAnimation(string animationName)
    {
        yield return null;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(animationName))
        {
            yield return new WaitForSeconds(stateInfo.length);
        }
    }

    public IEnumerator Damage(int damage)
    {
        m_Hp -= damage;

        TextManager.Instance.AddMessageText( "\n" + this.GetName() + "に" + damage + "ダメージ！");

        if (m_Hp <= 0)
        {
            m_Hp = 0;

            m_Alive = false;

            animator.Play(DieAnimationName, -1, 0f);

            yield return StartCoroutine(WaitForAnimation(DieAnimationName));

            TextManager.Instance.AddMessageText("\n" + this.GetName() + "は倒れた！");

            Destroy(gameObject);
        }
        else
        {
            animator.Play(DamageAnimationName, -1, 0f);

            yield return StartCoroutine(WaitForAnimation(DamageAnimationName));
        }
    }

    public IEnumerator Attack(CharacterBase opponent)
    {
        IsAttacking = true;

        float currentY = this.transform.position.y;
        m_DefaultPos = this.transform.position;
        m_DefaultPos.y = currentY;
        Vector3 opponentPos = opponent.transform.position;
        opponentPos.y = currentY;

        animator.Play(MoveForwardAnimationName, -1, 0f);

        while (Vector3.Distance(this.transform.position, opponentPos) > ATTACK_DISTANCE_MIN)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, opponentPos, MOVE_SPEED * Time.deltaTime);

            yield return null;
        }

        animator.Play(AttackAnimationName, -1, 0f);

        yield return opponent.StartCoroutine(opponent.Damage(m_Power));

        TextManager.Instance.SetStatus();
        yield return StartCoroutine(WaitForAnimation(AttackAnimationName));

        animator.Play(MoveBackAnimationName, -1, 0f);

        while (Vector3.Distance(this.transform.position, m_DefaultPos) > 0.1f)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, m_DefaultPos, MOVE_SPEED * Time.deltaTime);
           
            yield return null;
        }

        this.transform.position = m_DefaultPos;

        animator.Play(IdleAnimationName, -1, 0f);

        IsAttacking = false;
    }

    public IEnumerator Magic(CharacterBase opponent)
    {
        IsMagic = true;

        m_Mp -= 5;
        animator.Play(MagicAnimationName, -1, 0f);

        if (m_MagicType != MagicType.HEAL)
        {
            if (effectSpawner == null)
            {
                effectSpawner = GameObject.FindAnyObjectByType<EffectSpawner>();
            }

            TextManager.Instance.SetStatus();
            yield return StartCoroutine(WaitForAnimation(MagicAnimationName));

            switch (m_MagicType)
            {
                case MagicType.FIRE:
                    effectSpawner.PlayMagicEffect(EffectType.RED_MAGIC, opponent.transform.position);
                    break;
                case MagicType.WATER:
                    effectSpawner.PlayMagicEffect(EffectType.BLUE_MAGIC, opponent.transform.position);
                    break;
                case MagicType.THUNDER:
                    effectSpawner.PlayMagicEffect(EffectType.YELLOW_MAGIC, opponent.transform.position);
                    break;
            }

            int damage = CalcDamage(opponent, true, m_Magic);
            yield return opponent.StartCoroutine(opponent.Damage(damage));
        }
        else
        {
            if (m_IsPlayer)
            {
                this.HPHeal(m_Magic);
            }
            else
            {
                int maxHPDiff = -1;
                CharacterBase target = null;
                var characterList = CharacterManager.Instance.GetCharacterList();

                for (int i = 0; i < characterList.Count; i++)
                {
                    if (characterList[i].IsDestroyed()) continue;
                    if (characterList[i].IsPlayer()) continue;
                    int hpDiff = characterList[i].GetMaxHP() - characterList[i].GetHP();
                    if (hpDiff > maxHPDiff)
                    {
                        target = characterList[i];
                        maxHPDiff = hpDiff;
                    }
                }

                yield return StartCoroutine(WaitForAnimation(MagicAnimationName));

                

                target.HPHeal(m_Magic);
            }
        }

        animator.Play(IdleAnimationName, -1, 0f);

        IsMagic = false;
    }

    private int CalcDamage(CharacterBase opponent, bool isMagic, int power)
    {
        int result = 1;

        float magni = 0f; 

        if (isMagic)
        {
            switch (m_MagicType)
            {
                case MagicType.FIRE:
                    switch (opponent.GetMagicType())
                    {
                        case MagicType.WATER:
                            magni = 0.5f;
                            break;
                        case MagicType.THUNDER:
                            magni = 2f;
                            break;
                        default:
                            break;
                    }
                    break;
                case MagicType.WATER:
                    switch (opponent.GetMagicType())
                    {
                        case MagicType.FIRE:
                            magni = 2f;
                            break;
                        case MagicType.THUNDER:
                            magni = 0.5f;
                            break;
                        default:
                            break;
                    }
                    break;
                case MagicType.THUNDER:
                    switch (opponent.GetMagicType())
                    {
                        case MagicType.FIRE:
                            magni = 0.5f;
                            break;
                        case MagicType.WATER:
                            magni = 2f;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        if (m_MagicType == opponent.GetMagicType())
        {
            magni = 1f;
        }

        result = (int)(power * magni);

        return result;
    }

    public void HPHeal(int heal)
    {
        if (effectSpawner == null)
        {
            effectSpawner = GameObject.FindAnyObjectByType<EffectSpawner>();
        }

        effectSpawner.PlayMagicEffect(EffectType.HP_HEAL, this.transform.position);

        int hpDiff = m_MaxHp - m_Hp;

        if (hpDiff < heal)
        {
            heal = hpDiff;
        }

        m_Hp += heal;

        TextManager.Instance.AddMessageText("\n" + this.m_Name + "のHPが" + heal + "回復した！");
    }

    public void MPHeal(int heal)
    {
        if (effectSpawner == null)
        {
            effectSpawner = GameObject.FindAnyObjectByType<EffectSpawner>();
        }

        effectSpawner.PlayMagicEffect(EffectType.MP_HEAL, this.transform.position);

        m_Mp += heal;

        if (m_Mp > m_MaxMp)
        {
            m_Mp = m_MaxMp;
        }
    }

    public void Escape()
    {
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

            effectSpawner.PlayMagicEffect(EffectType.ESCAPE, this.transform.position);

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
                TextManager.Instance.SetMessageText("回復薬で" + m_Name + "のHPが10回復");
                break;
            case ItemType.MP_HEAL:
                TextManager.Instance.SetMessageText("魔力チャージで" + m_Name + "のMPが10回復");
                MPHeal(10);
                break;
            case ItemType.ESCAPE:
                TextManager.Instance.SetMessageText(m_Name + "は、煙玉を使った");
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
                    TextManager.Instance.SetMessageText(this.GetName() + "の攻撃！");
                    StartCoroutine(Attack(targetCharacter));
                    
                }
                else if (this.gameObject.CompareTag("Player"))
                {
                    TextManager.Instance.SetMessageText(this.GetName() + "の攻撃！");
                    StartCoroutine(Attack(targetCharacter));
                }

                if (targetCharacter.GetHP() <= 0)
                {
                    TextManager.Instance.AddMessageText("\n" + targetCharacter.GetName() + "は倒れた");
                }

                break;

            case ActionType.MAGIC:
                if (!m_IsPlayer)
                {
                    TextManager.Instance.SetMessageText(this.GetName() + "の魔法！");
                    StartCoroutine(Magic(targetCharacter));
                }
                else if (this.gameObject.CompareTag("Player"))
                {
                    TextManager.Instance.SetMessageText(this.GetName() + "の魔法！");
                    StartCoroutine(Magic(targetCharacter));
                }

                if (targetCharacter.GetHP() <= 0)
                {
                    TextManager.Instance.AddMessageText("\n" + targetCharacter.GetName() + "は倒れた");
                }

                break;

            case ActionType.ITEM:
                if (m_IsPlayer)
                {
                    
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

    IEnumerator WaitForKeyInput()
    {
        yield return null;

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
    }
}
