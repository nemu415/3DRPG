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

        CharacterBase enemy = m_CharacterManager.GetCharacterList()[1];

        string enemyName = enemy.GetName();

        m_TextManager.CreateText(TextType.MESSAGE_TEXT);
        //m_TextManager.SetMessageText("");
        m_TextManager.SetMessageText(enemyName + "が現れた！");

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
        yield return WaitForKeyInput();

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
                playerName + "は どうする？\n" +
                "1:攻撃 2:魔法 3:アイテム 4:逃げる"
                 );

            ActionType playerAction = ActionType.ATTACK;

            bool inputSelected = false;

            while (!inputSelected)
            {
                yield return null;

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    playerAction = ActionType.ATTACK;
                    inputSelected = true;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    playerAction = ActionType.MAGIC;
                    inputSelected = true;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    playerAction = ActionType.ITEM;
                    inputSelected = true;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    playerAction = ActionType.ESCAPE;
                    inputSelected = true;
                }
            }

            CharacterBase playerTargetCharacter = m_CharacterManager.GetCharacterList()[0];



            if (playerAction == ActionType.ATTACK || playerAction == ActionType.MAGIC)
            {
                m_TextManager.SetMessageText("誰に攻撃する？");

                for (int i = 1; i < characterList.Count; i++)
                {
                    m_TextManager.AddMessageText("\n" + i + "." + characterList[i].GetName());
                }

                if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3))
                {
                    yield return null;
                }

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

                for (int j = 0; j < sortedCharacterList.Count; j++)
                {
                    if (sortedCharacterList[i].IsPlayer())
                    {
                        playerAlive = true;
                    }
                }

                if (!playerAlive)
                {
                    StartCoroutine(Lose());

                    yield break;
                }
            }
        }

        StartCoroutine(Win());

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
