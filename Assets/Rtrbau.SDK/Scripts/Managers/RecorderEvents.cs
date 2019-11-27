/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 26/11/2019
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System.Collections.Generic;
using UnityEngine;
using System;
#endregion NAMESPACES

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// https://learn.unity.com/tutorial/create-a-simple-messaging-system-with-events
    /// </summary>
    public class RecorderEvents : MonoBehaviour
    {
        #region CLASS_MEMBERS
        private Dictionary<string, Action<OntologyFile>> imageRecordsDictionary;

        private static RecorderEvents recorderEventsManager;

        public static RecorderEvents instance
        {
            get
            {
                if (!recorderEventsManager)
                {
                    recorderEventsManager = FindObjectOfType(typeof(RecorderEvents)) as RecorderEvents;

                    if (!recorderEventsManager)
                    {
                        Debug.LogError("RecorderEvents::Instance: There needs to be a RecorderEvents script in the scene.");
                    }
                    else
                    {
                        recorderEventsManager.Initialise();
                    }

                }
                else { }

                return recorderEventsManager;
            }
        }
        #endregion CLASS_MEMBERS

        #region CLASS_METHODS
        #region PRIVATE
        void Initialise()
        {
            if (imageRecordsDictionary == null)
            {
                imageRecordsDictionary = new Dictionary<string, Action<OntologyFile>>();
            }
            else { }
        }
        #endregion PRIVATE

        #region PUBLIC
        #region AUDIO_EVENTS

        #endregion AUDIO_EVENTS

        #region IMAGE_EVENTS
        public static void StartListening(string eventName, Action<OntologyFile> eventListener)
        {
            Action<OntologyFile> thisEvent = null;

            if (instance.imageRecordsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += eventListener;
                instance.imageRecordsDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += eventListener;
                instance.imageRecordsDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, Action<OntologyFile> eventListener)
        {
            if (recorderEventsManager == null) { return; }

            Action<OntologyFile> thisEvent = null;

            if (instance.imageRecordsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= eventListener;
                instance.imageRecordsDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName, OntologyFile rtrbauFile)
        {
            Action<OntologyFile> thisEvent = null;

            if (instance.imageRecordsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(rtrbauFile);
            }
        }
        #endregion IMAGE_EVENTS

        #region VIDEO_EVENTS

        #endregion VIDEO_EVENTS
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}