using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenu : MonoBehaviour
{
    public static ItemMenu Instance;

    public GameObject menuRoot;
    public Transform contentParent;
    public GameObject itemSlotPrefab;

    public Image detailIcon;
    public Text detailName;
    public Text detailDescription;

    ItemData selectedItem;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        RefreshList();
    }

    public void RefreshList()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (var item in Inventory.Instance.items)
        {
            var slot = Instantiate(itemSlotPrefab, contentParent);
            slot.GetComponent<ItemSlot>().SetItem(item);
        }
    }

    public void ShowDetail(ItemData item)
    {
        selectedItem = item;
        detailIcon.sprite = item.icon;
        detailName.text = item.itemName;
        detailDescription.text = item.description;
    }

    public void UseItem()
    {
        if (selectedItem == null) return;

        Debug.Log($"{selectedItem.itemName} を使用した！");
        // 例：HP回復
        // Player.Instance.Heal(selectedItem.healAmount);
    }

    public void CloseMenu()
    {
        menuRoot.SetActive(false);
        Time.timeScale = 1f;
    }
}
