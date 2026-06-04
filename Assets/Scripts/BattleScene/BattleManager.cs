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
        m_TextManager.SetStatus();
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
                yield return null;

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
            }

            CharacterBase playerTargetCharacter = m_CharacterManager.GetCharacterList()[0];


            if (playerAction == CharacterManager.ActionType.ATTACK || playerAction == CharacterManager.ActionType.MAGIC)
            {
                m_TextManager.SetMessageText("誰に攻撃する？");

                bool targetSelected = false;

                while (!targetSelected)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        playerTargetCharacter = m_CharacterManager.GetCharacterList()[1];
                        targetSelected = true;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        playerTargetCharacter = m_CharacterManager.GetCharacterList()[2];
                        targetSelected = true;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        playerTargetCharacter = m_CharacterManager.GetCharacterList()[3];
                        targetSelected = true;
                    }

                    yield return null;
                }
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

                CharacterBase targetCharacter = m_CharacterManager.GetCharacterList()[0];

                CharacterManager.ActionType finalAction;

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
                    if (attackPercent > border)
                    {
                        finalAction = CharacterManager.ActionType.ATTACK;
                    }
                    else
                    {
                        finalAction = CharacterManager.ActionType.MAGIC;
                    }
                }

                currentCharacter.Action(finalAction, targetCharacter);

                //m_TextManager.SetStatus();
                
                yield return WaitForKeyInput();
            }
        }
    }

    
}
