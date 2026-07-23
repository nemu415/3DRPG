using UnityEngine;

public class Dice: MonoBehaviour
{
    public enum DiceType
    {
        D4, // 4面ダイス
        D6, // 6面ダイス
        D8, // 8面ダイス
        D12,// 12面ダイス
        D20 // 20面ダイス
    }

    // 初期所持数
    private static int d4Count = 1; // 4面ダイスの初期所持数
    private static int d6Count = 1; // 6面ダイスの初期所持数
    private static int d8Count = 1; // 8面ダイスの初期所持数
    private static int d12Count = 1;// 12面ダイスの初期所持数
    private static int d20Count = 1;// 20面ダイスの初期所持数

    // ダイスを追加
    public static void AddDice(DiceType type)
    {
        switch (type)   // ダイスの種類に応じて所持数を増やす
        {
            case DiceType.D4:
                d4Count++;
                break;

            case DiceType.D6:
                d6Count++;
                break;

            case DiceType.D8:
                d8Count++;
                break;

            case DiceType.D12:
                d12Count++;
                break;

            case DiceType.D20:
                d20Count++;
                break;
        }
    }

    // ダイスを1個振る
    public static int Roll(DiceType type)
    {
        switch (type) // ダイスの種類に応じてランダムな値を返す
        {
            case DiceType.D4:
                return RandomManager.GetInt(1, 4);

            case DiceType.D6:
                return RandomManager.GetInt(1, 6);

            case DiceType.D8:
                return RandomManager.GetInt(1, 8);

            case DiceType.D12:
                return RandomManager.GetInt(1, 12);

            case DiceType.D20:
                return RandomManager.GetInt(1, 20);
        }

        return 0;
    }

    // 全ダイスを振る
    public static int RollAll() // 所持している全てのダイスを振って合計値を返す
    {
        int total = 0;

        for (int i = 0; i < d4Count; i++)
            total += Roll(DiceType.D4);

        for (int i = 0; i < d6Count; i++)
            total += Roll(DiceType.D6);

        for (int i = 0; i < d8Count; i++)
            total += Roll(DiceType.D8);

        for (int i = 0; i < d12Count; i++)
            total += Roll(DiceType.D12);

        for (int i = 0; i < d20Count; i++)
            total += Roll(DiceType.D20);

        return total;
    }

    // 所持数取得
    public static int GetDiceCount(DiceType type)
    {
        switch (type)   // ダイスの種類に応じて所持数を返す
        {
            case DiceType.D4: return d4Count;
            case DiceType.D6: return d6Count;
            case DiceType.D8: return d8Count;
            case DiceType.D12: return d12Count;
            case DiceType.D20: return d20Count;
        }

        return 0;
    }

    // デバッグ表示
    public static string GetDiceText()  // 所持しているダイスの種類と数を文字列で返す
    {
        return
            "D4 : " + d4Count +
            "\nD6 : " + d6Count +
            "\nD8 : " + d8Count +
            "\nD12 : " + d12Count +
            "\nD20 : " + d20Count;
    }
}