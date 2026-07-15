using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    public static EffectSpawner Instance { get; private set; }

    [SerializeField]
    private GameObject redMagic;

    [SerializeField]
    private GameObject yellowMagic;

    [SerializeField]
    private GameObject blueMagic;

    [SerializeField]
    private GameObject hpHeal;

    [SerializeField]
    private GameObject mpHeal;

    [SerializeField]
    private GameObject escape;

    private Transform targetTranceform;

    private float destroyTime = 1f;

    public enum EffectType
    {
        RED_MAGIC,
        YELLOW_MAGIC,
        BLUE_MAGIC,
        HP_HEAL,
        MP_HEAL,
        ESCAPE,
        NONE
    }

    public void PlayMagicEffect(EffectType type,Vector3 pos)
    {
        GameObject spawnedEffect = null;

        Vector3 spawnPosition = pos;
        Quaternion spawnRotation = Quaternion.identity;

        switch (type)
        {
            case EffectType.RED_MAGIC:
                spawnedEffect = Instantiate(redMagic, spawnPosition, spawnRotation);
                if (redMagic == null)
                {
                    Debug.Log("MagicNull");
                }
                break;
            case EffectType.YELLOW_MAGIC:
                spawnedEffect = Instantiate(yellowMagic, spawnPosition, yellowMagic.transform.rotation);
                if (yellowMagic == null)
                {
                    Debug.Log("MagicNull");
                }
                break;
            case EffectType.BLUE_MAGIC:
                spawnedEffect = Instantiate(blueMagic, spawnPosition, blueMagic.transform.rotation);
                if (blueMagic == null)
                {
                    Debug.Log("MagicNull");
                }
                break;
            case EffectType.HP_HEAL:
                spawnedEffect = Instantiate(hpHeal, spawnPosition, spawnRotation);
                if (hpHeal == null)
                {
                    Debug.Log("HPNull");
                }
                break;
            case EffectType.MP_HEAL:
                spawnedEffect = Instantiate(mpHeal, spawnPosition, spawnRotation);
                if (mpHeal == null)
                {
                    Debug.Log("MPNull");
                }
                break;
            default:
                break;

        }

        Destroy(spawnedEffect, destroyTime);
    }

    public void PlayHPHeal(Vector3 pos)
    {
        if (hpHeal == null)
        {
            Debug.Log("–¢Š„‚è“–‚Ä");
        }

        Vector3 spawnPosition = pos;
        Quaternion spawnRotation = Quaternion.identity;

        GameObject spawnedEffect = Instantiate(hpHeal, spawnPosition, spawnRotation);

        Destroy(spawnedEffect, destroyTime);
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayMagicEffect(this.transform.position);
        }*/
    }
}
