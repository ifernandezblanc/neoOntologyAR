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
using TMPro;
#endregion NAMESPACES

namespace Rtrbau
{
    public class RecordKeyboardButton : MonoBehaviour, IVisualisable
    {
        #region INITIALISATION_VARIABLES
        public Action<string> recordText;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro recordButtonText;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Start()
        {
            if (recordButtonText == null)
            {
                throw new ArgumentException("NominateButton::Start: Script requires some prefabs to work.");
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
        public void Initialise(Action<string> textRecord)
        {
            if (textRecord != null)
            {
                recordText = textRecord;
            }
            else
            {
                recordText = null;
                throw new ArgumentException("RecordKeyboardButton::Initialise: No action to record text has been declared.");
            }
        }
        /// <summary>
        /// Returns user recorded text.
        /// </summary>
        /// <returns>Returns <see cref="string"/> if text recorded, otherwise returns <see cref="null"/>.</returns>
        //public string ReturnAttributeValue()
        //{
        //    if (recordButtonText.text != null)
        //    {
        //        return recordButtonText.text;
        //    }
        //    else { return null; }
        //}

        /// <summary>
        /// Invokes action to record text inputed from keyboard if distinct to null.
        /// </summary>
        public void RecordKeyboardText()
        {
            if (recordText != null)
            {
                if (recordButtonText.text != "Keyboard not supported" || recordButtonText.text != "Focus to open keyboard")
                {
                    recordText.Invoke(recordButtonText.text);
                }
                else
                {
                    recordText.Invoke(null);
                }
            }
            else { }
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}
