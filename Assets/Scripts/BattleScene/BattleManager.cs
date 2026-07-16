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

    private ItemType m_ItemType;

    private bool m_BattleStart;


    public interface IBattleCharacter
    {
        string Name { get; }
        int Speed { get; }

        //Task TakeTurn();
    }

    private IEnumerator Start()
    {
        m_CharacterManager.CreateCharacter();

        yield return null;

        m_TextManager.CreateText(TextType.MESSAGE_TEXT);

        var characterList = m_CharacterManager.GetCharacterList();
        bool textCreated = false;
        for (int i = 0; i < characterList.Count; i++)
        {
            if (characterList[i].IsPlayer()) continue;

            if (!textCreated)
            {
                m_TextManager.SetMessageText(characterList[i].GetName() + "が現れた！");
                textCreated = true;
            }
            else
            {
                m_TextManager.AddMessageText("\n" + characterList[i].GetName() + "が現れた！");
            }
        }
        
        yield return WaitForKeyInput();

        m_TextManager.CreateStatusText(m_CharacterManager.GetCharacterList().Count);
        m_TextManager.RefreshStatus(m_CharacterManager.GetCharacterList());

        turnOrder = CharacterList.OfType<IBattleCharacter>().OrderByDescending(c => c.Speed).ToList();

        StartCoroutine(BattleFlow());
    }

    private void Update()
    {
        
    }


    IEnumerator WaitForKeyInput()
    {
        yield return null;

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
    }

    IEnumerator BattleFlow()
    {
        yield return null;

        BattleStart();

        StartCoroutine(CharacterAction());
    }

    private void BattleStart()
    {
        m_MainCamera.BattleStart();
    }

    IEnumerator CharacterAction()
    {
        List<CharacterBase> characterList = new List<CharacterBase>();

        characterList = m_CharacterManager.GetCharacterList();

        CharacterBase player = m_CharacterManager.GetCharacterList()[0];

        string playerName = player.GetName();

        while (characterList.Count > 1)
        {
            m_TextManager.SetMessageText(
                playerName + "は どうする？\n\n" +
                " 攻　撃　　魔　法　アイテム　 逃げる"
                 );

            m_TextManager.CreateText(TextType.CURSOR);

            ActionType playerAction = ActionType.ATTACK;

            bool inputSelected = false;

            Vector2 cursorMove = Vector2.zero;
            int cursorIndexMax = 0;
            int cursorIndexNow = 0;

            while (!inputSelected)
            {
                yield return null;

                cursorIndexMax = 3;

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (cursorIndexNow < cursorIndexMax)
                    {
                        cursorMove = new Vector2(100.0f, 0.0f);
                        m_TextManager.CursorMove(cursorMove);
                        cursorIndexNow++;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (cursorIndexNow > 0)
                    {
                        cursorMove = new Vector2(-100.0f, 0.0f);
                        m_TextManager.CursorMove(cursorMove);
                        cursorIndexNow--;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    inputSelected = true;

                    switch (cursorIndexNow)
                    { 
                        case 0:
                            playerAction = ActionType.ATTACK;
                            break;
                        case 1:
                            playerAction = ActionType.MAGIC;
                            break;
                        case 2:
                            playerAction = ActionType.ITEM;
                            break;
                        case 3:
                            playerAction = ActionType.ESCAPE;
                            break;
                        default:
                            break;
                    }
                }
            }

            CharacterBase playerTargetCharacter = characterList[0];

            if (playerAction == ActionType.ATTACK || playerAction == ActionType.MAGIC)
            {
                m_TextManager.SetMessageText("誰に攻撃する？\n\n");

                for (int i = 1; i < characterList.Count; i++)
                {
                    string characterName = characterList[i].GetName();
                    int nameLength = characterName.Length;

                    m_TextManager.AddMessageText(characterList[i].GetName() + " ");
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //yield return null;
                }

                bool targetSelected = false;

                while (!targetSelected)
                {
                    //yield return null;

                    cursorIndexMax = characterList.Count;

                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (cursorIndexNow < cursorIndexMax)
                        {
                            cursorMove = new Vector2(100.0f, 0.0f);
                            m_TextManager.CursorMove(cursorMove);
                            cursorIndexNow++;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (cursorIndexNow > 0)
                        {
                            cursorMove = new Vector2(-100.0f, 0.0f);
                            m_TextManager.CursorMove(cursorMove);
                            cursorIndexNow--;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        inputSelected = true;

                        switch (cursorIndexNow)
                        {
                            case 0:
                                playerTargetCharacter = m_CharacterManager.GetCharacterList()[1];
                                if (playerTargetCharacter != null)
                                {
                                    targetSelected = true;
                                }
                                break;
                            case 1:
                                playerTargetCharacter = m_CharacterManager.GetCharacterList()[2];
                                if (playerTargetCharacter != null)
                                {
                                    targetSelected = true;
                                }
                                break;
                            case 2:
                                playerTargetCharacter = m_CharacterManager.GetCharacterList()[3];
                                if (playerTargetCharacter != null)
                                {
                                    targetSelected = true;
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    yield return null;
                }
            }

            if (playerAction == ActionType.ITEM)
            {
                m_TextManager.SetMessageText("どのアイテムを使う？");

            }

            List<CharacterBase> sortedCharacterList = characterList
                .OrderByDescending(c =>
                {
                    if (c.IsPlayer() && (playerAction == ActionType.ITEM || playerAction == ActionType.ESCAPE))
                    {
                        return 1;
                    }
                    return 0;
                })
                .ThenByDescending(c => c.GetSpeed())
                .ToList();

            for (int i = 0; i < sortedCharacterList.Count; i++)
            {
                if (sortedCharacterList[i] == null) continue;

                CharacterBase currentCharacter = sortedCharacterList[i];

                CharacterBase targetCharacter = m_CharacterManager.GetCharacterList()[0];

                ActionType finalAction;

                if (currentCharacter.IsPlayer())
                {
                    finalAction = playerAction;

                    targetCharacter = playerTargetCharacter;
                }
                else
                {
                    targetCharacter = m_CharacterManager.GetCharacterList()[0];

                    int attackPercent = currentCharacter.GetAttackPercent();
                    int border = Random.Range(0, 100);
                    if (attackPercent < border && currentCharacter.GetMP() > 0)
                    {
                        finalAction = ActionType.MAGIC;
                    }
                    else
                    {
                        finalAction = ActionType.ATTACK;
                    }
                }

                currentCharacter.Action(finalAction, targetCharacter);

                if (finalAction == ActionType.ATTACK)
                {
                    yield return new WaitWhile(() => currentCharacter.IsAttacking);
                }

                else if (finalAction == ActionType.MAGIC)
                {
                    yield return new WaitWhile(() => currentCharacter.IsMagic);
                }

                
                m_TextManager.SetStatus();

                yield return WaitForKeyInput();

                bool playerAlive = false;
                bool anyEnemyAlive = false; ;

                for (int j = 0; j < sortedCharacterList.Count; j++)
                {
                    if (sortedCharacterList[j].GetHP() <= 0) continue;

                    if (sortedCharacterList[j].IsPlayer())
                    {
                        playerAlive = true;
                    }
                    else
                    {
                        anyEnemyAlive = true;
                        
                    }
                }

                if (!playerAlive)
                {
                    StartCoroutine(Lose());

                    yield break;
                }

                if (!anyEnemyAlive)
                {
                    StartCoroutine(Win());

                    yield break;
                }
            }

            for (int i = 0; i < sortedCharacterList.Count; i++)
            {
                if (!sortedCharacterList[i].IsPoisoned())
                {
                    sortedCharacterList[i].Damage(5);
                }
            }
        }

        

        yield break;
    }

    private IEnumerator Win()
    {
        m_TextManager.SetMessageText("勝利");

        yield return WaitForKeyInput();

        SceneManager.LoadScene("SoshiKurosawa");

        yield break;
    }

    private IEnumerator Lose()
    {
        m_TextManager.SetMessageText("敗北");

        yield return WaitForKeyInput();

        SceneManager.LoadScene("SoshiKurosawa");

        yield break;
    }
}
