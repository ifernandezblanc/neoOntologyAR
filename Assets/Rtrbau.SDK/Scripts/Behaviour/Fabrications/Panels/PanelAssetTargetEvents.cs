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
using TMPro;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class PanelAssetTargetEvents : MonoBehaviour, ITrackableEventHandler
    {
        #region INITIALISATION_VARIABLES
        public GameObject assetTarget;
        public GameObject assetRegistrationButton;
        public TextMeshPro assetStatus;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public TrackableBehaviour assetTargetTrackableBehaviour;
        private bool assetTargetTrackableBehaviourFound;
        #endregion CLASS_VARIABLES

        #region MONOBEHAVIOUR_METHODS
        protected virtual void Start()
        {
            assetTargetTrackableBehaviourFound = false;
            StartCoroutine(ForceVuforiaRegistration());
        }

        protected virtual void Update()
        {

        }

        protected virtual void OnDestroy()
        {
            if (assetTargetTrackableBehaviour != null)
            {
                assetTargetTrackableBehaviour.UnregisterTrackableEventHandler(this);
            }
        }
        #endregion MONOBEHAVIOUR_METHODS

        #region ITRACKABLEEVENTHANDLER_METHODS
        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                Debug.Log("PanelAssetTargetEvents::OnTrackableStateChanged: " + assetTargetTrackableBehaviour.TrackableName + " found");
                WriteStatusChange("Asset found");
                assetTargetTrackableBehaviourFound = true;
                assetRegistrationButton.SetActive(true);
            }
            else if (previousStatus == TrackableBehaviour.Status.NO_POSE)
            {
                Debug.Log("PanelAssetTargetEvents::OnTrackableStateChanged: " + assetTargetTrackableBehaviour.TrackableName + " lost");
                WriteStatusChange("No pose found");
            }
            else if (previousStatus == TrackableBehaviour.Status.LIMITED)
            {
                WriteStatusChange("Asset on limited tracking");
            }
            else
            {
                // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
                // Vuforia is starting, but tracking has not been lost or found yet
                // Call OnTrackingLost() to hide the augmentations
                // OnTrackingLost();
                WriteStatusChange("Asset target destroyed");
            }
        }
        #endregion ITRACKABLEEVENTHANDLER_METHODS

        #region CLASS_METHODS

        #region PRIVATE
        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        void WriteStatusChange(string status)
        {
            if (assetStatus != null)
            {
                assetStatus.text = status;
            }
            else { }
        }

        IEnumerator ForceVuforiaRegistration()
        {
            int counter = 30;
            Debug.Log("PanelAssetTargetEvents::ForceVuforiaRegistration: waiting for " + counter + " seconds before forcing registration");
            while (counter > 0 && assetTargetTrackableBehaviourFound == false)
            {
                yield return new WaitForSeconds(1);
                counter--;
                Debug.Log("PanelAssetTargetEvents::ForceVuforiaRegistration: waiting for " + counter + " seconds before forcing registration");
            }
            Debug.Log("PanelAssetTargetEvents::ForceVuforiaRegistration: asset registration being forced");
            WriteStatusChange("Asset found");
            assetTargetTrackableBehaviourFound = true;
            assetRegistrationButton.SetActive(true);
        }
        #endregion PRIVATE

        #region PUBLIC
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetAsset"></param>
        /// <param name="statusPanel"></param>
        /// <param name="registrationButton"></param>
        public virtual void Initialise(GameObject targetAsset, TextMeshPro statusPanel, GameObject registrationButton)
        {
            assetTarget = targetAsset;

            assetTargetTrackableBehaviour = assetTarget.GetComponent<TrackableBehaviour>();

            if (assetTargetTrackableBehaviour != null)
            {
                Debug.Log("PanelAssetTargetEvents: Registered asset target: " + assetTargetTrackableBehaviour.TrackableName);
                assetTargetTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
            else
            {
                throw new ArgumentException("PanelAssetTargetEvents: Registered asset target: trackable behaviour not found");
            }

            if (statusPanel == null || registrationButton == null)
            {
                throw new ArgumentException("PanelAssetTargetEvents: Registered asset target: panels not found");
            }
            else
            {
                assetStatus = statusPanel;
                assetRegistrationButton = registrationButton;
            }

        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}


