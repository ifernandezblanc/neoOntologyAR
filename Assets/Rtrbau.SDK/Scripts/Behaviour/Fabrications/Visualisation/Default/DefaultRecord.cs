﻿/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 04/11/2019
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
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class DefaultRecord : MonoBehaviour, IFabricationable, IVisualisable, IRecordable
    {
        #region INITIALISATION_VARIABLES
        public AssetVisualiser visualiser;
        public RtrbauFabrication data;
        public Transform element;
        public Transform scale;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public string attributeText;
        #endregion CLASS_VARIABLES

        #region FACETS_VARIABLES
        #endregion FACETS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro fabricationText;
        public MeshRenderer fabricationSeenPanel;
        public MeshRenderer fabricationReportedPanel;
        public Material fabricationReportedMaterial;
        public Renderer fabricationBounds;
        public GameObject recordKeyboardButton;
        public GameObject recordedKeyboardText;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        private bool fabricationCreated;
        private bool reportingActive;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Start()
        {
            if (fabricationText == null || fabricationSeenPanel == null || fabricationReportedPanel == null || fabricationReportedMaterial == null || fabricationBounds == null || recordKeyboardButton == null || recordedKeyboardText == null)
            {
                throw new ArgumentException("DefaultRecord::Start: Script requires some prefabs to work.");
            }
        }

        void Update() { }

        void OnEnable() { }

        void OnDisable() { }

        void OnDestroy() { DestroyIt(); }
        #endregion MONOBEHVAIOUR_METHODS

        #region IFABRICATIONABLE_METHODS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetVisualiser"></param>
        /// <param name="fabrication"></param>
        /// <param name="elementParent"></param>
        /// <param name="fabricationParent"></param>
        public void Initialise(AssetVisualiser assetVisualiser, RtrbauFabrication fabrication, Transform elementParent, Transform fabricationParent)
        {
            visualiser = assetVisualiser;
            data = fabrication;
            element = elementParent;
            scale = fabricationParent;
            attributeText = null;
            fabricationCreated = false;
            reportingActive = false;
            recordKeyboardButton.GetComponent<RecordKeyboardButton>().Initialise(RecordText,TouchScreenKeyboardType.Default);
            Scale();
            InferFromText();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Scale()
        {
            float sX = this.transform.localScale.x / scale.transform.localScale.x;
            float sY = this.transform.localScale.y / scale.transform.localScale.y;
            float sZ = this.transform.localScale.z / scale.transform.localScale.z;

            this.transform.localScale = new Vector3(sX, sY, sZ);
        }

        /// <summary>
        /// 
        /// </summary>
        public void InferFromText()
        {
            DataFacet textfacet0 = DataFormats.DefaultRecord.formatFacets[0];
            RtrbauAttribute attribute;

            // Check data received meets fabrication requirements
            if (data.fabricationData.TryGetValue(textfacet0, out attribute))
            {
                fabricationText.text = Parser.ParseNamingOntologyAttribute(attribute.attributeName.Name(), element.GetComponent<ElementReport>().classElement.entity.Name());
                fabricationCreated = true;
            }
            else
            {
                throw new ArgumentException(data.fabricationName.ToString() + "::InferFromText: cannot implement attribute received.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnNextVisualisation()
        {
            DataFacet textfacet0 = DataFormats.DefaultRecord.formatFacets[0];
            RtrbauAttribute attribute;

            // Check data received meets fabrication requirements
            if (data.fabricationData.TryGetValue(textfacet0, out attribute))
            {
                // Update attribute value according to what user recorded
                // This assigns to RtrbauElement from ElementReport through RtrbauFabrication
                // attribute.attributeValue = recordKeyboardButton.GetComponent<RecordKeyboardButton>().ReturnAttributeValue();
                attribute.attributeValue = attributeText;
                // Change button colour for user confirmation
                fabricationReportedPanel.material = fabricationReportedMaterial;
                // Check if all attribute values have been recorded
                element.gameObject.GetComponent<ElementReport>().CheckAttributesReported();
                // Deactivate record button
                // DeactivateRecords();
            }
            else { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Renderer GetRenderer() { return fabricationBounds; }
        #endregion IFABRICATIONABLE_METHODS

        #region IVISUALISABLE_METHODS
        public void LocateIt()
        {
            /// Fabrication location is managed by <see cref="ElementReport"/>.
        }

        public void ActivateIt()
        {
            /// Fabrication activation is managed by <see cref="ElementReport"/>.
        }

        public void DestroyIt()
        {
            Destroy(this.gameObject);
        }

        public void ModifyMaterial(Material material)
        {
            fabricationSeenPanel.material = material;
        }
        #endregion IVISUALISABLE_METHODS

        #region IRECORDABLE_METHODS
        /// <summary>
        /// Activates record buttons when attribute name button is <see cref="OnFocus"/>.
        /// It also triggers deactivation of other record buttons fabrications.
        /// </summary>
        public void ActivateRecords()
        {
            // Call ElementReport to deactivate buttons from other record fabrications
            element.GetComponent<ElementReport>().DeactivateRecords(this.gameObject);
            // Call ElementReport to deactivate buttons from other nominate fabrications
            element.GetComponent<ElementReport>().DeactivateNominates(null);

            if (reportingActive == false)
            {
                if (fabricationCreated == true && recordKeyboardButton.activeSelf == false)
                {
                    recordKeyboardButton.SetActive(true);
                }
                else { }
            }
            else
            {
                if (fabricationCreated == true && recordedKeyboardText.activeSelf == false)
                {
                    recordedKeyboardText.SetActive(true);
                }
                else { }
            }
        }

        /// <summary>
        /// Deactivates record buttons.
        /// It is also called by <see cref="ElementReport"/> to deactivate record buttons when others are to become active.
        /// </summary>
        public void DeactivateRecords()
        {
            if (reportingActive == false)
            {
                if (fabricationCreated == true && recordKeyboardButton.activeSelf == true)
                {
                    recordKeyboardButton.SetActive(false);
                }
                else { }
            }
            else
            {
                if (fabricationCreated == true && recordedKeyboardText.activeSelf == true)
                {
                    recordedKeyboardText.SetActive(false);
                }
                else { }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ActivateReporting()
        {
            /// Fabrication reporting is managed by <see cref="RecordKeyboardButton"/>.
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="forcedReporting"></param>
        public void DeactivateReporting(bool forcedReporting)
        {
            if (attributeText != null || forcedReporting == true)
            {
                // Set recordedKeyboardText at same state as recordDictationButton
                if (recordKeyboardButton.activeSelf == true) { recordedKeyboardText.SetActive(true); }
                else { recordedKeyboardText.SetActive(false); }
                // Set dictatedText in recordTextPanel
                recordedKeyboardText.GetComponentInChildren<TextMeshPro>().text = attributeText;
                // Deactivate reporting from RecordDictationButton and destroy
                recordKeyboardButton.GetComponent<RecordKeyboardButton>().DeactivateReporting();
                Destroy(recordKeyboardButton);
                recordKeyboardButton = null;
                // Assign reporting as completed
                reportingActive = true;
            }
            else 
            {
                throw new ArgumentException("DefaultRecord::DeactivateReporting: this function should not be accesed before an individual is nominated.");
            }
        }
        #endregion IRECORDABLE_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        #endregion PRIVATE

        #region PUBLIC
        /// <summary>
        /// Activates OnNextVisualisation when recordText on attribute is received.
        /// </summary>
        /// <param name="recordedText"></param>
        public void RecordText(string recordedText)
        {
            if (recordedText != null)
            {
                // Update attribute value
                attributeText = recordedText;
                // Call to report attribute
                OnNextVisualisation();
            }
            else { }
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}
