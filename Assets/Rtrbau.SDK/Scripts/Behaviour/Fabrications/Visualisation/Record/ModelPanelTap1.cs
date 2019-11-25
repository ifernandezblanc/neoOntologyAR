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
using Microsoft.MixedReality.Toolkit.UI;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class ModelPanelTap1 : MonoBehaviour, IFabricationable, IVisualisable, IRecordable
    {
        #region INITIALISATION_VARIABLES
        public AssetVisualiser visualiser;
        public RtrbauFabrication data;
        public Transform element;
        public Transform scale;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public Dictionary<string, GameObject> selectableModels;
        public string selectedRecord;
        #endregion CLASS_VARIABLES

        #region FACETS_VARIABLES
        #endregion FACETS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro fabricationText;
        public MeshRenderer fabricationSeenPanel;
        public MeshRenderer fabricationReportedPanel;
        public Material fabricationSeenMaterial;
        public Material fabricationNonReportedMaterial;
        public Material fabricationReportedMaterial;
        public GameObject recordSelectButton;
        public TextMeshPro recordSelectButtonText;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        private bool fabricationCreated;
        private bool recordSelectModelsActive;
        private bool recordSelected;
        #endregion CLASS_EVENTS

        #region MONOBEHVAIOUR_METHODS
        void Start()
        {
            if (fabricationText == null || fabricationSeenPanel == null || fabricationReportedPanel == null || fabricationSeenMaterial == null || fabricationNonReportedMaterial == null || fabricationReportedMaterial == null || recordSelectButton == null || recordSelectButtonText == null)
            {
                throw new ArgumentException("DefaultNominate::Start: Script requires some prefabs to work.");
            }
            else { recordSelectButton.SetActive(false); }
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
            selectableModels = new Dictionary<string, GameObject>();
            selectedRecord = null;
            fabricationCreated = false;
            recordSelectModelsActive = false;
            recordSelected = false;
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
            DataFacet modelfacet1 = DataFormats.ModelPanelTap1.formatFacets[0];
            RtrbauAttribute attribute;

            // Check data received meets fabrication requirements
            if (data.fabricationData.TryGetValue(modelfacet1, out attribute))
            {
                // Set button name to relationship name
                fabricationText.text = attribute.attributeName.Name();
                // Create a selectable replica of each asset component
                foreach (GameObject componentModel in visualiser.manager.assetComponentsModels)
                {
                    // Instantiate component model and initialise selectable model and return name
                    KeyValuePair<string,GameObject> selectableModel = InitialiseSelectableModel(componentModel);
                    // Add to selectable models list
                    selectableModels.Add(selectableModel.Key, selectableModel.Value);
                }
                // Deactivate record select button
                recordSelectButton.SetActive(false);
                // Set fabrication as created
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
            // Check if a record has been selected
            if (recordSelected == true)
            {
                DataFacet modelfacet1 = DataFormats.ModelPanelTap1.formatFacets[0];
                RtrbauAttribute attribute;

                // Check data received meets fabrication requirements
                if (data.fabricationData.TryGetValue(modelfacet1, out attribute))
                {
                    // Update attributeValue and modify attributeRange and attributeType as pre-modified by RtrbauElement
                    // It could not be done before for attributes values re-initialisation after fabrications creation
                    // This assigns to RtrbauElement from ElementReport through RtrbauFabrication
                    attribute.attributeRange = new OntologyEntity(Rtrbauer.instance.component.URI());
                    attribute.attributeValue = attribute.attributeRange.Ontology().URI() + selectedRecord;
                    attribute.attributeType = new OntologyEntity(Rtrbauer.instance.owl.URI() + "ObjectProperty");
                    // Change button colour for user confirmation
                    fabricationReportedPanel.material = fabricationReportedMaterial;
                    // Check if all attribute values have been recorded
                    // If true, then ElementReport will input reported element into report
                    // If true, then ElementReport will change colour to reported
                    element.gameObject.GetComponent<ElementReport>().CheckAttributesReported();
                    // Deactivate record select buttons
                    DeactivateRecords();
                }
                else { }
            }
            else { }
        }
        #endregion IFABRICATIONABLE_METHODS

        #region IVISUALISABLE_METHODS
        public void LocateIt()
        {
            /// Fabrication location is managed by <see cref="ElementConsult"/>.
        }

        public void ActivateIt()
        {
            /// Fabrication activation is managed by <see cref="ElementConsult"/>.
        }

        public void DestroyIt()
        {
            // Destroy this button and its children
            Destroy(this.gameObject);
            // Destroy unparented model fabrications
            foreach (KeyValuePair<string, GameObject> recordSelectModel in selectableModels)
            {
                Destroy(recordSelectModel.Value);
            }
        }

        public void ModifyMaterial(Material material)
        {
            // Update button panel material
            fabricationSeenPanel.material = material;
            // Update record selects buttons panel material
            foreach (KeyValuePair<string, GameObject> recordSelectModel in selectableModels)
            {
                recordSelectModel.Value.GetComponentInChildren<MeshRenderer>().material = material;
            }
        }
        #endregion IVISUALISABLE_METHODS

        #region IRECORDABLE_METHODS
        /// <summary>
        /// Activates nominate buttons when attribute name button is <see cref="OnFocus"/>.
        /// It also triggers deactivation of other nominate buttons fabrications.
        /// </summary>
        public void ActivateRecords()
        {
            if (fabricationCreated == true)
            {
                // Call ElementReport to deactivate buttons from other record fabrications
                element.GetComponent<ElementReport>().DeactivateRecords(this.gameObject);
                // Call ElementReport to deactivate buttons from other nominate fabrications
                element.GetComponent<ElementReport>().DeactivateNominates(null);

                if (recordSelectModelsActive == false && recordSelected == false)
                {
                    // Activate all models
                    foreach (KeyValuePair<string, GameObject> selectModel in selectableModels)
                    {
                        selectModel.Value.SetActive(true);
                    }
                    // Activate record select button
                    recordSelectButton.SetActive(true);
                    // Check record select buttons as active
                    recordSelectModelsActive = true;
                }
                else if (recordSelectModelsActive == false && recordSelected == true)
                {
                    GameObject selectModel;
                    // Activate only nominated individual button
                    if (selectableModels.TryGetValue(selectedRecord, out selectModel))
                    {
                        selectModel.SetActive(true);
                    }
                    // Activate record select button
                    recordSelectButton.SetActive(true);
                    // Check record select buttons as active
                    recordSelectModelsActive = true;
                }
                else { }
            }
            else { }
        }

        /// <summary>
        /// Deactivates nominate buttons.
        /// It is also called by <see cref="ElementReport"/> to deactivate nominate buttons when others are to become active.
        /// </summary>
        public void DeactivateRecords()
        {
            if (fabricationCreated == true)
            {
                if (recordSelectModelsActive == true && recordSelected == false)
                {
                    // Deactivate all buttons
                    foreach (KeyValuePair<string, GameObject> selectModel in selectableModels)
                    {
                        selectModel.Value.SetActive(false);
                    }
                    // Deactivate record select button
                    recordSelectButton.SetActive(false);
                    // Check record select buttons as deactive
                    recordSelectModelsActive = false;
                }
                else if (recordSelectModelsActive == true && recordSelected == true)
                {
                    GameObject selectModel;
                    // Deactivate only nominated individual button
                    if (selectableModels.TryGetValue(selectedRecord, out selectModel))
                    {
                        selectModel.SetActive(false);
                    }
                    // Deactivate record select button
                    recordSelectButton.SetActive(false);
                    // Check record select buttons as deactive
                    recordSelectModelsActive = false;
                }
                else { }
            }
        }
        #endregion IRECORDABLE_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        KeyValuePair<string, GameObject> InitialiseSelectableModel(GameObject componentModel)
        {
            // Instantiate selectable model
            GameObject selectableModel = Instantiate(componentModel);
            // Instantiate selectable value as component name with ".obj"
            string selectableModelName = componentModel.transform.GetChild(0).name;
            // Assign name to selectable model
            selectableModel.name = this.name + "_" + selectableModelName + "_" + this.GetHashCode();
            // Assign visualiser as parent
            selectableModel.transform.SetParent(visualiser.transform, true);
            // Assign initial location and rotation to selectable model
            selectableModel.transform.position = componentModel.transform.position;
            selectableModel.transform.rotation = componentModel.transform.rotation;
            // Initialise material to non reported
            selectableModel.GetComponentInChildren<MeshRenderer>().material = fabricationNonReportedMaterial;
            // Add line renderer to fabrication record select button
            selectableModel.AddComponent<ElementsLine>().Initialise(selectableModel, recordSelectButton, fabricationReportedMaterial);
            // Add ManipulationHandler
            selectableModel.AddComponent<ManipulationHandler>();
            selectableModel.GetComponent<ManipulationHandler>().ManipulationType = ManipulationHandler.HandMovementType.OneAndTwoHanded;
            selectableModel.GetComponent<ManipulationHandler>().TwoHandedManipulationType = ManipulationHandler.TwoHandedManipulation.MoveRotate;
            // Add interactable event handler OnClick
            selectableModel.AddComponent<Interactable>();
            selectableModel.GetComponent<Interactable>().OnClick.AddListener(() => SelectRecord(selectableModelName));
            // Deactivate selectable model
            selectableModel.SetActive(false);
            // Return results to InferFromText()
            return new KeyValuePair<string, GameObject>(selectableModelName, selectableModel);
        }
        #endregion PRIVATE

        #region PUBLIC
        /// <summary>
        /// 
        /// </summary>
        /// <param name="individual"></param>
        public void SelectRecord(string setValue)
        {
            Debug.Log("ModelPanelTap1::SelectRecord: model select name is: " + setValue);
            // If fabrication created but individual not nominated, then nominate this individual
            if (fabricationCreated == true && recordSelected == false)
            {
                // Check if it is not record select button calling
                if (setValue != null)
                {
                    // Deactivate other models and update selected model material
                    foreach (KeyValuePair<string, GameObject> selectModel in selectableModels)
                    {
                        if (selectModel.Key == setValue) { selectModel.Value.GetComponentInChildren<MeshRenderer>().material = fabricationReportedMaterial; }
                        else { selectModel.Value.SetActive(false); }
                    }
                    // Update button material
                    fabricationReportedPanel.material = fabricationReportedMaterial;
                    // Assign value to record select button text
                    recordSelectButtonText.text = setValue;
                    // Assign set value as selected
                    selectedRecord = setValue;
                    // Check attribute selection record
                    recordSelected = true;
                }
                else { }
            }
            else if (fabricationCreated == true && recordSelected == true)
            {
                // Activate other buttons and update individual button material
                foreach (KeyValuePair<string, GameObject> selectModel in selectableModels)
                {
                    if (selectModel.Key == selectedRecord) { selectModel.Value.GetComponentInChildren<MeshRenderer>().material = fabricationNonReportedMaterial; }
                    else { selectModel.Value.SetActive(true); }
                }
                // Update button material
                fabricationReportedPanel.material = fabricationNonReportedMaterial;
                // Assign value to record select button text
                recordSelectButtonText.text = null;
                // Unassign set value as selected
                selectedRecord = null;
                // Uncheck attribute selection record
                recordSelected = false;
            }
            else { }
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}
