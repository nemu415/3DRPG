using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuRoot;
    private bool isOpen = false;

    void Start()
    {
        menuRoot.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

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

        Time.timeScale = isOpen ? 0 : 1;

        if (isOpen)
        {
            // メニュー中はカーソルを表示・解放
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // ゲームに戻るときはカーソルを非表示・ロック
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
