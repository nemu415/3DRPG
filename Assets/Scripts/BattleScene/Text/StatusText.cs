using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StatusText : MonoBehaviour
{
    public static Slider hpSlider;
    public static Slider mpSlider;

    public Slider hpSliderInput;
    public Slider mpSliderInput;

    private int m_Hp;
    private int m_Mp;

    private string m_Name;

    [SerializeField]
    private Player m_Player;

    //[SerializeField]
    //private Enemy m_Enemy;

    [SerializeField]
    private TextMeshProUGUI m_StatusText;

    public int GetHP() { return m_Hp; }
    public int GetMP() { return m_Mp; }

    private void Awake()
    {
        GameObject hpBarObj = GameObject.Find("HPBar");
        if (hpBarObj != null)
        {
            hpSlider = hpBarObj.GetComponent<Slider>();
        }

        GameObject mpBarObj = GameObject.Find("MPBar");
        if (mpBarObj != null)
        {
            mpSlider = mpBarObj.GetComponent<Slider>();
        }

        hpSlider = hpSliderInput;
        mpSlider = mpSliderInput;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (hpSlider != null) hpSlider.value = m_Hp;
        if (mpSlider != null) mpSlider.value = m_Mp;
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

        rectTransform.anchoredPosition = targetPosition;
    }
}


