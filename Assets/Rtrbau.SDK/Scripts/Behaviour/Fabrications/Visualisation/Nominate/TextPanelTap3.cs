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
using System.IO;
using TMPro;
using Microsoft.MixedReality.Toolkit.Utilities;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class TextPanelTap3 : MonoBehaviour, IFabricationable, IVisualisable, INominatable, IRecommendable
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

        #region RECOMMENDATION_VARIABLES
        public RecommendationFormat recommendationFormat;
        public JsonIndividualValues recommendationTarget;
        public List<JsonIndividualValues> recommendationCases;
        public RtrbauRecommendation recommendation;
        public int recommendationCasesDownloadable;
        public bool recommendationTargetDownloaded;
        public bool recommendationCasesDownloaded;
        #endregion RECOMMENDATION_VARIABLES

        #region FACETS_VARIABLES
        #endregion FACETS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro fabricationText;
        public MeshRenderer fabricationSeenPanel;
        public MeshRenderer fabricationReportedPanel;
        public Material fabricationSeenMaterial;
        public Material fabricationUnseenMaterial;
        public Material fabricationNonReportedMaterial;
        public Material fabricationReportedMaterial;
        public Renderer fabricationBounds;
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
            if (fabricationText == null || fabricationSeenPanel == null || fabricationReportedPanel == null || fabricationSeenMaterial == null || fabricationUnseenMaterial == null || fabricationNonReportedMaterial == null || fabricationReportedMaterial == null || fabricationBounds == null || nominateButtons == null || nominateButton == null)
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
            recommendationFormat = null;
            recommendation = null;
            recommendationTarget = null;
            recommendationCases = new List<JsonIndividualValues>();
            recommendationCasesDownloadable = 0;
            recommendationTargetDownloaded = false;
            recommendationCasesDownloaded = false;
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
                // Start recommendation procedure
                DetermineRecommendationFormat();
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
                            }
                            else
                            {
                                throw new ArgumentException("TextPanelTap3::OnNextVisualisation: nominated recordable should not occur at this point.");
                            }
                        }
                        else
                        {
                            // Assign user-reported attribute value to RtrbauElement from ElementReport through RtrbauFabrication
                            attribute.attributeValue = nominatedIndividual.URI();
                            // Assign nominated individual as non new element report
                            newReportNominated = false;
                        }
                        
                        // Change button colour for user confirmation
                        fabricationReportedPanel.material = fabricationReportedMaterial;
                        // Check if all attribute values have been recorded
                        element.gameObject.GetComponent<ElementReport>().CheckAttributesReported();
                    }
                    else
                    {
                        // Generate OntologyElement(s) to load RtrbauElement
                        OntologyElement elementClass = new OntologyElement(attribute.attributeRange.URI(), OntologyElementType.ClassProperties);
                        OntologyElement elementIndividual = new OntologyElement(attribute.attributeValue, OntologyElementType.IndividualProperties);
                        // Generate ontology entities to report connection to new RtrbauElement
                        OntologyEntity entityRelationship = new OntologyEntity(attribute.attributeName.URI());
                        // Report class selected: InputIntoReport()
                        Reporter.instance.ReportElement(entityRelationship, elementClass.entity, elementIndividual.entity);
                        // Load new RtrbauElement from AssetVisualiser, ensure user has selected the type of RtrbauElement to load
                        RtrbauerEvents.TriggerEvent("AssetVisualiser_CreateElement", elementIndividual, elementClass, Rtrbauer.instance.user.procedure);
                        // Destroy OntologyRecommendation
                        DestroyRecommendation();
                        // Deactivate reporting action of left nominates
                        foreach (KeyValuePair<OntologyEntity, GameObject> nominate in individualNominates)
                        {
                            nominate.Value.GetComponent<NominateDataButton>().DeactivateReporting();
                        }
                        // Check RtrbauElement to UnloadElement if necessary
                        // element.GetComponent<ElementReport>().CheckNewNominatesReported(this.gameObject);
                    }
                }
                else { }
            }
            else { Debug.Log("TextPanelTap3::7"); }
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
            if (fabricationCreated == true)
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
            if (fabricationCreated == true)
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

        public void ActivateReporting()
        {
            /// Fabrication reporting is managed by <see cref="NominateDataButton"/>.
        }


        /// <summary>
        /// 
        /// </summary>
        public void DeactivateReporting(bool forcedReporting)
        {
            // Check if han individual has been nominated
            if (individualNominated == true || forcedReporting == true)
            {
                // Deactivate nominates buttons
                DeactivateNominates();
                // Initialise list of destroyable nominates
                Dictionary<OntologyEntity, GameObject> destroyableNominates = new Dictionary<OntologyEntity, GameObject>();

                // Destroy all nominate buttons except that being nominated
                foreach (KeyValuePair<OntologyEntity, GameObject> nominate in individualNominates)
                {
                    // Ensure there is individual nominated URI to compare
                    string nominatedURI;
                    if (nominatedIndividual == null) { nominatedURI = null; }
                    else { nominatedURI = nominatedIndividual.URI(); }

                    // If individual nominate is not that being nominated
                    if (nominate.Key.URI() != nominatedURI)
                    {
                        // Add nominate to list of destroyables
                        destroyableNominates.Add(nominate.Key, nominate.Value);
                    }
                    else
                    {
                        // If nominated individual is new
                        if (nominate.Key.URI().Contains(Parser.ParseNamingNew()))
                        {
                            // Update fabrication material to non reported
                            fabricationReportedPanel.material = fabricationNonReportedMaterial;
                            // Update nominate material to non reported
                            nominate.Value.GetComponent<NominateDataButton>().ReportMaterial(fabricationNonReportedMaterial);
                            // Then fabrication nominates new report
                            nominatesNewReport = true;
                        }
                        else
                        {
                            // Otherwise deactivate reporting
                            nominate.Value.GetComponent<NominateDataButton>().DeactivateReporting();
                        }
                    }
                }

                // Remove destroyableNominates from individualNominates list
                foreach (KeyValuePair<OntologyEntity, GameObject> nominate in destroyableNominates)
                {
                    // Remove nominate from individualNominates
                    individualNominates.Remove(nominate.Key);
                    // Destroy nominate button
                    Destroy(nominate.Value);
                }
            }
            else
            {
                throw new ArgumentException("TextPanelTap3::DeactivateReporting: this function should not be accesed before an individual is nominated.");
            }
        }
        #endregion INOMINATABLE_METHODS

        #region IRECOMMENDABLE_METHODS
        /// <summary>
        /// 
        /// </summary>
        public void DetermineRecommendationFormat()
        {
            if (Dictionaries.RecommendFormats.TryGetValue(data.fabricationName, out recommendationFormat))
            {
                DownloadRecommendationElements();
            }
            else { throw new ArgumentException("TextPanelTap3::FindRecommendationFormat: Fabrication does not have recommendation format implemented"); }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DownloadRecommendationElements()
        {
            // Find recommendation target
            OntologyEntity target = Reporter.instance.FindLastReportedIndividualFromClass(recommendationFormat.formatRange);

            if (target != null)
            {
                // If target exists, then recommendation can be done
                OntologyElement targetElement = new OntologyElement(target.URI(), OntologyElementType.IndividualProperties);
                // Start OntologyElement target download
                LoaderEvents.StartListening(targetElement.EventName(), DownloadRecommendationTarget);
                Loader.instance.StartOntElementDownload(targetElement);
            }
            else
            {
                // Otherwise, simply return all downloadable cases
                recommendationTarget = null;
                recommendationTargetDownloaded = true;
            }

            // Generate recommendationCases OntologyRecommendation
            OntologyRecommendation casesRecommendation = new OntologyRecommendation(recommendationFormat.formatRange.URI(), RtrbauRecommendationType.ClassIndividuals);
            // Start OntologyRecommendation cases download
            LoaderEvents.StartListening(casesRecommendation.EventName(), DownloadRecommendationCases);
            Loader.instance.StartOntRecommendationDownload(casesRecommendation);
        }

        /// <summary>
        /// 
        /// </summary>
        public void GenerateRecommendation()
        {
            if (recommendationTargetDownloaded == true && recommendationCasesDownloaded == true && recommendationCasesDownloadable == recommendationCases.Count)
            {
                DataFacet textfacet7 = DataFormats.TextPanelTap3.formatFacets[0];
                RtrbauAttribute attribute;

                // Check data received meets fabrication requirements
                if (data.fabricationData.TryGetValue(textfacet7, out attribute))
                {
                    recommendation = new RtrbauRecommendation(recommendationFormat, attribute, recommendationTarget, recommendationCases);
                    RecommendCases();
                }
                else { throw new ArgumentException(data.fabricationName.ToString() + "::GenerateRecommendation: cannot recommend attribute received."); }
            }
            else { Debug.Log("TextPanelTap3::GenerateRecommendation: Recommendation elements still being downloaded"); }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RecommendCases()
        {
            DataFacet textfacet7 = DataFormats.TextPanelTap3.formatFacets[0];
            RtrbauAttribute attribute;

            // Check data received meets fabrication requirements
            if (data.fabricationData.TryGetValue(textfacet7, out attribute))
            {
                // Generate individual entity and values for "new" individual nominate
                OntologyEntity newIndividualEntity = new OntologyEntity(Parser.ParseAddNew(attribute.attributeRange.URI()));
                RtrbauElement newIndividualValues = new RtrbauElement();
                // Create "new" nominate button and assign to individual nominates list
                individualNominates.Add(newIndividualEntity, CreateIndividualButton(newIndividualEntity, newIndividualValues));
                // For each recommended case
                foreach (KeyValuePair<decimal,RtrbauElement> recommendedCase in recommendation.RecommendCases())
                {
                    Debug.Log("TextPanelTap3::RecommendCases: individual recommended is: " + recommendedCase.Value.elementName.Name() + " with similarity " + recommendedCase.Key);
                    // Create individual button and assign both to dictionary
                    individualNominates.Add(recommendedCase.Value.elementName, CreateIndividualButton(recommendedCase.Value.elementName, recommendedCase.Value));
                }
                // Set fabrication as created
                fabricationCreated = true;
            }
            else
            {
                throw new ArgumentException(data.fabricationName.ToString() + "::RecommendCases: cannot recommend attribute received.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DestroyRecommendation()
        {
            // To delete recommendation file so inferred knowledge is not kept
            // Similar to ElementReport destroying class individuals ontology file
            // Generate recommendationCases OntologyRecommendation
            OntologyRecommendation casesRecommendation = new OntologyRecommendation(recommendationFormat.formatRange.URI(), RtrbauRecommendationType.ClassIndividuals);
            // If json file exists, delete it to enable re-download with new recommendation
            // UPG: ensure no events are calling to the same file at the same time
            if (File.Exists(casesRecommendation.FilePath())) { File.Delete(casesRecommendation.FilePath()); }
        }
        #endregion IRECOMMENDABLE_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        GameObject CreateIndividualButton(OntologyEntity individualEntity, RtrbauElement individualValues)
        {
            // Instantiate nominate button
            GameObject individualButton = Instantiate(nominateButton);
            // Initialise nominate button with corresponding nominate function
            individualButton.GetComponent<NominateDataButton>().Initialise(NominateIndividual, individualEntity, individualValues, this.transform);
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

        void DownloadRecommendationTarget(OntologyElement target)
        {
            LoaderEvents.StopListening(target.EventName(), DownloadRecommendationTarget);

            if (File.Exists(target.FilePath())) 
            {
                string jsonFile = File.ReadAllText(target.FilePath());
                recommendationTarget = JsonUtility.FromJson<JsonIndividualValues>(jsonFile);
                recommendationTargetDownloaded = true; 
            }
            else { }

            GenerateRecommendation();
        }

        void DownloadRecommendationCases(OntologyRecommendation cases)
        {
            LoaderEvents.StopListening(cases.EventName(), DownloadRecommendationCases);

            if (File.Exists(cases.FilePath()))
            {
                string jsonFile = File.ReadAllText(cases.FilePath());
                JsonClassIndividuals casesIndividuals = JsonUtility.FromJson<JsonClassIndividuals>(jsonFile);

                if (casesIndividuals.ontClass == recommendationFormat.formatRange.URI())
                {
                    recommendationCasesDownloadable = casesIndividuals.ontIndividuals.Count;
                    recommendationCasesDownloaded = true;

                    foreach (JsonIndividual individual in casesIndividuals.ontIndividuals)
                    {
                        OntologyElement caseIndividual = new OntologyElement(individual.ontIndividual, OntologyElementType.IndividualProperties);
                        LoaderEvents.StartListening(caseIndividual.EventName(), DownloadRecommendationCase);
                        Loader.instance.StartOntElementDownload(caseIndividual);
                    }
                }
                else { throw new ArgumentException("TextPanelTap3::DownloadRecommendationCases: individuals do not match recommendation range class"); }
            }
            else { }
        }

        void DownloadRecommendationCase(OntologyElement individual)
        {
            LoaderEvents.StopListening(individual.EventName(), DownloadRecommendationCase);

            if (File.Exists(individual.FilePath())) 
            {
                string jsonFile = File.ReadAllText(individual.FilePath());
                JsonIndividualValues caseIndividual = JsonUtility.FromJson<JsonIndividualValues>(jsonFile);
                recommendationCases.Add(caseIndividual);
            }
            else { }

            GenerateRecommendation();
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
                // Call to report attribute
                OnNextVisualisation();
            }
            else if (fabricationCreated == true && individualNominated == true)
            {
                if (nominatesNewReport == false)
                {
                    // Activate other buttons and update individual button material
                    foreach (KeyValuePair<OntologyEntity, GameObject> nominateButton in individualNominates)
                    {
                        if (nominateButton.Key == individual) { nominateButton.Value.GetComponent<NominateDataButton>().ReportMaterial(fabricationNonReportedMaterial); }
                        else { ActivateIndividualButton(nominateButton.Value); }
                    }
                    // Update button material
                    fabricationReportedPanel.material = fabricationUnseenMaterial;
                    // Unassign individual as nominated
                    nominatedIndividual = null;
                    // Uncheck individual recordable nomination
                    individualRecordable = false;
                    // Uncheck individual nomination
                    individualNominated = false;
                }
                else if (nominatesNewReport == true)
                {
                    // Call to report attribute
                    OnNextVisualisation();
                }
                else { }
                
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
