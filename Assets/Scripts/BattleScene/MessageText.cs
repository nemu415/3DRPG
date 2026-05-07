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

        if (Times != "ENDTEXT")
        {
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
                    textNum = 3;
                    m_Player.Attack();
                }

                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    textNum = 5;
                    m_Player.Magic();
                }

                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    textNum = 7;
                    m_Enemy.Attack();
                }

                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    textNum = 9;
                    m_Enemy.Magic();
                }
            }

            // Player攻撃
            else if (textNum == 3)
            {
                m_MessageText.text = Times;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    textNum++;
                }
            }

            else if (textNum == 4)
            {
                m_MessageText.text = string.Format("{0}に{1}ダメージ!", m_Enemy.GetName(), m_Player.GetPower());

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    textNum = 1;
                }
            }

            // Player魔法
            else if (textNum == 5)
            {
                m_MessageText.text = string.Format("Playerの{0}魔法！", m_Player.m_Type);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    textNum++;
                }
            }

            else if (textNum == 6)
            {
                m_MessageText.text = string.Format("{0}に{1}ダメージ!", m_Enemy.GetName(), m_Player.GetMagic());

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    textNum = 1;
                }
            }

            // Enemy攻撃
            else if (textNum == 7)
            {
                m_MessageText.text = Times;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    textNum++;
                }
            }

            else if (textNum == 8)
            {
                m_MessageText.text = string.Format("{0}に{1}ダメージ!", m_Player.GetName(), m_Enemy.GetPower());

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    textNum = 1;
                }
            }

            // Enemy魔法
            else if (textNum == 9)
            {
                m_MessageText.text = string.Format("Playerの{0}魔法！", m_Enemy.m_Type);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    textNum++;
                }
            }

            else if (textNum == 10)
            {
                m_MessageText.text = string.Format("{0}に{1}ダメージ!", m_Player.GetName(), m_Enemy.GetMagic());

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    textNum = 1;
                }
            }
        }
    }
}
