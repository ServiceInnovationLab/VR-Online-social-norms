using UnityEngine;

public class PhoneApplicationSwitcher : MonoBehaviour
{

    [SerializeField] Transform[] app1;
    [SerializeField] Transform[] app2;

    public void SwitchToApp2()
    {
        foreach (var transform in app1)
        {
            transform.gameObject.SetActive(false);
        }

        foreach (var transform in app2)
        {
            transform.gameObject.SetActive(true);
        }
    }

}
