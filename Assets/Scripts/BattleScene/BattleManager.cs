using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using System.IO;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Threading.Tasks;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private MessageText m_MessageText;

    [SerializeField]
    private Player m_Player;

    [SerializeField]
    private Enemy m_Enemy;

    [SerializeField]
    private MainCamera m_MainCamera;

    [SerializeField]
    private GameObject m_PlayerStatusText;

    [SerializeField]
    private GameObject m_EnemyStatusText;

    [SerializeField]
    private ItemManager m_ItemManager;

    [SerializeField]
    private GameObject m_Item;

    [SerializeField]
    private GameObject m_Message;

    [SerializeField]
    private Transform canvasTransform;

    List<string[]> TextData = new List<string[]>();

    List<GameObject> EnemyTextList = new List<GameObject>();

    List<IBattleCharacter> CharacterList = new List<IBattleCharacter>();

    private List<IBattleCharacter> turnOrder = new List<IBattleCharacter>();

    private int m_PlayerAct;

    private int enemyNum;

    private int situationNum;

    private ItemManager.Itemtype m_ItemType;


    public interface IBattleCharacter
    {
        string Name { get; }
        int Speed { get; }

        Task TakeTurn();
    }

    private void Start()
    {
        Vector3 playerPos = new Vector3(-3.0f, 1.8f, 0.0f);

        Player player = Instantiate(m_Player, playerPos, this.transform.rotation);
        CharacterList.Add(player);

        Vector3 enemyPos = new Vector3(3.0f, 1.8f, 0.0f);

        enemyNum = 2;

        Enemy enemy = null;

        for (int i = 0; i < enemyNum; i++)
        {
            enemyPos.z = -(float)enemyNum + (float)i * 2;
            enemy = Instantiate(m_Enemy, enemyPos, this.transform.rotation);
            CharacterList.Add(enemy);
        }

        m_Item.SetActive(true);

        m_Message.SetActive(true);

        if (m_Player != null)
        {
            player.Init();
        }

        if (m_Enemy != null)
        {
            enemy.Init();
        }

        turnOrder = CharacterList.OfType<IBattleCharacter>().OrderByDescending(c => c.Speed).ToList();

        Debug.Log("行動順");
        for (int i = 0; i < turnOrder.Count; i++)
        {
            Debug.Log(turnOrder[i].Name);
        }

        m_MessageText.SetText("{0} が あらわれた！", m_Enemy.GetName());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            situationNum++;
        }

        if (situationNum == 1)
        {
            m_MainCamera.BattleStart();
            Vector3 playerTextPos = new Vector3(-400.0f, 130.0f, 0.0f);
            Vector3 enemyTextPos = new Vector3(400.0f, -270.0f, 0.0f);

            GameObject playerStatusText = Instantiate(m_PlayerStatusText, canvasTransform);
            GameObject enemyStatusText;

            RectTransform rectranceForm = playerStatusText.GetComponent<RectTransform>();
            if (rectranceForm != null)
            {
                rectranceForm.anchoredPosition = playerTextPos;
            }

            for (int i = 0; i < enemyNum; i++)
            {
                enemyTextPos.x += i * 100.0f;
                enemyStatusText = Instantiate(m_EnemyStatusText, canvasTransform);
                EnemyTextList.Add(enemyStatusText);
                rectranceForm = enemyStatusText.GetComponent<RectTransform>();
                if (rectranceForm != null)
                {
                    rectranceForm.anchoredPosition = enemyTextPos;
                }
            }
        }

        m_Player.ActedReset();
        m_Enemy.ActedReset();

        m_MessageText.SetText(
            "{0} は どうする？\n" +
            "1:攻撃 2:魔法 3:アイテム 4:逃げる",
            m_Player.GetName());

        // 攻撃
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_PlayerAct = 1;
            situationNum++;
        }

        // 魔法
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_PlayerAct = 2;
            situationNum++;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            m_PlayerAct = 3;
            situationNum++;
        }

        // 逃げる
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            m_PlayerAct = 4;
            situationNum++;
        }

        if (situationNum == 2)
        {
            Action();
        }
    }

    public async void Action()
    {
        for (int i = 0; i < turnOrder.Count; i++)
        {
            IBattleCharacter currentCharacter = turnOrder[i];

            Debug.Log($"{currentCharacter.Name}のターンです");

            await currentCharacter.TakeTurn();

            Debug.Log($"{currentCharacter.Name}のターン終了");
        }
        Debug.Log("全員のターン終了");
    }

        /*if (m_PlayerAct == 1)
        {
            m_MessageText.SetText("Playerの攻撃！");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Player.Attack();
                m_Player.Act();
                m_TextNum = BattleText.PLAYER_ATTACK_DAMAGE;
            }
        }
        else if (m_PlayerAct == 2)
        {
            if (m_Player.GetMP() > 5)
            {
                m_MessageText.SetText("Playerの{0}魔法！");
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_Player.Magic();
                    m_Player.Act();
                    m_TextNum = BattleText.PLAYER_MAGIC_DAMAGE;
                }
            }
            else
            {
                m_MessageText.SetText("MPが足りない！");
                if (Input.GetKeyDown(KeyCode.Space)) m_TextNum = BattleText.CHOOSE_ACTION;
            }
        }

        m_MessageText.SetText("Enemyの攻撃！");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_Enemy.Attack();
            m_Enemy.Act();
            m_TextNum = BattleText.ENEMY_ATTACK_DAMAGE;
        }

        m_MessageText.SetText("{0}に{1}ダメージ!", m_Enemy.GetName());

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_Enemy.GetHP() <= 0)
            {
                EnemyDie();
            }

            else if (m_Enemy.IsActed())
            {
                m_TextNum = BattleText.CHOOSE_ACTION;
            }
            else
            {
                m_TextNum = BattleText.ENEMY_ATTACK;
            }
        }

        m_MessageText.SetText("{0}に{1}ダメージ!", m_Enemy.GetName());

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_Enemy.GetHP() <= 0)
            {
                EnemyDie();
            }

            else if (m_Enemy.IsActed())
            {
                m_TextNum = BattleText.CHOOSE_ACTION;
            }
            else
            {
                m_TextNum = BattleText.ENEMY_ATTACK;
            }
        }

        m_MessageText.SetText("{0}に{1}ダメージ!", m_Player.GetName());

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_Player.GetHP() <= 0)
            {
                PlayerDie();
            }

            else if (m_Enemy.IsActed())
            {
                m_TextNum = BattleText.CHOOSE_ACTION;
            }
            else
            {
                m_TextNum = BattleText.PLAYER_ATTACK;
            }
        }

        Item();

        m_MessageText.SetText("うまく逃げ切れた");
        m_TextNum = BattleText.BATTLE_END;

        m_MessageText.SetText("逃げられなかった");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_TextNum = BattleText.ENEMY_ATTACK;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("SoshiKurosawa");
        }

    }

    private void PlayerDie()
    {
        m_MessageText.SetText("{0} は 力尽きた…", m_Player.GetName());
        m_TextNum = BattleText.BATTLE_END;
    }

    private void EnemyDie()
    {
        m_MessageText.SetText("{0} を 倒した！", m_Enemy.GetName());
        m_TextNum = BattleText.BATTLE_END;
    }

    private void Item()
    {
        m_Item.gameObject.SetActive(true);

        if (Input.GetKeyDown(KeyCode.A))
        {
            m_ItemType = ItemManager.Itemtype.HP_HEAL;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            m_ItemType = ItemManager.Itemtype.MP_HEAL;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            m_ItemType = ItemManager.Itemtype.ESCAPE;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch(m_ItemType)
            {
                case ItemManager.Itemtype.HP_HEAL:
                    m_Player.HPHeal(10);
                    m_ItemManager.SpendItem(ItemManager.Itemtype.HP_HEAL);
                    m_Player.Act();
                    m_TextNum = BattleText.ENEMY_ATTACK;
                    m_Item.gameObject.SetActive(false);
                    break;
                case ItemManager.Itemtype.MP_HEAL:
                    m_Player.MPHeal(10);
                    m_ItemManager.SpendItem(ItemManager.Itemtype.MP_HEAL);
                    m_Player.Act();
                    m_TextNum = BattleText.ENEMY_ATTACK;
                    m_Item.gameObject.SetActive(false);
                    break;
                case ItemManager.Itemtype.ESCAPE:
                    Escape(0);
                    m_ItemManager.SpendItem(ItemManager.Itemtype.ESCAPE);
                    m_Item.gameObject.SetActive(false);
                    break;
            }
        }

        if (m_ItemType == 0)
        {
            m_MessageText.SetText("HPを10回復");
        }
        else if (m_ItemType == ItemManager.Itemtype.MP_HEAL)
        {
            m_MessageText.SetText("MPを10回復");
        }
        else if (m_ItemType == ItemManager.Itemtype.ESCAPE)
        {
            m_MessageText.SetText("確実に逃げられる");
        }
    }

    private void Escape(int border)
    {
        int rand = Random.Range(1, 100);

        border -= (m_Player.GetSpeed() - m_Enemy.GetSpeed());

        if (rand > border)
        {
            m_TextNum = BattleText.ESCAPE_SUCCESS;
        }
        else
        {
            m_TextNum = BattleText.ESCAPE_FAILED;
            m_Player.Act();
        }
    }*/
}
