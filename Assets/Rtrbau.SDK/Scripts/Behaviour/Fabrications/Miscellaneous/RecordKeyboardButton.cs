/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 25/11/2019
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
#endregion NAMESPACES

namespace Rtrbau
{
    public class RecordKeyboardButton : MonoBehaviour, IVisualisable
    {
        #region INITIALISATION_VARIABLES
        public Action<string> recordText;
        public TouchScreenKeyboardType recordType;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public UnityAction report;
        public UnityAction open;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro recordedText;
        public TextMeshPro clickingText;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        private bool buttonCreated;
        private bool reportingActive;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Start()
        {
            if (recordedText == null || clickingText == null)
            {
                throw new ArgumentException("RecordKeyboardButton::Start: Script requires some prefabs to work.");
            }
            else { }
        }

        void OnDestroy() { DestroyIt(); }
        #endregion MONOBEHAVIOUR_METHODS

        #region IVISUALISABLE_METHODS
        public void LocateIt()
        {
            /// Fabrication location is managed by <see cref="IRecordable"/>.
        }

        public void ActivateIt()
        {
            /// Fabrication activation is managed by <see cref="IRecordable"/>.
        }

        public void DestroyIt()
        {
            Destroy(this.gameObject);
        }

        public void ModifyMaterial(Material material)
        {
            /// Fabrication material modification is managed by <see cref="IRecordable"/>.
        }
        #endregion IVISUALISABLE_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        #endregion PRIVATE

        #region PUBLIC
        /// <summary>
        /// Initialises RecordKeyboardButton, attaches action to submit recorded text.
        /// </summary>
        public void Initialise(Action<string> textRecord, TouchScreenKeyboardType typeRecord)
        {
            if (textRecord != null)
            {
                recordText = textRecord;
                recordType = typeRecord;
                report = RecordKeyboardText;
                open = OpenKeyboard;
                buttonCreated = true;
                reportingActive = false;
                ActivateReporting();
            }
            else
            {
                recordText = null;
                throw new ArgumentException("RecordKeyboardButton::Initialise: No action to record text has been declared.");
            }
        }
        
        /// <summary>
        /// Invokes action to record text inputed from keyboard if distinct to null.
        /// </summary>
        public void RecordKeyboardText()
        {
            if (recordText != null)
            {
                if (recordedText.text != "Keyboard not supported" || recordedText.text != "Focus to open keyboard")
                {
                    // Invoke record action
                    recordText.Invoke(recordedText.text);
                }
                else
                {
                    // Invoke record action
                    recordText.Invoke(null);
                }
            }
            else { }
            // Deactivate loading plate
            this.gameObject.GetComponentInParent<IElementable>().DeactivateLoadingPlate();
            // Provide instructions for user to open keyboard
            clickingText.text = "Look up to open keyboard";
        }

        /// <summary>
        /// Calls RtrbauKeyboard to open system keyboard
        /// </summary>
        public void OpenKeyboard()
        {
            if (reportingActive == true)
            {
                // Activate Rtrbau keyboard
                RtrbauKeyboard.instance.OpenRtrbauKeyboard(recordType, recordedText);
                // Activate loading plate
                this.gameObject.GetComponentInParent<IElementable>().ActivateLoadingPlate();
                // Provide instruction for user to click button
                clickingText.text = "Click to record";
            }
            else { }
        }

        public void ActivateReporting() 
        {
            if (buttonCreated == true)
            {
                this.gameObject.GetComponent<Interactable>().OnClick.AddListener(report);
                reportingActive = true;
            }
            else { }
        }

        public void DeactivateReporting()
        {
            if (buttonCreated == true)
            {
                this.gameObject.GetComponent<Interactable>().OnClick.RemoveListener(report);
                reportingActive = false;
            }
             else { }
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}
