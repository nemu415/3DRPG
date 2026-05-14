using UnityEngine;

public class ManagerStarter : MonoBehaviour
{
    [SerializeField]
    private GameObject m_BattleManager;

    private void Awake()
    {
        m_BattleManager.gameObject.SetActive(true);
    }
}
