using System;
using System.Collections.Generic;
using UnityEngine;

namespace Event
{
    public static class EventSystem
    {
        private static Dictionary<Type, List<Action<BaseEvent>>> _events;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            _events = new Dictionary<Type, List<Action<BaseEvent>>>();
        }

        public static void Subscribe<T>(Action<BaseEvent> action) where T : BaseEvent
        {
            if (_events.TryGetValue(typeof(T), out var list))
            {
                list.Add(action);
            }
            else
            {
                _events[typeof(T)] = new List<Action<BaseEvent>>();
                _events[typeof(T)].Add(action);
            }
        }

        public static void Unsubscribe<T>(Action<BaseEvent> action) where T : BaseEvent
        {
            if (_events.TryGetValue(typeof(T), out var list))
            {
                list.Remove(action);
            }
        }

        public static void Raise<T>(T data) where T : BaseEvent
        {
            if (_events.TryGetValue(typeof(T), out var list))
            {
                foreach (var action in list)
                {
                    action?.Invoke(data);
                }
            }
        }
    }
}