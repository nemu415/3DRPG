using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemText : MonoBehaviour
{
    private CanvasGroup m_CanvasGroup;

    [SerializeField]
    [Multiline(3)]
    private TextMeshProUGUI m_ItemText;

    [SerializeField]
    private ItemManager m_ItemManager;

    private void Awake()
    {
        m_ItemText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void SetText()
    {
        m_ItemText.text = string.Format(
            "1.回復薬　　　　{0}\n" +
            "2.魔力チャージ　{1}\n" +
            "3.煙玉　　　　　{2}",
            m_ItemManager.GetItemNum(ItemManager.ItemType.HP_HEAL),
            m_ItemManager.GetItemNum(ItemManager.ItemType.MP_HEAL), 
            m_ItemManager.GetItemNum(ItemManager.ItemType.ESCAPE)
        );
    }

    public void SetPos(Vector2 targetPosition)
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();

        if (rectTransform == null) return;

        rectTransform.anchoredPosition = targetPosition;
    }
}
