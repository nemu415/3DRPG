using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatusText : MonoBehaviour
{
    private int m_Hp;
    private int m_Mp;

    [SerializeField]
    private Player m_Player;

    //[SerializeField]
    //private Enemy m_Enemy;

    [SerializeField]
    private TextMeshProUGUI m_StatusText;

    private void Start()
    {
        
    }

    private void Update()
    {
        m_Hp = m_Player.GetHP();
        m_Mp = m_Player.GetMP();

        m_StatusText.text = "HP:" + m_Hp.ToString("D2") + "\n"
            + "MP:" + m_Mp.ToString("D2");
    }

    public void SetStatus(int hp, int mp)
    {
        m_Hp = hp;  
        m_Mp = mp;
    }

    public void SetPos(Vector2 targetPosition)
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();

        if (rectTransform == null) return;

        rectTransform.anchoredPosition = targetPosition;

        Debug.Log($"{this.gameObject.name} ‚ð {targetPosition} ‚É”z’u‚µ‚Ü‚µ‚½");
    }
}


