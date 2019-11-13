/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 22/08/2019
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// 
    /// </summary>
    public class RtrbauerEvents : MonoBehaviour
    {
        #region EVENTS_DICTIONARIES
        #region RTRBAU
        private Dictionary<string, Action<OntologyEntity>> rtrbauerEventsDictionary;
        #endregion RTRBAU
        #region VISUALISATION
        private Dictionary<string, Action<OntologyElement, OntologyElement, RtrbauElementType>> loadElementsEventsDictionary;
        private Dictionary<string, Action<GameObject, RtrbauElementType, RtrbauElementLocation>> locateElementsEventsDictionary;
        #endregion VISUALISATION
        #endregion EVENTS_DICTIONARIES

        #region SINGLETON_INITIALISATION
        private static RtrbauerEvents rtrbauerEventsManager;
        public static RtrbauerEvents instance
        {
            get
            {
                if (!rtrbauerEventsManager)
                {
                    rtrbauerEventsManager = FindObjectOfType(typeof(RtrbauerEvents)) as RtrbauerEvents;

                    if (!rtrbauerEventsManager)
                    {
                        Debug.LogError("There needs to be one active SelectionPanelEvents script on the Panel GameObject");
                    }
                    else
                    {
                        rtrbauerEventsManager.Initialise();
                    }
                }

                return rtrbauerEventsManager;
            }
        }

        void Initialise()
        {
            if (rtrbauerEventsDictionary == null)
            { rtrbauerEventsDictionary = new Dictionary<string, Action<OntologyEntity>>(); }

            if (loadElementsEventsDictionary == null)
            { loadElementsEventsDictionary = new Dictionary<string, Action<OntologyElement, OntologyElement, RtrbauElementType>>(); }

            if (locateElementsEventsDictionary == null)
            { locateElementsEventsDictionary = new Dictionary<string, Action<GameObject, RtrbauElementType, RtrbauElementLocation>>(); }

        }
        #endregion SINGLETON_INITIALISATION

        #region RTRBAU_EVENTS
        public static void StartListening(string eventName, Action<OntologyEntity> eventListener)
        {
            Action<OntologyEntity> thisEvent = null;

            if (instance.rtrbauerEventsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += eventListener;
                instance.rtrbauerEventsDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += eventListener;
                instance.rtrbauerEventsDictionary.Add(eventName, thisEvent);
            }

        }

        public static void StopListening(string eventName, Action<OntologyEntity> eventListener)
        {
            if (rtrbauerEventsManager == null) { return; }

            Action<OntologyEntity> thisEvent = null;

            if (instance.rtrbauerEventsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= eventListener;
                instance.rtrbauerEventsDictionary[eventName] = thisEvent;
            }
            else { }

        }

        public static void TriggerEvent(OntologyEntity eventEntity)
        {
            string eventName = eventEntity.Entity();
            Action<OntologyEntity> thisEvent = null;

            if (instance.rtrbauerEventsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(eventEntity);
                /// instance.selectionEventsDictionary[eventName]();
            }
        }
        #endregion RTRBAU_EVENTS

        #region VISUALISATION_EVENTS
        #region LOADING
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventListener"></param>
        public static void StartListening(string eventName, Action<OntologyElement, OntologyElement, RtrbauElementType> eventListener)
        {
            Action<OntologyElement, OntologyElement, RtrbauElementType> thisEvent = null;

            if (instance.loadElementsEventsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += eventListener;
                instance.loadElementsEventsDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += eventListener;
                instance.loadElementsEventsDictionary.Add(eventName, thisEvent);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventListener"></param>
        public static void StopListening(string eventName, Action<OntologyElement, OntologyElement, RtrbauElementType> eventListener)
        {
            if (rtrbauerEventsManager == null) { return; }
            else { }

            Action<OntologyElement, OntologyElement, RtrbauElementType> thisEvent = null;

            if (instance.loadElementsEventsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= eventListener;
                instance.loadElementsEventsDictionary[eventName] = thisEvent;
            }
            else { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventEntity"></param>
        public static void TriggerEvent(string eventName, OntologyElement elementIndividual, OntologyElement elementClass, RtrbauElementType elementType)
        {
            Action<OntologyElement, OntologyElement, RtrbauElementType> thisEvent = null;

            if (instance.loadElementsEventsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(elementIndividual, elementClass, elementType);
            }
        }
        #endregion LOADING

        #region LOCATION
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventListener"></param>
        public static void StartListening(string eventName, Action<GameObject, RtrbauElementType, RtrbauElementLocation> eventListener)
        {
            Action<GameObject, RtrbauElementType, RtrbauElementLocation> thisEvent = null;

            if (instance.locateElementsEventsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += eventListener;
                instance.locateElementsEventsDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += eventListener;
                instance.locateElementsEventsDictionary.Add(eventName, thisEvent);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventListener"></param>
        public static void StopListening(string eventName, Action<GameObject, RtrbauElementType, RtrbauElementLocation> eventListener)
        {
            if (rtrbauerEventsManager == null) { return; }
            else { }

            Action<GameObject, RtrbauElementType, RtrbauElementLocation> thisEvent = null;

            if (instance.locateElementsEventsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= eventListener;
                instance.locateElementsEventsDictionary[eventName] = thisEvent;
            }
            else { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventEntity"></param>
        public static void TriggerEvent(string eventName, GameObject element, RtrbauElementType type, RtrbauElementLocation location)
        {
            Action<GameObject, RtrbauElementType, RtrbauElementLocation> thisEvent = null;

            if (instance.locateElementsEventsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(element, type, location);
            }
        }
        #endregion LOCATION
        #endregion VISUALISATION_EVENTS

    }
}

