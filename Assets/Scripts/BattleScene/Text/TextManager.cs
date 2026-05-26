using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    private Vector2 m_PlayerStatusPos;
    private Vector2 m_EnemyStatusPos;

    [SerializeField] private float lineSpacing = 100f;

    [SerializeField]
    private MessageText m_MessageText;

    [SerializeField]
    private StatusText m_StatusText;

    public StatusText statusTextPrefab;

    [SerializeField]
    private Transform canvasTarget;

    public enum TextType
    {
        MESSAGE_TEXT,
        STATUS_TEXT,
        ITEM_TEXT,
        TEXT_TYPE_MAX,
        TEXT_TYPE_NONE = -1
    };

    List<StatusText> m_StatusTextList = new List<StatusText>();

    private void Start()
    {
        m_PlayerStatusPos = new Vector2(500.0f, -250.0f);
        m_EnemyStatusPos = new Vector2(-100.0f, 250.0f);
    }

    public void CreateText(TextType type)
    {
        switch (type)
        {
            case TextType.MESSAGE_TEXT:
                Instantiate(m_MessageText, canvasTarget);
                break;

            case TextType.STATUS_TEXT:
                StatusText text = Instantiate(m_StatusText,canvasTarget);
                m_StatusTextList.Add(text);
                SetText();
                break;

            default:
                break;
        }
    }

    public void SetText()
    {
        for (int i = 0; i < m_StatusTextList.Count; i++)
        {
            if (m_StatusTextList[i] == null) continue;

            if (i == 0)
            {
                m_StatusTextList[i].SetPos(m_PlayerStatusPos);
            }
            else
            {
                float xOffset = -(i - 1) * lineSpacing;
                Vector2 calcuratedPos = new Vector2(m_EnemyStatusPos.x + xOffset, m_EnemyStatusPos.y);

                m_StatusTextList[i].SetPos(calcuratedPos);
            }
        }
    }

}
