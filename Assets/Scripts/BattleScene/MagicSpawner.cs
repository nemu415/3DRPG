using UnityEngine;

public class MagicSpawner : MonoBehaviour
{
    public static MagicSpawner Instance { get; private set; }

    [SerializeField]
    private GameObject magicPrefab;

    private Transform targetTranceform;

    private float destroyTime = 1f;

    [ContextMenu("Test Play Effect")]
    public void PlayMagicEffect(Vector3 pos)
    {
        if (magicPrefab == null)
        {
            Debug.Log("–¢Š„‚è“–‚Ä");
        }

        Vector3 spawnPosition = pos;
        Quaternion spawnRotation = Quaternion.identity;

        GameObject spawnedEffect = Instantiate(magicPrefab, spawnPosition, spawnRotation);

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
