using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuRoot;
    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;
        menuRoot.SetActive(isOpen);

        // ÉQÅ[ÉÄí‚é~
        Time.timeScale = isOpen ? 0 : 1;
    }
}