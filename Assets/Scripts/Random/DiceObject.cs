using UnityEngine;

public class DiceObject : MonoBehaviour
{
    [SerializeField]
    private Dice.DiceType diceType; // ƒ_ƒCƒX‚Ìí—Ş

    private Vector3 rotateSpeed;    // ‰ñ“]‘¬“x

    private float timer = 1.5f;     // ‰ñ“]ŠÔ

    private void Start()
    {
        // ‰ñ“]‘¬“x‚ğƒ‰ƒ“ƒ_ƒ€‚Éİ’è
        rotateSpeed = new Vector3(
            Random.Range(360f, 720f),
            Random.Range(360f, 720f),
            Random.Range(360f, 720f));
    }

    private void Update()
    {
        // ‰ñ“]ŠÔ‚ª0ˆÈ‰º‚É‚È‚Á‚½‚ç‰ñ“]‚ğ~‚ß‚é
        if (timer <= 0)
            return;

        // ‰ñ“]‚³‚¹‚é
        transform.Rotate(rotateSpeed * Time.deltaTime);

        // ‰ñ“]ŠÔ‚ğŒ¸‚ç‚·
        timer -= Time.deltaTime;
    }
}