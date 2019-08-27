/*==============================================================================
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
using Vuforia;
using System.Linq;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// This script aims to manage all Vuforia components to be initialised for enable Vuforia Behaviour
    /// This script should control initialisation as well as activation of all Vuforia components
    /// The solution provided in the following link has been followed: 
    /// https://developer.vuforia.com/forum/faq/unity-load-dataset-setup-trackables-runtime
    /// https://forum.unity.com/threads/do-not-run-vufory-when-the-application-starts.498351/
    /// This script also aims to manage the load and activation of Vuforia Model Targets
    /// The solution provided in the following links has been followed:
    /// https://developer.vuforia.com/forum/model-targets/dynamically-loading-and-activating-datasets
    /// https://developer.vuforia.com/sites/default/files/LoadDataset.txt
    /// https://developer.vuforia.com/forum/model-targets/activate-model-target-database-unity
    /// https://developer.vuforia.com/forum/unity/deactivateactivate-datasets-model-target
    /// </summary>
    public class Tracker : MonoBehaviour
    {
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        #region SINGLETON_INSTANTIATION
        private static Tracker trackerManager;

        public static Tracker instance
        {
            get
            {
                if (!trackerManager)
                {
                    trackerManager = FindObjectOfType(typeof(Tracker)) as Tracker;

                    if (!trackerManager)
                    {
                        Debug.LogError("There needs to be a Tracker script in the scene.");
                    }
                    else
                    {
                        trackerManager.Initialise();
                    }
                }
                else { }

                return trackerManager;
            }
        }
        #endregion SINGLETON_INSTANTIATION

        #region CLASS_VARIABLES
        public GameObject vuforiaCamera;
        public GameObject tagAlongMenu;

        public string modelTargetDataset;
        public bool vuforiaFirstPlay;
        public bool vuforiaActive;
        public bool cameraActive;
        #endregion CLASS_VARIABLES


        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        #region MONOBEHAVIOUR_METHODS
        void Awake()
        {
            // TrackingEventHandler = GetComponent<TrackingEventHandler>();
            vuforiaCamera = GameObject.Find("ARCamera");
            // ModelTarget = GameObject.Find("ModelTarget");
            // WHEN UPGRADING, THIS SHOULD BE INSTANCIATED RATHER THAN ENABLED
            VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);

            // Initialise false until Vuforia starts for the first time
            vuforiaFirstPlay = false;
            vuforiaActive = false;
            cameraActive = false;
        }

        void Start()
        {
        }

        void Update()
        {

        }
        #endregion MONOBEHAVIOUR_METHODS

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        #region VUFORIA_METHODS
        // Implement in here methods to load dataset when found
        // May need to have a modeltarget game object already in the scene, to be found before loading dataset
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void InitialiseVuforia()
        {
            VuforiaRuntime.Instance.InitVuforia();
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void TerminateVuforia()
        {
            VuforiaRuntime.Instance.Deinit();
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void StartVuforia()
        {
            InitialiseVuforia();

            vuforiaCamera.GetComponent<VuforiaBehaviour>().enabled = true;
            vuforiaCamera.GetComponent<DefaultInitializationErrorHandler>().enabled = true;

            VuforiaBehaviour.Instance.enabled = true;

            if (vuforiaFirstPlay)
            {
                // SwitchTargetByName(modelTargetDataset);
            }
            else { }

            // ModelTarget to be generated when starting Vuforia or when Initiliasing?
            // Add extra function for ModelTargetGeneration()
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void StopVuforia()
        {
            VuforiaBehaviour.Instance.enabled = false;

            vuforiaCamera.GetComponent<VuforiaBehaviour>().enabled = false;
            vuforiaCamera.GetComponent<DefaultInitializationErrorHandler>().enabled = false;

            // TerminateVuforia();
            // To consider the modeltarget to be destroyed when it is started.
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void StartStreaming()
        {
            if (cameraActive != true)
            {
                CameraDevice.Instance.Init();
                CameraDevice.Instance.Start();
                cameraActive = true;
            }
            else { }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void StopStreaming()
        {
            if (cameraActive != false)
            {
                CameraDevice.Instance.Stop();
                CameraDevice.Instance.Deinit();
                cameraActive = false;
            }
            else { }
        }

        /// <summary>
        /// This function will de-activate all current datasets and activate the designated dataset.
        /// It is assumed the dataset being activated has already been loaded (either through code elsewhere or via the Vuforia Configuration).
        /// </summary>
        /// <param name="modelTargetName"></param>
        public void SwitchTargetByName(string modelTargetName)
        {
            TrackerManager trackerManager = (TrackerManager)TrackerManager.Instance;
            ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

            IEnumerable<DataSet> datasets = objectTracker.GetDataSets();
            IEnumerable<DataSet> activeDatasets = objectTracker.GetActiveDataSets();
            List<DataSet> activeDataSetsToBeRemoved = activeDatasets.ToList();

            //Loop through all the active datasets and deactivate them.
            foreach (DataSet ads in activeDataSetsToBeRemoved)
            {
                objectTracker.DeactivateDataSet(ads);
            }

            //Swapping of the datasets should not be done while the ObjectTracker is working at the same time.
            //So, Stop the tracker first.
            objectTracker.Stop();

            //Then, look up the new dataset and if one exists, activate it.
            foreach (DataSet ds in datasets)
            {
                if (ds.Path.Contains(modelTargetName))
                {
                    objectTracker.ActivateDataSet(ds);
                    Debug.Log(ds.ToString());
                    Debug.Log(modelTargetName);
                }
            }

            //Finally, start the object tracker.
            objectTracker.Start();
        }

        public GameObject LoadAssetTarget(string assetDatasetName)
        {
            GameObject assetTarget = new GameObject();
            bool assetTargetFound = false;

            // TrackerManager trackerManager = (TrackerManager)TrackerManager.Instance;
            ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

            Debug.Log("Tracker: LoadAssetTarget: objectTracker found: " + objectTracker.ToString());

            DataSet dataSet = objectTracker.CreateDataSet();

            string datasetFolder;

            if (Dictionaries.fileDataDirectories.TryGetValue(RtrbauFileType.dat, out datasetFolder))
            {
                string datasetPath = datasetFolder + "/" + assetDatasetName + ".xml";

                Debug.Log("Tracker: LoadAssetTarget: datasetPath: " + datasetPath);

                if (dataSet.Load(datasetPath, VuforiaUnity.StorageType.STORAGE_ABSOLUTE))
                {
                    // Stop the object tracker so a new one can be loaded
                    objectTracker.Stop();

                    // Activate the downloaded dataset
                    if (!objectTracker.ActivateDataSet(dataSet))
                    {
                        // Note: ImageTracker cannot have more than 100 total targets activated
                        Debug.LogError("Tracker: LoadAssetTarget: Failed to Activate DataSet: " + assetDatasetName + "</color>");
                    }

                    // Start the object tracker
                    if (!objectTracker.Start())
                    {
                        Debug.LogError("Tracker: LoadAssetTarget: Tracker failed to start.");
                    }

                    // Count targets being loaded from one dataset
                    int targetCounter = 0;

                    IEnumerable<TrackableBehaviour> vuforiaTrackableBehaviours = TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();

                    Debug.Log("Tracker: LoadAssetTarget: number of targets found: " + vuforiaTrackableBehaviours.Count<TrackableBehaviour>());

                    foreach(TrackableBehaviour vTB in vuforiaTrackableBehaviours)
                    {
                        if (vTB.name == "New Game Object")
                        {
                            // Add components to activate trackable behaviour
                            vTB.gameObject.AddComponent<DefaultTrackableEventHandler>();
                            vTB.gameObject.AddComponent<TurnOffBehaviour>();

                            if (vTB.TrackableName == assetDatasetName)
                            {
                                vTB.gameObject.name = "Target_" + vTB.TrackableName;
                                assetTarget = vTB.gameObject;
                                assetTargetFound = true;
                            }
                            else
                            {
                                vTB.gameObject.name = "DynamicImageTarget_"+ ++targetCounter + "_" + vTB.TrackableName;
                            }
                        }
                        else { }
                    }

                    Debug.Log("Tracker: LoadAssetTarget: GO from trackable behaviours: " + targetCounter);

                    if (assetTargetFound)
                    {
                        return assetTarget;
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    throw new ArgumentException("Tracker: LoadTracker: Failed to load dataset: " + assetDatasetName);
                }
            }
            else
            {
                throw new ArgumentException("Tracker: LoadTracker: Dataset file type not implemented: " + assetDatasetName);
            }
        }

        #endregion VUFORIA_METHODS

        #region VUFORIA_CALLBACKS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void OnVuforiaStarted()
        {
            vuforiaFirstPlay = true;
            vuforiaActive = true;
            cameraActive = true;

            // ActivateModelTarget(ModelTargetDataset);
            // SwitchTargetByName(modelTargetDataset);

            return;
        }
        #endregion VUFORIA_CALLBACKS

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        #region CLASS_METHODS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        void Initialise()
        {

        }
        #endregion CLASS_METHODS
    }
}