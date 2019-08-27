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
        public JsonIndividualValues individualAttributes;
        public JsonClassProperties classAttributes;
        public List<JsonClassProperties> objectClassesAttributes;
        public JsonDistance elementComponentDistance;
        public JsonDistance elementOperationDistance;
        public RtrbauElement rtrbauElement;
        public List<RtrbauFabrication> assignedFabrications;
        public List<KeyValuePair<RtrbauFabrication,GameObject>> elementFabrications;
        public RtrbauElementLocation rtrbauLocation;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro individualText;
        public TextMeshPro classText;
        public Material lineMaterial;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        public int objectClassesNumber;
        public bool fabricationsSelected;
        public bool componentDistanceDownloaded;
        public bool operationDistanceDownloaded;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Start() { }

        void Update() { UpdateLineRenderer(); }

        void OnEnable() { }

        void OnDisable() { }

        void OnDestroy() { DestroyIt(); }
        #endregion MONOBEHAVIOUR_METHODS

        #region INITIALISATION_METHODS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void Initialise(AssetVisualiser assetVisualiser, OntologyElement elementIndividual, GameObject previous)
        {
            if (individualText == null || classText == null || lineMaterial == null)
            {
                Debug.LogError("Consult Element: Fabrication not found. Please assign them in ElementConsult script.");
            }
            else
            {
                if (elementIndividual.type == OntologyElementType.IndividualProperties)
                {
                    visualiser = assetVisualiser;
                    individualElement = elementIndividual;
                    previousElement = previous;

                    objectClassesAttributes = new List<JsonClassProperties>();
                    objectClassesNumber = 0;

                    fabricationsSelected = false;

                    componentDistanceDownloaded = false;
                    operationDistanceDownloaded = false;

                    assignedFabrications = new List<RtrbauFabrication>();
                    elementFabrications = new List<KeyValuePair<RtrbauFabrication,GameObject>>();
                    AddLineRenderer();
                    DownloadElement();
                }
                else
                {
                    throw new ArgumentException("Consult Element: ontology element type not implemented.");
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
            Debug.Log("VisualiserConsult: DownloadElement");

            LoaderEvents.StartListening(individualElement.EventName(), DownloadedIndividual);
            Loader.instance.StartOntElementDownload(individualElement);
        }
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void EvaluateElement()
        {
            if (objectClassesAttributes.Count == objectClassesNumber)
            {
                Debug.Log("VisualiserConsult: CreateElement: download completed");
                rtrbauElement = new RtrbauElement(visualiser.manager, individualAttributes, classAttributes, objectClassesAttributes);
                Debug.Log("ElementConsult: CreateElement: rtrbauElement created");
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
            // RTRBAU ALGORITHM: Data formats matching (Loops 3 and 4 combined)
            foreach (DataFormat format in Dictionaries.DataFormatsConsult)
            {
                Debug.Log("ElementConsult: EvaluateElement: " + format.formatName + " required facets: " + format.formatRequiredFacets);
                List<RtrbauFabrication> formatAssignedFabrications = format.EvaluateFormat(rtrbauElement);
                
                if (formatAssignedFabrications != null)
                {
                    foreach (RtrbauFabrication fabrication in formatAssignedFabrications)
                    {
                        assignedFabrications.Add(fabrication);
                        Debug.Log("EvaluateElement: fabrication assigned: " + fabrication.fabricationName);
                    }
                }
            }

            // RTRBAU ALGORITHM: Environment and user formats matching (Loop 6 and 7 modified and combined)
            foreach (RtrbauFabrication fabrication in assignedFabrications.ToList())
            {
                Tuple<RtrbauAugmentation, RtrbauInteraction> envFacets = Dictionaries.EnvironmentFormatsConsult.Find(x => x.formatName == fabrication.fabricationName).EvaluateFormat();
                Tuple<RtrbauComprehensiveness, RtrbauDescriptiveness> userFacets = Dictionaries.UserFormatsConsult.Find(x => x.formatName == fabrication.fabricationName).EvaluateFormat();

                if (envFacets == null || userFacets == null)
                {
                    assignedFabrications.Remove(fabrication);
                }
                else
                {
                    fabrication.fabricationAugmentation = envFacets.Item1;
                    fabrication.fabricationInteraction = envFacets.Item2;
                    fabrication.fabricationComprehension = userFacets.Item1;
                    fabrication.fabricationDescription = userFacets.Item2;
                }
            }

            // RTRBAU ALGORITHM: Eliminate duplicated fabrications (Loop 5 modified)
            // Eliminate duplicated formats with lower number of attributes
            // Duplicates are those with similar source attributes and augmentation method
            foreach (RtrbauAugmentation augmentation in Enum.GetValues(typeof(RtrbauAugmentation)))
            {
                // Find similar source attribute fabrications with similar augmentation method
                List<RtrbauFabrication> similarAugmentationFabrications = assignedFabrications.FindAll(x => x.fabricationAugmentation == augmentation);

                if (similarAugmentationFabrications.Count > 1)
                {
                    foreach (RtrbauAttribute attribute in rtrbauElement.elementAttributes)
                    {
                        // Find fabrications with similar source attributes
                        List<RtrbauFabrication> similarSourceFabrications = similarAugmentationFabrications.FindAll(x => x.fabricationData.Any(y => y.Key.facetForm == RtrbauFacetForm.source && y.Value.attributeName == attribute.attributeName && y.Value.attributeValue == attribute.attributeValue));

                        if (similarSourceFabrications.Count > 1)
                        {
                            // Order similar fabrications by number of attributes
                            similarSourceFabrications.Sort((x, y) => x.fabricationData.Count.CompareTo(y.fabricationData.Count));
                            // Remove as duplicated the fabrication with the highest number of attributes
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

            //foreach (RtrbauFabrication fab in assignedFabrications)
            //{
            //    Debug.Log("EvaluateElement: fabrication evaluated: " + fab.fabricationName);
            //}

            fabricationsSelected = true;
            LocateElement();
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void LocateElement()
        {
            // RTRBAU ALGORITHM: Select element location (Loop 8 final)
            if (componentDistanceDownloaded && operationDistanceDownloaded && fabricationsSelected)
            {
                int componentDistance = int.Parse(elementComponentDistance.ontDistance);
                int operationDistance = int.Parse(elementOperationDistance.ontDistance);

                Debug.Log("ElementConsult: LocateElement: component distance is " + componentDistance);
                Debug.Log("ElementConsult: LocateElement: operation distance is " + operationDistance);

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

                Debug.Log("ElementConsult: LocateElement: rtrbau location is " + rtrbauLocation);

                // RtrbauerEvents.TriggerEvent("LocateElement", this.gameObject, rtrbauLocation);
                // To launch element location through visualiser
                CreateFabrications();
            }
            else { }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void CreateFabrications()
        {
            individualText.text = individualElement.entity.name;
            classText.text = classElement.entity.name;

            Debug.Log("Reached point: fabrications to be created.");

            foreach (RtrbauFabrication fabrication in assignedFabrications)
            {
                // UPG: list to know which script (class) to get component for
                // UPG: create a list maybe with prefabs pre-loaded to save time?
                // UPG: where to create list? would it be a dynamic dictionary?
                string fabricationPath = "Rtrbau/Prefabs/Fabrications/Visualisations/" + fabrication.fabricationName;

                Debug.Log(fabricationPath);

                // Make sure this goes correctly, otherwise it can create big issues
                GameObject goFabrication = Resources.Load(fabricationPath) as GameObject;

                if (goFabrication != null)
                {
                    GameObject newFabrication = Instantiate(goFabrication);
                    elementFabrications.Add(new KeyValuePair<RtrbauFabrication, GameObject>(fabrication, newFabrication));
                }
                else
                {
                    Debug.LogError("ElementConsult: " + fabrication.fabricationName + " is not implemented");
                }
            }

            InputIntoReport();
            LocateIt();
            // End sending loaded element to visualiser to locate it appropriately
            // Modified: to ensure non-panel fabrications have the right size and location
            // Modified: fabrications are located afterwards
        }


        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void InputIntoReport()
        {
            // Since element consults are not modified, they can be inputted into report just after being created
            Reporter.instance.ReportElement(individualElement.entity);
            // To wait for element to be used?
            // Or to input just after located?
        }
        #endregion IELEMENTABLE_METHODS

        #region IVISUALISABLE_METHODS
        /// <summary>
        /// Locates fabrications created by this element.
        /// </summary>
        public void LocateIt()
        {
            Transform fabricationsPanel = this.transform.GetChild(3);
            Transform fabricationsSidePanel = this.transform.GetChild(4);

            // UPG: add ordering for tiled fabrications (buttons, icons, text).
            foreach (KeyValuePair<RtrbauFabrication, GameObject> fabrication in elementFabrications)
            {
                if (fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Text ||
                    fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Icon ||
                    fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Audio)
                {
                    fabrication.Value.transform.SetParent(fabricationsPanel, false);
                    fabrication.Value.GetComponent<IFabricationable>().Initialise(visualiser, fabrication.Key, this.transform, this.transform);
                }
                else if (fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Image ||
                    fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Video)
                {
                    fabrication.Value.transform.SetParent(fabricationsSidePanel, false);
                    fabrication.Value.GetComponent<IFabricationable>().Initialise(visualiser, fabrication.Key, this.transform, this.transform);
                }

                else if (fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Model ||
                    fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Hologram ||
                    fabrication.Key.fabricationAugmentation == RtrbauAugmentation.Animation)
                {
                    fabrication.Value.transform.SetParent(visualiser.transform, true);
                    fabrication.Value.GetComponent<IFabricationable>().Initialise(visualiser, fabrication.Key, this.transform, fabrication.Value.transform);
                }
                else
                {
                    throw new ArgumentException("Fabrication type not implemented");
                }
            }

            // Disable tile grid object collection from side panel to allow image manipulation
            // Maybe do it with other fabrication panel as well?
            fabricationsSidePanel.GetComponent<TileGridObjectCollection>().enabled = false;

            // ElementConsult location is managed by its visualiser.
            RtrbauerEvents.TriggerEvent("LocateElement", this.gameObject, rtrbauLocation);
        }

        /// <summary>
        /// Destroys fabrications created by this element.
        /// </summary>
        public void DestroyIt()
        {
            foreach (KeyValuePair<RtrbauFabrication, GameObject> fabrication in elementFabrications)
            {
                fabrication.Value.GetComponent<IVisualisable>().DestroyIt();
            }
        }
        #endregion IVISUALISABLE_METHODS

        #region CLASS_METHODS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementIndividual"></param>
        void DownloadedIndividual(OntologyElement elementIndividual)
        {
            LoaderEvents.StopListening(elementIndividual.EventName(), DownloadedClass);

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
                Debug.LogError("File not found: " + individualElement.FilePath());
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

                // Download classes from class object properties
                foreach (JsonValue attributeIndividual in individualAttributes.ontProperties)
                {
                    if (attributeIndividual.ontType.Contains(OntologyPropertyType.ObjectProperty.ToString()))
                    {
                        JsonProperty attribute = classAttributes.ontProperties.Find(delegate (JsonProperty attClass) {return attClass.ontName == attributeIndividual.ontName;});

                        OntologyElement attributeClass = new OntologyElement(attribute.ontRange, OntologyElementType.ClassProperties);

                        LoaderEvents.StartListening(attributeClass.EventName(), DownloadedAttribute);
                        Loader.instance.StartOntElementDownload(attributeClass);

                        // UPG: do not download repeated properties
                        objectClassesNumber += 1;
                    }
                    else { }
                }

                // Download distances from class to component and operation classes
                OntologyDistance componentDistance = new OntologyDistance(elementClass.entity.URI(), RtrbauDistanceType.Component);
                LoaderEvents.StartListening(componentDistance.EventName(), DownloadedComponentDistance);
                Loader.instance.StartOntDistanceDownload(componentDistance);

                OntologyDistance operationDistance = new OntologyDistance(elementClass.entity.URI(), RtrbauDistanceType.Operation);
                LoaderEvents.StartListening(operationDistance.EventName(), DownloadedOperationDistance);
                Loader.instance.StartOntDistanceDownload(operationDistance);

                // Go to next step
                EvaluateElement();
            }
            else
            {
                Debug.LogError("File not found: " + classElement.FilePath());
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
                Debug.LogError("File not found: " + elementAttribute.FilePath());
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
                Debug.LogError("File not found: " + componentDistance.FilePath());
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
                Debug.LogError("File not found: " + operationDistance.FilePath());
            }
        }

        void AddLineRenderer()
        {
            if (previousElement != null)
            {
                // Set line widht at 10% of element consult panel
                float width = this.transform.localScale.x * 0.01f;
                this.gameObject.AddComponent<LineRenderer>();
                this.gameObject.GetComponent<LineRenderer>().useWorldSpace = true;
                this.gameObject.GetComponent<LineRenderer>().material = lineMaterial;
                this.gameObject.GetComponent<LineRenderer>().startWidth = width;
                this.gameObject.GetComponent<LineRenderer>().endWidth = width;
                UpdateLineRenderer();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void UpdateLineRenderer()
        {
            if (previousElement != null)
            {
                // Set start and end of line in world coordinates
                Vector3 start = this.transform.position;
                Vector3 end = previousElement.transform.position;
                this.gameObject.GetComponent<LineRenderer>().SetPosition(0, start);
                this.gameObject.GetComponent<LineRenderer>().SetPosition(1, end);
            }
        }
        #endregion CLASS_METHODS
    }
}


