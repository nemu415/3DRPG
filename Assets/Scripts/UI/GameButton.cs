using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButton : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public GameObject MenuRoot;

    public void GoBackGame()
    {
        MenuRoot.SetActive(false);

        Time.timeScale = 1f;
    }
}