/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 25/08/2019
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using TMPro;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class AudioTap1 : MonoBehaviour, IFabricationable, IVisualisable
    {
        #region INITIALISATION_VARIABLES
        public AssetVisualiser visualiser;
        public RtrbauFabrication data;
        public Transform element;
        public Transform scale;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public AudioClip audioSource;
        #endregion CLASS_VARIABLES

        #region FACET_VARIABLES
        public OntologyFile audioFile;
        #endregion FACET_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro fabricationText;
        public MeshRenderer fabricationSeenPanel;
        public Material fabricationSeenMaterial;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        public bool audioLoaded;
        public bool audioPlaying;
        #endregion CLASS_EVENTS

        #region MONOBEHVAIOUR_METHODS
        void Start()
        {
            if (fabricationText == null || fabricationSeenPanel == null)
            {
                throw new ArgumentException("AudioTap1 script requires some prefabs to work.");
            }
            else { }
        }

        void Update() { }

        void OnEnable() { }

        void OnDisable() { }

        void OnDestroy () { DestroyIt(); }
        #endregion MONOBEHVAIOUR_METHODS

        #region IFABRICATIONABLE_METHODS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetVisualiser"></param>
        /// <param name="fabrication"></param>
        /// <param name="scale"></param>
        public void Initialise(AssetVisualiser assetVisualiser, RtrbauFabrication fabrication, Transform elementParent, Transform fabricationParent)
        {
            // Is location necessary?
            // Maybe change inferfromtext by initialise in IFabricationable?
            visualiser = assetVisualiser;
            data = fabrication;
            element = elementParent;
            scale = fabricationParent;
            audioPlaying = false;
            // loc = location;
            Scale();
            InferFromText();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Scale()
        {
            // Debug.Log("Root: " + this.transform.root.name);

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
            DataFacet audiofacet1 = DataFormats.AudioTap1.formatFacets[0];
            RtrbauAttribute attribute;

            // Check data received meets fabrication requirements
            if (data.fabricationData.TryGetValue(audiofacet1, out attribute))
            {
                fabricationText.text = Parser.ParseNamingOntologyFormat(attribute.attributeName.Name()) + ": ";
                audioFile = new OntologyFile(attribute.attributeValue);
                LoaderEvents.StartListening(audioFile.EventName(), DownloadedAudio);
                Loader.instance.StartFileDownload(audioFile);
                Debug.Log("AudioTap1: InferFromText: Started audio download " + audioFile.URL());
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
            // Do nothing
            // Activation / de-activation is managed by
        }
        #endregion IFABRICATIONABLE_METHODS

        #region IVISUALISABLE_METHODS
        public void LocateIt()
        {
            /// Fabrication location is managed by <see cref="ElementConsult"/>.
        }

        public void ActivateIt()
        {
            /// Fabrication activation is managed by <see cref="ElementConsult"/>.
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

        #region CLASS_METHODS
        #region PRIVATE
        void DownloadedAudio(OntologyFile file)
        {
            LoaderEvents.StopListening(audioFile.EventName(), DownloadedAudio);
            Debug.Log("AudioTap1: LoadAudio: downloaded " + audioFile.URL());

            if (audioFile != null)
            {
                if (File.Exists(audioFile.FilePath()))
                {
                    audioFile = file;
                    StartCoroutine(LoadAudio(audioFile));
                }
                else
                {
                    Debug.LogError("AudioTap1: DownloadedAudio: " + audioFile.name + "not found.");
                }
            }
            else
            {
                Debug.LogError("AudioTap1: DownloadedAudio: " + audioFile.name + "not found.");
            }

        }

        IEnumerator LoadAudio(OntologyFile audioFile)
        {
            if(audioFile.type == RtrbauFileType.wav)
            {
                UnityWebRequest audioRequest = UnityWebRequestMultimedia.GetAudioClip(audioFile.FilePath(), AudioType.WAV);

                yield return audioRequest.SendWebRequest();

                if (audioRequest.isNetworkError || audioRequest.isHttpError)
                {
                    Debug.LogError(audioRequest.error);
                }
                else
                {
                    audioSource = DownloadHandlerAudioClip.GetContent(audioRequest);
                    this.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
                    this.gameObject.GetComponent<AudioSource>().clip = audioSource;
                    audioLoaded = true;
                }
            }
            else
            {
                Debug.LogError("AudioTap1::LoadAudio: " + audioFile.type + "not implemented for AudioTap1.");
            }
        }
        #endregion PRIVATE

        #region PUBLIC
        public void PlayAudio()
        {
            if (audioLoaded)
            {
                if (!audioPlaying)
                {
                    audioPlaying = true;
                    this.gameObject.GetComponent<AudioSource>().Play();
                }
                else
                {
                    audioPlaying = false;
                    this.gameObject.GetComponent<AudioSource>().Stop();
                }
            }
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}