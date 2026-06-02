using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.LowLevelPhysics2D.PhysicsLayers;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private Player m_Player;

    //[SerializeField]
    //private Enemy m_Enemy;

    [SerializeField]
    private CharacterManager m_CharacterManager;

    [SerializeField]
    private MainCamera m_MainCamera;

    [SerializeField]
    private TextManager m_TextManager;


    List<string[]> TextData = new List<string[]>();

    List<GameObject> EnemyTextList = new List<GameObject>();

    List<IBattleCharacter> CharacterList = new List<IBattleCharacter>();

    private List<IBattleCharacter> turnOrder = new List<IBattleCharacter>();

    private int m_PlayerAct;

    private int enemyNum;

    private int situationNum;

    private ItemManager.ItemType m_ItemType;

    private bool m_BattleStart;


    public interface IBattleCharacter
    {
        string Name { get; }
        int Speed { get; }

        //Task TakeTurn();
    }

    private void Start()
    {
        m_CharacterManager.CreateCharacter();

        CharacterBase enemy = m_CharacterManager.GetCharacterList()[1];

        string enemyName = enemy.GetName();

        m_TextManager.CreateText(TextManager.TextType.MESSAGE_TEXT);
        //m_TextManager.SetMessageText("");
        m_TextManager.SetMessageText(enemyName + "が現れた！");

        turnOrder = CharacterList.OfType<IBattleCharacter>().OrderByDescending(c => c.Speed).ToList();

        StartCoroutine(BattleFlow());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !m_BattleStart)
        {
            //GameObject playerStatusText = Instantiate(m_PlayerStatusText, canvasTransform);
            //GameObject enemyStatusText;

            //RectTransform rectranceForm = playerStatusText.GetComponent<RectTransform>();
            /*if (rectranceForm != null)
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
            }*/

            m_Player.ActedReset();
            //m_Enemy.ActedReset();

        }

        // 攻撃
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_PlayerAct = 1;
            Action();
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

    }

    public void Action()
    {
        for (int i = 0; i < turnOrder.Count; i++)
        {
            IBattleCharacter currentCharacter = turnOrder[i];

        }
    }

    IEnumerator WaitForKeyInput()
    {
        yield return null;

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
    }

    IEnumerator WaitForSelect()
    {
        yield return null;


    } 

    IEnumerator BattleFlow()
    {
        yield return WaitForKeyInput();

        Debug.Log("バトル開始");

        BattleStart();

        //yield return WaitForKeyInput();

        StartCoroutine(CharacterAction());
        

    }

    private void BattleStart()
    {
        m_MainCamera.BattleStart();

        m_CharacterManager.CreateStatusText();
    }

    IEnumerator CharacterAction()
    {
        int turnNum = 0;

        CharacterBase player = m_CharacterManager.GetCharacterList()[0];

        string playerName = player.GetName();

        while (turnNum < 5)
        {
            turnNum++;

            m_TextManager.SetMessageText("ターン" +  turnNum);

            yield return WaitForKeyInput();

            m_TextManager.SetMessageText(
                playerName + "は どうする？\n" +
                "1:攻撃 2:魔法 3:アイテム 4:逃げる"
                 );

            CharacterManager.ActionType playerAction = CharacterManager.ActionType.ATTACK;

            bool inputSelected = false;

            while(!inputSelected)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    playerAction = CharacterManager.ActionType.ATTACK;
                    inputSelected = true;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    playerAction = CharacterManager.ActionType.MAGIC;
                    inputSelected = true;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    playerAction = CharacterManager.ActionType.ITEM;
                    inputSelected = true;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    playerAction = CharacterManager.ActionType.ESCAPE;
                    inputSelected = true;
                }

                yield return null;
            }

            List<CharacterBase> characterList = m_CharacterManager.GetCharacterList();

            List<CharacterBase> sortedCharacterList = characterList
                .OrderByDescending(c =>
                {
                    if (c.IsPlayer() && (playerAction == CharacterManager.ActionType.ITEM || playerAction == CharacterManager.ActionType.ESCAPE))
                    {
                        return 1;
                    }
                    return 0;
                })
                .ThenByDescending(c => c.GetSpeed())
                .ToList();

            for (int i = 0; i < sortedCharacterList.Count; i++)
            {
                CharacterBase currentCharacter = sortedCharacterList[i];
                CharacterManager.ActionType finalAction;

                if (currentCharacter.IsPlayer())
                {
                    finalAction = playerAction;
                }
                else
                {
                    finalAction = CharacterManager.ActionType.ATTACK;
                }

                currentCharacter.Action(finalAction, characterList);

                m_TextManager.SetStatus();
                
                yield return WaitForKeyInput();
            }
        }
    }

    /*async Task<CharacterManager.ActionType> SelectAction()
    {
        await Task.Delay(100);

        if (Input.GetKey(KeyCode.Alpha1))
        {
            return CharacterManager.ActionType.ATTACK;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            return CharacterManager.ActionType.MAGIC;
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            return CharacterManager.ActionType.ITEM;
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            return CharacterManager.ActionType.ESCAPE;
        }
        else
        {
            return 0;
        }
    }*/

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
