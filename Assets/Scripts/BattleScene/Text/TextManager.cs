using System.Collections.Generic;
using UnityEngine;

public enum TextType
{
    MESSAGE_TEXT,
    STATUS_TEXT,
    ITEM_TEXT,
    TEXT_TYPE_MAX,
    TEXT_TYPE_NONE = -1
};

public class TextManager : MonoBehaviour
{
    public static TextManager Instance { get; private set; }

    private Vector2 m_PlayerStatusPos;
    private Vector2 m_EnemyStatusPos;
    private Vector2 m_MessageTextPos;

    private float lineSpacing = 170f;

    [SerializeField]
    private MessageText m_MessageText;

    [SerializeField]
    private StatusText m_StatusText;

    [SerializeField]
    private ItemText m_ItemText;

    [SerializeField]
    private GameObject itemText;

    [SerializeField]
    private Transform canvasTarget;

    [SerializeField]
    private CharacterManager m_CharacterManager;

    

    List<StatusText> m_StatusTextList = new List<StatusText>();


    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        m_PlayerStatusPos = new Vector2(360.0f, -170.0f);
        m_EnemyStatusPos = new Vector2(-250.0f, 200.0f);
        m_MessageTextPos = new Vector2(450.0f, 300.0f);
    }

    public void CreateText(TextType type)
    {
        switch (type)
        {
            case TextType.MESSAGE_TEXT:
                break;

            case TextType.STATUS_TEXT:
                StatusText text = Instantiate(m_StatusText, canvasTarget);
                m_StatusTextList.Add(text);
                SetStatusText();
                SetStatus();
                break;

            case TextType.ITEM_TEXT:
                itemText.SetActive(true);
                break;

            default:
                break;
        }
    }

    public void DeleteText(TextType type)
    {
        switch (type)
        {
            case TextType.MESSAGE_TEXT:
                break;

            case TextType.STATUS_TEXT:
                break;

            case TextType.ITEM_TEXT:
                itemText.SetActive(false);
                break;

            default:
                break;
        }
    }

    public void SetMessageText(string message)
    {
        m_MessageText.SetText(message);

        m_MessageText.SetPos(m_MessageTextPos);

    }

    public void AddMessageText(string message)
    {
        m_MessageText.AddText(message);

        m_MessageText.SetPos(m_MessageTextPos);

    }

    public void SetStatusText()
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
                float xOffset = (i - 1) * lineSpacing;
                Vector2 calcuratedPos = new Vector2(m_EnemyStatusPos.x + xOffset, m_EnemyStatusPos.y);

                m_StatusTextList[i].SetPos(calcuratedPos);
            }
        }
    }

    public void SetStatus()
    {
        for (int i = 0; i < m_StatusTextList.Count; i++)
        {
            if (m_StatusTextList[i] == null) continue;

            CharacterBase character = m_CharacterManager.GetCharacterList()[i];

            if (character == null) continue;

            int hp = character.GetHP();
            int mp = character.GetMP();
            string name = character.GetName();

            m_StatusTextList[i].SetStatus(hp, mp, name);
        }
    }

    public void SetItemText()
    {
        m_ItemText.SetText();
    }

}
