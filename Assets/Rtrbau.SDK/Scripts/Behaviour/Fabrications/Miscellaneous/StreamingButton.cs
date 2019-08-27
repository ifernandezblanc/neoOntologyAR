/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 25/07/2019
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
#endregion


namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class StreamingButton : MonoBehaviour
    {

        #region SINGLETON_INSTANTIATION
        private static StreamingButton streamingButton;
        public static StreamingButton instance
        {

            get
            {
                if (!streamingButton)
                {
                    streamingButton = FindObjectOfType(typeof(StreamingButton)) as StreamingButton;

                    if (!streamingButton)
                    {
                        Debug.LogError("There needs to be an Paneller script in the scene.");
                    }
                    else
                    {
                        // streamingButton.Initialise();
                    }
                }
                else { }

                return streamingButton;
            }
        }
        #endregion SINGLETON_INSTANTIATION

        #region CLASS_VARIABLES
        private bool cameraActive = false;
        private bool buttonActive = false;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public Material buttonMaterialActive;
        public Material buttonMaterialInactive;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_METHODS
        public void Initialise()
        {
            if (buttonMaterialActive == null || buttonMaterialInactive == null)
            {
                throw new ArgumentException("Materials need to be referenced in this script.");
            }
            else
            {
                cameraActive = true;
                buttonActive = true;

                this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Activate Streaming";
                this.transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material = buttonMaterialInactive;
            }
        }

        public void StreamingActivation()
        {
            if (buttonActive)
            {
                if (cameraActive)
                {
                    // Tracker.instance.StopStreaming();
                    this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Deactivate Streaming";
                    this.transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material = buttonMaterialActive;
                    cameraActive = false;
                }
                else
                {
                    // Tracker.instance.StartStreaming();
                    this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Activate Streaming";
                    this.transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material = buttonMaterialInactive;
                    cameraActive = true;
                }
            }
            else { }
        }
        #endregion CLASS_METHODS
    }
}