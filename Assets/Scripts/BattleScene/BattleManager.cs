using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using System.IO;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_MessageText;

    [SerializeField]
    private TextAsset m_BattleText;

    [SerializeField]
    private Player m_Player;

    [SerializeField]
    private Enemy m_Enemy;

    [SerializeField]
    private MainCamera m_MainCamera;

    [SerializeField]
    private GameObject m_PlayerStatusText;

    [SerializeField]
    private GameObject m_EnemyStatusText;

    [SerializeField]
    private ItemManager m_ItemManager;

    [SerializeField]
    private GameObject m_Item;

    List<string[]> TextData = new List<string[]>();

    private int m_PlayerAct;

    private ItemManager.Itemtype m_ItemType;

    private enum BattleText
    {
        BATTLE_START,
        CHOOSE_ACTION,
        PLAYER_ATTACK,
        ENEMY_ATTACK,
        PLAYER_ATTACK_DAMAGE,
        PLAYER_MAGIC_DAMAGE,
        ENEMY_ATTACK_DAMAGE,
        ITEM,
        ESCAPE_SUCCESS,
        ESCAPE_FAILED,
        ITEM_HP_HEAL,
        ITEM_MP_HEAL,
        ITEM_ESCAPE,
        BATTLE_END,
        BATTLE_TEXT_MAX
    }

    private BattleText m_TextNum;

    private void Start()
    {
        StringReader reader = new StringReader(m_BattleText.text);

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            TextData.Add(line.Split(','));
        }

        Vector3 playerPos = new Vector3(-3.0f, 1.8f, 0.0f);

        Instantiate(m_Player, playerPos, this.transform.rotation);

        Vector3 enemyPos = new Vector3(3.0f, 1.8f, 0.0f);
        

        int enemyNum = Random.Range(1, 3);

        for (int i = 0; i < enemyNum; i++)
        {
            enemyPos.z = -(float)enemyNum + (float)i * 2;
            Instantiate(m_Enemy, enemyPos, this.transform.rotation);
        }

        if (m_Player != null)
        {
            m_Player.Init();
        }

        if (m_Enemy != null)
        {
            m_Enemy.Init();
        }
    }

    private void Update()
    {
        int playerSpeed = m_Player.GetSpeed();
        int enemySpeed = m_Enemy.GetSpeed();

        switch (m_TextNum)
        {
            case BattleText.BATTLE_START:
                m_MessageText.text = string.Format("{0} が あらわれた！", m_Enemy.GetName());

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_TextNum++;
                    m_MainCamera.BattleStart();
                    Vector3 playerTextPos = new Vector3(0.0f, 0.0f, 0.0f);
                    Instantiate(m_PlayerStatusText, playerTextPos, this.transform.rotation);
                    Instantiate(m_EnemyStatusText);
                }
                break;

            case BattleText.CHOOSE_ACTION:
                m_Player.ActedReset();
                m_Enemy.ActedReset();

                m_MessageText.text = string.Format(
                    "{0} は どうする？\n" +
                    "1:攻撃 2:魔法 3:アイテム 4:逃げる",
                    m_Player.GetName());

                // 攻撃
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    m_PlayerAct = 1;
                    if (playerSpeed > enemySpeed)
                    {
                        m_TextNum = BattleText.PLAYER_ATTACK;
                    }
                    else
                    {
                        m_TextNum = BattleText.ENEMY_ATTACK;
                    }
                }

                // 魔法
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    m_PlayerAct = 2;
                    if (playerSpeed > enemySpeed)
                    {
                        m_TextNum = BattleText.PLAYER_ATTACK;
                    }
                    else
                    {
                        m_TextNum = BattleText.ENEMY_ATTACK;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    m_TextNum = BattleText.ITEM;
                }

                // 逃げる
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    Escape(50);
                }
                break;

            case BattleText.PLAYER_ATTACK:
                if (m_PlayerAct == 1)
                {
                    m_MessageText.text = string.Format("Playerの攻撃！");
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        m_Player.Attack();
                        m_Player.Act();
                        m_TextNum = BattleText.PLAYER_ATTACK_DAMAGE;
                    }
                }
                else if (m_PlayerAct == 2)
                {
                    if (m_Player.GetMP() > 5)
                    {
                        m_MessageText.text = string.Format("Playerの{0}魔法！", m_Player.m_Type);
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            m_Player.Magic();
                            m_Player.Act();
                            m_TextNum = BattleText.PLAYER_MAGIC_DAMAGE;
                        }
                    }
                    else
                    {
                        m_MessageText.text = string.Format("MPが足りない！");
                        if (Input.GetKeyDown(KeyCode.Space)) m_TextNum = BattleText.CHOOSE_ACTION;
                    }
                }
                break;

            case BattleText.ENEMY_ATTACK:
                m_MessageText.text = string.Format("Enemyの攻撃！");
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_Enemy.Attack();
                    m_Enemy.Act();
                    m_TextNum = BattleText.ENEMY_ATTACK_DAMAGE;
                }
                break;

            case BattleText.PLAYER_ATTACK_DAMAGE:
                m_MessageText.text = string.Format("{0}に{1}ダメージ!", m_Enemy.GetName(), m_Player.GetPower());

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (m_Enemy.GetHP() <= 0)
                    {
                        EnemyDie();
                    }

                    else if (m_Enemy.IsActed())
                    {
                        m_TextNum = BattleText.CHOOSE_ACTION;
                    }
                    else
                    {
                        m_TextNum = BattleText.ENEMY_ATTACK;
                    }
                }
                break;

            case BattleText.PLAYER_MAGIC_DAMAGE:
                m_MessageText.text = string.Format("{0}に{1}ダメージ!", m_Enemy.GetName(), m_Player.GetMagic());

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (m_Enemy.GetHP() <= 0)
                    {
                        EnemyDie();
                    }

                    else if (m_Enemy.IsActed())
                    {
                        m_TextNum = BattleText.CHOOSE_ACTION;
                    }
                    else
                    {
                        m_TextNum = BattleText.ENEMY_ATTACK;
                    }
                }
                break;

            case BattleText.ENEMY_ATTACK_DAMAGE:
                m_MessageText.text = string.Format("{0}に{1}ダメージ!", m_Player.GetName(), m_Enemy.GetPower());

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (m_Player.GetHP() <= 0)
                    {
                        PlayerDie();
                    }

                    else if (m_Enemy.IsActed())
                    {
                        m_TextNum = BattleText.CHOOSE_ACTION;
                    }
                    else
                    {
                        m_TextNum = BattleText.PLAYER_ATTACK;
                    }
                }
                break;

            case BattleText.ITEM:
                Item();
                break;

            case BattleText.ESCAPE_SUCCESS:
                m_MessageText.text = string.Format("うまく逃げ切れた");
                m_TextNum = BattleText.BATTLE_END;
                break;
            case BattleText.ESCAPE_FAILED:
                m_MessageText.text = string.Format("逃げられなかった");
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_TextNum = BattleText.ENEMY_ATTACK;
                }
                break;

            case BattleText.ITEM_HP_HEAL:
                break;
            case BattleText.ITEM_MP_HEAL:
                break;
            case BattleText.ITEM_ESCAPE:
                break;
            case BattleText.BATTLE_END:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SceneManager.LoadScene("SoshiKurosawa");
                }
                break;

            case BattleText.BATTLE_TEXT_MAX:
                break;
            default:
                break;
        }

        if (m_TextNum == BattleText.ITEM)
        {
            Item();
        }
    }

    private void PlayerDie()
    {
        m_MessageText.text = string.Format("{0} は 力尽きた…", m_Player.GetName());
        m_TextNum = BattleText.BATTLE_END;
    }

    private void EnemyDie()
    {
        m_MessageText.text = string.Format("{0} を 倒した！", m_Enemy.GetName());
        m_TextNum = BattleText.BATTLE_END;
    }

    private void Item()
    {
        m_Item.gameObject.SetActive(true);

        if (Input.GetKeyDown(KeyCode.A))
        {
            m_ItemType = ItemManager.Itemtype.HP_HEAL;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            m_ItemType = ItemManager.Itemtype.MP_HEAL;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            m_ItemType = ItemManager.Itemtype.ESCAPE;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch(m_ItemType)
            {
                case ItemManager.Itemtype.HP_HEAL:
                    m_Player.HPHeal(10);
                    m_ItemManager.SpendItem(ItemManager.Itemtype.HP_HEAL);
                    m_Player.Act();
                    m_TextNum = BattleText.ENEMY_ATTACK;
                    m_Item.gameObject.SetActive(false);
                    break;
                case ItemManager.Itemtype.MP_HEAL:
                    m_Player.MPHeal(10);
                    m_ItemManager.SpendItem(ItemManager.Itemtype.MP_HEAL);
                    m_Player.Act();
                    m_TextNum = BattleText.ENEMY_ATTACK;
                    m_Item.gameObject.SetActive(false);
                    break;
                case ItemManager.Itemtype.ESCAPE:
                    Escape(0);
                    m_ItemManager.SpendItem(ItemManager.Itemtype.ESCAPE);
                    m_Item.gameObject.SetActive(false);
                    break;
            }
        }

        if (m_ItemType == 0)
        {
            m_MessageText.text = string.Format("HPを10回復");
        }
        else if (m_ItemType == ItemManager.Itemtype.MP_HEAL)
        {
            m_MessageText.text = string.Format("MPを10回復");
        }
        else if (m_ItemType == ItemManager.Itemtype.ESCAPE)
        {
            m_MessageText.text = string.Format("確実に逃げられる");
        }
    }

    private void Escape(int border)
    {
        int rand = Random.Range(1, 100);

        border -= (m_Player.GetSpeed() - m_Enemy.GetSpeed());

        if (rand > border)
        {
            m_TextNum = BattleText.ESCAPE_SUCCESS;
        }
        else
        {
            m_TextNum = BattleText.ESCAPE_FAILED;
            m_Player.Act();
        }
    }
}
