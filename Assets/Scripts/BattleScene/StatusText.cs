using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatusText : MonoBehaviour
{
    private int hp;
    private int mp;

    [SerializeField]
    private Player m_Player;

    [SerializeField]
    private Enemy m_Enemy;

    [SerializeField]
    private TextMeshProUGUI m_StatusText;


    private void Start()
    {

    }

    private void Update()
    {
        if (this.gameObject.name == "PlayerStatus")
        {
            hp = m_Player.GetHP();
            mp = m_Player.GetMP();
        }
        else if (this.gameObject.name == "EnemyStatus")
        {
            hp = m_Enemy.GetHP();
            mp = m_Enemy.GetMP();
        }

        m_StatusText.text = "HP:" + hp.ToString("D2") + "\n"
            + "MP:" + mp.ToString("D2");
    }
}
