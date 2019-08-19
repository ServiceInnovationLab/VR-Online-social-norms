using UnityEngine;

public static class MonoBehaviourExtensions
{

    public static T GetOrCreateComponent<T>(this MonoBehaviour behaviour) where T : Component
    {
        T result = behaviour.GetComponent<T>();

        if (!result)
        {
            result = behaviour.gameObject.AddComponent<T>();
        }

        return result;
    }

    public static T GetOrCreateComponent<T>(this GameObject behaviour) where T : Component
    {
        T result = behaviour.GetComponent<T>();

        if (!result)
        {
            result = behaviour.gameObject.AddComponent<T>();
        }

        return result;
    }

}
