using UnityEngine;

[System.Serializable]
public struct FloatRange
{
    public float min, max;

    public float GetValue()
    {
        if (min == max)
            return min;

        return Random.Range(min, max);
    }

    public int GetIntValue()
    {
        return (int)GetValue();
    }
}