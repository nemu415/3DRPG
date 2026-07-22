using UnityEngine;

public static class Chest
{
    // 宝箱から何個ダイスが出る？
    public static int GetDiceCount()
    {
        // 1～3個の中からランダムな整数を取得
        return RandomManager.GetInt(1, 3);
    }

    // ダイスの種類
    public static Dice.DiceType GetDiceType()
    {
        int value = RandomManager.GetInt(1, 100);   // 1～100の中からランダムな整数を取得

        if (value <= 40)    // 40%の確率でD4
            return Dice.DiceType.D4;

        if (value <= 70)    // 30%の確率でD6
            return Dice.DiceType.D6;

        if (value <= 90)    // 20%の確率でD8
            return Dice.DiceType.D8;

        return Dice.DiceType.D20;   // 10%の確率でD20
    }
}