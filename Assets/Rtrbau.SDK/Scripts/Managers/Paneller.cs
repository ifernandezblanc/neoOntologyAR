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

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System;
using System.Collections.Generic;
using UnityEngine;
#endregion

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class Paneller : MonoBehaviour
    {
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        #region SINGLETON_INSTANTIATION
        private static Paneller panellerManager;
        public static Paneller instance
        {
            get
            {
                if (!panellerManager)
                {
                    panellerManager = FindObjectOfType(typeof(Paneller)) as Paneller;

                    if (!panellerManager)
                    {
                        Debug.LogError("There needs to be an Paneller script in the scene.");
                    }
                    else
                    {
                        panellerManager.Initialise();
                    }
                }
                else { }

                return panellerManager;
            }
        }
        #endregion SINGLETON_INSTANTIATION

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        #region CLASS_VARIABLES
        private GameObject panelConfiguration;
        private GameObject panelAssets;
        private GameObject panelAssetRegistrator;
        private GameObject panelOntologies;
        private GameObject panelClasses;
        private GameObject panelIndividuals;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public GameObject configurationPanel;
        public GameObject assetsPanel;
        public GameObject assetRegistratorPanel;
        public GameObject ontologiesPanel;
        public GameObject classesPanel;
        public GameObject individualsPanel;
        #endregion GAMEOBJECT_PREFABS

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        #region CLASS_METHODS
        public void Initialise()
        {
            if (configurationPanel == null || assetsPanel == null || assetRegistratorPanel == null || 
                ontologiesPanel == null || classesPanel == null || individualsPanel == null)
            {
                Debug.LogError("Rtrbau Panel elements not found, cannot initialise Paneller.");
            }
            else
            {
                // Declare paneller initialisation
                string serverconfig = Rtrbauer.instance.ontology.ontologyURI.AbsoluteUri + "/" + "server" + "#" + "connected";
                OntologyEntity configuration = new OntologyEntity(serverconfig);
                PanellerEvents.TriggerEvent("LoadConfiguration", configuration);
            }
        }

        public void LoadConfiguration(OntologyEntity entity)
        {
            // Destroy objects and events
            Debug.Log("Paneller: LoadConfiguration: " + entity.URI());
            PanellerEvents.StopListening("LoadConfiguration", LoadConfiguration);

            // Initialise fabrication
            panelConfiguration = Instantiate(configurationPanel, this.transform);
            panelConfiguration.GetComponent<PanelConfiguration>().Initialise(Rtrbauer.instance.server.serverURI.ToString());
        }

        public void LoadAssets(OntologyEntity entity)
        {
            // Destroy objects and events
            Debug.Log("Paneller: LoadAssets " + entity.URI());
            PanellerEvents.StopListening("LoadAssets", LoadAssets);
            Destroy(panelConfiguration);

            // Declare initialisation variables
            OntologyElement assetClass = new OntologyElement(entity.URI(), OntologyElementType.ClassIndividuals);

            // Initialise fabrication
            panelAssets = Instantiate(assetsPanel, this.transform);
            panelAssets.GetComponent<PanelAssets>().Initialise(assetClass);
        }

        public void LoadAssetRegistrator(OntologyEntity entity)
        {
            // Destroy objects and events
            Debug.Log("Paneller: LoadAssetRegistrator " + entity.URI());
            PanellerEvents.StopListening("LoadAssetRegistrator", LoadAssetRegistrator);
            Destroy(panelAssets);

            // Declare initialisation variables

            // Initialise fabrication
            panelAssetRegistrator = Instantiate(assetRegistratorPanel, this.transform);
            panelAssetRegistrator.GetComponent<PanelAssetRegistrator>().Initialise(entity);           
        }

        public void LoadOperationOntologies(OntologyEntity entity)
        {
            // Destroy objects and events
            Debug.Log("Paneller: LoadOperationOntologies " + entity.URI());
            // Disabled stop listening for new incoming reports (same user and time)
            // PanellerEvents.StopListening("LoadOperationOntologies", LoadOperationOntologies);
            Destroy(panelAssetRegistrator);

            // Declare initialisation variables
            OntologyElement ontologies = new OntologyElement(entity.URI(), OntologyElementType.Ontologies);

            // Initialise fabrication
            panelOntologies = Instantiate(ontologiesPanel, this.transform);
            panelOntologies.GetComponent<PanelOntologies>().Initialise(ontologies);
        }

        public void LoadOperationSubclasses(OntologyEntity entity)
        {
            // Destroy objects and events
            Debug.Log("Paneller: LoadOperationSubclasses " + entity.URI());
            // Disabled stop listening for new incoming reports (same user and time)
            // PanellerEvents.StopListening("LoadOperationSubclasses", LoadOperationSubclasses);
            Destroy(panelOntologies);

            // Declare initialisation variables
            // Remember that first trigger to subclasses comes from class with same name as ontology
            OntologyEntity ontologyEntity = new OntologyEntity(entity.URI() + entity.ontology);
            OntologyElement ontologyClasses = new OntologyElement(ontologyEntity.URI(), OntologyElementType.ClassSubclasses);

            // Initialise fabrication
            panelClasses = Instantiate(classesPanel, this.transform);
            panelClasses.GetComponent<PanelClassSubclasses>().Initialise(ontologyClasses);
        }

        public void LoadOperationIndividuals(OntologyEntity entity)
        {
            // Destroy objects and events
            Debug.Log("Paneller: LoadOperationIndividuals " + entity.URI());
            // Disabled stop listening for new incoming reports (same user and time)
            // PanellerEvents.StopListening("LoadOperationIndividuals", LoadOperationIndividuals);
            Destroy(panelClasses);

            // Declare initialisation variables
            OntologyElement classIndividuals = new OntologyElement(entity.URI(), OntologyElementType.ClassIndividuals);

            // Initialise fabrication
            panelIndividuals = Instantiate(individualsPanel, this.transform);
            panelIndividuals.GetComponent<PanelClassIndividuals>().Initialise(classIndividuals);
        }

        public void UnloadPaneller(OntologyEntity entity)
        {
            // Destroy objects and events
            Debug.Log("Paneller: LoadVisualiser " + entity.URI());
            // Disabled stop listening for new incoming reports (same user and time)
            // PanellerEvents.StopListening("UnloadPaneller", UnloadPaneller);
            Destroy(panelIndividuals);

            // The rationale ends here for now. Please send report.
            // Reporter.instance.SendReport();
            // Debug.Log("Paneller: LoadVisualiser: Report sent");
        }
        #endregion CLASS_METHODS

        #region MONOBEHAVIOUR_METHODS
        void Start()
        {
            // Uncomment if paneller to be used on its own for the moment
            // Initialise();
        }

        void OnEnable()
        {
            PanellerEvents.StartListening("LoadConfiguration", LoadConfiguration);
            PanellerEvents.StartListening("LoadAssets", LoadAssets);
            PanellerEvents.StartListening("LoadAssetRegistrator", LoadAssetRegistrator);
            PanellerEvents.StartListening("LoadOperationOntologies", LoadOperationOntologies);
            PanellerEvents.StartListening("LoadOperationSubclasses", LoadOperationSubclasses);
            PanellerEvents.StartListening("LoadOperationIndividuals", LoadOperationIndividuals);
            PanellerEvents.StartListening("UnloadPaneller", UnloadPaneller);
        }

        void Update()
        {

        }

        void OnDisable()
        {
            
        }
        #endregion MONOBEHAVIOUR_METHODS
    }
}