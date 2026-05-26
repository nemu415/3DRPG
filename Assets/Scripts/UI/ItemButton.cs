using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public GameObject MenuRoot;
    public GameObject ItemRoot;
    public void GoToItem()
    {
        MenuRoot.SetActive(false);

        ItemRoot.SetActive(true);
    }
}
