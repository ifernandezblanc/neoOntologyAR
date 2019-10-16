/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 02/08/2019
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
    public class ElementConsult : MonoBehaviour, IElementable, IVisualisable
    {
        #region INITIALISATION_VARIABLES
        public AssetVisualiser visualiser;
        public OntologyElement individualElement;
        public OntologyElement classElement;
        public GameObject previousElement;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public RtrbauElementType elementType;
        public JsonIndividualValues individualAttributes;
        public JsonClassProperties classAttributes;
        public List<JsonClassProperties> objectClassesAttributes;
        public JsonDistance elementComponentDistance;
        public JsonDistance elementOperationDistance;
        public RtrbauElement rtrbauElement;
        public List<RtrbauFabrication> assignedFabrications;
        public List<KeyValuePair<RtrbauFabrication,GameObject>> elementFabrications;
        public RtrbauElementLocation rtrbauLocation;
        // public List<GameObject> noChildFabrications;
        // public List<GameObject> childFabrications;
        public List<GameObject> unparentedFabrications;
        public float originalElementScaleX;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro classText;
        public TextMeshPro individualText;
        public TextMeshPro statusText;
        public MeshRenderer panelPrimary;
        public MeshRenderer panelSecondary;
        public Transform fabricationsObserveRest;
        public Transform fabricationsObserveImageVideo;
        public Transform fabricationsInspect;
        public Material seenMaterial;
        public SpriteRenderer activationButton;
        public Sprite activationButtonMaximise;
        public Sprite activationButtonMinimise;
        // private GameObject viewer;
        public Material lineMaterial;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        public int objectClassesNumber;
        public bool fabricationsSelected;
        public bool componentDistanceDownloaded;
        public bool operationDistanceDownloaded;
        public bool materialChanged;
        public bool fabricationsActive;
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
        public void Initialise(AssetVisualiser assetVisualiser, OntologyElement elementOntology, GameObject elementPrevious)
        {
            if (individualText == null || classText == null || statusText == null || fabricationsObserveRest == null || fabricationsObserveImageVideo == null || fabricationsInspect == null || panelPrimary == null || panelSecondary == null || seenMaterial == null || activationButton == null || activationButtonMaximise == null || activationButtonMinimise == null || lineMaterial == null) 
            {
                Debug.LogError("ElementConsult::Initialise: Fabrication not found. Please assign them in ElementConsult script.");
            }
            else
            {
                if (elementOntology.type == OntologyElementType.IndividualProperties)
                {
                    // lineMaterial = Resources.Load("Rtrbau/Materials/RtrbauMaterialStandardBlue") as Material;
                    // viewer = GameObject.FindGameObjectWithTag("MainCamera");

                    elementType = RtrbauElementType.Consult;

                    visualiser = assetVisualiser;
                    individualElement = elementOntology;
                    previousElement = elementPrevious;

                    objectClassesAttributes = new List<JsonClassProperties>();
                    objectClassesNumber = 0;

                    fabricationsSelected = false;

                    componentDistanceDownloaded = false;
                    operationDistanceDownloaded = false;

                    materialChanged = false;

                    fabricationsActive = false;

                    assignedFabrications = new List<RtrbauFabrication>();
                    elementFabrications = new List<KeyValuePair<RtrbauFabrication,GameObject>>();
                    //noChildFabrications = new List<GameObject>();
                    //childFabrications = new List<GameObject>();
                    unparentedFabrications = new List<GameObject>();

                    originalElementScaleX = this.transform.localScale.x;

                    AddLineRenderer();
                    DownloadElement();
                }
                else
                {
                    throw new ArgumentException("ElementConsult::Initialise: ontology element type not implemented.");
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
            Debug.Log("ElementConsult::DownloadElement: start downloading individual element:" + individualElement.entity.Entity());

            LoaderEvents.StartListening(individualElement.EventName(), DownloadedIndividual);
            Loader.instance.StartOntElementDownload(individualElement);
        }
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void EvaluateElement()
        {
            Debug.Log("ElementConsult::EvaluateElement: Number of attribute classes downloadable: " + objectClassesNumber);
            Debug.Log("ElementConsult::EvaluateElement: Number of attribute classes downloaded: " + objectClassesAttributes.Count);

            if (objectClassesAttributes.Count == objectClassesNumber)
            {
                Debug.Log("ElementConsult::EvaluateElement: All individual-related classes downloaded:" + individualElement.entity.Entity());
                rtrbauElement = new RtrbauElement(Rtrbauer.instance.user.procedure, visualiser.manager, individualAttributes, classAttributes, objectClassesAttributes);
                Debug.Log("ElementConsult::EvaluateElement: rtrbauElement created:" + individualElement.entity.Entity());
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
            // RTRBAU ALGORITHM: new extension (which original loop number is?)
            // RTRBAU ALGORITHM: elementPrevious two loops to reduce available formats merged into a single one
            // Checks if format is acceptable
            // If so, then evaluates the format agains the element to determine if it is assignable
            // Replaces the toogled foreach loops below
            foreach (Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat> format in Dictionaries.ConsultFormats)
            {
                // Check environment and user formats
                Tuple<RtrbauAugmentation, RtrbauInteraction> envFacets = format.Item3.EvaluateFormat();
                Tuple<RtrbauComprehensiveness, RtrbauDescriptiveness> userFacets = format.Item4.EvaluateFormat();

                if (envFacets != null && userFacets != null)
                {
                    Debug.Log("ElementConsult::SelectFabrications: Fabrication available: " + format.Item1);
                    Debug.Log("ElementConsult::SelectFabrications: " + format.Item2.formatName + " required facets: " + format.Item2.formatRequiredFacets);
                    
                    // Determine if format is assignable as fabrications to the element
                    List<RtrbauFabrication> formatAssignedFabrications = format.Item2.EvaluateFormat(rtrbauElement);

                    // Assign fabrications
                    if (formatAssignedFabrications != null)
                    {
                        foreach (RtrbauFabrication fabrication in formatAssignedFabrications)
                        {
                            Debug.Log("ElementConsult::SelectFabrications: Fabrication assigned: " + fabrication.fabricationName);
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
                                    Debug.Log("ElementReport::SelectFabrications: similar fabrication restrictiviy-ordered is " + fabrication.fabricationName + " for attribute " + attribute.attributeName.name);
                                }

                                // If facet restrictivity is equal, then compare format senses
                                if (similarSourceFabrications[similarSourceFabrications.Count() - 1].fabricationData.First().Key.facetRules.facetRestrictivity == similarSourceFabrications[similarSourceFabrications.Count() - 2].fabricationData.First().Key.facetRules.facetRestrictivity)
                                {
                                    // UPG: to ensure senses are treated accordingly to proper rules rather than enum comparison (declaration order)
                                    similarSourceFabrications.Sort((x, y) => x.fabricationInteraction.CompareTo(y.fabricationInteraction));

                                    foreach (RtrbauFabrication fabrication in similarSourceFabrications)
                                    {
                                        Debug.Log("ElementReport::SelectFabrications: similar fabrication interaction-ordered is " + fabrication.fabricationName + " for attribute " + attribute.attributeName.name);
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

            //foreach (RtrbauAttribute attribute in assignedAttributes)
            //{
            //    Debug.Log("EvaluateElement: assigned attributes: " + attribute.attributeName.name + " : " + attribute.attributeValue);
            //}

            // Identify attributes that have not been assigned to generated fabrications
            List<RtrbauAttribute> nonAssignedAttributes = rtrbauElement.elementAttributes.Where(x => assignedAttributes.All(y => y.attributeValue != x.attributeValue)).ToList();

            // Generate default fabrications for non-assigned attributes
            foreach (RtrbauAttribute attribute in nonAssignedAttributes)
            {
                // Debug.Log("EvaluateElement: non assignedAttribute: " + attribute.attributeName.name + " : " + attribute.attributeValue);

                if (attribute.fabricationType == RtrbauFabricationType.Observe)
                {
                    // Create new fabrication for non-assigned attribute
                    RtrbauFabrication fabrication = new RtrbauFabrication(RtrbauFabricationName.DefaultObserve, RtrbauFabricationType.Observe, new Dictionary<DataFacet, RtrbauAttribute>
                    {
                        { DataFormats.DefaultObserve.formatFacets[0], attribute }
                    });
                    // UPG: since environment and user formats are being evaluated before, there is no need to add them to RtrbauFabrication class?
                    fabrication.fabricationAugmentation = EnvironmentFormats.DefaultObserve.formatAugmentation.facetAugmentation;
                    fabrication.fabricationInteraction = EnvironmentFormats.DefaultObserve.formatInteraction.facetInteraction;
                    fabrication.fabricationComprehension = UserFormats.DefaultObserve.formatComprehension;
                    fabrication.fabricationDescription = UserFormats.DefaultObserve.formatDescription;
                    // Assign fabrication to assignedFabrications
                    assignedFabrications.Add(fabrication);
                }
                else if (attribute.fabricationType == RtrbauFabricationType.Inspect)
                {
                    // Create new fabrication for non-assigned attribute
                    RtrbauFabrication fabrication = new RtrbauFabrication(RtrbauFabricationName.DefaultInspect, RtrbauFabricationType.Inspect, new Dictionary<DataFacet, RtrbauAttribute>
                    {
                        { DataFormats.DefaultInspect.formatFacets[0], attribute }
                    });
                    // UPG: since environment and user formats are being evaluated before, there is no need to add them to RtrbauFabrication class?
                    fabrication.fabricationAugmentation = EnvironmentFormats.DefaultInspect.formatAugmentation.facetAugmentation;
                    fabrication.fabricationInteraction = EnvironmentFormats.DefaultInspect.formatInteraction.facetInteraction;
                    fabrication.fabricationComprehension = UserFormats.DefaultInspect.formatComprehension;
                    fabrication.fabricationDescription = UserFormats.DefaultInspect.formatDescription;
                    // Assign fabrication to assignedFabrications
                    assignedFabrications.Add(fabrication);
                }
                else
                {
                    throw new ArgumentException("ElementConsult::SelectFabrications: Default fabrications not implemented for fabrication type: " + attribute.fabricationType);
                }
            }

            //foreach (RtrbauFabrication fab in assignedFabrications)
            //{
            //    Debug.Log("EvaluateElement: fabrication evaluated: " + fab.fabricationName);
            //}

            fabricationsSelected = true;
            // LocateElement();
            CreateFabrications();
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void LocateElement()
        {
            // RTRBAU ALGORITHM: Select element location (Loop 8 final)
            // if (componentDistanceDownloaded && operationDistanceDownloaded && fabricationsSelected)
            if (componentDistanceDownloaded && operationDistanceDownloaded)
            {
                int componentDistance = int.Parse(elementComponentDistance.ontDistance);
                int operationDistance = int.Parse(elementOperationDistance.ontDistance);

                Debug.Log("ElementConsult::LocateElement: component distance is " + componentDistance);
                Debug.Log("ElementConsult::LocateElement: operation distance is " + operationDistance);

                if (componentDistance <= 1)
                {
                    rtrbauLocation = RtrbauElementLocation.Primary;
                }
                else if (operationDistance >= 1)
                {
                    rtrbauLocation = RtrbauElementLocation.Secondary;
                }
                else
                {
                    rtrbauLocation = RtrbauElementLocation.Tertiary;
                }

                Debug.Log("ElementConsult::LocateElement: rtrbau location is " + rtrbauLocation);

                // RtrbauerEvents.TriggerEvent("LocateElement", this.gameObject, rtrbauLocation);
                // To launch element location through visualiser
                // CreateFabrications();
                LocateIt();
            }
            else { }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void CreateFabrications()
        {
            if (fabricationsSelected)
            {
                individualText.text = individualElement.entity.name;
                classText.text = classElement.entity.name;

                Debug.Log("ElementConsult::CreateFabrications: Starting to create fabrications for: " + individualElement.entity.Entity());

                foreach (RtrbauFabrication fabrication in assignedFabrications)
                {
                    // UPG: list to know which script (class) to get component for
                    // UPG: create a list maybe with prefabs pre-loaded to save time?
                    // UPG: where to create list? would it be a dynamic dictionary?
                    string fabricationPath = "Rtrbau/Prefabs/Fabrications/Visualisations/" + fabrication.fabricationName;

                    Debug.Log(fabricationPath);

                    // Make sure this goes correctly, otherwise it can create big issues
                    GameObject fabricationGO = Resources.Load(fabricationPath) as GameObject;

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
                        Debug.LogError("ElementConsult::CreateFabrications: " + fabrication.fabricationName + " is not implemented");
                    }
                }

                // Set fabrications as active
                fabricationsActive = true;
                statusText.text = "Element maximised, click to hide information";

                // Disable tile grid object collection from side panel to allow image manipulation
                // Maybe do it with other fabrication panel as well?
                // fabricationsSidePanel.GetComponent<TileGridObjectCollection>().enabled = false;
                fabricationsObserveImageVideo.GetComponent<TileGridObjectCollection>().enabled = false;

                InputIntoReport();
                // LocateIt();
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
            // Function called after generating fabrications
            // Input into report
            Reporter.instance.ReportElement(individualElement.entity);
        }
        #endregion IELEMENTABLE_METHODS

        #region IVISUALISABLE_METHODS
        /// <summary>
        /// Locates fabrications created by this element.
        /// </summary>
        public void LocateIt()
        {
            // Modified to reduce foreach loop by connecting with CreateFabrications
            // Foreach loop now implemented as LocateFabrication function within CreateFabrications loop
            // Rest of functionality remains
            
            // ElementConsult location is managed by its visualiser.
            RtrbauerEvents.TriggerEvent("LocateElement", this.gameObject, RtrbauElementType.Consult, rtrbauLocation);
        }

        /// <summary>
        /// Modifies material 
        /// </summary>
        public void ModifyMaterial()
        {
            if (!materialChanged)
            {
                // Set material of element panels
                panelPrimary.material = seenMaterial;
                panelSecondary.material = seenMaterial;

                // Set material of fabrication panels
                foreach (KeyValuePair<RtrbauFabrication, GameObject> fabrication in elementFabrications)
                {
                    fabrication.Value.GetComponent<IVisualisable>().ModifyMaterial();
                }

                // Modify material change event
                materialChanged = true;
            }
            else { }
        }

        /// <summary>
        /// Destroys fabrications created by this element.
        /// </summary>
        public void DestroyIt()
        {
            foreach (GameObject fabrication in unparentedFabrications)
            {
                Destroy(fabrication);
            }
        }
        #endregion IVISUALISABLE_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementIndividual"></param>
        void DownloadedIndividual(OntologyElement elementIndividual)
        {
            LoaderEvents.StopListening(elementIndividual.EventName(), DownloadedIndividual);

            if (File.Exists(individualElement.FilePath()))
            {
                string jsonFile = File.ReadAllText(individualElement.FilePath());

                individualAttributes = JsonUtility.FromJson<JsonIndividualValues>(jsonFile);

                classElement = new OntologyElement(individualAttributes.ontClass, OntologyElementType.ClassProperties);

                LoaderEvents.StartListening(classElement.EventName(), DownloadedClass);
                Loader.instance.StartOntElementDownload(classElement);
            }
            else
            {
                Debug.LogError("ElementConsult::DownloadedIndividual: File not found: " + individualElement.FilePath());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementClass"></param>
        void DownloadedClass(OntologyElement elementClass)
        {
            LoaderEvents.StopListening(elementClass.EventName(), DownloadedClass);

            classElement = elementClass;

            if (File.Exists(classElement.FilePath()))
            {
                string jsonFile = File.ReadAllText(classElement.FilePath());

                classAttributes = JsonUtility.FromJson<JsonClassProperties>(jsonFile);

                // UPG: To check if object property class has already been asked to be downloaded
                // List<string> objectPropertiesClasses = new List<string>();

                // Check if object property class has already been evaluated (next foreach loop)
                List<string> objectPropertiesClasses = new List<string>();

                // Download classes from class object properties
                foreach (JsonValue attributeIndividual in individualAttributes.ontProperties)
                {
                    // UPG: handle error when individual attribute range do not coincide in class with class attribute
                    if (attributeIndividual.ontType.Contains(OntologyPropertyType.ObjectProperty.ToString()))
                    {
                        // Identify class property to be 
                        JsonProperty attribute = classAttributes.ontProperties.Find(delegate (JsonProperty attClass) {return attClass.ontName == attributeIndividual.ontName;});

                        // Check if object property class has not already been evaluated
                        if (!objectPropertiesClasses.Contains(attribute.ontRange))
                        {
                            // Create OntologyElement necessary to download attribute class
                            OntologyElement attributeClass = new OntologyElement(attribute.ontRange, OntologyElementType.ClassProperties);
                            // Download attribute (relationship) class to objectClassesAttributes
                            LoaderEvents.StartListening(attributeClass.EventName(), DownloadedAttribute);
                            Loader.instance.StartOntElementDownload(attributeClass);
                            // Add non-repeated attribute to downloadable classes
                            objectPropertiesClasses.Add(attribute.ontRange);
                            // Note non-repeated property as downloadable classes
                            // objectClassesNumber += 1;
                        }
                        else { }
                    }
                    else { }
                }

                // Note non-repeated property as downloadable classes: in case the foreach loop is slower than loader events or viceversa
                objectClassesNumber = objectPropertiesClasses.Count;

                // Download distance from class to component class
                OntologyDistance componentDistance = new OntologyDistance(elementClass.entity.URI(), RtrbauDistanceType.Component);
                LoaderEvents.StartListening(componentDistance.EventName(), DownloadedComponentDistance);
                Loader.instance.StartOntDistanceDownload(componentDistance);
                // Download distance from class to operation class
                OntologyDistance operationDistance = new OntologyDistance(elementClass.entity.URI(), RtrbauDistanceType.Operation);
                LoaderEvents.StartListening(operationDistance.EventName(), DownloadedOperationDistance);
                Loader.instance.StartOntDistanceDownload(operationDistance);

                // Go to next step: in case the foreach loop is slower than loader events or viceversa
                EvaluateElement();
            }
            else
            {
                Debug.LogError("ElementConsult::DownloadedClass: File not found: " + classElement.FilePath());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementAttribute"></param>
        void DownloadedAttribute(OntologyElement elementAttribute)
        {
            LoaderEvents.StopListening(elementAttribute.EventName(), DownloadedAttribute);

            if (File.Exists(elementAttribute.FilePath()))
            {
                string jsonFile = File.ReadAllText(elementAttribute.FilePath());

                objectClassesAttributes.Add(JsonUtility.FromJson<JsonClassProperties>(jsonFile));

                EvaluateElement();
            }
            else
            {
                Debug.LogError("ElementConsult::DownloadedAttribute: File not found: " + elementAttribute.FilePath());
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
                Debug.LogError("ElementConsult::DownloadedComponentDistance: File not found: " + componentDistance.FilePath());
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
                Debug.LogError("ElementConsult::DownloadedOperationDistance: File not found: " + operationDistance.FilePath());
            }
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

        /// <summary>
        /// 
        /// </summary>
        void LocateFabrication (KeyValuePair<RtrbauFabrication, GameObject> fabrication)
        {
            // UPG: Add ordering for tiled fabrications (buttons, icons, text).
            if (fabrication.Key.fabricationType == RtrbauFabricationType.Observe)
            {
                if (fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Text ||
                fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Icon ||
                fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Audio)
                {
                    fabrication.Value.transform.SetParent(fabricationsObserveRest, false);
                    fabrication.Value.GetComponent<IFabricationable>().Initialise(visualiser, fabrication.Key, this.transform, this.transform);
                }
                else if (fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Image ||
                fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Video)
                {
                    fabrication.Value.transform.SetParent(fabricationsObserveImageVideo, false);
                    fabrication.Value.GetComponent<IFabricationable>().Initialise(visualiser, fabrication.Key, this.transform, this.transform);
                }
                else if (fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Model ||
                fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Hologram ||
                fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Animation)
                {
                    fabrication.Value.transform.SetParent(fabricationsObserveRest, false);
                    fabrication.Value.GetComponent<IFabricationable>().Initialise(visualiser, fabrication.Key, this.transform, visualiser.transform);
                    unparentedFabrications.Add(fabrication.Value);
                }
            }
            else if (fabrication.Key.fabricationType == RtrbauFabricationType.Inspect)
            {
                fabrication.Value.transform.SetParent(fabricationsInspect, false);
                fabrication.Value.GetComponent<IFabricationable>().Initialise(visualiser, fabrication.Key, this.transform, this.transform);
            }
            else
            {
                throw new ArgumentException("ElementConsult::LocateIt: Fabrication type not implemented");
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
            float sM = this.transform.localScale.x / originalElementScaleX;
            float sX = fabrication.transform.localScale.x * sM;
            float sY = fabrication.transform.localScale.y * sM;
            float sZ = fabrication.transform.localScale.z;

            fabrication.transform.localScale = new Vector3(sX, sY, sZ);
        }
        #endregion PRIVATE

        #region PUBLIC
        public void ActivateFabrications()
        {
            // For fabrications with additional unparented fabrications, remember to add behaviour OnEnable and OnDisable
            if (fabricationsActive)
            {
                foreach(KeyValuePair<RtrbauFabrication, GameObject> fabrication in elementFabrications)
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
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}