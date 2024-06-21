using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SprayAR
{
    public class InGameDebugConsole : MonoBehaviour
    {
        [SerializeField] private bool _showConsole = false;
        [SerializeField] private TextMeshProUGUI _consoleText;

        private List<string> _logMessages = new List<string>();

        private void OnEnable()
        {

            if (_showConsole)
            {
                _consoleText.gameObject.SetActive(true);
            }
            else
            {
                _consoleText.gameObject.SetActive(false);
            }
            Application.logMessageReceived += HandleLog;
        }
        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        /// <summary>
        /// Handles the logging of messages to the in-game debug console.
        /// </summary>
        /// <param name="condition">The condition or message to be logged.</param>
        /// <param name="stackTrace">The stack trace associated with the log message.</param>
        /// <param name="type">The type of log message (e.g., error, warning, etc.).</param>
        private void HandleLog(string condition, string stackTrace, LogType type)
        {
            string logMessage = $"{type}: {condition}";
            _logMessages.Add(logMessage);
            UpdateConsoleText();
        }

        /// <summary>
        /// Updates the console text by joining the log messages with newlines.
        /// </summary>
        private void UpdateConsoleText()
        {
            if (_logMessages.Count > 20)
            {
                _logMessages.RemoveAt(0);
            }
            _consoleText.text = string.Join("\n", _logMessages);
        }

    }
}
