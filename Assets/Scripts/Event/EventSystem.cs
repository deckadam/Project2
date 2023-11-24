using System;
using System.Collections.Generic;
using UnityEngine;

namespace Event
{
    public static class EventSystem
    {
        private static Dictionary<Type, List<Action<object>>> _events;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            _events = new Dictionary<Type, List<Action<object>>>();
        }

        public static void Subscribe<T>(Action<object> action)
        {
            if (_events.TryGetValue(typeof(T), out var list))
            {
                list.Add(action);
            }
            else
            {
                _events[typeof(T)] = new List<Action<object>>();
                _events[typeof(T)].Add(action);
            }
        }

        public static void Unsubscribe<T>(Action<object> action)
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