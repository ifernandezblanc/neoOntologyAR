/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 21/08/2019
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
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class AnimationNone2 : MonoBehaviour, IFabricationable, IVisualisable
    {
        #region INITIALISATION_VARIABLES
        public AssetVisualiser visualiser;
        public RtrbauFabrication data;
        public Transform element;
        public Transform scale;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public GameObject component;
        public GameObject model;
        public Vector3 modelOriginPosition;
        public Quaternion modelOriginRotation;
        public GameObject componentPair;
        public GameObject modelPair;
        public Vector3 modelPairOriginPosition;
        public Quaternion modelPairOriginRotation;
        public Vector3 magnitudeTranslation;
        public Vector3 directionTranslation;
        public float magnitudeRotation;
        public Quaternion directionRotation;
        public Bounds animationBounds;
        #endregion CLASS_VARIABLES

        #region FACET_VARIABLES
        public string componentName;
        public bool freeTranslationX;
        public bool freeTranslationY;
        public bool freeTranslationZ;
        public bool freeRotationX;
        public bool freeRotationY;
        public bool freeRotationZ;
        public string componentPairName;
        public bool inverseMovement;
        #endregion FACET_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro fabricationText;
        public MeshRenderer fabricationSeenPanel;
        public Material lineMaterial;
        public Material modelMaterial;
        public Renderer fabricationBounds;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        public bool movementCalculated;
        #endregion CLASS_EVENTS

        #region MONOBEHVAIOUR_METHODS
        void Start()
        {
            if (fabricationSeenPanel == null || fabricationText == null || lineMaterial == null || modelMaterial == null || fabricationBounds == null)
            {
                throw new ArgumentException("AnimationNone2 script requires some prefabs to work.");
            }
        }

        void Update()
        {
            if (movementCalculated)
            {
                if (inverseMovement)
                {
                    if (animationBounds.Contains(model.transform.position))
                    {
                        ModelToOrigin(model, modelOriginPosition, modelOriginRotation);
                    }
                    else
                    {
                        ModelMove(model, component);
                    }
                }
                else
                {
                    if (animationBounds.Contains(model.transform.position))
                    {
                        ModelMove(model, component);
                    }
                    else
                    {
                        ModelToOrigin(model, modelOriginPosition, modelOriginRotation);
                    }
                }

                if (componentPair != null)
                {
                    ModelToOrigin(modelPair, modelPairOriginPosition, modelPairOriginRotation);
                }
            }
        }

        void OnEnable()
        {
            // For efficient use when parent fabrication is activated
            if (movementCalculated)
            {
                model.SetActive(true);
                if (componentPair != null) { modelPair.SetActive(true); }
            }
        }

        void OnDisable()
        {
            // For efficient use when parent fabrication is deactivated
            if (movementCalculated)
            {
                model.SetActive(false);
                if (componentPair != null) { modelPair.SetActive(false); }
            }
        }

        void OnDestroy() { DestroyIt(); }
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
            visualiser = assetVisualiser;
            data = fabrication;
            element = elementParent;
            scale = fabricationParent;

            fabricationText.text = "Animation available";
            Scale();
            InferFromText();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Scale()
        {
            // Debug.Log("Root: " + this.transform.root.name);

            float sX = this.transform.localScale.x / element.transform.localScale.x;
            float sY = this.transform.localScale.y / element.transform.localScale.y;
            float sZ = this.transform.localScale.z / element.transform.localScale.z;

            this.transform.localScale = new Vector3(sX, sY, sZ);
        }

        /// <summary>
        /// 
        /// </summary>
        public void InferFromText()
        {
            // Read attributes

            foreach (KeyValuePair<DataFacet,RtrbauAttribute> facet in data.fabricationData)
            {
                if (facet.Key == DataFormats.AnimationNone2.formatFacets[0])
                {
                    componentName = Parser.ParseURI(Parser.ParseURI(facet.Value.attributeValue, '/', RtrbauParser.post), '.', RtrbauParser.pre);

                    component = visualiser.manager.FindAssetComponentManipulator(componentName);

                    // Create model
                    model = Instantiate(component);
                    model.name = this.name + componentName + this.GetHashCode();
                    // model.transform.SetParent(scale, true);
                    model.transform.SetParent(scale, false);
                    model.GetComponentInChildren<MeshRenderer>().material = modelMaterial;
                    model.transform.position = component.transform.position;
                    model.transform.rotation = component.transform.rotation;
                    // Add line renderer for animated model
                    // model.AddComponent<ElementsLine>().Initialise(model.transform.GetChild(0).gameObject, element.gameObject, lineMaterial);
                    // model.AddComponent<ElementsLine>().Initialise(model, element.gameObject, lineMaterial);
                    model.AddComponent<ElementsLine>().Initialise(model, this.gameObject, lineMaterial);
                    // Set model origin
                    modelOriginPosition = model.transform.position;
                    modelOriginRotation = model.transform.rotation;

                }
                else if (facet.Key == DataFormats.AnimationNone2.formatFacets[1])
                {
                    freeTranslationX = (facet.Value.attributeValue == "true");
                }
                else if (facet.Key == DataFormats.AnimationNone2.formatFacets[2])
                {
                    freeTranslationY = (facet.Value.attributeValue == "true");
                }
                else if (facet.Key == DataFormats.AnimationNone2.formatFacets[3])
                {
                    freeTranslationZ = (facet.Value.attributeValue == "true");
                }
                else if (facet.Key == DataFormats.AnimationNone2.formatFacets[4])
                {
                    freeRotationX = (facet.Value.attributeValue == "true");
                }
                else if (facet.Key == DataFormats.AnimationNone2.formatFacets[5])
                {
                    freeRotationY = (facet.Value.attributeValue == "true");
                }
                else if (facet.Key == DataFormats.AnimationNone2.formatFacets[6])
                {
                    freeRotationZ = (facet.Value.attributeValue == "true");
                }
                else if (facet.Key == DataFormats.AnimationNone2.formatFacets[7])
                {
                    componentPairName = Parser.ParseURI(Parser.ParseURI(facet.Value.attributeValue, '/', RtrbauParser.post), '.', RtrbauParser.pre);

                    componentPair = visualiser.manager.FindAssetComponentManipulator(componentPairName);

                    // Create model pair: does not have line renderer
                    modelPair = Instantiate(componentPair);
                    modelPair.name = this.name + componentPairName + this.GetHashCode();
                    // modelPair.transform.SetParent(scale, true);
                    modelPair.transform.SetParent(scale, false);
                    modelPair.GetComponentInChildren<MeshRenderer>().material = lineMaterial;
                    modelPair.transform.position = componentPair.transform.position;
                    modelPair.transform.rotation = componentPair.transform.rotation;
                    // Set model pair origin
                    modelPairOriginPosition = modelPair.transform.position;
                    modelPairOriginRotation = modelPair.transform.rotation;

                }
                else if (facet.Key == DataFormats.AnimationNone2.formatFacets[8])
                {
                    inverseMovement = (facet.Value.attributeValue == "true");
                    // Debug.Log("Inverse Movement: " + inverseMovement);
                }
                else
                {
                    throw new ArgumentException(data.fabricationName.ToString() + "::InferFromText: cannot implement attribute received.");
                }
            }

            CalculateMovement();
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
            Destroy(model);
            Destroy(modelPair);
        }

        public void ModifyMaterial(Material material)
        {
            fabricationSeenPanel.material = material;
        }
        #endregion IVISUALISABLE_METHODS

        #region CLASS_METHODS
        void CalculateMovement()
        {
            // Generate fabrication features from read attributes
            // Calculate magnitude of translation
            magnitudeTranslation = component.GetComponentInChildren<MeshRenderer>().bounds.size * 0.1f;
            // Calculate magnitude of rotation
            magnitudeRotation = 0.05f;
            // Generate the area where animation will occur
            animationBounds = component.GetComponentInChildren<MeshRenderer>().bounds;
            // Calculate allowed directions to translate
            Vector3 freeTranslation = new Vector3(Convert.ToInt32(freeTranslationX), Convert.ToInt32(freeTranslationY), Convert.ToInt32(freeTranslationZ));
            // Calculate allowed directions to rotate
            Vector3 freeRotation = new Vector3(Convert.ToInt32(freeRotationX), Convert.ToInt32(freeRotationY), Convert.ToInt32(freeRotationZ));
            // Caclulate movement translation
            Vector3 movementTranslation;
            if (componentPair != null)
            // { movementTranslation = component.GetComponentInChildren<MeshRenderer>().bounds.center - componentPair.GetComponentInChildren<MeshRenderer>().bounds.center; }
            { movementTranslation = component.transform.position - componentPair.transform.position; }
            //{
            //    // Necessary to implement inverse directions because of asset rotation of 180 degrees over y-axis (x and z axis then inverted)
            //    if (freeTranslation.x > 0 || freeTranslation.z > 0) { movementTranslation =  componentPair.transform.position - component.transform.position; }
            //    else { movementTranslation = component.transform.position - componentPair.transform.position; }
            //}
            else
            { movementTranslation = new Vector3(1, 1, 1); }
            // Calculate movement inversion
            Vector3 inverseTranslation;
            if (inverseMovement == true)
            {
                // Debug.Log("Inverse Movement: " + inverseMovement);
                // Set new model origin
                model.transform.position += Vector3.Scale(freeTranslation, component.GetComponentInChildren<MeshRenderer>().bounds.size * 4f);
                // Debug.Log("inversed position: " + model.transform.position);
                modelOriginPosition = model.transform.position;
                modelOriginRotation = model.transform.rotation;
                inverseTranslation = new Vector3(-1f, -1f, -1f);
            }
            else
            {
                // Debug.Log("Inverse Movement: " + inverseMovement);
                inverseTranslation = new Vector3(1, 1, 1);
                animationBounds.Expand(0.5f);
            }
            // Assign results to animation translation
            directionTranslation = Vector3.Normalize(Vector3.Scale(Vector3.Scale(freeTranslation, movementTranslation), inverseTranslation));
            // Assign results to animation rotation
            directionRotation = Quaternion.Euler(Vector3.Normalize(freeRotation));
            // Activate movement
            movementCalculated = true;
        }

        void ModelMove(GameObject model, GameObject component)
        {
            // model.transform.position += Vector3.Scale(directionTranslation, magnitudeTranslation);
            model.transform.localPosition += Vector3.Scale(directionTranslation, magnitudeTranslation);
            // model.transform.RotateAround(model.transform.GetChild(0).position, directionRotation.eulerAngles, magnitudeRotation);
            // model.transform.rotation *= directionRotation;
            model.transform.localRotation *= directionRotation;
        }

        void ModelToOrigin(GameObject model, Vector3 originPosition, Quaternion originRotation)
        {
            model.transform.position = originPosition;
            model.transform.rotation = originRotation;
        }
        #endregion CLASS_METHODS
    }
}
