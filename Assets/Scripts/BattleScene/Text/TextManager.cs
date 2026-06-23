using System.Collections.Generic;
using UnityEngine;
using System.Collections;

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
    private Gauge m_Gauge;

    [SerializeField]
    private ItemText m_ItemText;

    [SerializeField]
    private GameObject itemText;

    [SerializeField]
    private Transform canvasTarget;

    [SerializeField]
    private CharacterManager m_CharacterManager;

    List<StatusText> m_StatusTextList = new List<StatusText>();

    List<Gauge> m_GaugeList = new List<Gauge>();
    List<Gauge> m_HPGaugeList = new List<Gauge>();
    List<Gauge> m_MPGaugeList = new List<Gauge>();

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
                StatusText text = Instantiate(m_StatusText, canvasTarget, false);
                m_StatusTextList.Add(text);
                SetStatusText();
                SetStatus();
                break;

            case TextType.ITEM_TEXT:
                itemText.SetActive(true);
                break;

            case TextType.HP_GAUGE:
                Gauge gauge = Instantiate(m_Gauge, canvasTarget, false);
                m_GaugeList.Add(gauge);
                m_HPGaugeList.Add(gauge);
                SetGauge();
                break;

            case TextType.MP_GAUGE:
                gauge = Instantiate(m_Gauge, canvasTarget, false);
                m_GaugeList.Add(gauge);
                m_MPGaugeList.Add(gauge);
                SetGauge();
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
        for (int i = 0; i < m_StatusTextList.Count; i++)
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
                m_StatusTextList.RemoveAt(i);
            }
        }
    }

    public void SetGauge()
    {
        for (int i = 0; i < m_GaugeList.Count; i++)
        {
            if (m_HPGaugeList[i] == null || m_MPGaugeList[i] == null) continue;

            CharacterBase character = m_CharacterManager.GetCharacterList()[i];

            if (character == null) continue;

            m_HPGaugeList[i].SetGauge(character);
            m_MPGaugeList[i].SetGauge(character);
        }
    }

    public void SetItemText()
    {
        m_ItemText.SetText();
    }

}
