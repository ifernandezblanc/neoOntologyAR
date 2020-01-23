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
        public RtrbauElementType rtrbauElementType;
        public RtrbauElementLocation rtrbauLocationType;
        public RtrbauElement rtrbauElement;
        public JsonIndividualValues individualAttributes;
        public JsonClassProperties classAttributes;
        public List<JsonClassProperties> objectPropertyAttributes;
        public JsonDistance elementComponentDistance;
        public JsonDistance elementOperationDistance;
        public List<RtrbauFabrication> assignedFabrications;
        public List<KeyValuePair<RtrbauFabrication,GameObject>> elementFabrications;
        public List<GameObject> observeFabrications;
        public List<GameObject> inspectFabrications;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public Transform fabricationsAttributes;
        public Transform fabricationsAttributesNonText;
        public Transform fabricationsRelationships;
        [SerializeField]
        private GameObject loadingPlate;
        public TextMeshPro classText;
        public TextMeshPro individualText;
        public TextMeshPro statusText;
        public MeshRenderer panelPlate;
        public Material lineMaterial;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        public bool individualDownloaded;
        public bool classDownloaded;
        public bool objectPropertiesDownloaded;
        public int objectPropertiesNumber;
        public bool componentDistanceDownloaded;
        public bool operationDistanceDownloaded;
        public bool fabricationsSelected;
        public bool fabricationsCreated;
        public bool fabricationsActive;
        public bool elementReported;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Start() { }

        void Update()
        {
            //if (Rtrbauer.instance.viewer != null)
            //{
            //    this.transform.LookAt(Rtrbauer.instance.viewer.transform.position);
            //    this.transform.Rotate(0, 180, 0);
            //}
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
            if (fabricationsAttributes == null || fabricationsAttributesNonText == null || fabricationsRelationships == null || loadingPlate == null || 
                classText == null || individualText == null || statusText == null ||  panelPlate == null || lineMaterial == null) 
            {
                Debug.LogError("ElementConsult::Initialise: Fabrication not found. Please assign them in ElementConsult script.");
            }
            else
            {
                if (elementIndividual.type == OntologyElementType.IndividualProperties)
                {
                    rtrbauElementType = RtrbauElementType.Consult;
                    rtrbauLocationType = RtrbauElementLocation.None;

                    visualiser = assetVisualiser;
                    individualElement = elementIndividual;
                    classElement = elementClass;
                    previousElement = elementPrevious;

                    objectPropertyAttributes = new List<JsonClassProperties>();

                    individualDownloaded = false;
                    classDownloaded = false;
                    objectPropertiesDownloaded = false;
                    objectPropertiesNumber = 0;
                    componentDistanceDownloaded = false;
                    operationDistanceDownloaded = false;

                    fabricationsSelected = false;
                    fabricationsCreated = false;

                    fabricationsActive = false;

                    elementReported = false;

                    assignedFabrications = new List<RtrbauFabrication>();
                    elementFabrications = new List<KeyValuePair<RtrbauFabrication,GameObject>>();
                    observeFabrications = new List<GameObject>();
                    inspectFabrications = new List<GameObject>();

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
            Debug.Log("ElementConsult::DownloadElement: download individual element:" + individualElement.entity.Entity());
            // Download individual values
            LoaderEvents.StartListening(individualElement.EventName(), DownloadedIndividual);
            Loader.instance.StartOntElementDownload(individualElement);

            Debug.Log("ElementConsult::DownloadElement: download class element:" + classElement.entity.Entity());
            // Download class properties
            LoaderEvents.StartListening(classElement.EventName(), DownloadedClass);
            Loader.instance.StartOntElementDownload(classElement);

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
            Debug.Log("ElementConsult::EvaluateElement: Number of attribute classes downloadable: " + objectPropertiesNumber);
            Debug.Log("ElementConsult::EvaluateElement: Number of attribute classes downloaded: " + objectPropertyAttributes.Count);

            if (objectPropertiesDownloaded == true && objectPropertyAttributes.Count == objectPropertiesNumber)
            {
                Debug.Log("ElementConsult::EvaluateElement: All individual-related classes downloaded:" + individualElement.entity.Entity());
                rtrbauElement = new RtrbauElement(Rtrbauer.instance.user.procedure, visualiser.manager, individualAttributes, classAttributes, objectPropertyAttributes);
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

                Debug.Log("ElementConsult::LocateElement: component distance is " + componentDistance);
                Debug.Log("ElementConsult::LocateElement: operation distance is " + operationDistance);

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

                Debug.Log("ElementConsult::LocateElement: rtrbau location is " + rtrbauLocationType);

                ScaleLoadingPlate();

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
                // Destroy game object
                Destroy(this.gameObject);
                // Return game object was destroyed
                return true;
            }
            else { return false; }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void CreateFabrications()
        {
            if (fabricationsSelected == true)
            {
                individualText.text = rtrbauElement.elementName.Name();
                classText.text = Parser.ParseNamingOntologyFormat(rtrbauElement.elementClass.Name());

                Debug.Log("ElementConsult::CreateFabrications: Starting to create fabrications for: " + individualElement.entity.Entity());

                foreach (RtrbauFabrication fabrication in assignedFabrications)
                {
                    // UPG: list to know which script (class) to get component for
                    // UPG: create a list maybe with prefabs pre-loaded to save time?
                    // UPG: where to create list? would it be a dynamic dictionary?

                    // Find fabrication GO by name in Rtrbauer dynamic library
                    GameObject fabricationGO = Rtrbauer.instance.FindFabrication(fabrication.fabricationName, RtrbauElementType.Consult);

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
                // UPG: do it with other fabrication panels as well?
                fabricationsAttributesNonText.GetComponent<TileGridObjectCollection>().enabled = false;

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
            // Function called after generating fabrications
            // Input into report
            Reporter.instance.ReportElement(rtrbauElement.elementName);
            // Assign element as reported
            elementReported = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ActivateLoadingPlate()
        {
            loadingPlate.SetActive(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeactivateLoadingPlate()
        {
            loadingPlate.SetActive(false);
        }
        #endregion IELEMENTABLE_METHODS

        #region IVISUALISABLE_METHODS
        /// <summary>
        /// Locates <see cref="IFabricationable"/> elements created by <see cref="ElementConsult"/>.
        /// </summary>
        public void LocateIt()
        {
            // Modified to reduce foreach loop by connecting with CreateFabrications
            // Foreach loop now implemented as LocateFabrication function within CreateFabrications loop
            // Rest of functionality remains
            
            // ElementConsult location is managed by its visualiser.
            RtrbauerEvents.TriggerEvent("AssetVisualiser_LocateElement", individualElement, rtrbauElementType, rtrbauLocationType);
            // Report element once it has been created and without the need to update data
            InputIntoReport();
        }

        /// <summary>
        /// Activates or deactivates <see cref="IFabricationable"/> elements managed by <see cref="ElementConsult"/> according to current state.
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
                //activationButton.sprite = activationButtonMaximise;
                //activationButton.size = new Vector2(0.75f, 0.75f);
            }
            else
            {
                foreach (KeyValuePair<RtrbauFabrication, GameObject> fabrication in elementFabrications)
                {
                    fabrication.Value.SetActive(true);
                }

                fabricationsActive = true;
                statusText.text = "Element maximised, click to hide information";
                //activationButton.sprite = activationButtonMinimise;
                //activationButton.size = new Vector2(0.75f, 0.75f);
            }
        }

        /// <summary>
        /// Destroys <see cref="IFabricationable"/> elements created by <see cref="ElementConsult"/>.
        /// </summary>
        public void DestroyIt()
        {
            foreach (GameObject fabrication in observeFabrications)
            {
                Destroy(fabrication);
            }

            foreach (GameObject fabrication in inspectFabrications)
            {
                Destroy(fabrication);
            }
        }

        /// <summary>
        /// Modifies <see cref="ElementConsult"/> materials as well as those <see cref="IFabricationable"/> managed elements.
        /// </summary>
        public void ModifyMaterial(Material material)
        {
            // Check if element has already been reported
            if (elementReported == true)
            {
                // Set material of element plate
                panelPlate.material = material;
                //panelPrimary.material = material;
                //panelSecondary.material = material;

                // Set material of fabrication panels
                foreach (KeyValuePair<RtrbauFabrication, GameObject> fabrication in elementFabrications)
                {
                    fabrication.Value.GetComponent<IVisualisable>().ModifyMaterial(material);
                }
            }
            else { }
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

                individualDownloaded = true;

                DownloadAttributes();
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

            if (File.Exists(classElement.FilePath()))
            {
                string jsonFile = File.ReadAllText(classElement.FilePath());

                classAttributes = JsonUtility.FromJson<JsonClassProperties>(jsonFile);

                classDownloaded = true;
                
                DownloadAttributes();
            }
            else
            {
                Debug.LogError("ElementConsult::DownloadedClass: File not found: " + classElement.FilePath());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void DownloadAttributes()
        {
            if (individualDownloaded == true && classDownloaded == true)
            {
                // Check if object property class has already been evaluated (next foreach loop)
                List<string> objectPropertiesClasses = new List<string>();

                // Download classes from class object properties
                foreach (JsonValue attributeIndividual in individualAttributes.ontProperties)
                {
                    // UPG: handle error when individual attribute range do not coincide in class with class attribute
                    if (attributeIndividual.ontType.Contains(OntologyPropertyType.ObjectProperty.ToString()))
                    {
                        // Identify class property to be 
                        JsonProperty attribute = classAttributes.ontProperties.Find(x => x.ontName == attributeIndividual.ontName);

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
                        }
                        else { }
                    }
                    else { }
                }

                // Note non-repeated property as downloadable classes: in case the foreach loop is slower than loader events or viceversa
                objectPropertiesNumber = objectPropertiesClasses.Count;
                objectPropertiesDownloaded = true;

                EvaluateElement();
            }
            else { }
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

                objectPropertyAttributes.Add(JsonUtility.FromJson<JsonClassProperties>(jsonFile));

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
        void LocateFabrication (KeyValuePair<RtrbauFabrication, GameObject> fabrication)
        {
            // UPG: Add ordering for tiled fabrications (buttons, icons, text).
            if (fabrication.Key.fabricationType == RtrbauFabricationType.Observe)
            {
                if (fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Text ||
                fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Icon ||
                fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Audio)
                {
                    fabrication.Value.transform.SetParent(fabricationsAttributes, false);
                    fabrication.Value.GetComponent<IFabricationable>().Initialise(visualiser, fabrication.Key, this.transform, this.transform);
                }
                else if (fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Image ||
                fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Video)
                {
                    fabrication.Value.transform.SetParent(fabricationsAttributesNonText, false);
                    fabrication.Value.GetComponent<IFabricationable>().Initialise(visualiser, fabrication.Key, this.transform, this.transform);
                }
                else if (fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Model ||
                fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Hologram ||
                fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Animation)
                {
                    fabrication.Value.transform.SetParent(fabricationsAttributes, false);
                    fabrication.Value.GetComponent<IFabricationable>().Initialise(visualiser, fabrication.Key, this.transform, visualiser.transform);
                    // unparentedFabrications.Add(fabrication.Value);
                }
                else { }
                observeFabrications.Add(fabrication.Value);
            }
            else if (fabrication.Key.fabricationType == RtrbauFabricationType.Inspect)
            {
                fabrication.Value.transform.SetParent(fabricationsRelationships, false);
                fabrication.Value.GetComponent<IFabricationable>().Initialise(visualiser, fabrication.Key, this.transform, this.transform);
                inspectFabrications.Add(fabrication.Value);
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
        void ScaleLoadingPlate()
        {
            // Determine scale factors according to UI game objects
            // Factors have been calculated ad-hoc per case using scales of elements involved (Backplates with parents at scale 1)
            float scaleFactor;
            float moveFactor;

            if (Rtrbauer.instance.archivedFabrications == true) { scaleFactor = 0.395f; moveFactor = -0.025f; }
            else { scaleFactor = 0.5f; moveFactor = -0.033f; }

            // Calculate maximum number of fabrications to which scale
            int attributeFabs = observeFabrications.Count();
            int relationshipFabs = inspectFabrications.Count();
            int maximumFabs;

            if (attributeFabs >= relationshipFabs) { maximumFabs = attributeFabs; }
            else { maximumFabs = relationshipFabs; }

            // Activate loading plate before scaling so it can acquire its initial scale
            loadingPlate.SetActive(true);
            // Rescale loading plate
            loadingPlate.transform.localScale += new Vector3(0, scaleFactor * maximumFabs, 0);
            // Move loading plate
            loadingPlate.transform.localPosition += new Vector3(0, moveFactor * maximumFabs, 0);
            // Rescale images and text on loading plate
            loadingPlate.GetComponent<LoadingAnimation>().ScaleLoadingImages();
            // Deactivate loading plate
            loadingPlate.SetActive(false);
        }
        #endregion PRIVATE

        #region PUBLIC
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}