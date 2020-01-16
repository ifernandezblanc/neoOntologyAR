/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2020 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2020 Cranfield University. All Rights Reserved.
Copyright (c) 2020 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 15/01/2020
==============================================================================*/


/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Proprietary class for debugging purposes
    /// </summary>
    public class RtrbauDebug : MonoBehaviour
    {
        #region SINGLETON_INSTANTIATION
        private static RtrbauDebug rtrbauDebug;

        public static RtrbauDebug instance
        {
            get
            {
                if (!rtrbauDebug)
                {
                    rtrbauDebug = FindObjectOfType(typeof(RtrbauDebug)) as RtrbauDebug;

                    if (!rtrbauDebug)
                    {
                        Debug.LogError("There needs to be a RtrbauDebugger script in the scene.");
                    }
                    else
                    {
                        rtrbauDebug.Initialise();
                    }
                }
                else { }

                return rtrbauDebug;
            }
        }
        #endregion SINGLETON_INSTANTIATION

        #region CLASS_MEMBERS
        private static string debugLog;
        private static string debugLogFilePath;

        #endregion CLASS_MEMBERS

        #region CLASS_EVENTS
        #endregion CLASS_EVENTS

        #region GAMEOBJECT_PREFABS
        [SerializeField]
        public TextMeshProUGUI debugField;
        #endregion GAMEOBJECT_PREFABS

        #region MONOBEHAVIOUR_METHODS
        void Awake() { }
        void Start() { }
        void Update() { }
        void OnApplicationQuit() { WriteLogFile(); }
        #endregion MONOBEHAVIOUR_METHODS

        #region INITIALISATION_METHODS
        public void Initialise()
        {
            if (debugField == null)
            {
                throw new ArgumentException("RtrbauDebugger::Initialise: Requires debugField assigned to show debug log real time");
            }
            else { }

            debugLog = Rtrbauer.instance.user.name + " Log:\n";
            debugLogFilePath = Dictionaries.logsFileDirectory + "/" + Rtrbauer.instance.user.name + ".txt";
        }
        #endregion INITIALISATION_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        private void WriteLogFile()
        {
            Debug.Log("RtrbauDebug::WriteLogFile: file path is " + debugLogFilePath);
            if (!File.Exists(debugLogFilePath))
            {
                File.WriteAllText(debugLogFilePath, debugLog);
            }
        }
        #endregion PRIVATE

        #region PUBLIC
        public void Log(string textString)
        {
            debugField.text = textString;
            debugLog += textString + "\n";
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}