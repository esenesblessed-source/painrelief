using UnityEngine;
using UnityEngine.EventSystems;

public class MobileUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string buttonName = "Fire"; // Fire, Aim, Jump, Run, Switch, EnterExit
    public bool useClick = false; // if true, use click instead of hold

    public void OnPointerDown(PointerEventData eventData)
    {
        if (useClick)
        {
            MobileInput.Instance?.ButtonClick(buttonName);
        }
        else
        {
            MobileInput.Instance?.ButtonDown(buttonName);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!useClick)
            MobileInput.Instance?.ButtonUp(buttonName);
    }
}
