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
using UnityEngine.Events;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
#endregion NAMESPACES

namespace Rtrbau
{
    public class RecordDictationButton : MonoBehaviour, IVisualisable
    {
        #region INITIALISATION_VARIABLES
        public Action<string> recordDictation;
        public Transform element;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro dictatedText;
        public DictationHandler dictationHandler;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        private bool buttonCreated;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Start()
        {
            if (dictatedText == null || dictationHandler == null)
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
        void RecordDictation(string dictatedText)
        {
            // Determine whether to invoke record action
            if (!dictatedText.Contains("<Dictation text will appear here>")|| !dictatedText.Contains("<Dictation error, please try again>") || !dictatedText.Contains("Dictation has timed out. Please try again."))
            {
                // yield return new WaitForSecondsRealtime(3f);
                recordDictation.Invoke(dictatedText);
            }
            else
            {
                recordDictation.Invoke(null);
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
            buttonCreated = true;
            ActivateReporting();
        }
        /// <summary>
        /// To trigger actions when dictationHandler starts
        /// </summary>
        public void OnDictationStarts() 
        {
            Debug.Log("RecordDictationButton::OnDictationStarts: Listener called");
            // Start dictationHandler
            dictationHandler.StartRecording();
            // Activate element loading plate
            element.GetComponent<IElementable>().ActivateLoadingPlate();
        }
        /// <summary>
        /// To trigger actions when dictationHandler ends
        /// </summary>
        public void OnDictationEnds(string dictatedText) 
        {
            Debug.Log("RecordDictationButton::OnDictationEnds: Listener called");
            // Stop dictationHandler
            dictationHandler.StopRecording();
            // Activate record action
            RecordDictation(dictatedText);
            // Deactivate element loading plate
            element.GetComponent<IElementable>().DeactivateLoadingPlate();
        }

        public void ActivateReporting()
        {
            if (buttonCreated == true)
            {
                this.gameObject.GetComponent<Interactable>().OnClick.AddListener(() => OnDictationStarts());
                dictationHandler.OnDictationResult.AddListener((string dictation) => OnDictationEnds(dictation));
                dictationHandler.OnDictationComplete.AddListener((string dictation) => OnDictationEnds(dictation));
                dictationHandler.OnDictationHypothesis.AddListener((string dictation) => dictatedText.text = dictation);
                dictationHandler.OnDictationResult.AddListener((string dictation) => dictatedText.text = dictation);
                dictationHandler.OnDictationError.AddListener((string dictation) => dictatedText.text = dictation);
                dictationHandler.OnDictationComplete.AddListener((string dictation) => dictatedText.text = dictation);
            }
            else { }
        }

        public void DeactivateReporting()
        {
            if (buttonCreated == true)
            {
                Debug.Log("RecordDictationButton::DeactivateReporting");
                this.gameObject.GetComponent<Interactable>().OnClick.RemoveListener(() => OnDictationStarts());
                dictationHandler.OnDictationResult.RemoveListener((string dictation) => OnDictationEnds(dictation));
                dictationHandler.OnDictationComplete.RemoveListener((string dictation) => OnDictationEnds(dictation));
                dictationHandler.OnDictationHypothesis.RemoveListener((string dictation) => dictatedText.text = dictation);
                dictationHandler.OnDictationResult.RemoveListener((string dictation) => dictatedText.text = dictation);
                dictationHandler.OnDictationError.RemoveListener((string dictation) => dictatedText.text = dictation);
                dictationHandler.OnDictationComplete.RemoveListener((string dictation) => dictatedText.text = dictation);
            }
            else { }
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}
