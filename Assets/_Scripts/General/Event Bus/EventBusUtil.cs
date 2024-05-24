using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SprayAR.General
{
    /// <summary>
    /// Utility class for managing event buses in the application.
    /// </summary>
    public static class EventBusUtil
    {
        public static IReadOnlyList<Type> EventTypes { get; set; }
        public static IReadOnlyList<Type> EventBusTypes { get; set; }

#if UNITY_EDITOR
        public static PlayModeStateChange PlayModeState { get; set; }

        /// <summary>
        /// Initializes the editor by subscribing to the play mode state changed event.
        /// The [InitializeOnLoadMethod] attribute ensures that this method is called every time the editor is loaded or a script is recompiled.
        /// This is useful to initialize states that are necessary during editing state that also apply to play mode.
        /// </summary>
        [InitializeOnLoadMethod]
        public static void InitializeEditor()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        /// <summary>
        /// Clears the event buses when the play mode state changes to exiting play mode.
        /// </summary>
        /// <param name="state">The new play mode state.</param>
        static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            PlayModeState = state;
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                ClearAllBuses();
            }
        }
#endif

        /// <summary>
        /// Initializes the event bus utility.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            EventTypes = PredefinedAssemblyUtil.GetTypes(typeof(IEvent));
            EventBusTypes = InitializeAllBuses();
        }

        /// <summary>
        /// Initializes all event buses for the specified event types.
        /// </summary>
        /// <returns>A list of event bus types that have been initialized.</returns>
        private static List<Type> InitializeAllBuses()
        {
            List<Type> eventBusTypes = new List<Type>();

            var typedef = typeof(EventBus<>);
            foreach (var eventType in EventTypes)
            {
                var busType = typedef.MakeGenericType(eventType);
                eventBusTypes.Add(busType);
                Debug.Log($"Initialized event bus for {eventType.Name}.");
            }

            return eventBusTypes;
        }

        /// <summary>
        /// Clears all event buses.
        /// </summary>
        public static void ClearAllBuses()
        {
            Debug.Log("Clearing all event buses.");
            for (int i = 0; i < EventBusTypes.Count; i++)
            {
                var busType = EventBusTypes[i];
                var clearMethod = busType.GetMethod("Clear", BindingFlags.Static | BindingFlags.NonPublic);
                clearMethod?.Invoke(null, null);
            }
        }
    }
}