/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 26/11/2019
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class Viewer : MonoBehaviour
    {
        #region CLASS_MEMBERS
        #region SINGLETON_INITIALISATION
        private static Viewer viewerManager;

        public static Viewer instance
        {
            get
            {
                if (!viewerManager)
                {
                    viewerManager = FindObjectOfType(typeof(Viewer)) as Viewer;

                    if (!viewerManager)
                    {
                        Debug.LogError("Viewer::Instance: There needs to be a Viewer script in the scene.");
                    }
                    else
                    {
                        viewerManager.Initialise();
                    }
                }
                else { }

                return viewerManager;
            }
        }
        #endregion SINGLETON_INITIALISATION

        #region PREFABS
        public GameObject cranfieldLogo;
        public GameObject babcockLogo;
        public GameObject recordingFrame;
        public GameObject recordingButton;
        public TextMeshProUGUI recordingTimerText;
        #endregion PREFABS

        #region EVENTS
        private bool recordingActive;
        #endregion EVENTS
        #endregion CLASS_MEMBERS

        #region CLASS_METHODS
        #region PRIVATE
        #region INITIALISATION
        void Initialise()
        {
            if (cranfieldLogo == null || babcockLogo == null || recordingFrame == null || recordingButton == null || recordingTimerText == null)
            {
                throw new ArgumentException("Viewer::Initialise: Viewer requires some prefabs to work correctly.");
            }
            else
            {
                cranfieldLogo.SetActive(true);
                babcockLogo.SetActive(true);
                recordingFrame.SetActive(false);
                recordingButton.SetActive(false);
                recordingTimerText.gameObject.SetActive(false);
                recordingTimerText.text = null;
                recordingActive = false;
            }
        }
        #endregion INITIALISATION
        #region VIEWER
        IEnumerator Recording()
        {
            recordingFrame.SetActive(true);
            while (recordingActive == true)
            {
                yield return new WaitForSeconds(0.5f);
                if (recordingButton.activeSelf) { recordingButton.SetActive(false); }
                else { recordingButton.SetActive(true); }
            }
            recordingFrame.SetActive(false);
        }

        IEnumerator RecordingForSeconds(int seconds)
        {
            int counter = seconds * 2;
            int timer = seconds;
            bool halfSecond = false;
            recordingActive = true;
            recordingFrame.SetActive(true);
            recordingTimerText.gameObject.SetActive(true);
            while (counter > 0)
            {
                yield return new WaitForSeconds(0.5f);
                if (recordingButton.activeSelf) { recordingButton.SetActive(false); }
                else { recordingButton.SetActive(true); }
                if (halfSecond == false)
                {
                    recordingTimerText.text = timer.ToString();
                    timer--;
                    halfSecond = true;
                }
                else { halfSecond = false; }
                counter--;
            }
            recordingTimerText.text = null;
            recordingTimerText.gameObject.SetActive(false);
            recordingFrame.SetActive(false);
            recordingActive = false;
        }
        #endregion VIEWER
        #endregion PRIVATE

        #region PUBLIC
        #region VIEWER
        public void StartRecording()
        {
            if (recordingActive == false)
            {
                recordingActive = true;
                StartCoroutine(Recording());
            }
            else { }
        }

        public void StopRecording()
        {
            if (recordingActive == true)
            {
                recordingActive = false;
                StopCoroutine(Recording());
            }
            else { }
        }

        public void StartRecordingForSeconds(int seconds)
        {
            StartCoroutine(RecordingForSeconds(seconds));
        }
        #endregion VIEWER
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}
