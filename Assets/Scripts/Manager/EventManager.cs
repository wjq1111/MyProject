using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

public enum EventId
{
    Invalid,
    StartGame,
    LoadGame,

    // UI event callback
    OnClickStartGameButton,
    OnClickButton1,
    OnClickButton2,
}

public class EventManager : Singleton<EventManager>
{
    public delegate void OnEventHandler();
    public Dictionary<EventId, List<OnEventHandler>> eventHandlerDict;
    public override void Init()
    {
        eventHandlerDict = new Dictionary<EventId, List<OnEventHandler>>();
    }

    public void UnInit()
    {
        eventHandlerDict.Clear();
        eventHandlerDict = null;
    }

    public void AddEventListener(EventId eventID, OnEventHandler onEventHandler)
    {
        if (eventID == EventId.Invalid || onEventHandler == null)
        {
            return;
        }

        if (eventHandlerDict.ContainsKey(eventID))
        {
            List<OnEventHandler> list = eventHandlerDict[eventID];
            list.Remove(onEventHandler);
            list.Add(onEventHandler);
        }
        else
        {
            List<OnEventHandler> list = new List<OnEventHandler>();
            list.Add(onEventHandler);
            eventHandlerDict[eventID] = list;
        }
        //Debug.Log("Add event listener:" + eventID.ToString());
    }

    public void RemoveEventListener(EventId eventID, OnEventHandler onEventHandler)
    {
        if (eventID == EventId.Invalid || onEventHandler == null)
        {
            return;
        }

        if (eventHandlerDict.ContainsKey(eventID) && eventHandlerDict[eventID] != null)
        {
            List<OnEventHandler> list = eventHandlerDict[eventID];
            list.Remove(onEventHandler);
        }
        //Debug.Log("Remove event listener:" + eventID.ToString());
    }

    public void DispatchEvent(EventId eventID)
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
                handler();
            }
        }
    }
}
