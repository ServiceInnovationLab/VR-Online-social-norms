using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(T)) as T;

                if (!instance)
                {
                    var obj = new GameObject(nameof(T));
                    instance = obj.AddComponent<T>();
                    Debug.Log(nameof(T) + " automatically created.");
                }
            }

            return instance;
        }
    }
}
