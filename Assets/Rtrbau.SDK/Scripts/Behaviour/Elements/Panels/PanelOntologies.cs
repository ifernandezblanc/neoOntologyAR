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
    public class PanelOntologies : MonoBehaviour, IElementable
    {
        #region INITIALISATION_VARIABLES
        public OntologyElement sourceElement;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public JsonOntologies ontologies;
        public Dictionary<OntologyEntity, GameObject> fabrications;

        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public GameObject fabricationPrefab;
        public GameObject fabricationLocator;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        private event Action OnSelectedFabrications;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void OnEnable()
        {
            OnSelectedFabrications += LocateElement;
        }

        void OnDisable()
        {
            OnSelectedFabrications -= LocateElement;
            // Remember to destroy all events generated in paneller events for the ontology (paneller being re-used)
            DestroyFabricationsListeners();
        }
        #endregion MONOBEHAVIOUR_METHODS

        #region INITIALISATION_METHODS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void Initialise(OntologyElement ontElement)
        {
            sourceElement = ontElement;
            ontologies = null;
            fabrications = new Dictionary<OntologyEntity, GameObject>();

            DownloadElement();
        }
        #endregion INITIALISATION_METHODS

        #region IELEMENTABLE_METHODS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void DownloadElement()
        {
            Debug.Log("DownloadElement: " + sourceElement.EventName());
            LoaderEvents.StartListening(sourceElement.EventName(), EvaluateOntologies);
            Loader.instance.StartOntElementDownload(sourceElement);
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void EvaluateElement()
        {
            if (File.Exists(sourceElement.FilePath()))
            {
                string jsonFile = File.ReadAllText(sourceElement.FilePath());

                // Debug.Log(jsonFile);

                ontologies = JsonUtility.FromJson<JsonOntologies>(jsonFile);

                // Debug.Log("EvaluateElement: " + jsonFile);

                // Debug.Log("EvaluateElement: has subclasses " + sourceElement.entity.Entity());

                SelectFabrications();

            }
            else
            {
                Debug.LogError("File not found: " + sourceElement.FilePath());
            }

        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void LocateElement()
        {
            // Debug.Log("LocateElement: Fabrications selected to creation");
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
            if (fabricationPrefab == null || fabricationLocator == null)
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
            // Debug.Log("CreateFabrications: Initialising fabrications");

            foreach (JsonOntology ontology in ontologies.ontOntologies)
            {
                OntologyEntity ontologyEntity = new OntologyEntity(ontology.ontUri);
                GameObject ontologyFabrication = Instantiate(fabricationPrefab, fabricationLocator.transform);

                Debug.Log(ontologyEntity.Entity());
                ontologyFabrication.GetComponent<PanelButton>().Initialise(ontologyEntity);

                PanellerEvents.StartListening(ontologyEntity.Entity(), NominatedOntology);

                // Debug.Log("CreateFabrications: Initialised button " + ontologyEntity.ontology);
            }

            fabricationLocator.GetComponent<GridObjectCollection>().UpdateCollection();

        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void InputIntoReport()
        {
            // Implemented in NominatedOntology()
        }
        #endregion IELEMENTABLE_METHODS

        #region CLASS_METHODS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void EvaluateOntologies(OntologyElement element)
        {
            // Debug.Log("EvaluateOntologiess: ontology downloaded " + element.EventName());
            LoaderEvents.StopListening(element.EventName(), EvaluateOntologies);
            EvaluateElement();
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void NominatedOntology(OntologyEntity entity)
        {
            // Confirm ontology chosen through log
            Debug.Log("PanelOntologies::NominatedOntology: Button Clicked " + entity.ontology);
            Debug.Log("PanelOntologies::NominatedOntology: Button Clicked " + DateTime.Now);

            // Report ontology selected: InputIntoReport()
            Reporter.instance.ReportElement(entity);
            // Move to next panel
            PanellerEvents.TriggerEvent("LoadOperationSubclasses", entity);
            // Destroy this element
            DestroyElement();
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void DestroyFabricationsListeners()
        {
            foreach (JsonOntology ontology in ontologies.ontOntologies)
            {
                OntologyEntity ontologyEntity = new OntologyEntity(ontology.ontUri);
                PanellerEvents.StopListening(ontologyEntity.Entity(), NominatedOntology);
            }
        }
        #endregion CLASS_METHODS
    }
}