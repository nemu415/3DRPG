using UnityEngine;

public  class RandomManager: MonoBehaviour 
{
    // min`max
    public static int GetInt(int min, int max)
    {
        return Random.Range(min, max + 1);
    }

    // Z“”»’è
    public static bool CheckPercent(int percent)
    {
        return Random.Range(0, 100) < percent;
    }

    // float”Å
    public static float GetFloat(float min, float max)
    {
        return Random.Range(min, max);
    }
}