using System.Collections.Generic;
using UnityEngine;
using static BattleManager;

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

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    [SerializeField]
    private GameObject m_Player;

    [SerializeField]
    private GameObject m_RedEnemy;

    [SerializeField]
    private GameObject m_YellowEnemy;

    [SerializeField]
    private GameObject m_BlueEnemy;

    [SerializeField]
    private TextManager m_TextManager;

    [SerializeField]
    private GameObject m_Status;

    public List<CharacterBase> CharacterList = new List<CharacterBase>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            CharacterBase character = CharacterList[i];
            if (character.GetHP() <= 0)
            {
                character.Die();
                CharacterList.RemoveAt(i);
            }
        }
    }

    

    public void CreateCharacter()
    {
        CreatePlayer();
        CreateEnemy();
    }

    private void CreatePlayer()
    {
        Vector3 spawnPos = new Vector3(-3, 1.8f, 0);

        GameObject instance = Instantiate(m_Player, spawnPos, Quaternion.identity);
        Player player = instance.GetComponentInChildren<Player>();
        CharacterList.Add(player);
        Debug.Log($"ê∂ê¨íºå„ÇÃç¿ïW: {instance.transform.position}");
    }

    private void CreateEnemy()
    {
        int enemyNum = Random.Range(1, 4);

        for (int i = 0;  i < enemyNum; i++)
        {
            int enemyType = Random.Range(1, 4);

            Vector3 spawnPos = new Vector3(0, 0, 0);

            spawnPos = new Vector3(3, 1.8f, 1.5f * (enemyNum - 1)  - i * 3);

            switch (enemyType)
            {
                case 1:
                    {
                        GameObject instance = Instantiate(m_RedEnemy, spawnPos, Quaternion.identity);
                        RedEnemy red = instance.GetComponentInChildren<RedEnemy>();
                        CharacterList.Add(red);
                    }
                    break;
                case 2:
                    {
                        GameObject instance = Instantiate(m_BlueEnemy, spawnPos, Quaternion.identity);
                        BlueEnemy blue = instance.GetComponentInChildren<BlueEnemy>();
                        CharacterList.Add(blue);
                    }
                    break;
                case 3:
                    {
                        GameObject instance = Instantiate(m_YellowEnemy, spawnPos, Quaternion.identity);
                        YellowEnemy yellow = instance.GetComponentInChildren<YellowEnemy>();
                        CharacterList.Add(yellow);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void CreateStatusText()
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            m_TextManager.CreateText(TextType.STATUS_TEXT);
        }
    }

    public List<CharacterBase> GetCharacterList()
    {
        if (CharacterList == null)
        {
            Debug.Log("null");
            return null;
        }
        else
        {
            return CharacterList;
        }
    }
}
