using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private enum Itemtype
    {
        HP_HEAL,
        MP_HEAL,
        ESCAPE,
        POWER_UP,
    }

    private Itemtype m_Itemtype;

    List<Itemtype[]> m_ItemData = new List<Itemtype[]>();

    private void Start()
    {
        m_ItemData.Add(new Itemtype[] { Itemtype.HP_HEAL });
        m_ItemData.Add(new Itemtype[] { Itemtype.MP_HEAL });
        m_ItemData.Add(new Itemtype[] { Itemtype.ESCAPE });
        m_ItemData.Add(new Itemtype[] { Itemtype.POWER_UP });
    }
}
