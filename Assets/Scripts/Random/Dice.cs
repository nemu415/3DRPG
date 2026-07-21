using UnityEngine;
using System.Collections.Generic;

public static class Dice
{
    public enum DiceType
    {
        D4,     // 4面ダイス
        D6,     // 6面ダイス
        D8,     // 8面ダイス
        D20     // 20面ダイス
    }

    // ダイス1個振る
    public static int Roll(DiceType type)
    {
        switch (type)
        {
            case DiceType.D4: return RandomManager.GetInt(1, 4);    // 1～4のランダムな整数を取得
            case DiceType.D6: return RandomManager.GetInt(1, 6);    // 1～6のランダムな整数を取得
            case DiceType.D8: return RandomManager.GetInt(1, 8);    // 1～8のランダムな整数を取得
            case DiceType.D20: return RandomManager.GetInt(1, 20);  // 1～20のランダムな整数を取得
        }

        return 0;
    }

    // 全部振る
    public static int RollAll(List<DiceType> diceList)
    {
        int total = 0;

        foreach (var dice in diceList)  // すべてのダイスを振る
        {
            total += Roll(dice);    // 振った結果を合計する
        }

        return total;   // 合計値を返す
    }
}