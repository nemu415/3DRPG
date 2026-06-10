using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image icon;
    public Text itemName;

    ItemData currentItem;

    public void SetItem(ItemData item)
    {
        currentItem = item;
        icon.sprite = item.icon;
        itemName.text = item.itemName;
    }

    public void OnClick()
    {
        ItemMenu.Instance.ShowDetail(currentItem);
    }
}