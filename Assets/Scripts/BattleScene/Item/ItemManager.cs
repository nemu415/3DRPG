using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ItemType
{
    HP_HEAL,
    MP_HEAL,
    ESCAPE,
}

public class ItemManager : MonoBehaviour
{
    private static ItemManager instance;

    public static ItemManager Instance
    {
        get
        {
            // インスタンスが空なら、シーン内から自動で探して持ってくる
            if (instance == null)
            {
                instance = UnityEngine.Object.FindFirstObjectByType<ItemManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private HPHeal m_HPheal;

    [SerializeField]
    private MPHeal m_MPheal;

    [SerializeField]
    private Escape m_Escape;

    public ItemType m_Itemtype;

    private List<ItemBase> ItemList = new List<ItemBase>();

    private void Awake()
    {
        HPHeal hpHeal = Instantiate(m_HPheal);
        MPHeal mpHeal = Instantiate(m_MPheal);
        Escape escape = Instantiate(m_Escape);

        ItemList.Add(hpHeal);
        ItemList.Add(mpHeal);
        ItemList.Add(escape);
    }

    private void Update()
    {
        
    }

    public int GetItemNum(ItemType type)
    {
        switch (type)
        { 
            case ItemType.HP_HEAL:
                return m_HPheal.GetNum();
            case ItemType.MP_HEAL:
                return m_MPheal.GetNum();
            case ItemType.ESCAPE:
                return m_Escape.GetNum();
            default:
                return 0;
        }
    }

    public void SpendItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.HP_HEAL:
                if (m_HPheal != null) m_HPheal.Spend();
                break;

            case ItemType.MP_HEAL:
                if (m_MPheal != null) m_MPheal.Spend();
                break;

            case ItemType.ESCAPE:
                if (m_Escape != null) m_Escape.Spend();
                break;

            default:
                Debug.LogWarning($"未定義のアイテムタイプ: {type}");
                break;
        }

    }
}
