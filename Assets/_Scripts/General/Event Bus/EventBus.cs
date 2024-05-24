using System.Collections.Generic;
using UnityEngine;

// An implementation of the event bus pattern. Reference to this specific implementation can be found on git-amend's youtube channel: https://www.youtube.com/@git-amend


namespace SprayAR.General
{
    /// <summary>
    /// Represents a generic event bus that allows registering, unregistering, and raising events of type T.
    /// </summary>
    /// <typeparam name="T">The type of event to be handled.</typeparam>
    public static class EventBus<T> where T : IEvent
    {
        static HashSet<IEventBinding<T>> _bindings = new();

        public static void Register(EventBinding<T> binding) => _bindings.Add(binding);

        public static void Unregister(EventBinding<T> binding) => _bindings.Remove(binding);

        /// <summary>
        /// Raises an event by invoking all registered event handlers.
        /// </summary>
        /// <typeparam name="T">The type of the event.</typeparam>
        /// <param name="event">The event to raise.</param>
        public static void Raise(T @event)
        {
            var bindingsCopy = new HashSet<IEventBinding<T>>(_bindings);
            foreach (var binding in bindingsCopy)
            {
                binding.OnEvent.Invoke(@event);
                binding.OnEventNoArgs.Invoke();
            }
        }

        static void Clear()
        {
            Debug.Log($"Clearing {typeof(T).Name} event bindings.");
            _bindings.Clear();
        }
    }
}