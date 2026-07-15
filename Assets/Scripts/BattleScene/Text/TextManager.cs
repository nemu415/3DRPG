using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public enum TextType
{
    MESSAGE_TEXT,
    STATUS_TEXT,
    ITEM_TEXT,
    CURSOR,
    TEXT_TYPE_MAX,
    TEXT_TYPE_NONE = -1
};

public class TextManager : MonoBehaviour
{
    public static TextManager Instance { get; private set; }

    private Vector2 cursorPos = new Vector2(-330f, -255f);

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
    private GameObject cursor;

    [SerializeField]
    private CursorText m_Cursor;

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
        m_MessageTextPos = new Vector2(300.0f, 200.0f);

        if (m_HPBarRectList.Count > 0 && m_HPBarRectList[0] != null)
        {
            maxBarWidth = m_HPBarRectList[0].rect.width;
        }
    }

    public void CreateStatusText(int characterCount)
    {
        foreach (var ui in m_StatusTextList)
        {
            if (ui != null) Destroy(ui.gameObject);
        }
        m_StatusTextList.Clear();

        for (int i = 0; i < characterCount; i++)
        {
            StatusText uiInstance = Instantiate(m_StatusText, canvasTarget, false);
            m_StatusTextList.Add(uiInstance);

            SetUIFixedPosition(uiInstance, i);
        }
    }

    private void SetUIFixedPosition(StatusText ui, int index)
    {
        RectTransform rect = ui.GetComponent<RectTransform>();
        if (rect == null) return;

        if (index == 0)
        {
            rect.anchorMin = new Vector2(1f, 0);
            rect.anchorMax = new Vector2(1f, 0);
            rect.pivot = new Vector2(1f, 0);
            ui.SetPos(new Vector2(-20f, 40f));
        }
        else
        {
            rect.anchorMin = new Vector2(0, 1f);
            rect.anchorMax = new Vector2(0, 1f);
            rect.pivot = new Vector2(0, 1f);

            int enemyIndex = index - 1;
            float startX = 20f;
            float startY = -20f;
            float xOffset = enemyIndex * 250f;

            ui.SetPos(new Vector2(startX + xOffset, startY));
        }

        rect.transform.localScale = Vector3.one;
        rect.transform.localPosition = new Vector3(rect.transform.localPosition.x, rect.transform.localPosition.y, 0);
    }

    public void RefreshStatus(List<CharacterBase> characterList)
    {
        if (characterList == null) return;

        for (int i = 0; i < m_StatusTextList.Count; i++)
        {
            if (m_StatusTextList[i] == null || !m_StatusTextList[i]) continue;

            StatusText currentUI = m_StatusTextList[i];

            if (i >= characterList.Count || characterList[i] == null)
            {
                UpdateUIVisibility(currentUI.gameObject, true);
                continue;
            }

            CharacterBase character = characterList[i];
            int hp = character.GetHP();
            int mp = character.GetMP();
            string name = character.GetName();

            if (hp <= 0 && !string.IsNullOrEmpty(name))
            {
                UpdateUIVisibility(currentUI.gameObject, false);
            }
            else
            {
                UpdateUIVisibility(currentUI.gameObject, true);
                currentUI.SetStatus(hp, mp, name);
            }
        }
    }

    private void UpdateUIVisibility(GameObject targetObj, bool isVisible)
    {
        if (targetObj == null || !targetObj) return;

        var canvasGroup = targetObj.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            targetObj.SetActive(isVisible);
            return;
        }

        canvasGroup.alpha = isVisible ? 1f : 0f;
        canvasGroup.blocksRaycasts = isVisible;
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
                    break;
                }

                // テキスト枠（親）を生成してリストに追加
                StatusText text = Instantiate(m_StatusText, canvasTarget, false);
                m_StatusTextList.Add(text);

                ShowStatusUI(text.gameObject);
                SetStatusText(); // 配置位置の計算

                break;

            case TextType.ITEM_TEXT:
                itemText.SetActive(true);
                break;
            case TextType.CURSOR:
                cursor.SetActive(true);
                m_Cursor.SetPos(cursorPos);
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
        var characterList = m_CharacterManager.GetCharacterList();
        if (characterList == null) return;

        for (int i = 0; i < m_StatusTextList.Count; i++)
        { 
            if (m_StatusTextList[i] == null) continue;

            if (i >= characterList.Count || characterList[i] == null)
            {
                HideStatusUI(m_StatusTextList[i].gameObject);
                continue;
            }

            CharacterBase character = characterList[i];
            int hp = character.GetHP();
            int mp = character.GetMP();
            string name = character.GetName();

            m_StatusTextList[i].SetStatus(hp, mp, name);

            if (hp <= 0)
            {
                HideStatusUI(m_StatusTextList[i].gameObject);
            }
            else
            {
                m_StatusTextList[i].SetStatus(hp, mp, name);
                ShowStatusUI(m_StatusTextList[i].gameObject);
            }
        }
    } 

    private void HideStatusUI(GameObject targetObj)
    {
        var canvasGroup = targetObj.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            targetObj.SetActive(false);
        }
    }

    private void ShowStatusUI(GameObject targetObj)
    {
        var canvasGroup = targetObj?.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            targetObj.SetActive(true);
        }
    }

    public void SetItemText()
    {
        m_ItemText.SetText();
    }

}
