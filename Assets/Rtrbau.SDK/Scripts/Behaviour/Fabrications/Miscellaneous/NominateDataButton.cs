/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 07/12/2019
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
    public class NominateDataButton : MonoBehaviour, IVisualisable
    {
        #region INITIALISATION_VARIABLES
        public Action<OntologyEntity, bool> nominate;
        public OntologyEntity individualEntity;
        public RtrbauElement individualValues;
        public Transform parent;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public UnityAction report;
        public List<GameObject> nominatedDataFabs;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro buttonText;
        public MeshRenderer seenPanel;
        public MeshRenderer reportedPanel;
        public GameObject nominatedDataText;
        public Transform nominatedDataPanel;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        public bool buttonCreated;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Start()
        {
            if (buttonText == null || seenPanel == null || reportedPanel == null || nominatedDataText == null || nominatedDataPanel == null)
            {
                throw new ArgumentException("NominateDataButton::Start: Script requires some prefabs to work.");
            }
            else {}
        }

        void OnEnable() { }

        void OnDisable() { }

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
            foreach (GameObject nominatedData in nominatedDataFabs)
            {
                Destroy(nominatedData);
            }
            Destroy(this.gameObject);
        }

        public void ModifyMaterial(Material material)
        {
            /// Fabrication material modification is managed by <see cref="INominatable"/>.
        }
        #endregion IVISUALISABLE_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        void CreateButton()
        {
            // Show individual name the button refers to
            buttonText.text = individualEntity.Name();
            // Ensure the RtrbauElement received is not empty, other wise do not add its attributes
            if (!individualValues.Equals(default(RtrbauElement)) && !individualEntity.Name().Contains(Parser.ParseNamingNew()))
            {
                // For each RtrbauAttribute in RtrbauElement of nominated individual
                foreach (RtrbauAttribute individualProperty in individualValues.elementAttributes)
                {
                    // Generate data fabs for each individual property
                    CreateNominatedText(individualProperty);
                }
            }
            else { }
            // Assign button as created
            buttonCreated = true;
            // Assign report action
            ActivateReporting();
        }

        void CreateNominatedText(RtrbauAttribute individualProperty)
        {
            // If property is of datatype
            if (individualProperty.attributeType.URI() == Rtrbauer.instance.owl.URI() + "DatatypeProperty")
            {
                // UPG: add audio and image properties
                if (individualProperty.attributeValue.Contains("http://")) { }
                else
                {
                    // Create nominate datatype property text panel
                    GameObject nominatedText = CreateNominatedPropertyText(individualProperty, true);
                    // Assign nominated text to list of nominated data texts
                    nominatedDataFabs.Add(nominatedText);
                }
            }
            else if (individualProperty.attributeType.URI() == Rtrbauer.instance.owl.URI() + "ObjectProperty")
            {
                // Identify if object property refers to individual of component ontology
                if (individualProperty.attributeRange.URI() == Rtrbauer.instance.component.URI())
                {
                    // Obtain component name
                    string nominatedComponentName = Parser.ParseURI(individualProperty.attributeValue, '#', RtrbauParser.post);
                    GameObject nominatedComponentModel = parent.GetComponent<TextPanelTap3>().visualiser.manager.FindAssetComponentManipulator(nominatedComponentName);
                    // If component is found in scene create nominated model panel, otherwise destroy nominate data button
                    if (nominatedComponentModel != null)
                    {
                        // Create nominate object property text panel
                        GameObject nominatedText = CreateNominatedPropertyText(individualProperty, false);
                        // Assign nominated text to list of nominated data texts
                        nominatedDataFabs.Add(nominatedText);
                        // Create nominate object property model panel
                        GameObject nominatedModel = CreateNominatedPropertyModel(nominatedText, nominatedComponentModel);
                        // Assign nominated model to list of nominated data texts
                        nominatedDataFabs.Add(nominatedModel);
                    }
                    else { }
                }
                else
                {
                    // Create nominate object property text panel
                    GameObject nominatedText = CreateNominatedPropertyText(individualProperty, false);
                    // Assign nominated text to list of nominated data texts
                    nominatedDataFabs.Add(nominatedText);
                }
            }
            else { }
        }

        // GameObject CreateNominatedPropertyText(JsonValue nominatedProperty, bool isDatatype)
        GameObject CreateNominatedPropertyText(RtrbauAttribute nominatedProperty, bool isDatatype)
        {
            // Instantiate nominated text panel
            GameObject nominatedText = Instantiate(nominatedDataText);
            // Initialise nominated text panel
            nominatedText.transform.SetParent(nominatedDataPanel, false);
            // Scale nominated text panel
            ScaleNominatedText(nominatedText);
            // Generate datatype property text values
            string propertyName = nominatedProperty.attributeName.Name();
            string propertyValue;
            if (isDatatype == true) { propertyValue = nominatedProperty.attributeValue; }
            else { propertyValue = Parser.ParseURI(nominatedProperty.attributeValue, '#', RtrbauParser.post); }
            // Add nominated datatype property text to panel
            nominatedText.GetComponentInChildren<TextMeshPro>().text = propertyName + ": " + propertyValue;
            // Deactivate nominated data text
            nominatedText.SetActive(false);
            // Return game object
            return nominatedText;
        }

        GameObject CreateNominatedPropertyModel(GameObject nominatedPropertyText, GameObject nominatedPropertyComponent)
        {
            // Instantiate nominated model
            GameObject nominatedModel = Instantiate(nominatedPropertyComponent);
            // Instantiate selectable value as component name with ".obj"
            string nominatedModelName = nominatedPropertyComponent.transform.GetChild(0).name;
            // Assign name to selectable model
            nominatedModel.name = this.name + "_" + nominatedModelName + "_" + this.GetHashCode();
            // Assign visualiser as parent
            nominatedModel.transform.SetParent(parent.GetComponent<TextPanelTap3>().visualiser.transform, true);
            // Assign initial location and rotation to nominated model
            nominatedModel.transform.position = nominatedPropertyComponent.transform.position;
            nominatedModel.transform.rotation = nominatedPropertyComponent.transform.rotation;
            // Initialise material to non reported
            nominatedModel.GetComponentInChildren<MeshRenderer>().material = parent.GetComponent<TextPanelTap3>().fabricationNonReportedMaterial;
            // Add line renderer to fabrication record select button
            nominatedModel.AddComponent<ElementsLine>().Initialise(nominatedModel, nominatedPropertyText, parent.GetComponent<TextPanelTap3>().fabricationReportedMaterial);
            // Add ManipulationHandler
            nominatedModel.AddComponent<ManipulationHandler>();
            nominatedModel.GetComponent<ManipulationHandler>().ManipulationType = ManipulationHandler.HandMovementType.OneAndTwoHanded;
            nominatedModel.GetComponent<ManipulationHandler>().TwoHandedManipulationType = ManipulationHandler.TwoHandedManipulation.MoveRotate;
            // Deactivate selectable model
            nominatedModel.SetActive(false);
            // Return game object
            return nominatedModel;
        }

        void ScaleNominatedText(GameObject nominatedData)
        {
            // Assuming all rtrbau fabrications have the same scale, the child should have the same scale as the parent
            // This is because both are childs of tile grid object collections with game objects of scale 1
            // UPG: to create a method that does not have any assumptions
            float sX = parent.transform.localScale.x;
            float sY = parent.transform.localScale.y;
            float sZ = parent.transform.localScale.z;

            nominatedData.transform.localScale = new Vector3(sX, sY, sZ);
        }
        #endregion PRIVATE

        #region PUBLIC
        public void Initialise(Action<OntologyEntity, bool> nominateIndividual, OntologyEntity entityIndividual, RtrbauElement valuesIndividual, Transform fabricationParent)
        {
            // Initialise button variables
            nominate = nominateIndividual;
            individualEntity = entityIndividual;
            individualValues = valuesIndividual;
            parent = fabricationParent;
            report = NominateIndividual;
            nominatedDataFabs = new List<GameObject>();
            buttonCreated = false;
            // Generate nominate data button
            CreateButton();
        }

        public void NominateIndividual()
        {
            if (buttonCreated == true)
            {
                // Trigger the nominate individual action for this button as non-recordable
                nominate.Invoke(individualEntity, false);
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

        public void ActivateNominateDataTexts()
        {
            if (buttonCreated == true)
            {
                parent.GetComponent<TextPanelTap3>().DeactivateNominatesData();

                foreach (GameObject nominatedFab in nominatedDataFabs)
                {
                    nominatedFab.SetActive(true);
                }
            }
            else { }
        }

        public void DeactivateNominateDataTexts()
        {
            if (buttonCreated == true)
            {
                foreach (GameObject nominatedFab in nominatedDataFabs)
                {
                    nominatedFab.SetActive(false);
                }
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
