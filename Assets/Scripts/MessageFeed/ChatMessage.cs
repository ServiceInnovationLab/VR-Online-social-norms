using UnityEngine;
using UnityEngine.UI;

public class ChatMessage : MonoBehaviour
{
    public string from;
    public string message;

    [SerializeField] Text messageText;
    [SerializeField] Text fromText;
    [SerializeField] float timeToLive;

    float time = 0;

    public Text GetMessageTextField()
    {
        return messageText;
    }

    private void Awake()
    {
        fromText.text = "[All] " + from;
        messageText.text = message;
    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;

        if (time > timeToLive)
        {
            Destroy(gameObject);
        }
    }
}