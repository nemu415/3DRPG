using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;

public enum TextType
{
    MESSAGE_TEXT,
    STATUS_TEXT,
    ITEM_TEXT,
    HP_GAUGE,
    MP_GAUGE,
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

    [SerializeField] private List<RectTransform> m_HPBarRectList = new List<RectTransform>();

    private float maxBarWidth = 100f;

    private MessageText m_ActiveMessage;

    [SerializeField]
    private CharacterManager m_CharacterManager;

    List<StatusText> m_StatusTextList = new List<StatusText>();

    [SerializeField] private GameObject m_HPGaugePrefab;
    [SerializeField] private GameObject m_MPGaugePrefab;

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

        if (m_HPBarRectList.Count > 0 && m_HPBarRectList[0] != null)
        {
            maxBarWidth = m_HPBarRectList[0].rect.width;
        }
    }

    public void CreateText(TextType type)
    {
        switch (type)
        {
            case TextType.MESSAGE_TEXT:
                // すでに画面に古いメッセージがあるなら削除（増殖防止）
                if (m_ActiveMessage != null) Destroy(m_ActiveMessage.gameObject);

                // 元のプレハブ(m_MessageText)から生成し、画面に表示中の実体(m_ActiveMessage)として保存する
                m_ActiveMessage = Instantiate(m_MessageText, canvasTarget, false);
                break;

            case TextType.STATUS_TEXT:
                var charList = m_CharacterManager.GetCharacterList();
                if (charList == null) return;

                // 増殖防止の安全装置：すでにキャラクター数以上のテキストがあるなら生成しない
                if (m_StatusTextList != null && m_StatusTextList.Count >= charList.Count)
                {
                    SetStatus();
                    break;
                }

                // テキスト枠（親）を生成してリストに追加
                StatusText text = Instantiate(m_StatusText, canvasTarget, false);
                m_StatusTextList.Add(text);
                SetStatusText(); // 配置位置の計算

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
        if (m_ActiveMessage != null)
        {
            m_ActiveMessage.SetText(message);
            m_ActiveMessage.SetPos(m_MessageTextPos);
        }

    }

    public void AddMessageText(string message)
    {
        if (m_ActiveMessage != null)
        {
            m_ActiveMessage.AddText(message);
            m_ActiveMessage.SetPos(m_MessageTextPos);
        }
    }

    public void SetStatusText()
    {
        if (canvasTarget == null) return;

        for (int i = 0; i < m_StatusTextList.Count; i++)
        {
            if (m_StatusTextList[i] == null) continue;

            RectTransform uiRect = m_StatusTextList[i].GetComponent<RectTransform>();
            if (uiRect == null) continue;

            if (i == 0)
            {
                uiRect.anchorMin = new Vector2(1f, 0f);
                uiRect.anchorMax = new Vector2(1f, 0f);
                uiRect.pivot = new Vector2(1f, 0f);
                m_StatusTextList[i].SetPos(new Vector2(-20f, 40f));
            }
            else
            {
                uiRect.anchorMin = new Vector2(0f, 1f);
                uiRect.anchorMax = new Vector2(0f, 1f);
                uiRect.pivot = new Vector2(0f, 1f);

                int enemyIndex = i - 1;
                float startX = 20f;
                float startY = -20f;

                float xOffset = enemyIndex * 250f;
                m_StatusTextList[i].SetPos(new Vector2(startX + xOffset, startY));
            }

            uiRect.transform.localScale = Vector3.one;
            uiRect.transform.localPosition = new Vector3(uiRect.transform.localPosition.x, uiRect.transform.localPosition.y, 0f);
        }
    }

    public void SetStatus()
    {
        for (int i = m_StatusTextList.Count - 1; i >= 0; i--)
        {
            if (m_StatusTextList[i] == null) continue;

            CharacterBase character = m_CharacterManager.GetCharacterList()[i];
            if (character == null) continue;

            int hp = character.GetHP();
            int mp = character.GetMP();
            string name = character.GetName();

            m_StatusTextList[i].SetStatus(hp, mp, name);

            if (hp <= 0)
            {
                Destroy(m_StatusTextList[i].gameObject);
                m_StatusTextList.RemoveAt(i); // 後ろから消せば番号がズレず、メモリ破壊が起きません
            }
        }
    }

    public void SetItemText()
    {
        m_ItemText.SetText();
    }

}
