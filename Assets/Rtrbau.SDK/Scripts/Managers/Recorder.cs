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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine.Assertions;
using UnityEngine.XR.WSA.WebCam;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// Image Recorder Manager: https://github.com/JannikLassahn/hololens-photocapture/blob/master/Assets/Scripts/PhotoManager.cs
    /// Considers usage of IL2CPP back-end for .Net back-end please visit the link above
    /// Considers automatic camera initialisation, assuming it has not been initialised before hand
    /// Audio Record Example 1: http://www.41post.com/4884/programming/unity-capturing-audio-from-a-microphone
    /// Audio Record Example 2: https://docs.unity3d.com/2019.1/Documentation/ScriptReference/Microphone.Start.html
    /// </summary>
    public class Recorder : MonoBehaviour
    {
        #region CLASS_MEMBERS
        #region SINGLETON_INITIALISATION
        private static Recorder recorderManager;

        public static Recorder instance
        {
            get
            {
                if (!recorderManager)
                {
                    recorderManager = FindObjectOfType(typeof(Recorder)) as Recorder;

                    if (!recorderManager)
                    {
                        Debug.LogError("Recorder::Instance: There needs to be a Recorder script in the scene.");
                    }
                    else
                    {
                        recorderManager.Initialise();
                    }
                }
                else { }

                return recorderManager;
            }
        }
        #endregion SINGLETON_INITIALISATION

        #region AUDIO
        private int audioRecordMaxLength;
        private int microphoneMaxFrequency;
        private AudioSource microphoneSource;
        #endregion AUDIO

        #region IMAGE
        private bool showHolograms;
        private bool cameraIsReady;
        private PhotoCapture imageCapture;
        private OntologyFile imageFile;
        #endregion IMAGE
        #endregion CLASS_MEMBERS

        #region CLASS_METHODS
        #region PRIVATE
        #region INITIALISATION
        void Initialise()
        {
            microphoneSource = this.gameObject.AddComponent<AudioSource>();
            microphoneMaxFrequency = 44100;
            audioRecordMaxLength = 5;
        }

        void InitialiseCamera(OntologyFile image)
        {
            showHolograms = false;
            imageCapture = null;
            cameraIsReady = false;
            imageFile = image;
        }
        #endregion INITIALISATION

        #region AUDIO
        IEnumerator AudioCapture(OntologyFile audioFile)
        {
            if (Microphone.devices.Length <= 0)
            {
                throw new ArgumentException("Recorder::AudioCapture: Microphone not connected.");
            }
            else 
            {
                if (Microphone.IsRecording("Built-in Microphone"))
                {
                    throw new ArgumentException("Recorder:AudioCapture: Microphone is recording.");
                }
                else
                {
                    microphoneSource.clip = Microphone.Start("Built-in Microphone", false, audioRecordMaxLength*2, microphoneMaxFrequency);
                    Viewer.instance.StartRecordingForSeconds(audioRecordMaxLength + 1);
                    yield return new WaitForSeconds(audioRecordMaxLength);
                    Microphone.End("Built-in Microphone");
                    AudioSaver.Save(audioFile.FilePath(), microphoneSource.clip);
                    RecorderEvents.TriggerEvent(audioFile.EventName(), audioFile);
                }
            }
        }
        #endregion AUDIO

        #region IMAGE
        void StartImageCapture(OntologyFile imageFile)
        {
            if (cameraIsReady)
            {
                Debug.Log("Recorder::StartCamera: camera is already running.");
                return;
            }
            else 
            { 
                InitialiseCamera(imageFile); 
            }
            PhotoCapture.CreateAsync(showHolograms, OnPhotoCaptureCreated);
        }

        void StopImageCapture()
        {
            if (cameraIsReady)
            {
                imageCapture.StopPhotoModeAsync(OnPhotoModeStopped);
            }
        }

        void OnPhotoCaptureCreated(PhotoCapture captureObject)
        {
            // Assign capture object
            imageCapture = captureObject;
            // Identify camera resolution supported
            Resolution resolution = PhotoCapture.SupportedResolutions.OrderByDescending(res => res.width * res.height).First();
            // Assign camera parameters
            CameraParameters c = new CameraParameters(WebCamMode.PhotoMode);
            c.hologramOpacity = 1.0f;
            c.cameraResolutionWidth = resolution.width;
            c.cameraResolutionHeight = resolution.height;
            c.pixelFormat = CapturePixelFormat.BGRA32;
            // Return camera parameters to start image record mode
            imageCapture.StartPhotoModeAsync(c, OnPhotoModeStarted);
        }

        void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult captureObjectResult)
        {
            // Assign camera ready value
            cameraIsReady = captureObjectResult.success;
            // Call to image record method
            StartCoroutine(ImageCapture());
        }

        IEnumerator ImageCapture()
        {
            if (cameraIsReady)
            {
                if (imageFile != null)
                {
                    PhotoCaptureFileOutputFormat imageFormat;

                    if (imageFile.type == RtrbauFileType.png) { imageFormat = PhotoCaptureFileOutputFormat.PNG; }
                    else if (imageFile.type == RtrbauFileType.jpg) { imageFormat = PhotoCaptureFileOutputFormat.JPG; }
                    else { throw new ArgumentException("Recorder::RecordImage: file type not available for images."); }

                    Viewer.instance.StartRecordingForSeconds(6);
                    yield return new WaitForSeconds(5);

                    imageCapture.TakePhotoAsync(imageFile.FilePath(), imageFormat, OnCapturedPhotoToDisk);
                }
                else
                {
                    throw new ArgumentException("Recorder::RecordImage: imageFile has not been assigned.");
                }
            }
            else
            {
                Debug.LogWarning("Recorder::RecordImage: The camera is not ready yet.");
            }
        }

        void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult captureObjectResult)
        {
            if (captureObjectResult.success)
            {
                Debug.Log("Recorder::OnCapturedPhotoToDisk: Image recorded save at " + imageFile.FilePath());
            }
            else
            {
                Debug.Log(string.Format("Recorder::OnCapturedPhotoToDisk: Failed to save image to disk ({0})", captureObjectResult.hResult));
            }
            // Stop camera
            StopImageCapture();
        }

        void OnPhotoModeStopped(PhotoCapture.PhotoCaptureResult captureObjectResult)
        {
            // Dispose image capture
            imageCapture.Dispose();
            // Return image file event trigger
            RecorderEvents.TriggerEvent(imageFile.EventName(), imageFile);
            // Initialise camera with null OntologyFile
            InitialiseCamera(null);
        }
        #endregion IMAGE
        #endregion PRIVATE

        #region PUBLIC
        #region AUDIO
        public void StartAudioRecord(OntologyFile audioFile)
        {
            StartCoroutine(AudioCapture(audioFile));
        }
        #endregion AUDIO

        #region IMAGE
        public void StartImageRecord(OntologyFile imageFile)
        {
            StartImageCapture(imageFile);
        }
        #endregion IMAGE
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}
