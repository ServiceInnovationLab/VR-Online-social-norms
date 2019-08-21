using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, Action> eventDictionary;
    private Dictionary<string, Action<IEventArgs>> eventArgsDictionary;

    private static EventManager eventManager;

    public static EventManager Instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    var obj = new GameObject("EventManager");
                    eventManager = obj.AddComponent<EventManager>();
                    Debug.Log("Event Manager automatically created.");
                }

                eventManager.Init();
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action>();
        }

        if (eventArgsDictionary == null)
        {
            eventArgsDictionary = new Dictionary<string, Action<IEventArgs>>();
        }
    }

    public static void StartListening(string eventName, Action listener)
    {
        Action thisEvent;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Add more event to the existing one
            thisEvent += listener;

            //Update the Dictionary
            Instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            //Add event to the Dictionary for the first time
            thisEvent += listener;
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StartListening(string eventName, Action<IEventArgs> listener)
    {
        Action<IEventArgs> thisEvent;
        if (Instance.eventArgsDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Add more event to the existing one
            thisEvent += listener;

            //Update the Dictionary
            Instance.eventArgsDictionary[eventName] = thisEvent;
        }
        else
        {
            //Add event to the Dictionary for the first time
            thisEvent += listener;
            Instance.eventArgsDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, Action listener)
    {
        if (eventManager == null) return;
        Action thisEvent;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Remove event from the existing one
            thisEvent -= listener;

            //Update the Dictionary
            Instance.eventDictionary[eventName] = thisEvent;
        }
    }

    public static void StopListening(string eventName, Action<IEventArgs> listener)
    {
        if (eventManager == null) return;
        Action<IEventArgs> thisEvent;
        if (Instance.eventArgsDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Remove event from the existing one
            thisEvent -= listener;

            //Update the Dictionary
            Instance.eventArgsDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(string eventName)
    {
        Action thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke();
        }
    }

    public static void TriggerEvent(string eventName, IEventArgs eventArgs)
    {
        Action<IEventArgs> thisEvent = null;
        if (Instance.eventArgsDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke(eventArgs);
        }
    }
}