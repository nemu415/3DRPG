using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    public void LoadTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}