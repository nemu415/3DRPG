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

    [SerializeField]
    private TextManager m_TextManager;

    [SerializeField]
    private GameObject m_Status;


    public enum CharacterType
    {
        PLAYER,
        RED_ENEMY,
        YELLOW_ENEMY,
        BLUE_ENEMY,
        ENEMY_TYPE_MAX
    }

    public enum ActionType
    {
        ATTACK,
        MAGIC,
        ITEM,
        ESCAPE,
        ACTION_TYPE_MAX
    }

    List<CharacterBase> CharacterList = new List<CharacterBase>();

    public void CreateCharacter()
    {
        CreatePlayer();
        CreateEnemy();
    }

    private void CreatePlayer()
    {
       Player player = Instantiate(m_Player);
       CharacterList.Add(player);
    }

    private void CreateEnemy()
    {
        RedEnemy red = Instantiate(m_RedEnemy, new Vector3(3, 1.8f, 3), Quaternion.identity);
        BlueEnemy blue = Instantiate(m_BlueEnemy, new Vector3(3, 1.8f, 0), Quaternion.identity);
        YellowEnemy yellow = Instantiate(m_YellowEnemy, new Vector3(3, 1.8f, -3), Quaternion.identity);
        CharacterList.Add(red);
        CharacterList.Add(blue);
        CharacterList.Add(yellow);
    }

    public void CreateStatusText()
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            m_TextManager.CreateText(TextManager.TextType.STATUS_TEXT);
        }
    }

    public List<CharacterBase> GetCharacterList()
    {
        return CharacterList;
    }
}
