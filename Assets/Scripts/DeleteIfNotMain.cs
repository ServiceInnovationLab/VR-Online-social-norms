using UnityEngine;

public class DeleteIfNotMain : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectOfType<AddAdditionalScenes>())
        {
            Destroy(gameObject);
        }
    }
}
