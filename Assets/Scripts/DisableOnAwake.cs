using UnityEngine;

public class DisableOnAwake : MonoBehaviour
{

    private void Awake()
    {
        gameObject.SetActive(false);
    }
}
