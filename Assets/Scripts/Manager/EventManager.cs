using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public delegate void OnEventHandler();
    public delegate void OnUIEventHandler(UIEventParams eventParams = new UIEventParams());
    public Dictionary<EventId, List<OnUIEventHandler>> eventHandlerDict;
    public override void Init()
    {
        eventHandlerDict = new Dictionary<EventId, List<OnUIEventHandler>>();
    }

    public void UnInit()
    {
        eventHandlerDict.Clear();
        eventHandlerDict = null;
    }

    public void AddEventListener(EventId eventID, OnUIEventHandler onEventHandler)
    {
        if (eventID == EventId.Invalid || onEventHandler == null)
        {
            return;
        }

        if (eventHandlerDict.ContainsKey(eventID))
        {
            List<OnUIEventHandler> list = eventHandlerDict[eventID];
            list.Remove(onEventHandler);
            list.Add(onEventHandler);
        }
        else
        {
            List<OnUIEventHandler> list = new List<OnUIEventHandler>();
            list.Add(onEventHandler);
            eventHandlerDict[eventID] = list;
        }
        //Debug.Log("Add event listener:" + eventID.ToString());
    }

    public void AddEventListener(EventId eventID, OnEventHandler noParams)
    {
        OnUIEventHandler onUIEventHandler = (eventParams) =>
        {
            noParams();
        };

        AddEventListener(eventID, onUIEventHandler);
    }

    public void RemoveEventListener(EventId eventID, OnUIEventHandler onEventHandler)
    {
        if (eventID == EventId.Invalid || onEventHandler == null)
        {
            return;
        }

        if (eventHandlerDict.ContainsKey(eventID) && eventHandlerDict[eventID] != null)
        {
            List<OnUIEventHandler> list = eventHandlerDict[eventID];
            list.Remove(onEventHandler);
        }
        //Debug.Log("Remove event listener:" + eventID.ToString());
    }

    public void RemoveEventListener(EventId eventID, OnEventHandler noParams)
    {
        OnUIEventHandler onUIEventHandler = (eventParams) =>
        {
            noParams();
        };

        RemoveEventListener(eventID, onUIEventHandler);
    }

    public void DispatchEvent(EventId eventID, UIEventParams eventParams = new UIEventParams())
    {
        if (eventID == EventId.Invalid)
        {
            Debug.Log("Invalid EventID!");
            return;
        }

        //Debug.Log("eventID:" + eventID);

        if (eventHandlerDict.ContainsKey(eventID))
        {
            foreach (var handler in eventHandlerDict[eventID])
            {
                handler(eventParams);
            }
        }
    }
}
