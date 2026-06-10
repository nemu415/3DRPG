using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject menuRoot;
    public GameObject itemRoot;

    public void ShowMenu()
    {
        menuRoot.SetActive(true);
        itemRoot.SetActive(false);
    }

    public void ShowItem()
    {
        menuRoot.SetActive(false);
        itemRoot.SetActive(true);
    }

    public void HideAll()
    {
        menuRoot.SetActive(false);
        itemRoot.SetActive(false);
    }
}
