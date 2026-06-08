using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemText : MonoBehaviour
{
    private CanvasGroup m_CanvasGroup;

    [SerializeField]
    [Multiline(3)]
    private TextMeshProUGUI m_ItemText;

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
            ItemManager.Instance.GetItemNum(ItemType.HP_HEAL),
            ItemManager.Instance.GetItemNum(ItemType.MP_HEAL),
            ItemManager.Instance.GetItemNum(ItemType.ESCAPE)
        );
    }

    public void SetPos(Vector2 targetPosition)
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();

        if (rectTransform == null) return;

        rectTransform.anchoredPosition = targetPosition;
    }
}
