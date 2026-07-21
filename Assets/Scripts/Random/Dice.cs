using UnityEngine;

public class Dice : MonoBehaviour
{
    // ダイスの種類を定義する列挙型
    public enum DiceType
    {
        D4,     // 4面ダイス
        D6,     // 6面ダイス
        D8,     // 8面ダイス
        D20     // 20面ダイス
    }

    // ダイスを振るメソッド
    public static int Roll(DiceType diceType)
    {
        // ダイスの種類に応じてランダムな値を返す
        switch (diceType)
        {
            case DiceType.D4:
                return RandomManager.GetInt(1, 4);

            case DiceType.D6:
                return RandomManager.GetInt(1, 6);

            case DiceType.D8:
                return RandomManager.GetInt(1, 8);

            case DiceType.D20:
                return RandomManager.GetInt(1, 20);

            default:
                return 0;
        }
    }

    // 複数回
    public int RollMultiple(int diceCount, int sides)
    {
        int total = 0;

        for (int i = 0; i < diceCount; i++)
        {
            total += Roll((DiceType)sides);
        }

        return total;
    }
}