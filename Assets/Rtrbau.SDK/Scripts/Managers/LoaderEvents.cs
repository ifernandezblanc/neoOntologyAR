/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 08/07/2019
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
    /// </summary>
    public class LoaderEvents : MonoBehaviour
    {
        private Dictionary<string, Action<OntologyElement>> downloadElementsDictionary;
        private Dictionary<string, Action<OntologyUpload>> uploadElementsDictionary;
        private Dictionary<string, Action<OntologyDistance>> downloadDistancesDictionary;
        private Dictionary<string, Action<RtrbauFile>> downloadFilesDictionary;

        private static LoaderEvents loaderEventsManager;

        public static LoaderEvents instance
        {
            get
            {
                if (!loaderEventsManager)
                {
                    loaderEventsManager = FindObjectOfType(typeof(LoaderEvents)) as LoaderEvents;

                    if (!loaderEventsManager)
                    {
                        Debug.LogError("There needs to be a LoaderEvents script in the scene.");
                    }
                    else
                    {
                        loaderEventsManager.Initialise();
                    }

                } else { }

                return loaderEventsManager;
            }
        }

        #region CLASS_METHODS
        void Initialise()
        {
            if (downloadElementsDictionary == null)
            {
                downloadElementsDictionary = new Dictionary<string, Action<OntologyElement>>();
            } else { }

            if (uploadElementsDictionary == null)
            {
                uploadElementsDictionary = new Dictionary<string, Action<OntologyUpload>>();
            } else { }

            if (downloadDistancesDictionary == null)
            {
                downloadDistancesDictionary = new Dictionary<string, Action<OntologyDistance>>();
            } else { }

            if (downloadFilesDictionary == null)
            {
                downloadFilesDictionary = new Dictionary<string, Action<RtrbauFile>>();
            } else { }
        }
        #endregion CLASS_METHODS

        #region DOWNLOAD_EVENTS
        #region ONTOLOGY_EVENTS
        public static void StartListening(string eventName, Action<OntologyElement> eventListener)
        {
            Action<OntologyElement> thisEvent = null;

            if (instance.downloadElementsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += eventListener;
                instance.downloadElementsDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += eventListener;
                instance.downloadElementsDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, Action<OntologyElement> eventListener)
        {
            if (loaderEventsManager == null) { return; }

            Action<OntologyElement> thisEvent = null;

            if (instance.downloadElementsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= eventListener;
                instance.downloadElementsDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName, OntologyElement ontElement)
        {
            Action<OntologyElement> thisEvent = null;

            if (instance.downloadElementsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(ontElement);
            }
        }
        #endregion ONTOLOGY_EVENTS

        #region DISTANCE_EVENTS
        public static void StartListening(string eventName, Action<OntologyDistance> eventListener)
        {
            Action<OntologyDistance> thisEvent = null;

            if (instance.downloadDistancesDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += eventListener;
                instance.downloadDistancesDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += eventListener;
                instance.downloadDistancesDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, Action<OntologyDistance> eventListener)
        {
            if (loaderEventsManager == null) { return; }

            Action<OntologyDistance> thisEvent = null;

            if (instance.downloadDistancesDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= eventListener;
                instance.downloadDistancesDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName, OntologyDistance ontDistance)
        {
            Action<OntologyDistance> thisEvent = null;

            if (instance.downloadDistancesDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(ontDistance);
            }
        }
        #endregion DISTANCE_EVENTS

        #region FILE_EVENTS
        public static void StartListening(string eventName, Action<RtrbauFile> eventListener)
        {
            Action<RtrbauFile> thisEvent = null;

            if (instance.downloadFilesDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += eventListener;
                instance.downloadFilesDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += eventListener;
                instance.downloadFilesDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, Action<RtrbauFile> eventListener)
        {
            if (loaderEventsManager == null) { return; }

            Action<RtrbauFile> thisEvent = null;

            if (instance.downloadFilesDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= eventListener;
                instance.downloadFilesDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName, RtrbauFile fileElement)
        {
            Action<RtrbauFile> thisEvent = null;

            if (instance.downloadFilesDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(fileElement);
            }
        }
        #endregion FILE_EVENTS
        #endregion DOWNLOAD_EVENTS

        #region UPLOAD_EVENTS
        #region ONTOLOGY_EVENTS
        public static void StartListening(string eventName, Action<OntologyUpload> eventListener)
        {
            Action<OntologyUpload> thisEvent = null;

            if (instance.uploadElementsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += eventListener;
                instance.uploadElementsDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += eventListener;
                instance.uploadElementsDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, Action<OntologyUpload> eventListener)
        {
            if (loaderEventsManager == null) { return; }

            Action<OntologyUpload> thisEvent = null;

            if (instance.uploadElementsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= eventListener;
                instance.uploadElementsDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName, OntologyUpload ontElement)
        {
            Action<OntologyUpload> thisEvent = null;

            if (instance.uploadElementsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(ontElement);
            }
        }
        #endregion ONTOLOGY_EVENTS

        #region FILE_EVENTS
        #endregion FILE_EVENTS
        #endregion UPLOAD_EVENTS
    }
}
