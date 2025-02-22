﻿/*==============================================================================
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
        public TileGridObjectCollection fabricationLocator;
        public TextMeshPro panelUsage;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        private event Action OnSelectedFabrications;
        private event Action<OntologyEntity> OnSubclassEvaluated;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Awake()
        {
            // OnAwake assign classElement to null
            classElement = null;
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
            // Check if it is the first time the panel is being used
            if (classElement == null)
            {
                // Assign previous class element to be able to go back
                previousClassElement = ontElement;
            }
            else
            {
                // Assign previous class element to be able to go back
                previousClassElement = classElement;

                if (fabrications.Count != 0)
                {
                    DestroyFabrications();
                    DestroyFabricationListeners();
                }
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

                panelUsage.text = "Click to select one of the following operations.";
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
        public void LocateElement()
        {
            //Debug.Log("PanelClassSubclasses::LocateElement: Fabrications selected to creation");
            CreateFabrications();
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        /// <returns>Describe script outcomes</returns>
        public bool DestroyElement()
        {
            // Destroy game object
            Destroy(this.gameObject);
            // Return game object was destroyed
            return true;
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
        public void CreateFabrications()
        {
            Debug.Log("PanelClassSubclasses::CreateFabrications: Initialising fabrications");

            foreach (JsonSubclass subclass in subclasses.ontSubclasses)
            {
                // fabrications.Add(Instantiate(fabricationPrefab, fabricationLocator.transform));
                OntologyEntity subclassEntity = new OntologyEntity(subclass.ontSubclass);
                fabricationLocator.enabled = false;
                GameObject subclassFabrication = Instantiate(fabricationPrefab, fabricationLocator.transform);
                fabricationLocator.enabled = true;
                fabrications.Add(subclassEntity, subclassFabrication);

                subclassFabrication.GetComponent<PanelButton>().Initialise(subclassEntity);

                PanellerEvents.StartListening(subclassEntity.Entity(), NominatedSubclass);

                Debug.Log("PanelClassSubclasses::CreateFabrications: Initialised button " + subclassEntity.Name());
            }

            // fabricationLocator.GetComponent<GridObjectCollection>().UpdateCollection();

        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        // Called from Initialise (where middle classes accessed, or at final evaluation when criteria met
        public void InputIntoReport()
        {
            // Report class subclass and initialise any OntologyEntities required
            OntologyEntity relationship = new OntologyEntity(Rtrbauer.instance.rdfs, "subclassOf");
            Reporter.instance.ReportElement(relationship, classElement.entity, null);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ActivateLoadingPlate() { }

        /// <summary>
        /// 
        /// </summary>
        public void DeactivateLoadingPlate() { }
        #endregion IELEMENTABLE_METHODS

        #region CLASS_METHODS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void EvaluateClass (OntologyElement element)
        {
            Debug.Log("PanelClassSubclasses::EvaluateClass: class downloaded" + element.entity.Name());
            LoaderEvents.StopListening(element.EventName(), EvaluateClass);
            EvaluateElement();
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void NominatedSubclass (OntologyEntity entity)
        {
            Debug.Log("PanelClassSubclasses::NominatedSubclass: Class selected is " + entity.Name());

            OntologyElement subclassSubclasses = new OntologyElement(entity.URI(), OntologyElementType.ClassSubclasses);

            OntologyElement subclassProperties = new OntologyElement(entity.URI(), OntologyElementType.ClassProperties);

            OntologyElement subclassIndividuals = new OntologyElement(entity.URI(), OntologyElementType.ClassIndividuals);

            LoaderEvents.StartListening(subclassSubclasses.EventName(), EvaluateSubclassSubclasses);

            Loader.instance.StartOntElementDownload(subclassSubclasses);

            LoaderEvents.StartListening(subclassProperties.EventName(), EvaluateSubclassProperties);

            Loader.instance.StartOntElementDownload(subclassProperties);

            LoaderEvents.StartListening(subclassIndividuals.EventName(), EvaluateSubclassIndividuals);

            Loader.instance.StartOntElementDownload(subclassIndividuals);
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

                Debug.Log("PanelClassSubclasses::EvaluateSubclassSubclasses: " + subclassSubclasses.ontClass);
                Debug.Log("PanelClassSubclasses::EvaluateSubclassSubclasses: subclasses number " + subclassSubclasses.ontSubclasses.Count);

                // If the subclass has subclasses, it is assumed the subclass has no properties nor individuals
                if (subclassSubclasses.ontSubclasses.Count != 0)
                {
                    subclassHasSubclasses = true;
                    Debug.Log("PanelClassSubclasses::EvaluateSubclassSubclasses: has subclasses " + subclassSubclasses.ontClass);
                }
                else
                {
                    // If it doesn't, then it is necessary to evaluate its properties and individuals
                    subclassHasSubclasses = false;
                    Debug.Log("PanelClassSubclasses::EvaluateSubclassSubclasses: has not subclasses " + subclassEvaluation);
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
                Debug.LogError("PanelClassSubclasses::EvaluateSubclassSubclasses: Element not found.");
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

                Debug.Log("PanelClassSubclasses::EvaluateSubclassProperties: " + subclassProperties.ontClass);
                Debug.Log("PanelClassSubclasses::EvaluateSubclassProperties: properties number " + subclassProperties.ontProperties.Count);

                // If the subclass has no properties, then it cannot be neither reported nor consulted
                if (subclassProperties.ontProperties.Count != 0)
                {
                    Debug.Log("PanelClassSubclasses::EvaluateSubclassProperties: has properties " + element.entity.Name());

                    subclassHasProperties = true;                    
                }
                else
                {
                    Debug.Log("PanelClassSubclasses::EvaluateSubclassProperties: has not properties " + element.entity.Name());

                    subclassHasProperties = false;
                }

                // subclassEvaluation += 1;

                Debug.Log("PanelClassSubclasses::EvaluateSubclassProperties : " + subclassEvaluation.ToString());

                if (OnSubclassEvaluated != null)
                {
                    subclassEvaluation += 1;
                    OnSubclassEvaluated.Invoke(element.entity);
                }
                else { }
            }
            else
            {
                Debug.LogError("PanelClassSubclasses::EvaluateSubclassProperties: Element not found.");
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

                Debug.Log("PanelClassSubclasses::EvaluateSubclassIndividuals: " + subclassIndividuals.ontClass);
                Debug.Log("PanelClassSubclasses::EvaluateSubclassIndividuals: individuals number " + subclassIndividuals.ontIndividuals.Count);

                // If the subclass has no inviduals, then it cannot be consulted
                if (subclassIndividuals.ontIndividuals.Count != 0)
                {
                    Debug.Log("PanelClassSubclasses::EvaluateSubclassIndividuals: has individuals " + element.entity.Name());

                    subclassHasIndividuals = true;
                }
                else
                {
                    Debug.Log("PanelClassSubclasses::EvaluateSubclassIndividuals: has not individuals " + element.entity.Name());

                    subclassHasIndividuals = false;
                }

                // subclassEvaluation += 1;
                Debug.Log("PanelClassSubclasses::EvaluateSubclassIndividuals: " + subclassEvaluation);

                if (OnSubclassEvaluated != null)
                {
                    subclassEvaluation += 1;
                    OnSubclassEvaluated.Invoke(element.entity);
                }
                else { }

            }
            else
            {
                Debug.LogError("PanelClassSubclasses::EvaluateSubclassIndividual: Element not found.");
            }

        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void EvaluateNextStep(OntologyEntity entity)
        {
            Debug.Log("PanelClassSubclasses::EvaluateNextStep: invoke number: " + subclassEvaluation);

            // Check all 3 evaluations made their invoke to the function
            if (subclassEvaluation == 3)
            {
                // OnSubclassEvaluated = null;
                subclassEvaluation = 0;

                Debug.Log("PanelClassSubclasses::EvaluateNextStep: " + "subclassHasSubclasses: " + subclassHasSubclasses + " | " + "subclassHasProperties: " + subclassHasProperties + " | " + "subclassHasIndividuals: " + subclassHasIndividuals + " .");

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
                        Debug.Log("PanelClassSubclasses::EvaluateNextStep: " + entity.Name() + " can be reported and consulted");
                        panelUsage.text = entity.Name() + " can be reported and consulted";

                        // Check if user wants to do procedure(s) available
                        if (Rtrbauer.instance.user.procedure == RtrbauElementType.Consult)
                        {
                            // Remember to add operation to configuration
                            Rtrbauer.instance.operation = new OntologyEntity(entity.URI());
                            // Generate ontology entities to report connection to new RtrbauElement
                            OntologyEntity relationship = new OntologyEntity(Rtrbauer.instance.rdfs, "subClassOf");
                            // Report class selected: InputIntoReport()
                            Reporter.instance.ReportElement(relationship, entity, null);
                            // Complete paneller behaviour
                            PanellerEvents.TriggerEvent("LoadOperationIndividuals", entity);
                            // Complete paneller behaviour: destroy this element
                            DestroyElement();
                        }
                        else if (Rtrbauer.instance.user.procedure == RtrbauElementType.Report)
                        {
                            // Remember to add operation to configuration
                            Rtrbauer.instance.operation = new OntologyEntity(entity.URI());
                            // Complete paneller behaviour
                            PanellerEvents.TriggerEvent("UnloadPaneller", entity);
                            // Generate OntologyElement(s) to load RtrbauElement
                            OntologyElement elementClass = new OntologyElement(entity.URI(), OntologyElementType.ClassProperties);
                            OntologyElement elementIndividual = new OntologyElement(Parser.ParseAddDateTime(entity.URI()), OntologyElementType.IndividualProperties);
                            // Generate ontology entities to report connection to new RtrbauElement
                            OntologyEntity entityRelationship = new OntologyEntity(Rtrbauer.instance.rdf, "type");
                            // Report class selected: InputIntoReport()
                            Reporter.instance.ReportElement(entityRelationship, elementClass.entity, elementIndividual.entity);
                            // Load new RtrbauElement from AssetVisualiser, ensure user has selected the type of RtrbauElement to load
                            RtrbauerEvents.TriggerEvent("AssetVisualiser_CreateElement", elementIndividual, elementClass, Rtrbauer.instance.user.procedure);
                            // Complete paneller behaviour: destroy this element
                            DestroyElement();
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
                        Debug.Log("PanelClassSubclasses::EvaluateNextStep: " + entity.Name() + " can only be reported");
                        panelUsage.text = entity.Name() + " can only be reported";

                        // Check if user wants to do procedure(s) available
                        if (Rtrbauer.instance.user.procedure == RtrbauElementType.Report)
                        {
                            // Remember to add operation to configuration
                            Rtrbauer.instance.operation = new OntologyEntity(entity.URI());
                            // Complete paneller behaviour
                            PanellerEvents.TriggerEvent("UnloadPaneller", entity);
                            // Generate OntologyElement(s) to load RtrbauElement
                            OntologyElement elementClass = new OntologyElement(entity.URI(), OntologyElementType.ClassProperties);
                            OntologyElement elementIndividual = new OntologyElement(Parser.ParseAddDateTime(entity.URI()), OntologyElementType.IndividualProperties);
                            // Generate ontology entities to report connection to new RtrbauElement
                            OntologyEntity entityRelationship = new OntologyEntity(Rtrbauer.instance.rdf, "type");
                            // Report class selected: InputIntoReport()
                            Reporter.instance.ReportElement(entityRelationship, elementClass.entity, elementIndividual.entity);
                            // Load new RtrbauElement from AssetVisualiser, ensure user has selected the type of RtrbauElement to load
                            RtrbauerEvents.TriggerEvent("AssetVisualiser_CreateElement", elementIndividual, elementClass, Rtrbauer.instance.user.procedure);
                            // Complete paneller behaviour: destroy this element
                            DestroyElement();
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
                        Debug.Log("PanelClassSubclasses::EvaluateNextStep: " + entity.Name() + " is made of datatype individuals");
                        panelUsage.text = entity.Name() + " is made of datatype individuals";

                        // Return to previous class nomination
                        Initialise(previousClassElement);
                    }
                    else if (subclassHasProperties == false && subclassHasIndividuals == false)
                    {
                        // This case should never happen?
                        Debug.LogError("PanelClassSubclasses::EvaluateNextStep: " + "Rationale error: this case should never happen");
                        panelUsage.text = entity.Name() + "Rationale error: this case should never happen";

                        // Return to previous class nomination
                        Initialise(previousClassElement);
                    }
                    else
                    {
                        // This case should never happen?
                        Debug.LogError("PanelClassSubclasses::EvaluateNextStep: " + "Rationale error: this case should never happen");
                        panelUsage.text = entity.Name() + "Rationale error: this case should never happen";

                        // Return to previous class nomination
                        Initialise(previousClassElement);
                    }
                }
                else
                {
                    // This case should never happen?
                    Debug.LogError("PanelClassSubclasses::EvaluateNextStep: " + "Rationale error: this case should never happen");
                    panelUsage.text = entity.Name() + "Rationale error: this case should never happen";

                    // Return to previous class nomination
                    Initialise(previousClassElement);
                }
            }
            else
            {
                // subclassEvaluation += 1;
                Debug.Log("PanelClassSubclasses::EvaluateNextStep: " + entity.Name() + " | evaluationInvoke number: " + subclassEvaluation);
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


