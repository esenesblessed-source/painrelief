using UnityEngine;
using UnityEngine.EventSystems;

public class OnScreenJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform background;
    public RectTransform handle;
    public float handleRange = 50f;

    private Vector2 pointerDownPos;
    private bool isActive = false;
    private Vector2 input = Vector2.zero;

    public float Horizontal => input.x;
    public float Vertical => input.y;

    void Start()
    {
        if (background == null) background = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isActive = true;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out pointerDownPos);
        UpdateHandle(pointerDownPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isActive) return;
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out pos);
        Vector2 delta = pos - pointerDownPos;
        float max = handleRange;
        if (delta.magnitude > max) delta = delta.normalized * max;
        input = delta / max;
        UpdateHandle(pointerDownPos + delta);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isActive = false;
        input = Vector2.zero;
        if (handle != null) handle.anchoredPosition = Vector2.zero;
    }

    private void UpdateHandle(Vector2 anchoredPos)
    {
        if (handle != null) handle.anchoredPosition = anchoredPos;
    }
}
