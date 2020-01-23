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
    public class ImageRecordTap1: MonoBehaviour, IFabricationable, IVisualisable, IRecordable
    {
        #region INITIALISATION_VARIABLES
        public AssetVisualiser visualiser;
        public RtrbauFabrication data;
        public Transform element;
        public Transform scale;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public string imageGenericName;
        public OntologyFile imageRecord;
        #endregion CLASS_VARIABLES

        #region FACETS_VARIABLES
        #endregion FACETS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro fabricationText;
        public MeshRenderer fabricationSeenPanel;
        public MeshRenderer fabricationReportedPanel;
        public Material fabricationReportedMaterial;
        public Renderer fabricationBounds;
        public GameObject recordImageButton;
        public TextMeshPro imageStatus;
        public SpriteRenderer imageRenderer;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        private bool fabricationCreated;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Start()
        {
            if (fabricationText == null || fabricationSeenPanel == null || fabricationReportedPanel == null || fabricationReportedMaterial == null || fabricationBounds == null || recordImageButton == null || imageStatus == null || imageRenderer == null)
            {
                throw new ArgumentException("DefaultRecord::Start: Script requires some prefabs to work.");
            }
            else { recordImageButton.SetActive(false); }
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
            imageGenericName = null;
            imageRecord = null;
            fabricationCreated = false;
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
            DataFacet picturefacet1 = DataFormats.ImageRecordTap1.formatFacets[0];
            RtrbauAttribute attribute;

            // Check data received meets fabrication requirements
            if (data.fabricationData.TryGetValue(picturefacet1, out attribute))
            {
                fabricationText.text = Parser.ParseNamingOntologyFormat(attribute.attributeName.Name());
                imageGenericName = attribute.attributeName.Name();
                fabricationCreated = true;
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
            if (imageRecord != null)
            {
                DataFacet picturefacet1 = DataFormats.ImageRecordTap1.formatFacets[0];
                RtrbauAttribute attribute;

                // Check data received meets fabrication requirements
                if (data.fabricationData.TryGetValue(picturefacet1, out attribute))
                {
                    // Update attribute value according to what user recorded
                    // This assigns to RtrbauElement from ElementReport through RtrbauFabrication
                    attribute.attributeValue = imageRecord.URL();
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
            else
            {
                imageStatus.text = "Please take a picture before reporting it.";
            }
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

            if (fabricationCreated == true && recordImageButton.activeSelf == false)
            {
                recordImageButton.SetActive(true);
            }
            else { }
        }

        /// <summary>
        /// Deactivates record buttons.
        /// It is also called by <see cref="ElementReport"/> to deactivate record buttons when others are to become active.
        /// </summary>
        public void DeactivateRecords()
        {
            if (fabricationCreated == true && recordImageButton.activeSelf == true)
            {
                recordImageButton.SetActive(false);
            }
            else { }
        }
        #endregion IRECORDABLE_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        void OnPictureTaken(OntologyFile imageFile)
        {
            RecorderEvents.StopListening(imageFile.EventName(), OnPictureTaken);

            imageStatus.text = "Picture take";

            OntologyFileUpload fileUpload = new OntologyFileUpload(imageFile);

            LoaderEvents.StartListening(fileUpload.EventName(), OnPictureUploaded);

            Loader.instance.StartFileUpload(fileUpload);
        }

        void OnPictureUploaded(OntologyFileUpload fileUpload)
        {
            LoaderEvents.StopListening(fileUpload.EventName(), OnPictureUploaded);

            StartCoroutine(LoadImage(fileUpload.file));

            Debug.Log("RecordImageButton::OnPictureUploaded: Image uploaded, start rendering...");
        }

        IEnumerator LoadImage(OntologyFile imageFile)
        {
            if (imageFile != null)
            {
                UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(imageFile.FilePath());

                yield return imageRequest.SendWebRequest();

                if (imageRequest.isNetworkError || imageRequest.isHttpError)
                {
                    Debug.LogError(imageRequest.error);
                }
                else
                {
                    // Download image texture from uploaded file
                    Texture2D imageTexture = DownloadHandlerTexture.GetContent(imageRequest);
                    // Setup imageTexture as imageRender accordingly to Unity documentation
                    Sprite imageSource = Sprite.Create(imageTexture, new Rect(0.0f, 0.0f, imageTexture.width, imageTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                    imageRenderer.sprite = imageSource;
                    imageRenderer.drawMode = SpriteDrawMode.Sliced;
                    // According to fabrication current size
                    imageRenderer.size = new Vector2(0.15f, 0.15f);
                    // Setup image rendered as image recorded
                    imageRecord = imageFile;
                    // Inform the user of picture rendered
                    imageStatus.text = "Click again to take another picture.";
                }
            }
            else
            {
                Debug.LogError("ImageManipulation1: LoadAudio: " + imageFile.type + "not implemented for ImageManipulation1.");
            }

            // Call to report attribute
            OnNextVisualisation();
            // Remember to deactivate loading plate
            element.GetComponent<IElementable>().DeactivateLoadingPlate();
        }
        #endregion PRIVATE

        #region PUBLIC
        public void RecordImage()
        {
            // Generate image file name
            string imageName = Parser.ParseAddDateTime(imageGenericName);
            // Create new ontology file
            imageRecord = new OntologyFile(imageName, RtrbauFileType.jpg.ToString());
            // Initialise on image recorded event
            RecorderEvents.StartListening(imageRecord.EventName(), OnPictureTaken);
            // Start image record
            Recorder.instance.StartImageRecord(imageRecord);
            // Activate element loading plate
            element.GetComponent<IElementable>().ActivateLoadingPlate();
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}