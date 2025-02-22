﻿/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 23/07/2019
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using AsImpL;
using Microsoft.MixedReality.Toolkit.UI;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class PanelAssetRegistrator : MonoBehaviour, IElementable
    {
        #region INITIALISATION_VARIABLES
        public OntologyEntity assetEntity;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public OntologyFile assetModelOBJ;
        public OntologyFile assetTargetXML;
        public OntologyFile assetTargetDAT;

        public VuforiaTargetType assetTargetType;
        public GameObject assetTarget;
        public GameObject assetModel;

        public ObjectImporter assetModelImporter;
        public ImportOptions assetModelImportOptions;
        
        public int assetDatasetFilesDownloaded;
        public int assetRegistratorObjectsLoaded;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public Material assetModelMaterial;
        public TextMeshPro assetNamePanel;
        public TextMeshPro assetStatusPanel;
        public GameObject loadingPlate;
        public GameObject assetRegistrationButton;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        private event Action OnDatasetFileDownloaded;
        private event Action OnRegistratorObjectsDownloaded;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Awake()
        {
            // Add ObjectImporter from AsImpL and configure
            assetModelImporter = gameObject.AddComponent<ObjectImporter>();
            ConfigureModelImport();
            
            // Register event callbacks
            assetModelImporter.ImportedModel += LoadAssetModel;
            OnDatasetFileDownloaded += LoadAssetTarget;
            OnRegistratorObjectsDownloaded += LoadAssetRegistrator;
        }

        void Start()
        {
        }

        void OnEnable()
        {
        }

        void Update()
        {
        }

        void OnDisable()
        {
        }

        void OnDestroy()
        {
            // Unregister events callbacks
            assetModelImporter.ImportedModel -= LoadAssetModel;
            OnDatasetFileDownloaded -= LoadAssetTarget;
            OnRegistratorObjectsDownloaded -= LoadAssetRegistrator;
        }
        #endregion MONOBEHAVIOUR_METHODS

        #region INITIALISATION_METHODS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="asset"></param>
        public void Initialise(OntologyEntity asset)
        {
            if (assetModelMaterial == null || loadingPlate == null || assetNamePanel == null || assetStatusPanel == null || assetRegistrationButton == null)
            {
                Debug.LogError("Fabrications not found for this rtrbau element.");
            }
            else
            {
                assetEntity = asset;

                assetModel = new GameObject(assetEntity.Name());

                assetDatasetFilesDownloaded = 0;
                assetRegistratorObjectsLoaded = 0;

                DeactivateLoadingPlate();
                assetRegistrationButton.SetActive(false);

                DownloadElement();
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
            Debug.Log("AssetRegistrator: DownloadElement: Started download");

            assetModelOBJ = new OntologyFile(assetEntity.Name(), "obj");
            assetTargetXML = new OntologyFile(assetEntity.Name(), "xml");
            assetTargetDAT = new OntologyFile(assetEntity.Name(), "dat");

            LoaderEvents.StartListening(assetModelOBJ.EventName(), DownloadedAssetModel);
            Loader.instance.StartFileDownload(assetModelOBJ);
            Debug.Log("AssetRegistrator: DownloadElement: Started download " + assetModelOBJ.URL());

            LoaderEvents.StartListening(assetTargetXML.EventName(), DownloadedAssetTargetXML);
            Loader.instance.StartFileDownload(assetTargetXML);
            Debug.Log("AssetRegistrator: DownloadElement: Started download " + assetTargetXML.URL());

            LoaderEvents.StartListening(assetTargetDAT.EventName(), DownloadedAssetTargetDAT);
            Loader.instance.StartFileDownload(assetTargetDAT);
            Debug.Log("AssetRegistrator: DownloadElement: Started download " + assetTargetXML.URL());
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void EvaluateElement()
        {

        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void LocateElement()
        {

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

        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void CreateFabrications()
        {

        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void InputIntoReport()
        {
            // When asset has been registered with Vuforia
            // Detach asset model
            assetTarget.transform.DetachChildren();
            // Initialise asset model as asset manager
            assetModel.AddComponent<AssetManager>();
            // Initialise asset registrator (manual)
            assetModel.GetComponent<AssetManager>().Initialise();
            // Initialise asset registration button
            RegistrationButton.instance.Initialise(assetModel.GetComponent<AssetManager>().assetRegistrator);
            // Destroy asset target
            Destroy(assetTarget);
            // Terminate Vuforia
            Tracker.instance.StopVuforia();
            Tracker.instance.TerminateVuforia();
            
            // Move to next panel
            string ontologiesURI = Rtrbauer.instance.server.AbsoluteUri + "ontologies#ontologies";
            OntologyEntity ontologies = new OntologyEntity(ontologiesURI);
            PanellerEvents.TriggerEvent("LoadOperationOntologies", ontologies);

            // Destroy panel
            DestroyElement();
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

        #region CLASS_METHODS
        #region PRIVATE
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void ConfigureModelImport()
        {
            // Configure import options
            assetModelImportOptions.zUp = false;
            assetModelImportOptions.modelScaling = 0.001f;
            assetModelImportOptions.buildColliders = true;
            // UPG: check if its possible to de-activate materials loading
            assetModelImportOptions.hideWhileLoading = true;
            // assetModelImportOptions.localEulerAngles = new Vector3(0, 180, 0);
        }
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void DownloadedAssetModel(OntologyFile modelAsset)
        {
            LoaderEvents.StopListening(modelAsset.EventName(), DownloadedAssetModel);
            Debug.Log("RegistratorAsset: DownloadedAssetModel: downloaded " + modelAsset.URL());

            if (modelAsset != null)
            {
                string modelName = "Model_" + modelAsset.name;
                assetModelImporter.ImportModelAsync(modelName, modelAsset.FilePath(), assetModel.transform, assetModelImportOptions);
                assetStatusPanel.text = "Loading Asset Model";
                ActivateLoadingPlate();
            }
            else
            {
                Debug.LogError("File not found.");
            }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void DownloadedAssetTargetXML(OntologyFile targetXMLAsset)
        {
            LoaderEvents.StopListening(targetXMLAsset.EventName(), DownloadedAssetTargetXML);
            Debug.Log("RegistratorAsset: DownloadedAssetTargetXML: downloaded " + targetXMLAsset.URL());

            if (targetXMLAsset != null)
            {
                assetDatasetFilesDownloaded += 1;

                if (OnDatasetFileDownloaded != null)
                {
                    OnDatasetFileDownloaded.Invoke();
                }
                else { }
            }
            else
            {
                Debug.LogError("File not found.");
            }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void DownloadedAssetTargetDAT(OntologyFile targetDATAsset)
        {
            LoaderEvents.StopListening(targetDATAsset.EventName(), DownloadedAssetTargetDAT);
            Debug.Log("RegistratorAsset: DownloadedAssetTargetDAT: downloaded " + targetDATAsset.URL());

            if (targetDATAsset != null)
            {
                assetDatasetFilesDownloaded += 1;

                if (OnDatasetFileDownloaded != null)
                {
                    OnDatasetFileDownloaded.Invoke();
                }
                else { }
            }
            else
            {
                Debug.LogError("File not found");
            }
        }

        void LoadAssetTarget()
        {
            if (assetDatasetFilesDownloaded == 2)
            {
                // Load asset target using tracker
                KeyValuePair<VuforiaTargetType,GameObject> assetTargetPair = Tracker.instance.LoadAssetTarget(assetTargetDAT.name);

                if (assetTargetPair.Key == VuforiaTargetType.DifferentTarget)
                {
                    throw new ArgumentException("PanelAssetRegistrator::LoadAssetTarget: Vuforia target type not implemented.");
                }
                else
                {
                    // Load asset target using tracker
                    assetTargetType = assetTargetPair.Key;
                    assetTarget = assetTargetPair.Value;
                    assetNamePanel.text = assetTargetDAT.name;
                    assetStatusPanel.text = "Look around and gaze at your asset.";

                    // Load asset target events
                    this.gameObject.AddComponent<PanelAssetTargetEvents>();
                    this.gameObject.GetComponent<PanelAssetTargetEvents>().Initialise(assetTarget, assetStatusPanel, assetRegistrationButton);

                    assetRegistratorObjectsLoaded += 1;

                    if (OnRegistratorObjectsDownloaded != null)
                    {
                        OnRegistratorObjectsDownloaded.Invoke();
                    }
                    else { }
                }
            }
            else
            {
                Debug.Log("RegistratorAsset: :LoadAssetTarget: Wait for other dataset files " + assetDatasetFilesDownloaded);
            }
        }

        void LoadAssetModel(GameObject gameObject, string absolutePath)
        {
            // Find mesh renderers for asset components models
            MeshRenderer[] components = assetModel.transform.GetChild(0).GetComponentsInChildren<MeshRenderer>();
            // Assign manipulator and material to components models meshes renderers
            LoadAssetModelsManipulators(components, assetModelMaterial);
            
            assetRegistratorObjectsLoaded += 1;

            if (OnRegistratorObjectsDownloaded != null)
            {
                assetStatusPanel.text = "Loaded Asset Model";
                DeactivateLoadingPlate();
                OnRegistratorObjectsDownloaded.Invoke();
            }
            else { }
        }

        void LoadAssetRegistrator()
        {
            if (assetRegistratorObjectsLoaded == 2)
            {
                // Add asset model as children of asset target
                assetModel.transform.SetParent(assetTarget.transform, false);
                // Rotate asset model 180 degrees on y-axis to align with target
                assetModel.transform.Rotate(new Vector3(0, 180, 0), Space.Self);

                // Redefine asset model scale in case asset target is of type image target
                if (assetTargetType == VuforiaTargetType.ImageTarget)
                {
                    // Assumes the image target is of width smaller than one meter (scale < 1)
                    float sX = assetModel.transform.localScale.x / assetTarget.transform.localScale.x;
                    float sY = assetModel.transform.localScale.y / assetTarget.transform.localScale.y;
                    float sZ = assetModel.transform.localScale.z / assetTarget.transform.localScale.z;
                    assetModel.transform.localScale = new Vector3(sX, sY, sZ);
                }

                Debug.Log("RegistratorAsset: LoadAssetRegistrator: Asset Registrator Objects found.");

                assetStatusPanel.text = "Asset Registrator Objects found. Please gaze at your asset.";
            }
            else
            {
                Debug.Log("RegistratorAsset: :LoadAssetRegistrator: Wait for other dataset files " + assetRegistratorObjectsLoaded);
            }
        }

        void LoadAssetModelsManipulators(MeshRenderer[] models, Material material)
        {
            foreach (MeshRenderer model in models)
            {
                // Set material of model mesh renderer
                model.material = material;
            }
        }
        #endregion PRIVATE
        #endregion CLASS_METHODS
    }
}
