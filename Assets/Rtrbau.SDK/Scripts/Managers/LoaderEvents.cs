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
        private Dictionary<string, Action<OntologyElement>> elementsDictionary;
        private Dictionary<string, Action<OntologyDistance>> distancesDictionary;
        private Dictionary<string, Action<RtrbauFile>> filesDictionary;

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

        void Initialise()
        {
            if (elementsDictionary == null)
            {
                elementsDictionary = new Dictionary<string, Action<OntologyElement>>();
            } else { }

            if (distancesDictionary == null)
            {
                distancesDictionary = new Dictionary<string, Action<OntologyDistance>>();
            } else { }

            if (filesDictionary == null)
            {
                filesDictionary = new Dictionary<string, Action<RtrbauFile>>();
            }
            else { }
        }

        #region ONTOLOGY_EVENTS
        public static void StartListening(string eventName, Action<OntologyElement> eventListener)
        {
            Action<OntologyElement> thisEvent = null;

            if (instance.elementsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += eventListener;
                instance.elementsDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += eventListener;
                instance.elementsDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, Action<OntologyElement> eventListener)
        {
            if (loaderEventsManager == null) { return; }

            Action<OntologyElement> thisEvent = null;

            if (instance.elementsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= eventListener;
                instance.elementsDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName, OntologyElement ontElement)
        {
            Action<OntologyElement> thisEvent = null;

            if (instance.elementsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(ontElement);
            }
        }
        #endregion ONTOLOGY_EVENTS

        #region DISTANCE_EVENTS
        public static void StartListening(string eventName, Action<OntologyDistance> eventListener)
        {
            Action<OntologyDistance> thisEvent = null;

            if (instance.distancesDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += eventListener;
                instance.distancesDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += eventListener;
                instance.distancesDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, Action<OntologyDistance> eventListener)
        {
            if (loaderEventsManager == null) { return; }

            Action<OntologyDistance> thisEvent = null;

            if (instance.distancesDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= eventListener;
                instance.distancesDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName, OntologyDistance ontDistance)
        {
            Action<OntologyDistance> thisEvent = null;

            if (instance.distancesDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(ontDistance);
            }
        }
        #endregion DISTANCE_EVENTS

        #region FILE_EVENTS
        public static void StartListening(string eventName, Action<RtrbauFile> eventListener)
        {
            Action<RtrbauFile> thisEvent = null;

            if (instance.filesDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += eventListener;
                instance.filesDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += eventListener;
                instance.filesDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, Action<RtrbauFile> eventListener)
        {
            if (loaderEventsManager == null) { return; }

            Action<RtrbauFile> thisEvent = null;

            if (instance.filesDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= eventListener;
                instance.filesDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName, RtrbauFile fileElement)
        {
            Action<RtrbauFile> thisEvent = null;

            if (instance.filesDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(fileElement);
            }
        }
        #endregion FILE_EVENTS

    }
}
