using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_Pointer), typeof(VRTK_UIPointer))]
public class ShowPointerOnUiHit : MonoBehaviour
{
    VRTK_Pointer pointer;
    VRTK_UIPointer uiPointer;

    private void Awake()
    {
        pointer = GetComponent<VRTK_Pointer>();
        uiPointer = GetComponent<VRTK_UIPointer>();

        uiPointer.UIPointerElementEnter += UIPointerElementEnter;
    }

    private void UIPointerElementEnter(object sender, UIPointerEventArgs e)
    {
        pointer.holdButtonToActivate = false;
        pointer.Toggle(true);

        uiPointer.UIPointerElementEnter -= UIPointerElementEnter;
    }
}