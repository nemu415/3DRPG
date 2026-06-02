using TMPro;
using UnityEngine;

public class MessageText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_MessageText;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void SetPos(Vector2 targetPosition)
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();

        if (rectTransform == null) return;

        rectTransform.anchoredPosition = targetPosition;
    }

    public void SetText(string text)
    {
        m_MessageText.text = text;
    }

    public void AddText(string text)
    {
        m_MessageText.text += text;
    }
}
