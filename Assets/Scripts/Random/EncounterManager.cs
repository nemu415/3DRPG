using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterManager : MonoBehaviour
{
    [SerializeField] EncounterZone zone;
    [SerializeField] float encounterDistance = 10f; // 10m•à‚­‚Æ”»’è
    [SerializeField] float encounterRate = 0.3f;

    Vector3 lastPos;
    float walked = 0f;

    void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        if (!zone.playerInside) return;

        float d = Vector3.Distance(transform.position, lastPos);
        walked += d;
        lastPos = transform.position;

        if (walked >= encounterDistance)
        {
            walked = 0f;

            if (Random.value < encounterRate)
            {
                SceneManager.LoadScene("BattleScene");
            }
        }
    }
}
