using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StatusText : MonoBehaviour
{
    private int m_Hp;
    private int m_Mp;

    private string m_Name;

    //[SerializeField]
    //private Enemy m_Enemy;

    [SerializeField]
    private TextMeshProUGUI m_StatusText;

    public int GetHP() { return m_Hp; }
    public int GetMP() { return m_Mp; }


    private void Start()
    {
        
    }

    private void Update()
    {
    }

    public void SetStatus(int hp, int mp, string name)
    {
        m_Hp = hp;
        m_Mp = mp;
        m_Name = name;

        m_StatusText.text = m_Name + "\n"
           + "HP:" + m_Hp.ToString("D2") + "\n"
           + "MP:" + m_Mp.ToString("D2");
    }

    public void SetPos(Vector2 targetPosition)
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();

        if (rectTransform == null) return;

        Vector2 originalSize = rectTransform.sizeDelta;

        rectTransform.anchoredPosition = targetPosition;

        rectTransform.sizeDelta = originalSize;
    }
}


