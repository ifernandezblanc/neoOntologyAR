/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 09/10/2019
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using TMPro;
using Microsoft.MixedReality.Toolkit.Utilities;
#endregion

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class ElementReport : MonoBehaviour, IElementable, IVisualisable
    {
        #region INITIALISATION_VARIABLES
        public AssetVisualiser visualiser;
        public OntologyElement classElement;
        public OntologyElement individualElement;
        public OntologyElement exampleElement;
        public GameObject previousElement;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public RtrbauElementType rtrbauElementType;
        public RtrbauElementLocation rtrbauLocationType;
        public RtrbauElement rtrbauElement;
        public JsonClassProperties classAttributes;
        public JsonIndividualValues exampleAttributes;
        public List<JsonClassProperties> objectClassesAttributes;
        public List<JsonClassIndividuals> objectClassesIndividuals;
        public JsonDistance elementComponentDistance;
        public JsonDistance elementOperationDistance;
        public List<RtrbauFabrication> assignedFabrications;
        public List<KeyValuePair<RtrbauFabrication, GameObject>> elementFabrications;
        public List<GameObject> unparentedFabrications;
        public List<GameObject> recordFabrications;
        public List<GameObject> nominateFabrications;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro classText;
        public TextMeshPro individualText;
        public TextMeshPro statusText;
        public MeshRenderer panelPrimary;
        public MeshRenderer panelSecondary;
        public Transform fabricationsRecordRest;
        public Transform fabricationsRecordImageVideo;
        public Transform fabricationsNominate;
        public Material reportedMaterial;
        public SpriteRenderer activationButton;
        public Sprite activationButtonMaximise;
        public Sprite activationButtonMinimise;
        public Material lineMaterial;
        public GameObject loadingPanel;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        public bool classDownloaded;
        public bool exampleDownloaded;
        public int objectPropertiesNumber;
        public bool componentDistanceDownloaded;
        public bool operationDistanceDownloaded;
        public bool fabricationsSelected;
        public bool fabricationsCreated;
        public bool fabricationsActive;
        public bool forcedReported;
        public bool elementReported;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Start() { }

        void Update()
        {
            if (Rtrbauer.instance.viewer != null)
            {
                this.transform.LookAt(Rtrbauer.instance.viewer.transform.position);
                this.transform.Rotate(0, 180, 0);
            }
        }

        void OnEnable() { }

        void OnDisable() { }

        void OnDestroy()
        {
            DestroyIt();
        }
        #endregion MONOBEHAVIOUR_METHODS

        #region INITIALISATION_METHODS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void Initialise(AssetVisualiser assetVisualiser, OntologyElement elementIndividual, OntologyElement elementClass, GameObject elementPrevious)
        {
            if (individualText == null || classText == null || statusText == null || fabricationsRecordRest == null || fabricationsRecordImageVideo == null || fabricationsNominate == null || panelPrimary == null || panelSecondary == null || reportedMaterial == null || activationButton == null || activationButtonMaximise == null || activationButtonMinimise == null || lineMaterial == null || loadingPanel == null)
            {
                Debug.LogError("ElementReport::Initialise: Fabrication not found. Please assign them in ElementReport script.");
            }
            else
            {
                if (elementClass.type == OntologyElementType.ClassProperties)
                {
                    rtrbauElementType = RtrbauElementType.Report;
                    rtrbauLocationType = RtrbauElementLocation.None;

                    visualiser = assetVisualiser;
                    individualElement = elementIndividual;
                    classElement = elementClass;
                    previousElement = elementPrevious;

                    objectClassesAttributes = new List<JsonClassProperties>();
                    objectClassesIndividuals = new List<JsonClassIndividuals>();

                    classDownloaded = false;
                    exampleDownloaded = false;

                    objectPropertiesNumber = 0;

                    componentDistanceDownloaded = false;
                    operationDistanceDownloaded = false;

                    fabricationsSelected = false;
                    fabricationsCreated = false;

                    fabricationsActive = false;

                    forcedReported = false;
                    elementReported = false;

                    assignedFabrications = new List<RtrbauFabrication>();
                    elementFabrications = new List<KeyValuePair<RtrbauFabrication, GameObject>>();
                    unparentedFabrications = new List<GameObject>();
                    nominateFabrications = new List<GameObject>();

                    AddLineRenderer();
                    DownloadElement();
                }
                else
                {
                    throw new ArgumentException("ElementReport::Initialise: ontology element type not implemented.");
                }
            }
        }
        #endregion INITIALISATION_METHODS

        #region IELEMENTABLE_METHODS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void DownloadElement()
        {
            Debug.Log("ElementReport::DownloadElement: start downloading class element:" + classElement.URL());

            // Download class structure: datatype and object properties
            LoaderEvents.StartListening(classElement.EventName(), DownloadedClass);
            Loader.instance.StartOntElementDownload(classElement);

            // Generate ontology element for class example
            exampleElement = new OntologyElement(classElement.entity.URI(), OntologyElementType.ClassExample);

            // Download class example: class individual with most object properties
            LoaderEvents.StartListening(exampleElement.EventName(), DownloadedExample);
            Loader.instance.StartOntElementDownload(exampleElement);

            // Download distance from class to component class
            OntologyDistance componentDistance = new OntologyDistance(classElement.entity.URI(), RtrbauDistanceType.Component);
            LoaderEvents.StartListening(componentDistance.EventName(), DownloadedComponentDistance);
            Loader.instance.StartOntDistanceDownload(componentDistance);

            // Download distance from class to operation class
            OntologyDistance operationDistance = new OntologyDistance(classElement.entity.URI(), RtrbauDistanceType.Operation);
            LoaderEvents.StartListening(operationDistance.EventName(), DownloadedOperationDistance);
            Loader.instance.StartOntDistanceDownload(operationDistance);
        }
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void EvaluateElement()
        {
            if (classDownloaded == true && exampleDownloaded == true)
            {
                Debug.Log("ElementReport::EvaluateElement: All class-related elements downloaded:" + classElement.entity.Entity());
                // New RtrbauElement declaration form to define report elements
                rtrbauElement = new RtrbauElement(Rtrbauer.instance.user.procedure, visualiser.manager, individualElement, classAttributes, exampleAttributes);
                // Check new individual name has been generated correctly
                Debug.Log("ElementReport::EvaluateElement: rtrbauElement created:" + rtrbauElement.elementName.Entity());
                // Call to next step
                SelectFabrications();
            }
            else { }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void SelectFabrications()
        {
            Debug.Log("ElementReport::SelectFabrications: rtrbau attributes to be evaluated against formats");
            // RTRBAU ALGORITHM: new extension (which original loop number is?)
            // Checks if format is acceptable
            // If so, then evaluates the format agains the element to determine if it is assignable
            // Replaces the toogled foreach loops below
            // UPG: similar to ElementConsult: make one single script for consult and report
            foreach (Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat> format in Dictionaries.ReportFormats)
            {
                // Check environment and user formats
                Tuple<RtrbauAugmentation, RtrbauInteraction> envFacets = format.Item3.EvaluateFormat();
                Tuple<RtrbauComprehensiveness, RtrbauDescriptiveness> userFacets = format.Item4.EvaluateFormat();

                if (envFacets != null && userFacets != null)
                {
                    Debug.Log("ElementReport::SelectFabrications: Fabrication available: " + format.Item1);
                    Debug.Log("ElementReport::SelectFabrications: " + format.Item2.formatName + " required facets: " + format.Item2.formatRequiredFacets);

                    // Determine if format is assignable as fabrications to the element
                    List<RtrbauFabrication> formatAssignedFabrications = format.Item2.EvaluateFormat(rtrbauElement);
                    // FABRICATIONS CREATION TRIAL: USE PREVIOUS WHEN COMPLETED
                    //List<RtrbauFabrication> formatAssignedFabrications = null;

                    // Assign fabrications
                    if (formatAssignedFabrications != null)
                    {
                        foreach (RtrbauFabrication fabrication in formatAssignedFabrications)
                        {
                            Debug.Log("ElementReport::SelectFabrications: Fabrication assigned: " + fabrication.fabricationName);
                            // Add environment and user features to fabrication
                            // UPG: since environment and user formats are being evaluated before, there is no need to add them to RtrbauFabrication class?
                            fabrication.fabricationAugmentation = format.Item3.formatAugmentation.facetAugmentation;
                            fabrication.fabricationInteraction = format.Item3.formatInteraction.facetInteraction;
                            fabrication.fabricationComprehension = format.Item4.formatComprehension;
                            fabrication.fabricationDescription = format.Item4.formatDescription;
                            // Add fabrication to assigned fabrications list
                            assignedFabrications.Add(fabrication);
                        }
                    }
                }
                else { }
            }

            // RTRBAU ALGORITHM: Eliminate duplicated fabrications (Loop 5 modified)
            // Eliminate duplicated formats with lower number of attributes
            // Duplicates are those with similar source attributes and augmentation method
            foreach (RtrbauAugmentation augmentation in Enum.GetValues(typeof(RtrbauAugmentation)))
            {
                // Find similar source attribute fabrications with similar augmentation method
                List<RtrbauFabrication> similarAugmentationFabrications = assignedFabrications.FindAll(x => x.fabricationAugmentation == augmentation);

                // In case there is more than one similar fabrication
                if (similarAugmentationFabrications.Count > 1)
                {
                    foreach (RtrbauAttribute attribute in rtrbauElement.elementAttributes)
                    {
                        // Find fabrications with similar source attributes
                        List<RtrbauFabrication> similarSourceFabrications = similarAugmentationFabrications.FindAll(x => x.fabricationData.Any(y => y.Key.facetForm == RtrbauFacetForm.source && y.Value.attributeName == attribute.attributeName && y.Value.attributeValue == attribute.attributeValue));

                        if (similarSourceFabrications.Count > 1)
                        {
                            // UPG: consider where no. of attribute equals 1, then modify according to user and environment formats
                            // Order similar fabrications by number of attributes
                            similarSourceFabrications.Sort((x, y) => x.fabricationData.Count.CompareTo(y.fabricationData.Count));

                            // If highest-attributes-number fabrication has only one, then compare facet restrictivity
                            if (similarSourceFabrications[similarSourceFabrications.Count() - 1].fabricationData.Count() == 1)
                            {
                                // It assumes all fabrications have only one facet
                                // Order similar fabrications by facet restrictivity on single attribute
                                similarSourceFabrications.Sort((x, y) => x.fabricationData.First().Key.facetRules.facetRestrictivity.CompareTo(y.fabricationData.First().Key.facetRules.facetRestrictivity));

                                foreach (RtrbauFabrication fabrication in similarSourceFabrications)
                                {
                                    Debug.Log("ElementReport::SelectFabrications: similar fabrication restrictiviy-ordered is " + fabrication.fabricationName + " for attribute " + attribute.attributeName.Name());
                                }

                                // If facet restrictivity is equal, then compare format senses
                                if (similarSourceFabrications[similarSourceFabrications.Count() - 1].fabricationData.First().Key.facetRules.facetRestrictivity == similarSourceFabrications[similarSourceFabrications.Count() - 2].fabricationData.First().Key.facetRules.facetRestrictivity)
                                {
                                    // UPG: to ensure senses are treated accordingly to proper rules rather than enum comparison (declaration order)
                                    similarSourceFabrications.Sort((x, y) => x.fabricationInteraction.CompareTo(y.fabricationInteraction));

                                    foreach (RtrbauFabrication fabrication in similarSourceFabrications)
                                    {
                                        Debug.Log("ElementReport::SelectFabrications: similar fabrication interaction-ordered is " + fabrication.fabricationName + " for attribute " + attribute.attributeName.Name());
                                    }
                                }
                                else
                                {
                                    // UPG: to ensure all facet restrictivities are considered, not only first two fabrications on attributes-sorted list
                                }
                            }
                            else
                            {
                                // UPG: to ensure similar fabrications are re-sorted when number of attributes are equal but different to 1
                                // UPG: for cases when there is more than one attribute, but number of attributes still equal for more than one similar fabrication
                            }

                            // Remove as duplicated the fabrication with the highest number of attributes or by facet restrictivity when number of attributes equals 1
                            similarSourceFabrications.RemoveAt(similarSourceFabrications.Count() - 1);
                            // Remove rest of duplicated fabrications from assignedFabrications
                            for (int i = 0; i < similarSourceFabrications.Count; i++)
                            {
                                assignedFabrications.Remove(similarSourceFabrications[i]);
                            }
                        }
                        else { }
                    }
                }
                else { }
            }

            // RTRBAU ALGORITHM: Identify non-assigned attributes and create default fabrications for them (new extension)
            // UPG: Merge with following foreach loop to increase speed, long loops for very few cases
            // UPG: An idea to reduce loops is as follows: List<RtrbauAttribute> nonAssAtt = rtrbauElement.elementAttributes.Where(x => assignedFabrications.All(y => y.fabricationData.Values.ToList().All(z => z.attributeValue != x.attributeValue))).ToList();
            // UPG: This extension could be discarded in case it is ensured by design that non-assigned attributes won't exist

            // Identify attributes assigned to generated fabrications
            List<RtrbauAttribute> assignedAttributes = new List<RtrbauAttribute>();

            foreach (RtrbauFabrication fabrication in assignedFabrications)
            {
                assignedAttributes.AddRange(fabrication.fabricationData.Values.ToList());
            }

            foreach (RtrbauAttribute attribute in assignedAttributes)
            {
                Debug.Log("ElementReport::SelectFabrications: assigned attributes: " + attribute.attributeName.Name() + " : " + attribute.attributeValue);
            }

            // Identify attributes that have not been assigned to generated fabrications
            List<RtrbauAttribute> nonAssignedAttributes = rtrbauElement.elementAttributes.Where(x => assignedAttributes.All(y => y.attributeValue != x.attributeValue)).ToList();

            // Generate default fabrications for non-assigned attributes
            // UPG: similar to ElementConsult: make one single script for consult and report
            foreach (RtrbauAttribute attribute in nonAssignedAttributes)
            {
                // Debug.Log("EvaluateElement: non assignedAttribute: " + attribute.attributeName.name + " : " + attribute.attributeValue);

                if (attribute.fabricationType == RtrbauFabricationType.Record)
                {
                    // Create new fabrication for non-assigned attribute
                    RtrbauFabrication fabrication = new RtrbauFabrication(RtrbauFabricationName.DefaultRecord, RtrbauFabricationType.Record, new Dictionary<DataFacet, RtrbauAttribute>
                    {
                        { DataFormats.DefaultRecord.formatFacets[0], attribute }
                    });
                    // UPG: since environment and user formats are being evaluated before, there is no need to add them to RtrbauFabrication class?
                    fabrication.fabricationAugmentation = EnvironmentFormats.DefaultRecord.formatAugmentation.facetAugmentation;
                    fabrication.fabricationInteraction = EnvironmentFormats.DefaultRecord.formatInteraction.facetInteraction;
                    fabrication.fabricationComprehension = UserFormats.DefaultRecord.formatComprehension;
                    fabrication.fabricationDescription = UserFormats.DefaultRecord.formatDescription;
                    // Assign fabrication to assignedFabrications
                    assignedFabrications.Add(fabrication);
                    Debug.Log("ElementReport::SelectFabrications: attribute with DefaultRecord is: " + attribute.attributeName.Name());
                }
                else if (attribute.fabricationType == RtrbauFabricationType.Nominate)
                {
                    // Create new fabrication for non-assigned attribute
                    RtrbauFabrication fabrication = new RtrbauFabrication(RtrbauFabricationName.DefaultNominate, RtrbauFabricationType.Nominate, new Dictionary<DataFacet, RtrbauAttribute>
                    {
                        { DataFormats.DefaultNominate.formatFacets[0], attribute }
                    });
                    // UPG: since environment and user formats are being evaluated before, there is no need to add them to RtrbauFabrication class?
                    fabrication.fabricationAugmentation = EnvironmentFormats.DefaultNominate.formatAugmentation.facetAugmentation;
                    fabrication.fabricationInteraction = EnvironmentFormats.DefaultNominate.formatInteraction.facetInteraction;
                    fabrication.fabricationComprehension = UserFormats.DefaultNominate.formatComprehension;
                    fabrication.fabricationDescription = UserFormats.DefaultNominate.formatDescription;
                    // Assign fabrication to assignedFabrications
                    assignedFabrications.Add(fabrication);
                    Debug.Log("ElementReport::SelectFabrications: attribute with DefaultNominate is: " + attribute.attributeName.Name());
                }
                else
                {
                    throw new ArgumentException("ElementReport::SelectFabrications: Default fabrications not implemented for fabrication type: " + attribute.fabricationType);
                }
            }

            foreach (RtrbauFabrication fab in assignedFabrications)
            {
                Debug.Log("ElementReport::SelectFabrications: fabrication evaluated: " + fab.fabricationName);
            }

            fabricationsSelected = true;
            CreateFabrications();
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void LocateElement()
        {
            // RTRBAU ALGORITHM: Select element location (Loop 8 final)
            if (componentDistanceDownloaded == true && operationDistanceDownloaded == true && fabricationsCreated == true)
            {
                int componentDistance = int.Parse(elementComponentDistance.ontDistance);
                int operationDistance = int.Parse(elementOperationDistance.ontDistance);

                Debug.Log("ElementReport::LocateElement: component distance is " + componentDistance);
                Debug.Log("ElementReport::LocateElement: operation distance is " + operationDistance);

                if (componentDistance <= 1)
                {
                    rtrbauLocationType = RtrbauElementLocation.Primary;
                }
                else if (operationDistance >= 1)
                {
                    rtrbauLocationType = RtrbauElementLocation.Secondary;
                }
                else
                {
                    rtrbauLocationType = RtrbauElementLocation.Tertiary;
                }

                Debug.Log("ElementReport::LocateElement: rtrbau location is " + rtrbauLocationType);

                LocateIt();
            }
            else { }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        /// <returns>Describe script outcomes</returns>
        public bool DestroyElement()
        {
            // Check if element reported
            if (elementReported == true)
            {
                // Check if any nominate report still exist
                if (nominateFabrications.Count == 0)
                {
                    // Destroy game object
                    Destroy(this.gameObject);
                    // Return game object was destroyed
                    return true;
                }
                else { return false; }
            }
            else { return false; }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void CreateFabrications()
        {
            Debug.Log("ElementReport::CreateFabrications: Number of attributes downloadable: " + objectPropertiesNumber);
            Debug.Log("ElementReport::CreateFabrications: Number of attribute classes downloaded: " + objectClassesAttributes.Count);
            Debug.Log("ElementReport::CreateFabrications: Number of attribute individuals downloaded: " + objectClassesIndividuals.Count);
            Debug.Log("ElementReport::CreateFabrications: Fabrications selected is: " + fabricationsSelected);

            // Check fabrications selected as well as object classes attributes and individuals have been downloaded
            if (objectClassesAttributes.Count == objectPropertiesNumber && objectClassesIndividuals.Count == objectPropertiesNumber && fabricationsSelected == true)
            {
                individualText.text = rtrbauElement.elementName.Name();
                classText.text = rtrbauElement.elementClass.Name();

                Debug.Log("ElementReport::CreateFabrications: Starting to create fabrications for: " + rtrbauElement.elementName.Name());

                foreach (RtrbauFabrication fabrication in assignedFabrications)
                {
                    // UPG: list to know which script (class) to get component for
                    // UPG: create a list maybe with prefabs pre-loaded to save time?
                    // UPG: where to create list? would it be a dynamic dictionary?

                    // Find fabrication GO by name in Rtrbauer dynamic library
                    GameObject fabricationGO = Rtrbauer.instance.FindFabrication(fabrication.fabricationName, RtrbauElementType.Report);

                    if (fabricationGO != null)
                    {
                        GameObject goFabrication = Instantiate(fabricationGO);
                        KeyValuePair<RtrbauFabrication, GameObject> fabricationPair = new KeyValuePair<RtrbauFabrication, GameObject>(fabrication, goFabrication);
                        elementFabrications.Add(fabricationPair);
                        ScaleFabrication(goFabrication);
                        LocateFabrication(fabricationPair);
                    }
                    else
                    {
                        Debug.LogError("ElementReport::CreateFabrications: " + fabrication.fabricationName + " is not implemented");
                    }
                }

                // Set fabrications as active
                fabricationsActive = true;
                statusText.text = "Element maximised, click to hide information";

                // Disable tile grid object collection from side panel to allow image manipulation
                // UPG: do it with other fabrication panel as well?
                fabricationsRecordImageVideo.GetComponent<TileGridObjectCollection>().enabled = false;

                Debug.Log("ElementReport::CreateFabrications: fabrications created for: " + rtrbauElement.elementName.Name() + " and values emptied");
                // Clean RtrbauElement attributes values when fabrications created for further update on user report
                CleanRtrbauElement();

                // Set fabrications as created
                fabricationsCreated = true;
                LocateElement();
            }
            else
            {
                // UPG: ErrorHandling: Fabrications couldn't be selected, return to previous element and destroy this one
            }
        }


        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void InputIntoReport()
        {
            // Create JsonIndividualValues from RtrbauElement
            JsonIndividualValues reportedIndividual = new JsonIndividualValues();

            reportedIndividual.ontIndividual = rtrbauElement.elementName.URI();
            reportedIndividual.ontClass = rtrbauElement.elementClass.URI();

            // Avoids to include attributes whose values have not been reported
            foreach (RtrbauAttribute attribute in rtrbauElement.elementAttributes)
            {
                if (attribute.attributeValue != null)
                {
                    JsonValue attributeValue = new JsonValue();
                    attributeValue.ontName = attribute.attributeName.URI();
                    attributeValue.ontValue = attribute.attributeValue;
                    attributeValue.ontType = attribute.attributeType.URI();
                    reportedIndividual.ontProperties.Add(attributeValue);
                }
                else { }
            }

            // Create individualElement to write JsonIndividualValues
            // OntologyElement individualElement = new OntologyElement(rtrbauElement.elementName.URI(), OntologyElementType.IndividualProperties);
            // Write JsonFile for JsonIndividualValues
            File.WriteAllText(individualElement.FilePath(), JsonUtility.ToJson(reportedIndividual));

            Debug.Log("ElementReport::InputIntoReport: individual element is: " + individualElement.entity.Entity());
            Debug.Log("ElementReport::InputIntoReport: class element is: " + classElement.entity.Entity());

            // Create OntologyElementUpload element to upload individualElement to server
            OntologyElementUpload individualUpload = new OntologyElementUpload(individualElement, classElement);

            // Upload ontology individual reported to server
            LoaderEvents.StartListening(individualUpload.EventName(), FinaliseReporting);
            Loader.instance.StartOntElementUpload(individualUpload);

            // Deactivate fabrications
            ActivateIt();
            // Set status panel to uploading element
            statusText.text = "Element reported being updated to the server";
            // Activate loading panel
            loadingPanel.SetActive(true);

            // Final actions to be completed on FinaliseReporting()
        }
        #endregion IELEMENTABLE_METHODS

        #region IVISUALISABLE_METHODS
        /// <summary>
        /// Locates <see cref="IFabricationable"/> elements created by <see cref="ElementReport"/>.
        /// </summary>
        public void LocateIt()
        {
            // Modified to reduce foreach loop by connecting with CreateFabrications
            // Foreach loop now implemented as LocateFabrication function within CreateFabrications loop
            // Rest of functionality remains
            // UPG: Add ordering for tiled fabrications (buttons, icons, text).

            // ElementConsult location is managed by its visualiser.
            RtrbauerEvents.TriggerEvent("AssetVisualiser_LocateElement", individualElement, rtrbauElementType, rtrbauLocationType);
        }

        /// <summary>
        /// Activates or deactivates <see cref="IFabricationable"/> elements managed by <see cref="ElementReport"/> according to current state.
        /// </summary>
        public void ActivateIt()
        {
            // For fabrications with additional unparented fabrications, remember to add behaviour OnEnable and OnDisable
            if (fabricationsActive)
            {
                foreach (KeyValuePair<RtrbauFabrication, GameObject> fabrication in elementFabrications)
                {
                    fabrication.Value.SetActive(false);
                }

                fabricationsActive = false;
                statusText.text = "Element minimised, click to show information";
                activationButton.sprite = activationButtonMaximise;
                activationButton.size = new Vector2(0.75f, 0.75f);
            }
            else
            {
                foreach (KeyValuePair<RtrbauFabrication, GameObject> fabrication in elementFabrications)
                {
                    fabrication.Value.SetActive(true);
                }

                fabricationsActive = true;
                statusText.text = "Element maximised, click to hide information";
                activationButton.sprite = activationButtonMinimise;
                activationButton.size = new Vector2(0.75f, 0.75f);
            }
        }

        /// <summary>
        /// Destroys <see cref="IFabricationable"/> elements created by <see cref="ElementReport"/>.
        /// </summary>
        public void DestroyIt()
        {
            foreach (GameObject fabrication in recordFabrications)
            {
                Destroy(fabrication);
            }

            foreach (GameObject fabrication in nominateFabrications)
            {
                Destroy(fabrication);
            }
        }

        /// <summary>
        /// Modifies <see cref="ElementReport"/> materials as well as those <see cref="IVisualisable"/> managed elements.
        /// </summary>
        public void ModifyMaterial(Material material)
        {
            if (elementReported == false)
            {
                // Set material of element panels
                panelPrimary.material = material;
                panelSecondary.material = material;
                //panelTertiary.material = material;

                // Set material of fabrication panels
                foreach (KeyValuePair<RtrbauFabrication, GameObject> fabrication in elementFabrications)
                {
                    fabrication.Value.GetComponent<IVisualisable>().ModifyMaterial(material);
                }
            }
            else
            {
                // Set material of element panels
                panelPrimary.material = material;
                panelSecondary.material = material;

                // Set material of fabrication panels
                foreach (GameObject fabrication in nominateFabrications)
                {
                    fabrication.GetComponent<IVisualisable>().ModifyMaterial(material);
                }
            }
        }
        #endregion IVISUALISABLE_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementClass"></param>
        void DownloadedClass(OntologyElement elementClass)
        {
            LoaderEvents.StopListening(elementClass.EventName(), DownloadedClass);

            if (File.Exists(classElement.FilePath()))
            {
                string jsonFile = File.ReadAllText(classElement.FilePath());

                classAttributes = JsonUtility.FromJson<JsonClassProperties>(jsonFile);

                // Check if object properties have already been evaluated (next foreach loop)
                List<string> objectProperties = new List<string>();

                // Download individuals list from class object properties
                foreach (JsonProperty objectProperty in classAttributes.ontProperties)
                {
                    if (objectProperty.ontType.Contains(OntologyPropertyType.ObjectProperty.ToString()))
                    {
                        if (!objectProperties.Contains(objectProperty.ontRange))
                        {
                            // Create OntologyElement to download attribute class
                            OntologyElement attributeClass = new OntologyElement(objectProperty.ontRange, OntologyElementType.ClassProperties);
                            // Download attribute (relationship) class to objectClassesAttributes
                            LoaderEvents.StartListening(attributeClass.EventName(), DownloadedAttributeClass);
                            Loader.instance.StartOntElementDownload(attributeClass);
                            // Create OntologyElement to download attribute individuals
                            OntologyElement attributeIndividuals = new OntologyElement(objectProperty.ontRange, OntologyElementType.ClassIndividuals);
                            // Download attribute (relationship) individuals to objectClassesIndividuals
                            LoaderEvents.StartListening(attributeIndividuals.EventName(), DownloadedAttributeIndividuals);
                            Loader.instance.StartOntElementDownload(attributeIndividuals);
                            // Add non-repeated attribute to downloadable classes
                            objectProperties.Add(objectProperty.ontRange);
                        }
                        else { }
                    }
                    else { }
                }

                // Note non-repeated property as downloadable classes: in case the foreach loop is slower than loader events or viceversa
                objectPropertiesNumber = objectProperties.Count;

                // Ensure class element has been downloaded
                classDownloaded = true;

                // Call to next step: in case the foreach loop is slower than loader events or viceversa
                EvaluateElement();
            }
            else
            {
                Debug.LogError("ElementReport::DownloadedClass: File not found: " + classElement.FilePath());
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementExample"></param>
        void DownloadedExample(OntologyElement elementExample)
        {
            LoaderEvents.StopListening(elementExample.EventName(), DownloadedExample);

            if (File.Exists(exampleElement.FilePath()))
            {
                string jsonFile = File.ReadAllText(exampleElement.FilePath());

                exampleAttributes = JsonUtility.FromJson<JsonIndividualValues>(jsonFile);

                exampleDownloaded = true;

                EvaluateElement();
            }
            else
            {
                Debug.LogError("ElementReport::DownloadedExample: File not found: " + exampleElement.FilePath());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementAttributeClass"></param>
        void DownloadedAttributeClass(OntologyElement elementAttributeClass)
        {
            LoaderEvents.StopListening(elementAttributeClass.EventName(), DownloadedAttributeClass);

            if (File.Exists(elementAttributeClass.FilePath()))
            {
                string jsonFile = File.ReadAllText(elementAttributeClass.FilePath());

                objectClassesAttributes.Add(JsonUtility.FromJson<JsonClassProperties>(jsonFile));

                CreateFabrications();
            }
            else
            {
                Debug.LogError("ElementReport::DownloadedAttributeClass: File not found: " + elementAttributeClass.FilePath());
            }
        }
        /// <summary>
        /// 
        /// UPG: function can be merged with previous, same downloader only changes element type
        /// </summary>
        /// <param name="elementAttributeIndividuals"></param>
        void DownloadedAttributeIndividuals(OntologyElement elementAttributeIndividuals)
        {
            LoaderEvents.StopListening(elementAttributeIndividuals.EventName(), DownloadedAttributeIndividuals);

            if (File.Exists(elementAttributeIndividuals.FilePath()))
            {
                string jsonFile = File.ReadAllText(elementAttributeIndividuals.FilePath());

                objectClassesIndividuals.Add(JsonUtility.FromJson<JsonClassIndividuals>(jsonFile));

                CreateFabrications();
            }
            else
            {
                Debug.LogError("ElementReport::DownloadedAttributeIndividuals: File not found: " + elementAttributeIndividuals.FilePath());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentDistance"></param>
        void DownloadedComponentDistance(OntologyDistance componentDistance)
        {
            LoaderEvents.StopListening(componentDistance.EventName(), DownloadedComponentDistance);

            if (File.Exists(componentDistance.FilePath()))
            {
                string jsonFile = File.ReadAllText(componentDistance.FilePath());

                elementComponentDistance = JsonUtility.FromJson<JsonDistance>(jsonFile);

                componentDistanceDownloaded = true;

                LocateElement();
            }
            else
            {
                Debug.LogError("ElementReport::DownloadedComponentDistance: File not found: " + componentDistance.FilePath());
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationDistance"></param>
        void DownloadedOperationDistance(OntologyDistance operationDistance)
        {
            LoaderEvents.StopListening(operationDistance.EventName(), DownloadedOperationDistance);

            if (File.Exists(operationDistance.FilePath()))
            {
                string jsonFile = File.ReadAllText(operationDistance.FilePath());

                elementOperationDistance = JsonUtility.FromJson<JsonDistance>(jsonFile);

                operationDistanceDownloaded = true;

                LocateElement();
            }
            else
            {
                Debug.LogError("ElementReport::DownloadedOperationDistance: File not found: " + operationDistance.FilePath());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void LocateFabrication(KeyValuePair<RtrbauFabrication, GameObject> fabrication)
        {
            // UPG: Add ordering for tiled fabrications (buttons, icons, text).
            if (fabrication.Key.fabricationType == RtrbauFabricationType.Record)
            {
                if (fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Text ||
                fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Icon ||
                fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Audio)
                {
                    fabrication.Value.transform.SetParent(fabricationsRecordRest, false);
                    fabrication.Value.GetComponent<IFabricationable>().Initialise(visualiser, fabrication.Key, this.transform, this.transform);
                }
                else if (fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Image ||
                fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Video)
                {
                    // fabrication.Value.transform.SetParent(fabricationsRecordImageVideo, false);
                    fabrication.Value.transform.SetParent(fabricationsRecordRest, false);
                    fabrication.Value.GetComponent<IFabricationable>().Initialise(visualiser, fabrication.Key, this.transform, this.transform);
                }
                else if (fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Model ||
                fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Hologram ||
                fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Animation)
                {
                    fabrication.Value.transform.SetParent(fabricationsRecordRest, false);
                    fabrication.Value.GetComponent<IFabricationable>().Initialise(visualiser, fabrication.Key, this.transform, visualiser.transform);
                }
                else { }
                recordFabrications.Add(fabrication.Value);
            }
            else if (fabrication.Key.fabricationType == RtrbauFabricationType.Nominate)
            {
                fabrication.Value.transform.SetParent(fabricationsNominate, false);
                fabrication.Value.GetComponent<IFabricationable>().Initialise(visualiser, fabrication.Key, this.transform, this.transform);
                nominateFabrications.Add(fabrication.Value);
            }
            else
            {
                throw new ArgumentException("ElementReport::LocateIt: Fabrication type not implemented");
            }
        }

        /// <summary>
        /// Adapts <paramref name="fabrication"/> scale (only x and y) to adapt to element size
        /// </summary>
        /// <param name="fabrication"></param>
        void ScaleFabrication(GameObject fabrication)
        {
            // Re-scale fabrication to match horizontal element scale (x-axis) after being re-scaled
            // It assumes original fabrication and element were already scaled properly
            if (this.transform.localScale.x > fabrication.transform.localScale.x)
            {
                // float sM = originalElementScaleX / fabrication.transform.localScale.x;
                float sM = this.transform.localScale.x / fabrication.transform.localScale.x;
                float sX = fabrication.transform.localScale.x * sM;
                float sY = fabrication.transform.localScale.y * sM;
                float sZ = fabrication.transform.localScale.z;

                fabrication.transform.localScale = new Vector3(sX, sY, sZ);
            }
            else { }
        }

        /// <summary>
        /// Assign null to <see cref="RtrbauAttribute.attributeValue"/> variables to clean <see cref="RtrbauElement"/> for user to report new.
        /// </summary>
        void CleanRtrbauElement()
        {
            // Find attribute values and assign null awaiting for user reported values
            foreach (RtrbauAttribute attribute in rtrbauElement.elementAttributes)
            {
                attribute.attributeValue = null;
            }

            Debug.Log("ElementReport::CleanRtrbauElement: individual emptied is: " + rtrbauElement.elementName.Name());

            foreach (RtrbauAttribute attribute in rtrbauElement.elementAttributes)
            {
                Debug.Log("ElementReport::CleanRtrbauElement: attribute is: " + attribute.attributeName.Name() + " whose value is: " + attribute.attributeValue);
            }
        }

        /// <summary>
        /// Identifies nominate fabrications that report new elements to report.
        /// Destroys the rest of fabrications of all types.
        /// </summary>
        void AssignNewReportElements()
        {
            // Create list for new nominate fabrications
            List<GameObject> reportElementFabrications = new List<GameObject>();

            // Check if nominate fabrication reports new element, otherwise destroy
            foreach (GameObject nominate in nominateFabrications)
            {
                if (nominate.GetComponent<INominatable>().NominatesNewReportElement(forcedReported))
                {
                    reportElementFabrications.Add(nominate);
                }
                else { Destroy(nominate); }
            }

            // Assign new list of nominate fabrications
            nominateFabrications = reportElementFabrications;

            // Destroy all record fabrications
            foreach (GameObject record in recordFabrications)
            {
                Destroy(record);
            }

            // Assign empty list of record fabrications
            recordFabrications = new List<GameObject>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uploadElement"></param>
        void FinaliseReporting(OntologyElementUpload uploadElement)
        {
            LoaderEvents.StopListening(uploadElement.EventName(), FinaliseReporting);

            // Create class individuals element for rtrbauElement class
            OntologyElement classIndividuals = new OntologyElement(rtrbauElement.elementClass.URI(), OntologyElementType.ClassIndividuals);
            // If json file exists, delete it to enable re-download with new element reported
            // UPG: ensure no events are calling to the same file at the same time
            if (File.Exists(classIndividuals.FilePath())) { File.Delete(classIndividuals.FilePath()); }

            // Report rtrbauElement to reporter
            Reporter.instance.ReportElement(rtrbauElement.elementName);

            // Activate fabrications
            ActivateIt();

            // Assign new report elements
            AssignNewReportElements();

            // Update status text
            statusText.text = "Element reported, please click on the buttons below to continue reporting one of those elements.";

            // Assign element as reported
            elementReported = true;

            // Deactivate loading panel
            loadingPanel.SetActive(false);

            // Update rtrbauLocation
            rtrbauLocationType = RtrbauElementLocation.Quaternary;

            // Relocate element in quaternary location
            RtrbauerEvents.TriggerEvent("AssetVisualiser_LocateElement", individualElement, rtrbauElementType, rtrbauLocationType);
        }

        /// <summary>
        /// 
        /// </summary>
        void AttributesReported()
        {
            // Modify element panels materials to reported
            panelPrimary.material = reportedMaterial;
            panelSecondary.material = reportedMaterial;
            // Modify fabrications panels materials to reported
            ModifyMaterial(reportedMaterial);
            // Update status text to show all attributes have been reported
            statusText.text = "All attributes reported, please click on the next attribute to report.";
            // Submit reported rtrbauElement for reporting
            Debug.Log("ElementReport::AttributesReported: all values reported, inputting new individual into report");
            // Report rtrbauElement
            InputIntoReport();
        }

        /// <summary>
        /// 
        /// </summary>
        void AddLineRenderer()
        {
            if (previousElement != null)
            {
                this.gameObject.AddComponent<ElementsLine>().Initialise(this.gameObject, previousElement, lineMaterial);
            }
        }
        #endregion PRIVATE

        #region PUBLIC
        /// <summary>
        /// Deactivates record buttons from all other record fabrications that are not active.
        /// Pass null as argument if all records buttons to be deactivated.
        /// </summary>
        /// <param name="activeRecord"></param>
        public void DeactivateRecords(GameObject activeRecord)
        {
            List<GameObject> deactivateRecords = recordFabrications.FindAll(x => x != activeRecord);

            foreach (GameObject record in deactivateRecords)
            {
                record.GetComponent<IRecordable>().DeactivateRecords();
            }
        }

        /// <summary>
        /// Deactivates nominate buttons from all other nominate fabrications that are not active.
        /// Pass null as argument if all nominates buttons to be deactivated.
        /// </summary>
        /// <param name="activeNominate"></param>
        public void DeactivateNominates(GameObject activeNominate)
        {
            List<GameObject> deactivateNominates = nominateFabrications.FindAll(x => x != activeNominate);

            foreach (GameObject nominate in deactivateNominates)
            {
                nominate.GetComponent<INominatable>().DeactivateNominates();
            }
        }

        /// <summary>
        /// Checks if <see cref="RtrbauElement"/> has been updated with values for each <see cref="RtrbauAttribute"/>.
        /// These represent properties and relationships for ontology individuals recorded.
        /// </summary>
        /// <returns>
        /// Returns true if all attribute values are different from null.
        /// It turns the element to green if all attributes have been recorded.
        /// </returns>
        public bool CheckAttributesReported()
        {
            // Identify attributes whose values have not been reported
            List<RtrbauAttribute> nonReportedAttributes = rtrbauElement.elementAttributes.FindAll(x => x.attributeValue == null);

            Debug.Log("ElementReport::CheckAttributesReported: checking if all attribute values reported...");

            foreach (RtrbauAttribute attribute in rtrbauElement.elementAttributes) 
            {
                Debug.Log("ElementReport::CheckAttributesReported: attribute " + attribute.attributeName.Name() + " has value " + attribute.attributeValue);
            }

            // If there are attributes with non-reported values
            if (nonReportedAttributes.Count == 0)
            {
                Debug.Log("ElementReport::CheckAttributesReported: all attribute values reported.");
                // Report rtrbauElement
                AttributesReported();
                return true;
            }
            else
            {
                Debug.Log("ElementReport::CheckAttributesReported: some attribute values still to be reported.");
                // Update status text to show all attributes have not been reported
                statusText.text = "Not all attributes reported, please complete them and click again for reporting.";
                return false;
            }
        }

        /// <summary>
        /// Forces element to be reported even if not all attributes have been reported.
        /// </summary>
        public void ForceAttributesReported()
        {
            // Asigned forcedReported as true
            forcedReported = true;
            // Report rtrbauElement
            AttributesReported();
        }

        /// <summary>
        /// Removes the nominate button <paramref name="nominate"/> and checks if all new nominates have been reported.
        /// If so, unloads element from visualiser.
        /// </summary>
        /// <param name="nominate"></param>
        /// <returns>Returns true always.</returns>
        public bool CheckNewNominatesReported(GameObject nominate)
        {
            if (elementReported == true)
            {
                // Remove new nominate button from list
                nominateFabrications.Remove(nominate);
                // Destroy new nominate button
                Destroy(nominate);
                // Unload RtrbauElement if no more new nominates reported are left
                // if (nominateFabrications.Count() == 0) { visualiser.UnloadElement(this.gameObject); }
                // Return true
                return true;
            }
            else { throw new ArgumentException("ElementReport::CheckNewNominatesReported: this function should not be reached if element not reported."); }
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}