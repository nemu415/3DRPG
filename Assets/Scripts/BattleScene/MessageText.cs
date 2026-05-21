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

    public void SetText(string text)
    {
        m_MessageText.text = text;
    }

    public void AddText(string text)
    {
        m_MessageText.text += text;
    }
}
