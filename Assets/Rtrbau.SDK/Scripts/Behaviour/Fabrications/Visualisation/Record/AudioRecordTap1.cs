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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class AudioRecordTap1 : MonoBehaviour, IFabricationable, IVisualisable, IRecordable
    {
        #region INITIALISATION_VARIABLES
        public AssetVisualiser visualiser;
        public RtrbauFabrication data;
        public Transform element;
        public Transform scale;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public UnityAction report;
        public UnityAction play;
        public string audioGenericName;
        public OntologyFile audioRecord;
        public AudioClip audioClip;
        #endregion CLASS_VARIABLES

        #region FACETS_VARIABLES
        #endregion FACETS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro fabricationText;
        public MeshRenderer fabricationSeenPanel;
        public MeshRenderer fabricationReportedPanel;
        public Material fabricationReportedMaterial;
        public Renderer fabricationBounds;
        public GameObject recordAudioButton;
        public GameObject playAudioButton;
        public AudioSource audioRenderer;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        private bool fabricationCreated;
        private bool audioRecorded;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Start()
        {
            if (fabricationText == null || fabricationSeenPanel == null || fabricationReportedPanel == null || fabricationReportedMaterial == null || fabricationBounds == null || recordAudioButton == null || playAudioButton == null || audioRenderer == null)
            {
                throw new ArgumentException("DefaultRecord::Start: Script requires some prefabs to work.");
            }
            else
            {
                recordAudioButton.SetActive(false);
                playAudioButton.SetActive(false);
            }
        }

        void Update() { }

        void OnEnable() { }

        void OnDisable() { }

        void OnDestroy() { DestroyIt(); }
        #endregion MONOBEHVAIOUR_METHODS

        #region IFABRICATIONABLE_METHODS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetVisualiser"></param>
        /// <param name="fabrication"></param>
        /// <param name="elementParent"></param>
        /// <param name="fabricationParent"></param>
        public void Initialise(AssetVisualiser assetVisualiser, RtrbauFabrication fabrication, Transform elementParent, Transform fabricationParent)
        {
            visualiser = assetVisualiser;
            data = fabrication;
            element = elementParent;
            scale = fabricationParent;
            report = RecordAudio;
            play = PlayAudio;
            audioGenericName = null;
            audioRecord = null;
            audioClip = null;
            fabricationCreated = false;
            audioRecorded = false;
            Scale();
            InferFromText();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Scale()
        {
            float sX = this.transform.localScale.x / scale.transform.localScale.x;
            float sY = this.transform.localScale.y / scale.transform.localScale.y;
            float sZ = this.transform.localScale.z / scale.transform.localScale.z;

            this.transform.localScale = new Vector3(sX, sY, sZ);
        }

        /// <summary>
        /// 
        /// </summary>
        public void InferFromText()
        {
            DataFacet audiofacet1 = DataFormats.AudioRecordTap1.formatFacets[0];
            RtrbauAttribute attribute;

            // Check data received meets fabrication requirements
            if (data.fabricationData.TryGetValue(audiofacet1, out attribute))
            {
                fabricationText.text = Parser.ParseNamingOntologyAttribute(attribute.attributeName.Name(), element.GetComponent<ElementReport>().classElement.entity.Name());
                audioGenericName = attribute.attributeName.Name();
                fabricationCreated = true;
                ActivateReporting();
            }
            else
            {
                throw new ArgumentException(data.fabricationName.ToString() + "::InferFromText: cannot implement attribute received.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnNextVisualisation()
        {
            if (audioRecorded == true)
            {
                DataFacet audiofacet1 = DataFormats.AudioRecordTap1.formatFacets[0];
                RtrbauAttribute attribute;

                // Check data received meets fabrication requirements
                if (data.fabricationData.TryGetValue(audiofacet1, out attribute))
                {
                    // Update attribute value according to what user recorded
                    // This assigns to RtrbauElement from ElementReport through RtrbauFabrication
                    attribute.attributeValue = audioRecord.URL();
                    // Change button colour for user confirmation
                    fabricationReportedPanel.material = fabricationReportedMaterial;
                    // Check if all attribute values have been recorded
                    // If true, then ElementReport will input reported element into report
                    // If true, then ElementReport will change colour to reported
                    element.gameObject.GetComponent<ElementReport>().CheckAttributesReported();
                    // Deactivate record button
                    // DeactivateRecords();
                }
                else { }
            }
            else { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Renderer GetRenderer() { return fabricationBounds; }
        #endregion IFABRICATIONABLE_METHODS

        #region IVISUALISABLE_METHODS
        public void LocateIt()
        {
            /// Fabrication location is managed by <see cref="ElementReport"/>.
        }

        public void ActivateIt()
        {
            /// Fabrication activation is managed by <see cref="ElementReport"/>.
        }

        public void DestroyIt()
        {
            Destroy(this.gameObject);
        }

        public void ModifyMaterial(Material material)
        {
            fabricationSeenPanel.material = material;
        }
        #endregion IVISUALISABLE_METHODS

        #region IRECORDABLE_METHODS
        /// <summary>
        /// Activates record buttons when attribute name button is <see cref="OnFocus"/>.
        /// It also triggers deactivation of other record buttons fabrications.
        /// </summary>
        public void ActivateRecords()
        {
            // Call ElementReport to deactivate buttons from other record fabrications
            element.GetComponent<ElementReport>().DeactivateRecords(this.gameObject);
            // Call ElementReport to deactivate buttons from other nominate fabrications
            element.GetComponent<ElementReport>().DeactivateNominates(null);

            if (fabricationCreated == true && recordAudioButton.activeSelf == false)
            {
                if (audioRecorded == true) { playAudioButton.SetActive(true); }
                recordAudioButton.SetActive(true);
            }
            else { }
        }

        /// <summary>
        /// Deactivates record buttons.
        /// It is also called by <see cref="ElementReport"/> to deactivate record buttons when others are to become active.
        /// </summary>
        public void DeactivateRecords()
        {
            if (fabricationCreated == true && recordAudioButton.activeSelf == true)
            {
                if (audioRecorded == true) { playAudioButton.SetActive(false); }
                recordAudioButton.SetActive(false);
            }
            else { }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ActivateReporting()
        {
            if (fabricationCreated == true)
            {
                recordAudioButton.GetComponent<Interactable>().OnClick.AddListener(report);
                playAudioButton.GetComponent<Interactable>().OnClick.AddListener(play);
            }
            else { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="forcedReporting"></param>
        public void DeactivateReporting(bool forcedReporting)
        {
            if (audioRecorded == true || forcedReporting == true)
            {
                recordAudioButton.GetComponent<Interactable>().OnClick.RemoveListener(report);
            }
            else 
            {
                throw new ArgumentException("AudioRecordTap1::DeactivateReporting: this function should not be accesed before an individual is nominated.");
            }
        }
        #endregion IRECORDABLE_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        void OnAudioRecorded(OntologyFile audioFile)
        {
            RecorderEvents.StopListening(audioFile.EventName(), OnAudioRecorded);

            OntologyFileUpload fileUpload = new OntologyFileUpload(audioFile);

            LoaderEvents.StartListening(fileUpload.EventName(), OnAudioUploaded);

            Loader.instance.StartFileUpload(fileUpload);
        }

        void OnAudioUploaded(OntologyFileUpload fileUpload)
        {
            LoaderEvents.StopListening(fileUpload.EventName(), OnAudioUploaded);

            StartCoroutine(LoadAudio(fileUpload.file));

            Debug.Log("AudioRecordDoubleTap1::OnAudioUploaded: Audio file successfully uploaded.");
        }

        IEnumerator LoadAudio(OntologyFile audioFile)
        {
            if (audioFile != null)
            {
                UnityWebRequest audioRequest = UnityWebRequestMultimedia.GetAudioClip(audioFile.FilePath(), AudioType.WAV);

                yield return audioRequest.SendWebRequest();

                if (audioRequest.isNetworkError || audioRequest.isHttpError)
                {
                    Debug.LogError(audioRequest.error);
                }
                else
                {
                    audioClip = DownloadHandlerAudioClip.GetContent(audioRequest);
                    audioRenderer.clip = audioClip;
                    playAudioButton.SetActive(true);
                    audioRecorded = true;
                }
            }
            else
            {
                Debug.LogError("AudioRecordDoubleTap1::LoadAudio: audio file not found");
            }

            // Call to report attribute
            OnNextVisualisation();
            // Remember to deactivate loading plate
            element.GetComponent<IElementable>().DeactivateLoadingPlate();
        }
        #endregion PRIVATE

        #region PUBLIC
        public void RecordAudio()
        {
            // Generate audio file name
            string audioName = Parser.ParseAddDateTime(audioGenericName);
            // Create new ontology file
            audioRecord = new OntologyFile(audioName, RtrbauFileType.wav.ToString());
            // Initialise on audio recorded event
            RecorderEvents.StartListening(audioRecord.EventName(), OnAudioRecorded);
            // Start audio record
            Recorder.instance.StartAudioRecord(audioRecord);
            // Deactivate audio play button
            playAudioButton.SetActive(false);
            // Activate element loading plate
            element.GetComponent<IElementable>().ActivateLoadingPlate();
        }

        public void PlayAudio()
        {
            if (audioRecorded == true && !audioRenderer.isPlaying)
            {
                audioRenderer.PlayOneShot(audioClip);
            }
            else { }
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}
