using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using System.IO;
using UnityEngine.UI;
using Unity.VisualScripting;

public class BattleManager : MonoBehaviour
{
    int textNum = 0;

    [SerializeField]
    private TextMeshProUGUI m_MessageText;

    [SerializeField]
    private TextAsset m_BattleText;

    [SerializeField]
    private Player m_Player;

    [SerializeField]
    private Enemy m_Enemy;

    [SerializeField]
    private StatusText m_PlayerStatusText;

    [SerializeField]
    private StatusText m_EnemyStatusText;

    [SerializeField]
    private ItemManager m_ItemManager;

    [SerializeField]
    private GameObject m_Item;

    List<string[]> TextData = new List<string[]>();

    private int m_PlayerAct;

    private ItemManager.Itemtype m_ItemType;

    string m_Times;

    private void Start()
    {
        StringReader reader = new StringReader(m_BattleText.text);

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            TextData.Add(line.Split(','));
        }

        m_PlayerStatusText.Activate();
        m_EnemyStatusText.Activate();
    }

    private void Update()
    {
        m_Times = TextData[textNum][0].ToString();

        int playerSpeed = m_Player.GetSpeed();
        int enemySpeed = m_Enemy.GetSpeed();

        // バトル開始
        if (textNum == 0)
        {
            m_MessageText.text = string.Format("{0} が あらわれた！", m_Enemy.GetName());

            if (Input.GetKeyDown(KeyCode.Space))
            {
                textNum++;
            }
        }

        // Playerはどうする？
        else if (textNum == 1)
        {
            m_Player.ActedReset();
            m_Enemy.ActedReset();

            m_MessageText.text = m_Times;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                textNum++;
            }
        }

        // 選択肢
        else if (textNum == 2)
        {
            m_MessageText.text = m_Times;

            // 攻撃
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                m_PlayerAct = 1;
                if (playerSpeed > enemySpeed)
                {
                    textNum = 3;
                }
                else
                {
                    textNum = 4;
                }
            }

            // 魔法
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {                    
                m_PlayerAct = 2;
                if (playerSpeed > enemySpeed)
                {
                    textNum = 3;
                }
                else
                {
                    textNum = 4;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Item();
            }

            // 逃げる
            if (Input.GetKeyDown (KeyCode.Alpha4))
            {
                Escape(50);
            }
        }

        // 各キャラクター行動
        else if (textNum == 3)
        {
            if (m_PlayerAct == 1)
            {
                m_MessageText.text = string.Format("Playerの攻撃！");
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_Player.Attack();
                    m_Player.Act();
                    textNum = 5;
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
                        textNum = 6;
                    }
                }
                else
                {
                    m_MessageText.text = string.Format("MPが足りない！");
                    if (Input.GetKeyDown(KeyCode.Space)) textNum = 2;
                }
            }
        }

        else if (textNum == 4)
        {
            m_MessageText.text = string.Format("Enemyの攻撃！");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Enemy.Attack();
                m_Enemy.Act();
                textNum = 7;
            }
        }

        // プレイヤーの攻撃
        else if (textNum == 5)
        {
            m_MessageText.text = string.Format("{0}に{1}ダメージ!", m_Enemy.GetName(), m_Player.GetPower());

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (m_Enemy.GetHP() <= 0)
                {
                    EnemyDie();
                }

                else if (m_Enemy.IsActed())
                {
                    textNum = 1;
                }
                else
                {
                    textNum = 4;
                }
            }
        }

        // プレイヤーの魔法
        else if (textNum == 6)
        {
            m_MessageText.text = string.Format("{0}に{1}ダメージ!", m_Enemy.GetName(), m_Player.GetMagic());

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (m_Enemy.GetHP() <= 0)
                {
                    EnemyDie();
                }

                else if (m_Enemy.IsActed())
                {
                    textNum = 1;
                }
                else
                {
                    textNum = 4;
                }
            }
        }

        // 敵の攻撃
        else if (textNum == 7)
        {
            m_MessageText.text = string.Format("{0}に{1}ダメージ!", m_Player.GetName(), m_Enemy.GetPower());

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (m_Player.GetHP() <= 0)
                {
                    PlayerDie();
                }

                else if (m_Player.IsActed())
                {
                    textNum = 1;
                }
                else
                {
                    textNum = 3;
                }
            }
        }

        else if (textNum == 8)
        {
            m_MessageText.text = string.Format("うまく逃げ切れた！");
        }

        else if (textNum == 9)
        {
            m_MessageText.text = string.Format("逃げられなかった！");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                textNum = 4;
            }
        }

        if (textNum == 11)
        {
            Item();
        }
    }

    private void PlayerDie()
    {
        m_MessageText.text = string.Format("{0} は 力尽きた…", m_Player.GetName());
        textNum = 10;
    }

    private void EnemyDie()
    {
        m_MessageText.text = string.Format("{0} を 倒した！", m_Enemy.GetName());
        textNum = 10;
    }

    private void Item()
    {
        m_Item.gameObject.SetActive(true);

        textNum = 11;

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
                    textNum = 4;
                    m_Item.gameObject.SetActive(false);
                    break;
                case ItemManager.Itemtype.MP_HEAL:
                    m_Player.MPHeal(10);
                    m_ItemManager.SpendItem(ItemManager.Itemtype.MP_HEAL);
                    m_Player.Act();
                    textNum = 4;
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
            textNum = 8;
        }
        else
        {
            textNum = 9;
            m_Player.Act();
        }
    }
}
