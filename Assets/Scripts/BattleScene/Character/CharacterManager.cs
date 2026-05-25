using System.Collections.Generic;
using UnityEngine;
using static BattleManager;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    private Player m_Player;

    [SerializeField]
    private RedEnemy m_RedEnemy;

    [SerializeField]
    private YellowEnemy m_YellowEnemy;

    [SerializeField]
    private BlueEnemy m_BlueEnemy;

    public enum CharacterType
    {
        PLAYER,
        RED_ENEMY,
        YELLOW_ENEMY,
        BLUE_ENEMY,
        ENEMY_TYPE_MAX
    }

    List<CharacterBase> CharacterList = new List<CharacterBase>();

    public void CreatePlayer()
    {
       Player player = Instantiate(m_Player);
       CharacterList.Add(player);
    }

    public void CreateEnemy()
    {
        RedEnemy red = Instantiate(m_RedEnemy);
        BlueEnemy blue = Instantiate(m_BlueEnemy);
        YellowEnemy yellow = Instantiate(m_YellowEnemy);
        CharacterList.Add(red);
        CharacterList.Add(blue);
        CharacterList.Add(yellow);

        for (int i = 0; i < CharacterList.Count; i++)
        {
            Debug.Log(CharacterList[i].GetName());
        }
    }
}
