using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventScript : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    public EventId onClickEventId;
    public EventId onDownEventId;
    public bool eventEnabled = true;

    private void Awake()
    {
        // Ĭ���ǿ����ģ�Ҫ������Ҫ�ڴ��������
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
        EventManager.Instance.DispatchEvent(onClickEventId);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (!eventEnabled || onDownEventId == EventId.Invalid)
        {
            return;
        }
        EventManager.Instance.DispatchEvent(onDownEventId);
    }
}
