using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventScript : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public EventId onClickEventId;
    public EventId onDownEventId;
    public EventId onEnterEventId;
    public EventId onExitEventId;

    public bool eventEnabled = true;
    public UIEventParams eventParams;

    private void Awake()
    {
        // 默认是开启的，要禁用需要在代码里禁用
        eventEnabled = true;
    }

    public UIEventScript(EventId name)
    {
        onClickEventId = name;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!eventEnabled || onClickEventId == EventId.Invalid)
        {
            return;
        }
        EventManager.Instance.DispatchEvent(onClickEventId, eventParams);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (!eventEnabled || onDownEventId == EventId.Invalid)
        {
            return;
        }
        EventManager.Instance.DispatchEvent(onDownEventId, eventParams);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (!eventEnabled || onEnterEventId == EventId.Invalid)
        {
            return;
        }
        EventManager.Instance.DispatchEvent(onEnterEventId, eventParams);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (!eventEnabled || onExitEventId == EventId.Invalid)
        {
            return;
        }
        EventManager.Instance.DispatchEvent(onExitEventId, eventParams);
    }
}
