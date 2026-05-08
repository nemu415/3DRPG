using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using System.IO;
using UnityEngine.UI;

public class MessageText : MonoBehaviour
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

    List<string[]> TextData = new List<string[]>();

    private int m_PlayerAct;

    private void Start()
    {
        StringReader reader = new StringReader(m_BattleText.text);

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            TextData.Add(line.Split(','));
        }
    }

    private void Update()
    {
        string Times = TextData[textNum][0].ToString();

        int playerSpeed = m_Player.GetSpeed();
        int enemySpeed = m_Enemy.GetSpeed();

        // バトル開始
        if (textNum == 0)
        {
            m_MessageText.text = Times;

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

            m_MessageText.text = Times;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                textNum++;
            }
        }

        // 選択肢
        else if (textNum == 2)
        {
            m_MessageText.text = Times;

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

        else if (textNum == 5)
        {
            m_MessageText.text = string.Format("{0}に{1}ダメージ!", m_Enemy.GetName(), m_Player.GetPower());

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (m_Enemy.IsActed())
                {
                    textNum = 1;
                }
                else
                {
                    textNum = 4;
                }
            }
        }

        else if (textNum == 6)
        {
            m_MessageText.text = string.Format("{0}に{1}ダメージ!", m_Enemy.GetName(), m_Player.GetMagic());

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (m_Enemy.IsActed())
                {
                    textNum = 1;
                }
                else
                {
                    textNum = 4;
                }
            }
        }

        else if (textNum == 7)
        {
            m_MessageText.text = string.Format("{0}に{1}ダメージ!", m_Player.GetName(), m_Enemy.GetPower());

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (m_Player.IsActed())
                {
                    textNum = 1;
                }
                else
                {
                    textNum = 3;
                }
            }
        }

        if (m_Player.GetHP() <= 0)
        {
            textNum = 8;
        }

        if (m_Enemy.GetHP() <= 0)
        {
            textNum = 9;
        }
    }
}
