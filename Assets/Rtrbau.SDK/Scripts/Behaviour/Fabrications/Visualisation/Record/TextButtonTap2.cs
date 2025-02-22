﻿/*==============================================================================
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
    public class TextButtonTap2 : MonoBehaviour, IFabricationable, IVisualisable, IRecordable
    {
        #region INITIALISATION_VARIABLES
        public AssetVisualiser visualiser;
        public RtrbauFabrication data;
        public Transform element;
        public Transform scale;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public string attributeDateTimeValue;
        #endregion CLASS_VARIABLES

        #region FACETS_VARIABLES
        #endregion FACETS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro fabricationText;
        public MeshRenderer fabricationSeenPanel;
        public MeshRenderer fabricationReportedPanel;
        public Material fabricationReportedMaterial;
        public Renderer fabricationBounds;
        public GameObject recordTimeButton;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        private bool fabricationCreated;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Start()
        {
            if (fabricationText == null || fabricationSeenPanel == null || fabricationReportedPanel == null || fabricationReportedMaterial == null || fabricationBounds == null || recordTimeButton == null)
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
            attributeDateTimeValue = null;
            fabricationCreated = false;
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
            DataFacet textfacet4 = DataFormats.TextButtonTap2.formatFacets[0];
            RtrbauAttribute attribute;

            // Check data received meets fabrication requirements
            if (data.fabricationData.TryGetValue(textfacet4, out attribute))
            {
                // Assign fabrication to attributeName
                fabricationText.text = Parser.ParseNamingOntologyAttribute(attribute.attributeName.Name(), element.GetComponent<ElementReport>().classElement.entity.Name());
                // Initialise record time button
                recordTimeButton.GetComponent<RecordTimeButton>().Initialise(RecordTime);
                // Check fabrication creation as true
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
            DataFacet textfacet4 = DataFormats.TextButtonTap2.formatFacets[0];
            RtrbauAttribute attribute;

            // Check data received meets fabrication requirements
            if (data.fabricationData.TryGetValue(textfacet4, out attribute))
            {
                // Update attributeValue assigned automatically when fabrication created (InferFromText)
                // It could not be done before for attributes values re-initialisation after fabrications creation
                // This assigns to RtrbauElement from ElementReport through RtrbauFabrication
                attribute.attributeValue = attributeDateTimeValue;
                // Change button colour for user confirmation
                fabricationReportedPanel.material = fabricationReportedMaterial;
                // Check if all attribute values have been recorded
                // If true, then ElementReport will input reported element into report
                // If true, then ElementReport will change colour to reported
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

            if (fabricationCreated == true && recordTimeButton.activeSelf == false)
            {
                recordTimeButton.SetActive(true);
            }
            else { }
        }

        /// <summary>
        /// Deactivates record buttons.
        /// It is also called by <see cref="ElementReport"/> to deactivate record buttons when others are to become active.
        /// </summary>
        public void DeactivateRecords()
        {
            if (fabricationCreated == true && recordTimeButton.activeSelf == true)
            {
                recordTimeButton.SetActive(false);
            }
            else { }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ActivateReporting()
        {
            /// Fabrication reporting is managed by <see cref="RecordTimeButton"/>.
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="forcedReporting"></param>
        public void DeactivateReporting(bool forcedReporting)
        {
            if (attributeDateTimeValue != null || forcedReporting == true)
            {
                recordTimeButton.GetComponent<RecordTimeButton>().DeactivateReporting();
            }
            else
            {
                throw new ArgumentException("TextButtonTap2::DeactivateReporting: this function should not be accesed before an individual is nominated.");
            }
        }
        #endregion IRECORDABLE_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        void RecordTime(DateTimeOffset timeRecorded)
        {
            // Parse DateTime recorded as string
            attributeDateTimeValue = Parser.ParseNamingDateTimeXSD(timeRecorded);
            // Call to record attribute
            OnNextVisualisation();
        }
        #endregion PRIVATE

        #region PUBLIC
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}
