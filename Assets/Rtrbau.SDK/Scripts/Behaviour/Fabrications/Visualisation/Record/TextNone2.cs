/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 15/11/2019
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
    public class TextNone2 : MonoBehaviour, IFabricationable, IVisualisable
    {
        #region INITIALISATION_VARIABLES
        public AssetVisualiser visualiser;
        public RtrbauFabrication data;
        public Transform element;
        public Transform scale;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        #endregion CLASS_VARIABLES

        #region FACETS_VARIABLES
        #endregion FACETS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro fabricationText;
        public MeshRenderer fabricationPanel;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        private bool fabricationCreated;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Start()
        {
            if (fabricationText == null || fabricationPanel == null)
            {
                throw new ArgumentException("TextNone2::Start: Script requires some prefabs to work.");
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
            DataFacet textfacet5 = DataFormats.TextNone2.formatFacets[0];
            RtrbauAttribute attribute;

            // Check data received meets fabrication requirements
            if (data.fabricationData.TryGetValue(textfacet5, out attribute))
            {
                fabricationText.text = attribute.attributeName.Name();
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
            //DataFacet textfacet0 = DataFormats.DefaultRecord.formatFacets[0];
            //RtrbauAttribute attribute;

            //// Check data received meets fabrication requirements
            //if (data.fabricationData.TryGetValue(textfacet0, out attribute))
            //{
            //    // Update attribute value according to what user recorded
            //    // This assigns to RtrbauElement from ElementReport through RtrbauFabrication
            //    // TextMeshPro - InputField creates an extra child that is assigned
            //    attribute.attributeValue = recordButton.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;
            //    // Change button colour for user confirmation
            //    fabricationReportedPanel.material = fabricationReportedMaterial;
            //    // Check if all attribute values have been recorded
            //    // If true, then ElementReport will input reported element into report
            //    // If true, then ElementReport will change colour to reported
            //    element.gameObject.GetComponent<ElementReport>().CheckAttributesReported();
            //    // Deactivate record button
            //    DeactivateRecords();
            //}
            //else { }
        }
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
            //fabricationSeenPanel.material = material;
        }
        #endregion IVISUALISABLE_METHODS

        #region IRECORDABLE_METHODS
        /// <summary>
        /// Activates record buttons when attribute name button is <see cref="OnFocus"/>.
        /// It also triggers deactivation of other record buttons fabrications.
        /// </summary>
        public void ActivateRecords()
        {
            //// Call ElementReport to deactivate buttons from other record fabrications
            //element.GetComponent<ElementReport>().DeactivateRecords(this.gameObject);

            //if (fabricationCreated == true && recordButton.activeSelf == false)
            //{
            //    recordButton.SetActive(true);
            //}
            //else { }
        }

        /// <summary>
        /// Deactivates record buttons.
        /// It is also called by <see cref="ElementReport"/> to deactivate record buttons when others are to become active.
        /// </summary>
        public void DeactivateRecords()
        {
            //if (fabricationCreated == true && recordButton.activeSelf == true)
            //{
            //    recordButton.SetActive(false);
            //}
            //else { }
        }
        #endregion IRECORDABLE_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        #endregion PRIVATE

        #region PUBLIC
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}