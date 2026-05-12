using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private CanvasGroup m_CanvasGroup;

    [SerializeField]
    [Multiline(3)]
    private TextMeshProUGUI m_ItemText;

    public enum Itemtype
    {
        HP_HEAL,
        MP_HEAL,
        ESCAPE,
    }

    public Itemtype m_Itemtype;

    private int[] m_ItemNum = { 0, 0, 0};

    private void Start()
    {
       for (int i = 0; i < m_ItemNum.Length; i++)
       {
            m_ItemNum[i] = 5 - i;
       }
    }

    private void Update()
    {
        m_ItemText.text = string.Format(
            "1.回復薬　　　　{0}\n" +
            "2.魔力チャージ　{1}\n" +
            "3.煙玉　　　　　{2}",
            m_ItemNum[0], m_ItemNum[1], m_ItemNum[2]);
    }

    public void SpendItem(Itemtype type)
    {
        m_ItemNum[(int)type]--;
    }
}
