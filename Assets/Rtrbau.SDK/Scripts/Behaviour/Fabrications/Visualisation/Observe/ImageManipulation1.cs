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
    public class ImageManipulation1 : MonoBehaviour, IFabricationable, IVisualisable
    {
        #region INITIALISATION_VARIABLES
        public AssetVisualiser visualiser;
        public RtrbauFabrication data;
        public Transform element;
        public Transform scale;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public Sprite imageSource;
        #endregion CLASS_VARIABLES

        #region FACET_VARIABLES
        public OntologyFile imageFile;
        #endregion FACET_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro fabricationText;
        public SpriteRenderer fabricationSprite;
        public MeshRenderer fabricationSeenPanel;
        public Renderer fabricationBounds;
        public Material lineMaterial;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        #endregion CLASS_EVENTS

        #region MONOBEHVAIOUR_METHODS
        void Start()
        {
            if (fabricationText == null || fabricationSprite == null || fabricationSeenPanel == null || fabricationBounds == null || lineMaterial == null)
            {
                throw new ArgumentException("ImageManipulation1::Start: script requires some prefabs to work.");
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
            DataFacet imagefacet1 = DataFormats.ImageManipulation1.formatFacets[0];
            RtrbauAttribute attribute;

            // Check data received meets fabrication requirements
            if (data.fabricationData.TryGetValue(imagefacet1, out attribute))
            {
                fabricationText.text = Parser.ParseNamingOntologyAttribute(attribute.attributeName.Name(), element.GetComponent<ElementConsult>().classElement.entity.Name());
                imageFile = new OntologyFile(attribute.attributeValue);
                LoaderEvents.StartListening(imageFile.EventName(), DownloadedImage);
                Loader.instance.StartFileDownload(imageFile);
                Debug.Log("ImageManipulation1: InferFromText: Started audio download " + imageFile.URL());
                this.gameObject.AddComponent<ElementsLine>();
                this.gameObject.GetComponent<ElementsLine>().Initialise(this.gameObject, element.gameObject, lineMaterial);
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Renderer GetRenderer() { return fabricationBounds; }
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
        void DownloadedImage(OntologyFile imageFile)
        {
            LoaderEvents.StopListening(imageFile.EventName(), DownloadedImage);
            Debug.Log("ImageManipulation1: LoadAudio: downloaded " + imageFile.URL());

            if (imageFile != null)
            {
                if (File.Exists(imageFile.FilePath()))
                {
                    StartCoroutine(LoadImage(imageFile));
                }
                else
                {
                    Debug.LogError("ImageManipulation1: DownloadedAudio: " + imageFile.name + "not found.");
                }
            }
            else
            {
                Debug.LogError("ImageManipulation1: DownloadedAudio: " + imageFile.name + "not found.");
            }

        }

        IEnumerator LoadImage(OntologyFile imageFile)
        {
            if(imageFile != null)
            {
                UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(imageFile.FilePath());

                yield return imageRequest.SendWebRequest();

                if (imageRequest.isNetworkError || imageRequest.isHttpError)
                {
                    Debug.LogError(imageRequest.error);
                }
                else
                {
                    Texture2D imageTexture = DownloadHandlerTexture.GetContent(imageRequest);
                    // According to unity documentation
                    imageSource = Sprite.Create(imageTexture, new Rect(0.0f, 0.0f, imageTexture.width, imageTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                    fabricationSprite.sprite = imageSource;
                    fabricationSprite.drawMode = SpriteDrawMode.Sliced;
                    // According to fabrication current size
                    fabricationSprite.size = new Vector2(0.15f, 0.15f);
                }
            }
            else
            {
                Debug.LogError("ImageManipulation1: LoadAudio: " + imageFile.type + "not implemented for ImageManipulation1.");
            }
        }
        #endregion PRIVATE

        #region PUBLIC
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}