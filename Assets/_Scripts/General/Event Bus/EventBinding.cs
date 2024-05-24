using System;

namespace SprayAR.General
{


    /// <summary>
    /// Represents a binding between an event and its corresponding actions.
    /// </summary>
    /// <typeparam name="T">The type of the event argument.</typeparam>
    public interface IEventBinding<T>
    {
        /// <summary>
        /// Gets or sets the action to be executed when the event occurs with the specified argument.
        /// </summary>
        Action<T> OnEvent { get; set; }

        /// <summary>
        /// Gets or sets the action to be executed when the event occurs without any arguments.
        /// </summary>
        Action OnEventNoArgs { get; set; }
    }

    /// <summary>
    /// Represents a binding between an event and its associated actions.
    /// </summary>
    /// <typeparam name="T">The type of event.</typeparam>
    public class EventBinding<T> : IEventBinding<T> where T : IEvent
    {
        Action<T> onEvent = _ => { };
        Action onEventNoArgs = () => { };

        Action<T> IEventBinding<T>.OnEvent { get => onEvent; set => onEvent = value; }

        Action IEventBinding<T>.OnEventNoArgs { get => onEventNoArgs; set => onEventNoArgs = value; }

        public EventBinding(Action<T> onEvent) => this.onEvent = onEvent;

        public EventBinding(Action onEventNoArgs) => this.onEventNoArgs = onEventNoArgs;

        public void Add(Action<T> onEvent) => this.onEvent += onEvent;
        public void Remove(Action<T> onEvent) => this.onEvent -= onEvent;

        public void Add(Action onEventNoArgs) => this.onEventNoArgs += onEventNoArgs;
        public void Remove(Action onEventNoArgs) => this.onEventNoArgs -= onEventNoArgs;

    }
}