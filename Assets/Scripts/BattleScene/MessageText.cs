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
    int count = 0;

    [SerializeField]
    private TextMeshProUGUI m_MessageText;

    [SerializeField]
    private TextAsset m_BattleText;

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
        string Times = TextData[textNum][count].ToString();

        if (Times != "ENDTEXT")
        {
            if (Times != "END")
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    count++;
                }

                m_MessageText.text = Times;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    count = 0;
                    textNum++;
                }
            }
        }
    }
}
