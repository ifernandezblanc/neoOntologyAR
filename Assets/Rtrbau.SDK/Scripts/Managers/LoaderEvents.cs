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
        #region CLASS_MEMBERS
        private Dictionary<string, Action<OntologyElement>> downloadElementsDictionary;
        private Dictionary<string, Action<OntologyElementUpload>> uploadElementsDictionary;
        private Dictionary<string, Action<OntologyDistance>> downloadDistancesDictionary;
        private Dictionary<string, Action<OntologyFile>> downloadFilesDictionary;
        private Dictionary<string, Action<OntologyFileUpload>> uploadFilesDictionary;

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
                        Debug.LogError("LoaderEvents::Instance: There needs to be a LoaderEvents script in the scene.");
                    }
                    else
                    {
                        loaderEventsManager.Initialise();
                    }

                } else { }

                return loaderEventsManager;
            }
        }
        #endregion CLASS_MEMBERS

        #region CLASS_METHODS
        #region PRIVATE
        void Initialise()
        {
            if (downloadElementsDictionary == null)
            {
                downloadElementsDictionary = new Dictionary<string, Action<OntologyElement>>();
            } 
            else { }

            if (uploadElementsDictionary == null)
            {
                uploadElementsDictionary = new Dictionary<string, Action<OntologyElementUpload>>();
            } 
            else { }

            if (downloadDistancesDictionary == null)
            {
                downloadDistancesDictionary = new Dictionary<string, Action<OntologyDistance>>();
            } 
            else { }

            if (downloadFilesDictionary == null)
            {
                downloadFilesDictionary = new Dictionary<string, Action<OntologyFile>>();
            } 
            else { }

            if (uploadFilesDictionary == null)
            {
                uploadFilesDictionary = new Dictionary<string, Action<OntologyFileUpload>>();
            }
            else { }
        }
        #endregion PRIVATE

        #region PUBLIC
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
        public static void StartListening(string eventName, Action<OntologyFile> eventListener)
        {
            Action<OntologyFile> thisEvent = null;

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

        public static void StopListening(string eventName, Action<OntologyFile> eventListener)
        {
            if (loaderEventsManager == null) { return; }

            Action<OntologyFile> thisEvent = null;

            if (instance.downloadFilesDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= eventListener;
                instance.downloadFilesDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName, OntologyFile fileElement)
        {
            Action<OntologyFile> thisEvent = null;

            if (instance.downloadFilesDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(fileElement);
            }
        }
        #endregion FILE_EVENTS
        #endregion DOWNLOAD_EVENTS

        #region UPLOAD_EVENTS
        #region ONTOLOGY_EVENTS
        public static void StartListening(string eventName, Action<OntologyElementUpload> eventListener)
        {
            Action<OntologyElementUpload> thisEvent = null;

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

        public static void StopListening(string eventName, Action<OntologyElementUpload> eventListener)
        {
            if (loaderEventsManager == null) { return; }

            Action<OntologyElementUpload> thisEvent = null;

            if (instance.uploadElementsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= eventListener;
                instance.uploadElementsDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName, OntologyElementUpload ontElement)
        {
            Action<OntologyElementUpload> thisEvent = null;

            if (instance.uploadElementsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(ontElement);
            }
        }
        #endregion ONTOLOGY_EVENTS

        #region FILE_EVENTS
        public static void StartListening(string eventName, Action<OntologyFileUpload> eventListener)
        {
            Action<OntologyFileUpload> thisEvent = null;

            if (instance.uploadFilesDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += eventListener;
                instance.uploadFilesDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += eventListener;
                instance.uploadFilesDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, Action<OntologyFileUpload> eventListener)
        {
            if (loaderEventsManager == null) { return; }

            Action<OntologyFileUpload> thisEvent = null;

            if (instance.uploadFilesDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= eventListener;
                instance.uploadFilesDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName, OntologyFileUpload ontElement)
        {
            Action<OntologyFileUpload> thisEvent = null;

            if (instance.uploadFilesDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(ontElement);
            }
        }
        #endregion FILE_EVENTS
        #endregion UPLOAD_EVENTS
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}
