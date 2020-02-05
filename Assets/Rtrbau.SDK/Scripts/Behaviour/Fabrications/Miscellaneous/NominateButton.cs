/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 05/11/2019
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
using System.IO;
using Microsoft.MixedReality.Toolkit.UI;
#endregion NAMESPACES

namespace Rtrbau
{
    public class NominateButton : MonoBehaviour, IVisualisable
    {
        #region INITIALISATION_VARIABLES
        public Action<OntologyEntity, bool> nominate;
        public Action<OntologyEntity> destroy;
        public OntologyEntity individual;
        public OntologyElement range;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public UnityAction report;
        public JsonClassProperties rangeProperties;
        public string recordableText;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro buttonText;
        public MeshRenderer seenPanel;
        public MeshRenderer reportedPanel;
        public GameObject recordButton;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        private bool buttonCreated;
        private bool recordableNominate;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Start()
        {
            if (buttonText == null || seenPanel == null || reportedPanel == null || recordButton == null)
            {
                throw new ArgumentException("NominateButton::Start: Script requires some prefabs to work.");
            }
            else { recordButton.SetActive(false); }
        }

        void OnDestroy() { DestroyIt(); }
        #endregion MONOBEHAVIOUR_METHODS

        #region IVISUALISABLE_METHODS
        public void LocateIt() 
        {
            /// Fabrication location is managed by <see cref="INominatable"/>.
        }

        public void ActivateIt() 
        {
            /// Fabrication activation is managed by <see cref="INominatable"/>.
        }

        public void DestroyIt() 
        { 
            Destroy(this.gameObject);
        }

        public void ModifyMaterial(Material material)
        {
            /// Fabrication material modification is managed by <see cref="INominatable"/>.
        }
        #endregion IVISUALISABLE_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        void DownloadRangeProperties(OntologyElement rangeElement)
        {
            // Stop range class download
            LoaderEvents.StopListening(range.EventName(), DownloadRangeProperties);

            if (File.Exists(range.FilePath()))
            {
                string jsonFile = File.ReadAllText(range.FilePath());

                rangeProperties = JsonUtility.FromJson<JsonClassProperties>(jsonFile);
                // If range properties are zero, then activate nominate record button
                if (rangeProperties.ontProperties.Count == 0)
                {
                    // UPG: Only if the nominate name is new [Requires changes in server to work]
                    // UPG: For the time being when this case occurs invoke the function to destroy this nominate
                    // if (individual.Name().Contains(Parser.ParseNamingNew())) { LoadButton(true); }
                    if (individual.Name().Contains(Parser.ParseNamingNew())) { destroy.Invoke(individual); }
                    else { LoadButton(false); }
                }
                else { LoadButton(false); }
            }
            else
            {
                Debug.LogError("NominateButton::DownloadedClass: File not found: " + range.FilePath());
            }
        }

        void LoadButton(bool recordable)
        {
            if (recordable == false)
            {
                // Show individual name the button refers to
                buttonText.text = individual.Name();
                // Assig nominate as non recordable
                recordableNominate = false;
            }
            else
            {
                // Show individual name the button refers to
                buttonText.text = individual.Name();
                // Assign nominate as recordable
                recordableNominate = true;
                // Activate record button
                recordButton.SetActive(true);
                // Initialise record button
                recordButton.GetComponent<RecordKeyboardButton>().Initialise(RecordRecordableNominate, TouchScreenKeyboardType.Default);
            }

            // Assign button as created
            buttonCreated = true;
            // Assign report action
            ActivateReporting();
        }
        #endregion PRIVATE

        #region PUBLIC
        public void Initialise(Action<OntologyEntity, bool> nominateIndividual, Action<OntologyEntity> destroyIndividual, OntologyEntity relationshipValue, OntologyEntity relationshipRange)
        {
            // Assign button variables
            nominate = nominateIndividual;
            destroy = destroyIndividual;
            individual = relationshipValue;
            report = NominateIndividual;
            // To optimise speed, only check range if nominate is new
            if (individual.Name().Contains(Parser.ParseNamingNew()))
            {
                // If nominated individual is new, then download class
                range = new OntologyElement(relationshipRange.URI(), OntologyElementType.ClassProperties);
                // Start range class download
                LoaderEvents.StartListening(range.EventName(), DownloadRangeProperties);
                Loader.instance.StartOntElementDownload(range);
            }
            else
            {
                // Otherwise load button as non recordable
                LoadButton(false);
            }
        }

        public void NominateIndividual()
        {
            if (buttonCreated == true && recordableNominate == false)
            {
                // Trigger the nominate individual action for this button as non-recordable
                nominate.Invoke(individual, recordableNominate);
            }
            else if (buttonCreated == true && recordableNominate == true)
            {
                if (recordableText != null)
                {
                    // Trigger the nominate individual action for this button as recordable
                    nominate.Invoke(individual, recordableNominate);
                }
                else { }
            }
            else { }
        }

        public void ReportMaterial(Material material)
        {
            reportedPanel.material = material;
        }

        public void SeenMaterial(Material material)
        {
            seenPanel.material = material;
        }

        public void ActivateRecordButton()
        {
            if (buttonCreated == true)
            {
                if (recordableNominate == true)
                {
                    if (recordButton.activeSelf == false) { recordButton.SetActive(true); }
                    else { recordButton.SetActive(false); }
                }
            }
        }

        public void RecordRecordableNominate(string textRecordable)
        {
            if (recordableNominate == true)
            {
                recordableText = textRecordable;
                buttonText.text = textRecordable;
            }
            else { }
        }

        public void ActivateReporting()
        {
            if (buttonCreated == true)
            {
                this.gameObject.GetComponent<Interactable>().OnClick.AddListener(report);
            }
            else { }
        }

        public void DeactivateReporting()
        {
            if (buttonCreated == true)
            {
                this.gameObject.GetComponent<Interactable>().OnClick.RemoveListener(report);
            }
            else { }
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}
