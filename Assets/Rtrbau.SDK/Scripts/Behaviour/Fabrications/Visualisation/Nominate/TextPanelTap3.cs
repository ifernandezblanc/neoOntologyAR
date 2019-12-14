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
using System.Collections;
using TMPro;
using Microsoft.MixedReality.Toolkit.Utilities;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class TextPanelTap3 : MonoBehaviour, IFabricationable, IVisualisable, INominatable
    {
        #region INITIALISATION_VARIABLES
        public AssetVisualiser visualiser;
        public RtrbauFabrication data;
        public Transform element;
        public Transform scale;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public Dictionary<OntologyEntity, GameObject> individualNominates;
        public OntologyEntity nominatedIndividual;
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
        public Transform nominateButtons;
        public GameObject nominateButton;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        private bool fabricationCreated;
        private bool nominateButtonsActive;
        private bool individualNominated;
        private bool individualRecordable;
        private bool newReportNominated;
        private bool nominatesNewReport;
        #endregion CLASS_EVENTS

        #region MONOBEHVAIOUR_METHODS
        void Start()
        {
            if (fabricationText == null || fabricationSeenPanel == null || fabricationReportedPanel == null || fabricationSeenMaterial == null || fabricationNonReportedMaterial == null || fabricationReportedMaterial == null || nominateButtons == null || nominateButton == null)
            {
                throw new ArgumentException("DefaultNominate::Start: Script requires some prefabs to work.");
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
            individualNominates = new Dictionary<OntologyEntity, GameObject>();
            nominatedIndividual = null;
            fabricationCreated = false;
            nominateButtonsActive = false;
            individualNominated = false;
            individualRecordable = false;
            newReportNominated = false;
            nominatesNewReport = false;
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
            DataFacet textfacet7 = DataFormats.TextPanelTap3.formatFacets[0];
            RtrbauAttribute attribute;

            // Check data received meets fabrication requirements
            if (data.fabricationData.TryGetValue(textfacet7, out attribute))
            {
                // Set button name to relationship name
                fabricationText.text = Parser.ParseNamingOntologyFormat(attribute.attributeName.Name());
                // Assign nominated class
                OntologyEntity individualRange = attribute.attributeRange;
                // Use generic name (class) to generate ontology entity for "new" individual
                OntologyEntity newIndividualEntity = new OntologyEntity(Parser.ParseAddNew(attribute.attributeRange.URI()));
                // Create and assign "new" individual in case user wants to define a new one
                individualNominates.Add(newIndividualEntity, CreateIndividualButton(newIndividualEntity, individualRange));
                // Retrieve JsonIndividuals from ElementReport
                List<JsonIndividual> individuals = element.GetComponent<ElementReport>().objectClassesIndividuals.Find(x => x.ontClass == attribute.attributeRange.URI()).ontIndividuals;
                // Add individuals to fabrication list
                foreach (JsonIndividual individual in individuals)
                {
                    Debug.Log("DefaultNominate::InferFromtText: individual nominated is: " + individual.ontIndividual);
                    // Generate OntologyEntity to parse individual URI
                    OntologyEntity individualEntity = new OntologyEntity(individual.ontIndividual);
                    // Create individual button and assign both to dictionary
                    individualNominates.Add(individualEntity, CreateIndividualButton(individualEntity, individualRange));
                }
                // Ensure no nominates buttons are unloadable
                StartCoroutine(UnloadNominates());
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
            // Check if an individual has been nominated
            if (individualNominated == true)
            {
                DataFacet textfacet7 = DataFormats.TextPanelTap3.formatFacets[0];
                RtrbauAttribute attribute;

                // Check data received meets fabrication requirements
                if (data.fabricationData.TryGetValue(textfacet7, out attribute))
                {
                    if (nominatesNewReport == false)
                    {
                        //Debug.Log("DefaultNominate::OnNextVisualisation: nominatedIndividual is: " + nominatedIndividual.Name());
                        //Debug.Log("DefaultNominate::OnNextVisualisation: nominatedIndividual is: " + nominatedIndividual.URI());
                        // If nominatedIndividual is new, then attribute value needs to be modified
                        if (nominatedIndividual.Name().Contains(Parser.ParseNamingNew()))
                        {
                            // Check if new individual is recordable
                            if (individualRecordable == false)
                            {
                                // Generate OntologyEntity to parse new individual name with date time
                                OntologyEntity newIndividual = new OntologyEntity(Parser.ParseAddDateTime(attribute.attributeRange.URI()));
                                // Assign user-reported attribute value to RtrbauElement from ElementReport through RtrbauFabrication
                                attribute.attributeValue = newIndividual.URI();
                                // Assign nominated individual as new element report
                                newReportNominated = true;
                                Debug.Log("TextPanelTap3::OnNextVisualisation: nominatedIndividual " + nominatedIndividual.Name() + " is new to report");
                            }
                            else
                            {
                                throw new ArgumentException("TextPanelTap3::OnNextVisualisation: nominated recordable should not occur at this point.");
                                //// Parse text from record button in nominate button to generate new individual entity
                                //GameObject individual;
                                //string newNominatedRecorded;
                                //// Deactivate only nominated individual button
                                //if (individualNominates.TryGetValue(nominatedIndividual, out individual))
                                //{
                                //    newNominatedRecorded = individual.GetComponent<NominateButton>().recordButtonText.text;
                                //}
                                //else { throw new ArgumentException("DefaultNominate::OnNextVisualisation: nominated individual not found"); }
                                //// Assign user-reported attribute value to RtrbauElement from ElementReport through RtrbauFabrication
                                //attribute.attributeValue = nominatedIndividual.Ontology().URI() + newNominatedRecorded;
                                ////Debug.Log("DefaultNominate::OnNextVisualisation: nominatedIndividual " + nominatedIndividual.Name() + " is new to record");
                            }
                        }
                        else
                        {
                            // Assign user-reported attribute value to RtrbauElement from ElementReport through RtrbauFabrication
                            attribute.attributeValue = nominatedIndividual.URI();
                        }
                        
                        // Change button colour for user confirmation
                        fabricationReportedPanel.material = fabricationReportedMaterial;
                        // Check if all attribute values have been recorded
                        // If true, then ElementReport will input reported element into report
                        // If true, then ElementReport will change colour to reported
                        element.gameObject.GetComponent<ElementReport>().CheckAttributesReported();
                        // Deactivate nominate buttons
                        DeactivateNominates();
                    }
                    else
                    {
                        //Debug.Log("DefaultNominate::OnNextVisualisation: new element to report is of range: " + attribute.attributeRange.Name());
                        //Debug.Log("DefaultNominate::OnNextVisualisation: new element to report name is: " + attribute.attributeValue);
                        // Generate ontology entity to report connection to new RtrbauElement
                        OntologyEntity relationship = new OntologyEntity(attribute.attributeName.URI());
                        // Report relationship attribute to load next RtrbauElement
                        Reporter.instance.ReportElement(relationship);
                        // Generate OntologyElement(s) to load RtrbauElement
                        OntologyElement elementClass = new OntologyElement(attribute.attributeRange.URI(), OntologyElementType.ClassProperties);
                        OntologyElement elementIndividual = new OntologyElement(attribute.attributeValue, OntologyElementType.IndividualProperties);
                        // Load new RtrbauElement from AssetVisualiser, ensure user has selected the type of RtrbauElement to load
                        RtrbauerEvents.TriggerEvent("AssetVisualiser_CreateElement", elementIndividual, elementClass, Rtrbauer.instance.user.procedure);
                        // Check RtrbauElement to UnloadElement if necessary
                        element.GetComponent<ElementReport>().CheckNewNominatesReported(this.gameObject);
                    }
                }
                else { }
            }
            else { }
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
            // Update button panel material
            fabricationSeenPanel.material = material;
            // Update nominate buttons panel material
            foreach (KeyValuePair<OntologyEntity, GameObject> nominateButton in individualNominates)
            {
                nominateButton.Value.GetComponent<NominateDataButton>().SeenMaterial(material);
            }
        }
        #endregion IVISUALISABLE_METHODS

        #region INOMINATABLE_METHODS
        /// <summary>
        /// Activates nominate buttons when attribute name button is <see cref="OnFocus"/>.
        /// It also triggers deactivation of other nominate buttons fabrications.
        /// </summary>
        public void ActivateNominates()
        {
            if (fabricationCreated == true && nominatesNewReport == false)
            {
                // Call ElementReport to deactivate buttons from other nominate fabrications
                element.GetComponent<ElementReport>().DeactivateNominates(this.gameObject);
                // Call ElementReport to deactivate buttons from other record fabrications
                element.GetComponent<ElementReport>().DeactivateRecords(null);

                if (nominateButtonsActive == false && individualNominated == false)
                {
                    // Activate all buttons
                    foreach (KeyValuePair<OntologyEntity, GameObject> nominateButton in individualNominates)
                    {
                        ActivateIndividualButton(nominateButton.Value);
                    }
                    // Check individual buttons as active
                    nominateButtonsActive = true;
                }
                else if (nominateButtonsActive == false && individualNominated == true)
                {
                    GameObject individual;
                    // Activate only nominated individual button
                    if (individualNominates.TryGetValue(nominatedIndividual, out individual))
                    {
                        ActivateIndividualButton(individual);
                    }
                    // Check individual buttons as active
                    nominateButtonsActive = true;
                }
                else { }
            }
            else { }
        }

        /// <summary>
        /// Deactivates nominate buttons.
        /// It is also called by <see cref="ElementReport"/> to deactivate nominate buttons when others are to become active.
        /// </summary>
        public void DeactivateNominates()
        {
            if (fabricationCreated == true && nominatesNewReport == false)
            {
                if (nominateButtonsActive == true && individualNominated == false)
                {
                    // Deactivate all buttons
                    foreach (KeyValuePair<OntologyEntity, GameObject> nominateButton in individualNominates)
                    {
                        DeactivateIndividualButton(nominateButton.Value);
                    }
                    // Check individual buttons as deactive
                    nominateButtonsActive = false;
                }
                else if (nominateButtonsActive == true && individualNominated == true)
                {
                    GameObject individual;
                    // Deactivate only nominated individual button
                    if (individualNominates.TryGetValue(nominatedIndividual, out individual))
                    {
                        DeactivateIndividualButton(individual);
                    }
                    // Check individual buttons as deactive
                    nominateButtonsActive = false;
                }
                else { }
            }
        }

        /// <summary>
        /// Checks if the individual nominated is to create a new individual or that element reported has been forced.
        /// </summary>
        /// <returns>
        /// If true, returns the nominate button from which create a new report element as an <see cref="OntologyEntity"/>.
        /// Otherwise, returns a null <see cref="OntologyEntity"/>.
        /// </returns>
        public bool NominatesNewReportElement(bool reportedForced)
        {
            // Check if han individual has been nominated
            if (individualNominated == true || reportedForced == true)
            {
                // Check if nominated individual name is that for a new individual
                if (newReportNominated == true)
                {
                    //Debug.Log("DefaultNominate::CreateNewElementReport: individual to report is new.");
                    // Deactivate nominates buttons
                    DeactivateNominates();
                    // Assign fabrication as to nominate new RtrbauElement
                    nominatesNewReport = true;
                    // Then return true
                    return true;
                }
                else
                {
                    //Debug.Log("DefaultNominate::CreateNewElementReport: individual to report is not new.");
                    // Then return false
                    return false;
                }
            }
            else
            {
                throw new ArgumentException("DefaultNominate::CreatesNewElementReport: this function should not be accessible before an individual is nominated.");
            }
        }
        #endregion INOMINATABLE_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        GameObject CreateIndividualButton(OntologyEntity individual, OntologyEntity range)
        {
            // Instantiate nominate button
            GameObject individualButton = Instantiate(nominateButton);
            // Initialise nominate button with corresponding nominate function
            individualButton.GetComponent<NominateDataButton>().Initialise(NominateIndividual, individual, this.transform);
            // Scale buttons to possible change in fabrications scale
            ScaleIndividualButton(individualButton);
            // Set tile grid object collection as button parent
            DeactivateIndividualButton(individualButton);
            // Return button created and modified
            return individualButton;
        }

        void ScaleIndividualButton(GameObject button)
        {
            // Declare scaling parameters
            float sX;
            float sY;
            float sZ;
            // Assuming buttons (fabrications) have equal scales for all axis
            decimal bS = (decimal)button.transform.localScale.x;
            // In case the button has a scale smaller than 1
            // That also means the fabrication has an equal scale smaller than 1
            if (bS < 1)
            {
                sX = 1 / this.transform.localScale.x;
                sY = 1 / this.transform.localScale.y;
                sZ = 1 / this.transform.localScale.z;
            }
            else
            {
                sX = button.transform.localScale.x / this.transform.localScale.x;
                sY = button.transform.localScale.y / this.transform.localScale.y;
                sZ = button.transform.localScale.z / this.transform.localScale.z;
            }

            button.transform.localScale = new Vector3(sX, sY, sZ);
        }

        void ActivateIndividualButton(GameObject button)
        {
            // Deactivate tile grid object collection
            nominateButtons.gameObject.GetComponent<TileGridObjectCollection>().enabled = false;
            // Set tile grid object collection as button parent
            button.transform.SetParent(nominateButtons, false);
            // Set game object button to active
            button.SetActive(true);
            // Activate tile grid object collection
            nominateButtons.gameObject.GetComponent<TileGridObjectCollection>().enabled = true;
        }

        void DeactivateIndividualButton(GameObject button)
        {
            // Deactivate tile grid object collection
            nominateButtons.gameObject.GetComponent<TileGridObjectCollection>().enabled = false;
            // Set fabrication root as button parent
            button.transform.SetParent(this.transform, false);
            // Set game object button children to deactive
            button.GetComponent<NominateDataButton>().DeactivateNominateDataTexts();
            // Set game object button to deactive
            button.SetActive(false);
            // Activate tile grid object collection
            nominateButtons.gameObject.GetComponent<TileGridObjectCollection>().enabled = true;
        }

        IEnumerator UnloadNominates()
        {
            Dictionary<OntologyEntity, GameObject> unloadableNominates = new Dictionary<OntologyEntity, GameObject>();

            foreach (KeyValuePair<OntologyEntity,GameObject> individualNominate in individualNominates)
            {
                // Wait until all nominates buttons have been created
                while(individualNominate.Value.GetComponent<NominateDataButton>().buttonCreated == false)
                {
                    yield return null;
                }

                // If created button is unloadable, then add to unloadable nominates
                if (individualNominate.Value.GetComponent<NominateDataButton>().unloadableNominate == true)
                {
                    unloadableNominates.Add(individualNominate.Key, individualNominate.Value);
                }
                else {}
            }

            foreach (KeyValuePair<OntologyEntity, GameObject> nominate in unloadableNominates)
            {
                // Remove from individual Nominates
                individualNominates.Remove(nominate.Key);
                // Destroy button
                Destroy(nominate.Value);
            }

            //GameObject nominateDataButton;
            //if (individualNominates.TryGetValue(nominate, out nominateDataButton))
            //{
            //    individualNominates.Remove(nominate);
            //    Destroy(nominateDataButton);
            //    Debug.Log("TextPanelTap3::UnloadNominate: individual nominate being unloaded: " + nominate.Entity());
            //}
            //else { throw new ArgumentException("TextPanelTap3::UnloadNominate: Nominate should be found: " + nominate.Entity()); }
        }
        #endregion PRIVATE

        #region PUBLIC
        /// <summary>
        /// 
        /// </summary>
        /// <param name="individual"></param>
        public void NominateIndividual(OntologyEntity individual, bool nominatedRecordable)
        {
            // If fabrication created but individual not nominated, then nominate this individual
            if (fabricationCreated == true && individualNominated == false)
            {
                // Deactivate other buttons and update individual button material
                foreach (KeyValuePair<OntologyEntity, GameObject> nominateButton in individualNominates)
                {
                    if (nominateButton.Key == individual) { nominateButton.Value.GetComponent<NominateDataButton>().ReportMaterial(fabricationReportedMaterial); }
                    else { DeactivateIndividualButton(nominateButton.Value); }
                }
                // Update button material
                fabricationReportedPanel.material = fabricationReportedMaterial;
                // Assign individual as nominated
                nominatedIndividual = individual;
                // Check individual nomination as recordable
                individualRecordable = nominatedRecordable;
                // Check individual nomination
                individualNominated = true;
            }
            else if (fabricationCreated == true && individualNominated == true)
            {
                // Activate other buttons and update individual button material
                foreach (KeyValuePair<OntologyEntity, GameObject> nominateButton in individualNominates)
                {
                    if (nominateButton.Key == individual) { nominateButton.Value.GetComponent<NominateDataButton>().ReportMaterial(fabricationNonReportedMaterial); }
                    else { ActivateIndividualButton(nominateButton.Value); }
                }
                // Update button material
                fabricationReportedPanel.material = fabricationNonReportedMaterial;
                // Unassign individual as nominated
                nominatedIndividual = null;
                // Uncheck individual recordable nomination
                individualRecordable = false;
                // Uncheck individual nomination
                individualNominated = false;
            }
            else { }
        }

        public void DeactivateNominatesData()
        {
            if (fabricationCreated == true)
            {
                foreach (KeyValuePair<OntologyEntity, GameObject> nominate in individualNominates)
                {
                    nominate.Value.GetComponent<NominateDataButton>().DeactivateNominateDataTexts();
                }
            }
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}
