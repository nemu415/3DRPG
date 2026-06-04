using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    public enum ItemType
    {
        HP_HEAL,
        MP_HEAL,
        ESCAPE,
    }

    public ItemType m_Itemtype;

    private List<int> ItemList = new List<int>();

    private void Awake()
    {
        int itemCount = System.Enum.GetValues(typeof(ItemType)).Length;

       for (int i = 0; i < itemCount; i++)
       {
            int initialCount = 5 - i;
            ItemList.Add(initialCount);
        }
    }

    private void Update()
    {
        
    }

    public int GetItemNum(ItemType type)
    {
        int index = (int)type;

        if (index >= 0 && index < ItemList.Count)
        {
           
            return ItemList[index];
        }
        return 0;
    }

    public void SpendItem(ItemType type)
    {
        ItemList[(int)type]--;
    }
}
