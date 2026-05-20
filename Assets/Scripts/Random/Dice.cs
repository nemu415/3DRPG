using UnityEngine;

public class Dice : MonoBehaviour
{
    // 16–Êƒ_ƒCƒX‚ğ1‰ñU‚é
    public int Roll()
    {
        return Random.Range(1, 17);
    }

    // nŒÂU‚é
    public int RollMultiple(int diceCount)
    {
        int total = 0;
        
        for (int i = 0; i < diceCount; i++)
        {
            total += Roll();
        }

        return total;
    }
}
