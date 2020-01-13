/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 19/11/2019
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
using TMPro;
#endregion NAMESPACES

namespace Rtrbau
{
    public class RecordDictationButton : MonoBehaviour, IVisualisable
    {
        #region INITIALISATION_VARIABLES
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public Action<string> recordDictation;
        public Transform element;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro dictatedText;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        private bool buttonCreated;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Start()
        {
            if (dictatedText == null)
            {
                throw new ArgumentException("RecordDictationButton::Start: Script requires some prefabs to work.");
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
        /// <summary>
        /// Records dictated text after being visualised by the user.
        /// </summary>
        /// <returns>Awaits for user to visualise text dictated before recording it.</returns>
        void RecordDictation()
        {
            // Determine whether to invoke record action
            if (!dictatedText.text.Contains("<Dictation text will appear here>")|| !dictatedText.text.Contains("<Dictation error, please try again>") || !dictatedText.text.Contains("Dictation has timed out. Please try again."))
            {
                // yield return new WaitForSecondsRealtime(3f);
                recordDictation.Invoke(dictatedText.text);
            }
            else
            {
                dictatedText.text = "<Dictation error, please try again>";
            }
        }
        #endregion PRIVATE

        #region PUBLIC
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictateAction"></param>
        /// <param name="elementFabrication"></param>
        public void Initialise(Action<string> recordAction, Transform elementFabrication)
        {
            // Initialise class variables
            recordDictation = recordAction;
            element = elementFabrication;
        }
        /// <summary>
        /// To trigger actions when dictation starts
        /// </summary>
        public void OnDictationStarts() 
        { 
            // Activate element loading panel
            element.GetComponent<ElementReport>().loadingPanel.SetActive(true); 
        }
        /// <summary>
        /// To trigger actions when dictation ends
        /// </summary>
        public void OnDictationEnds() 
        {
            // Activate record action
            RecordDictation();
            // Deactivate element loading panel
            element.GetComponent<ElementReport>().loadingPanel.SetActive(false);
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}
