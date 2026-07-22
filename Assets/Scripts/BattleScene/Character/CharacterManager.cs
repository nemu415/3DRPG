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
    private GameObject m_Mashroom;

    [SerializeField]
    private GameObject m_Cactus;

    [SerializeField]
    private GameObject m_Shell;

    [SerializeField]
    private GameObject m_Slime;

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

    public void CreateCharacter()
    {
        CreatePlayer();
        CreateEnemy();
    }

    private void CreatePlayer()
    {
        Vector3 spawnPos = new Vector3(-3, 2, 0);

        GameObject instance = Instantiate(m_Player, spawnPos, Quaternion.Euler(0f, 90f, 0f));
        Player player = instance.GetComponentInChildren<Player>();
        CharacterList.Add(player);
        player.IsPlayer();
        Debug.Log($"ê∂ê¨íºå„ÇÃç¿ïW: {instance.transform.position}");
    }

    private void CreateEnemy()
    {
        int enemyNum = Random.Range(1, 4);

        for (int i = 0;  i < enemyNum; i++)
        {
            int enemyType = Random.Range(1, 5);
            enemyType = i + 1;

            Vector3 spawnPos = new Vector3(0, 0, 0);

            spawnPos = new Vector3(3, 1.8f, 1.5f * (enemyNum - 1)  - i * 3);

            switch (enemyType)
            {
                case 1:
                    {
                        GameObject instance = Instantiate(m_Mashroom, spawnPos, Quaternion.Euler(0f, -90f, 0f));
                        Mashroom mashroom = instance.GetComponentInChildren<Mashroom>();
                        CharacterList.Add(mashroom);
                    }
                    break;
                case 2:
                    {
                        GameObject instance = Instantiate(m_Shell, spawnPos, Quaternion.Euler(0f, -90f, 0f));
                        Shell shell = instance.GetComponentInChildren<Shell>();
                        CharacterList.Add(shell);
                    }
                    break;
                case 3:
                    {
                        GameObject instance = Instantiate(m_Cactus, spawnPos, Quaternion.Euler(0f, -90f, 0f));
                        Cactus cactus = instance.GetComponentInChildren<Cactus>();
                        CharacterList.Add(cactus);
                    }
                    break;
                case 4:
                    {
                        GameObject instance = Instantiate(m_Slime, spawnPos, Quaternion.Euler(0f, -90f, 0f));
                        Slime slime = instance.GetComponentInChildren<Slime>();
                        CharacterList.Add(slime);
                    }
                    break;
                default:
                    break;
            }
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
