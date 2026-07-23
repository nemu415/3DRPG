using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField]
    private AudioClip swordAttack;
    [SerializeField]
    private AudioClip enemyAttack;
    [SerializeField]
    private AudioClip magic;
    [SerializeField]
    private AudioClip heal;

    public enum SEType
    {
        SE_SWORD_ATTACK,
        SE_ENEMY_ATTACK,
        SE_MAGIC,
        SE_HEAL,
        SE_TYPE_MAX
    }


    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySE(SEType type)
    {
        AudioClip audio;

        switch (type)
        { 
            case SEType.SE_SWORD_ATTACK:
                audio = swordAttack;
                break;
            case SEType.SE_ENEMY_ATTACK:
                audio = enemyAttack;
                break;
            case SEType.SE_MAGIC:
                audio = magic;
                break;
            case SEType.SE_HEAL:
                audio = heal;
                break;
            default:
                audio = null;
                break;
        }

        audioSource.PlayOneShot(audio);

    }
}
