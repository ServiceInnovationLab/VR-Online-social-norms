using UnityEngine;
using VRTK;

public class PauseOnAppButton : MonoBehaviour
{

    void Start()
    {
        GetComponentInChildren<VRTK_ControllerEvents>().ButtonTwoPressed += (s, a) =>
        {
            Debug.Break();
        };
    }

}
