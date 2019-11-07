using UnityEngine;
using System.Collections.Generic;

public static class ArrayExtensions
{

    public static int IndexOf<T>(this T[] array, T item)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (EqualityComparer<T>.Default.Equals(array[i], item))
                return i;
        }

        return -1;
    }

    public static T RandomItem<T>(this T[] array)
    {
        int index = Random.Range(0, array.Length);
        return array[index];
    }

}

