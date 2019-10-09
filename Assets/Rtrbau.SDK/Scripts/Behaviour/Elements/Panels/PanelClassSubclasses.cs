/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 08/07/2019
==============================================================================*/

#region NAMESPACES
/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.IO;
using TMPro;
using Microsoft.MixedReality.Toolkit.Utilities;
#endregion

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class PanelClassSubclasses: MonoBehaviour, IElementable
    {
        #region INITIALISATION_VARIABLES
        public OntologyElement classElement;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public OntologyElement previousClassElement;
        public JsonClassSubclasses subclasses;
        public Dictionary<OntologyEntity, GameObject> fabrications;

        public bool subclassHasSubclasses;
        public bool subclassHasProperties;
        public bool subclassHasIndividuals;
        public int subclassEvaluation;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public GameObject fabricationPrefab;
        public GameObject fabricationLocator;
        public GameObject panelUsage;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        private event Action OnSelectedFabrications;
        private event Action<OntologyEntity> OnSubclassEvaluated;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Awake()
        {
            Debug.Log(classElement.entity.name);
            Debug.Log("");
        }
        void OnEnable()
        {
            OnSelectedFabrications += LocateElement;
            OnSubclassEvaluated += EvaluateNextStep;   
        }

        void OnDisable()
        {
            OnSelectedFabrications -= LocateElement;
            OnSubclassEvaluated -= EvaluateNextStep;
            DestroyFabrications();
            DestroyFabricationListeners();
        }
        #endregion MONOBEHAVIOUR_METHODS

        #region INITIALISATION_METHODS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void Initialise(OntologyElement ontElement)
        {
            // Check if fabrications were there before
            if (classElement.entity.name != "")
            {
                // Assign previous class element to be able to go back
                previousClassElement = classElement;

                if (fabrications.Count != 0)
                {
                    DestroyFabrications();
                    DestroyFabricationListeners();
                }
            }
            else
            {
                // Assign previous class element to be able to go back
                previousClassElement = ontElement;
            }
            
            classElement = ontElement;
            subclasses = null;
            fabrications = new Dictionary<OntologyEntity, GameObject>();
            subclassHasSubclasses = false;
            subclassHasProperties = false;
            subclassHasIndividuals = false;
            subclassEvaluation = 0;

            DownloadElement();
            InputIntoReport();
        }
        #endregion INITIALISATION_METHODS

        #region IELEMENTABLE_METHODS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void DownloadElement()
        {
            Debug.Log("DownloadElement: evaluation number " + subclassEvaluation);
            Debug.Log("DownloadElement: " + classElement.entity.Entity());
            LoaderEvents.StartListening(classElement.EventName(), EvaluateClass);
            Loader.instance.StartOntElementDownload(classElement);
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void EvaluateElement()
        {
            if (File.Exists(classElement.FilePath()))
            {
                string jsonFile = File.ReadAllText(classElement.FilePath());

                // Debug.Log(jsonFile);

                subclasses = JsonUtility.FromJson<JsonClassSubclasses>(jsonFile);

                Debug.Log("EvaluateElement: " + jsonFile);

                Debug.Log("EvaluateElement: has subclasses " + classElement.entity.Entity());

                // If it does, then display fabrications to select
                SelectFabrications();

                panelUsage.GetComponent<TextMeshProUGUI>().text = "Click to select one of the following operations.";
            }
            else
            {
                Debug.LogError("File not found: " + classElement.FilePath());
            }
            
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void SelectFabrications()
        {
            // Find if prefabs have been instantiated
            if (fabricationPrefab == null || fabricationLocator == null || panelUsage == null)
            {
                Debug.LogError("Fabrications not found for this rtrbau element.");
            }
            else
            {
                if (OnSelectedFabrications != null)
                {
                    OnSelectedFabrications.Invoke();
                }
                else { }
            }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void LocateElement()
        {
            Debug.Log("LocateElement: Fabrications selected to creation");
            CreateFabrications();
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void CreateFabrications()
        {
            Debug.Log("CreateFabrications: Initialising fabrications");

            foreach (JsonSubclass subclass in subclasses.ontSubclasses)
            {
                // fabrications.Add(Instantiate(fabricationPrefab, fabricationLocator.transform));
                OntologyEntity subclassEntity = new OntologyEntity(subclass.ontSubclass);
                GameObject subclassFabrication = Instantiate(fabricationPrefab, fabricationLocator.transform);
                
                fabrications.Add(subclassEntity, subclassFabrication);

                subclassFabrication.GetComponent<PanelButton>().Initialise(subclassEntity);

                PanellerEvents.StartListening(subclassEntity.Entity(), NominatedSubclass);

                Debug.Log("CreateFabrications: Initialised button " + subclassEntity.name);
            }

            fabricationLocator.GetComponent<GridObjectCollection>().UpdateCollection();

        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        // Called from Initialise (where middle classes accessed, or at final evaluation when criteria met
        public void InputIntoReport()
        {
            Reporter.instance.ReportElement(classElement.entity);
        }
        #endregion IELEMENTABLE_METHODS

        #region CLASS_METHODS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void EvaluateClass (OntologyElement element)
        {
            Debug.Log("EvaluateClass: class downloaded" + element.entity.name);
            LoaderEvents.StopListening(element.EventName(), EvaluateClass);
            EvaluateElement();
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void NominatedSubclass (OntologyEntity entity)
        {
            Debug.Log("NominatedSubclass: Button Clicked " + entity.name);

            OntologyElement subclassSubclasses = new OntologyElement(entity.URI(), OntologyElementType.ClassSubclasses);

            OntologyElement subclassProperties = new OntologyElement(entity.URI(), OntologyElementType.ClassProperties);

            OntologyElement subclassIndividuals = new OntologyElement(entity.URI(), OntologyElementType.ClassIndividuals);

            LoaderEvents.StartListening(subclassSubclasses.EventName(), EvaluateSubclassSubclasses);

            Loader.instance.StartOntElementDownload(subclassSubclasses);

            LoaderEvents.StartListening(subclassProperties.EventName(), EvaluateSubclassProperties);

            Loader.instance.StartOntElementDownload(subclassProperties);

            LoaderEvents.StartListening(subclassIndividuals.EventName(), EvaluateSubclassIndividuals);

            Loader.instance.StartOntElementDownload(subclassIndividuals);

            // EvaluateNextStep(entity);

        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void EvaluateSubclassSubclasses(OntologyElement element)
        {
            LoaderEvents.StopListening(element.EventName(), EvaluateSubclassSubclasses);

            if (element != null)
            {
                JsonClassSubclasses subclassSubclasses;

                Debug.Log(File.ReadAllText(element.FilePath()));

                subclassSubclasses = JsonUtility.FromJson<JsonClassSubclasses>(File.ReadAllText(element.FilePath()));

                Debug.Log("EvaluateSubclassSubclasses: " + subclassSubclasses.ontClass);
                Debug.Log("EvaluateSubclassSubclasses: subclasses number " + subclassSubclasses.ontSubclasses.Count);

                // If the subclass has subclasses, it is assumed the subclass has no properties nor individuals
                if (subclassSubclasses.ontSubclasses.Count != 0)
                {
                    subclassHasSubclasses = true;
                    Debug.Log("EvaluateSubclassSubclasses: has subclasses " + subclassSubclasses.ontClass);
                }
                else
                {
                    // If it doesn't, then it is necessary to evaluate its properties and individuals
                    subclassHasSubclasses = false;
                    Debug.Log("EvaluateSubclassSubclasses: has not subclasses " + subclassEvaluation);
                }

                // Invoke next step evaluation call
                if (OnSubclassEvaluated != null)
                {
                    subclassEvaluation += 1;
                    OnSubclassEvaluated.Invoke(element.entity);
                }
                else { }
            }
            else
            {
                Debug.LogError("Element not found.");
            }
        }


        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void EvaluateSubclassProperties (OntologyElement element)
        {
            LoaderEvents.StopListening(element.EventName(), EvaluateSubclassProperties);

            // Check the element could have been downloaded
            if (element != null)
            {
                JsonClassProperties subclassProperties;

                // Debug.Log(File.ReadAllText(element.FilePath()));

                subclassProperties = JsonUtility.FromJson<JsonClassProperties>(File.ReadAllText(element.FilePath()));

                Debug.Log("EvaluateSubclassProperties: " + subclassProperties.ontClass);
                Debug.Log("EvaluateSubclassProperties: properties number " + subclassProperties.ontProperties.Count);

                // If the subclass has no properties, then it cannot be neither reported nor consulted
                if (subclassProperties.ontProperties.Count != 0)
                {
                    Debug.Log("EvaluateSubclassProperties: has properties " + element.entity.name);

                    subclassHasProperties = true;                    
                }
                else
                {
                    Debug.Log("EvaluateSubclassProperties: has not properties " + element.entity.name);

                    subclassHasProperties = false;
                }

                // subclassEvaluation += 1;

                Debug.Log("EvaluateSubclassProperties : " + subclassEvaluation.ToString());

                if (OnSubclassEvaluated != null)
                {
                    subclassEvaluation += 1;
                    OnSubclassEvaluated.Invoke(element.entity);
                }
                else { }
            }
            else
            {
                Debug.LogError("Element not found.");
            }

        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void EvaluateSubclassIndividuals (OntologyElement element)
        {
            LoaderEvents.StopListening(element.EventName(), EvaluateSubclassIndividuals);

            // Check the element could have been downloaded
            if (element != null)
            {
                JsonClassIndividuals subclassIndividuals;

                subclassIndividuals = JsonUtility.FromJson<JsonClassIndividuals>(File.ReadAllText(element.FilePath()));

                Debug.Log("EvaluateSubclassIndividuals: " + subclassIndividuals.ontClass);
                Debug.Log("EvaluateSubclassIndividuals: individuals number " + subclassIndividuals.ontIndividuals.Count);

                // If the subclass has no inviduals, then it cannot be consulted
                if (subclassIndividuals.ontIndividuals.Count != 0)
                {
                    Debug.Log("EvaluateSubclassIndividuals: has individuals " + element.entity.name);

                    subclassHasIndividuals = true;
                }
                else
                {
                    Debug.Log("EvaluateSubclassIndividuals: has not individuals " + element.entity.name);

                    subclassHasIndividuals = false;
                }

                // subclassEvaluation += 1;
                Debug.Log("EvaluateSubclassIndividuals: " + subclassEvaluation);

                if (OnSubclassEvaluated != null)
                {
                    subclassEvaluation += 1;
                    OnSubclassEvaluated.Invoke(element.entity);
                }
                else { }

            }
            else
            {
                Debug.LogError("Element not found.");
            }

        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void EvaluateNextStep(OntologyEntity entity)
        {
            Debug.Log("Evaluate next step on: evaluationInvoke number: " + subclassEvaluation);

            // Check all 3 evaluations made their invoke to the function
            if (subclassEvaluation == 3)
            {
                // OnSubclassEvaluated = null;
                subclassEvaluation = 0;

                Debug.Log("Evaluate next step: " + "| " + "subclassHasSubclasses: " + subclassHasSubclasses + " | " + "subclassHasProperties: " + subclassHasProperties + " | " + "subclassHasIndividuals: " + subclassHasIndividuals + " .");

                if (subclassHasSubclasses == true)
                {
                    OntologyElement element = new OntologyElement(entity.URI(), OntologyElementType.ClassSubclasses);

                    Initialise(element);
                }
                else if (subclassHasSubclasses == false)
                {
                    if (subclassHasProperties == true && subclassHasIndividuals == true)
                    {
                        // In this case, class can be reported and consulted
                        Debug.Log("Evaluate next step: " + entity.name + " can be reported and consulted");
                        panelUsage.GetComponent<TextMeshProUGUI>().text = entity.name + " can be reported and consulted";

                        // Check if user wants to do procedure(s) available
                        if (Rtrbauer.instance.user.procedure == RtrbauElementType.Consult)
                        {
                            // InputIntoReport()
                            Reporter.instance.ReportElement(entity);
                            // Remember to add operation to configuration
                            Rtrbauer.instance.operation.operationURI = entity.URI();
                            // Complete paneller behaviour
                            PanellerEvents.TriggerEvent("LoadOperationIndividuals", entity);
                            // Complete paneller behaviour: destroy this game object
                            Destroy(this.gameObject);
                        }
                        else if (Rtrbauer.instance.user.procedure == RtrbauElementType.Report)
                        {
                            // WIP: CARAR
                            // InputIntoReport()
                            Reporter.instance.ReportElement(entity);
                            // Remember to add operation to configuration
                            Rtrbauer.instance.operation.operationURI = entity.URI();
                            // Complete paneller behaviour
                            PanellerEvents.TriggerEvent("UnloadPaneller", entity);
                            // Start visualiser behaviour (pre-loaded with the asset registrator)
                            OntologyElement element = new OntologyElement(entity.URI(), OntologyElementType.ClassProperties);
                            // Make sure the element is of correct RtrbauElementType
                            RtrbauerEvents.TriggerEvent("LoadElement", element, Rtrbauer.instance.user.procedure);
                            // Complete paneller behaviour: destroy this game object
                            Destroy(this.gameObject);
                        }
                        else
                        {
                            // Otherwise, return to previous class nomination
                            Initialise(previousClassElement);
                        }
                    }
                    else if (subclassHasProperties == true && subclassHasIndividuals == false)
                    {
                        // In this case, class can only be reported
                        Debug.Log("Evaluate next step: " + entity.name + " can only be reported");
                        panelUsage.GetComponent<TextMeshProUGUI>().text = entity.name + " can only be reported";

                        // Check if user wants to do procedure(s) available
                        if (Rtrbauer.instance.user.procedure == RtrbauElementType.Report)
                        {
                            // WIP: CARAR
                            // InputIntoReport()
                            Reporter.instance.ReportElement(entity);
                            // Remember to add operation to configuration
                            Rtrbauer.instance.operation.operationURI = entity.URI();
                            // Complete paneller behaviour
                            PanellerEvents.TriggerEvent("UnloadPaneller", entity);
                            // Start visualiser behaviour (pre-loaded with the asset registrator)
                            OntologyElement element = new OntologyElement(entity.URI(), OntologyElementType.ClassProperties);
                            // Make sure the element is of correct RtrbauElementType
                            RtrbauerEvents.TriggerEvent("LoadElement", element, Rtrbauer.instance.user.procedure);
                            // Complete paneller behaviour: destroy this game object
                            Destroy(this.gameObject);
                        }
                        else
                        {
                            // Otherwise, return to previous class nomination
                            Initialise(previousClassElement);
                        }
                    }
                    else if (subclassHasProperties == false && subclassHasIndividuals == true)
                    {
                        // In this case, class is made of datatype individuals
                        Debug.Log("Evaluate next step: " + entity.name + " is made of datatype individuals");
                        panelUsage.GetComponent<TextMeshProUGUI>().text = entity.name + " is made of datatype individuals";

                        // Return to previous class nomination
                        Initialise(previousClassElement);
                    }
                    else if (subclassHasProperties == false && subclassHasIndividuals == false)
                    {
                        // This case should never happen?
                        Debug.LogError("Evaluate next step: " + "Rationale error: this case should never happen");
                        panelUsage.GetComponent<TextMeshProUGUI>().text = entity.name + "Rationale error: this case should never happen";

                        // Return to previous class nomination
                        Initialise(previousClassElement);
                    }
                    else
                    {
                        // This case should never happen?
                        Debug.LogError("Evaluate next step: " + "Rationale error: this case should never happen");
                        panelUsage.GetComponent<TextMeshProUGUI>().text = entity.name + "Rationale error: this case should never happen";

                        // Return to previous class nomination
                        Initialise(previousClassElement);
                    }
                }
                else
                {
                    // This case should never happen?
                    Debug.LogError("Evaluate next step: " + "Rationale error: this case should never happen");
                    panelUsage.GetComponent<TextMeshProUGUI>().text = entity.name + "Rationale error: this case should never happen";

                    // Return to previous class nomination
                    Initialise(previousClassElement);
                }
            }
            else
            {
                // subclassEvaluation += 1;
                Debug.Log("Evaluate next step on: " + entity.name + " | evaluationInvoke number: " + subclassEvaluation);
            }

            // Needs Downloading --> To Generalise
            // If class has subclasses --> Then navigate
            // If not --> If subclass has properties
            // If not --> Say class cannot be reported/consulted
            // If yes --> If subclass has individuals
            // If not --> Subclass can only be reported (not consulted)
            // If yes --> Subclass can be reported and consulted
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void DestroyFabrications()
        {
            foreach (KeyValuePair<OntologyEntity, GameObject> fabrication in fabrications)
            {
                PanellerEvents.StopListening(fabrication.Key.Entity(), NominatedSubclass);
                GameObject.Destroy(fabrication.Value);
            }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired 
        /// </summary>
        void DestroyFabricationListeners()
        {
            foreach (JsonSubclass subclass in subclasses.ontSubclasses)
            {
                OntologyEntity subclassEntity = new OntologyEntity(subclass.ontSubclass);
                PanellerEvents.StopListening(subclassEntity.Entity(), NominatedSubclass);
            }
        }

        #endregion CLASS_METHODS
    }
}


