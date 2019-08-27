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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public class PanellerEvents : MonoBehaviour
    {
        private Dictionary<string, Action<OntologyEntity>> panellerEventsDictionary;

        private static PanellerEvents panellerEventsManager;

        public static PanellerEvents instance
        {
            get
            {
                if (!panellerEventsManager)
                {
                    panellerEventsManager = FindObjectOfType(typeof(PanellerEvents)) as PanellerEvents;

                    if (!panellerEventsManager)
                    {
                        Debug.LogError("There needs to be one active SelectionPanelEvents script on the Panel GameObject");
                    }
                    else
                    {
                        panellerEventsManager.Initialise();
                    }
                }

                return panellerEventsManager;
            }
        }


        void Initialise()
        {
            if (panellerEventsDictionary == null)
            {
                panellerEventsDictionary = new Dictionary<string, Action<OntologyEntity>>();
            }
        }

        public static void StartListening(string eventName, Action<OntologyEntity> eventListener)
        {
            Action<OntologyEntity> thisEvent = null;

            if (instance.panellerEventsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += eventListener;
                instance.panellerEventsDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += eventListener;
                instance.panellerEventsDictionary.Add(eventName, thisEvent);
            }

        }

        public static void StopListening(string eventName, Action<OntologyEntity> eventListener)
        {
            if (panellerEventsManager == null) { return; }
            else { }

            Action<OntologyEntity> thisEvent = null;

            if (instance.panellerEventsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= eventListener;
                instance.panellerEventsDictionary[eventName] = thisEvent;
            }
            else { }

        }

        public static void TriggerEvent(string eventName, OntologyEntity eventEntity)
        {
            Action<OntologyEntity> thisEvent = null;

            if (instance.panellerEventsDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(eventEntity);
                /// instance.selectionEventsDictionary[eventName]();
            }
        }
    }
}

