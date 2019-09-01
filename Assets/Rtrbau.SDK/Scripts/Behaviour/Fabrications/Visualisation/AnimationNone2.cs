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
        //public GameObject text;
        public GameObject componentPair;
        public GameObject modelPair;
        //public GameObject textPair;
        public Vector3 magnitudeTranslation;
        public Vector3 directionTranslation;
        public Quaternion directionRotation;
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
        //public GameObject textPanel;
        public Material lineMaterial;
        public Material modelMaterial;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS

        #endregion CLASS_EVENTS

        #region MONOBEHVAIOUR_METHODS
        void Start()
        {
            //if (textPanel == null || material == null)
            if (lineMaterial == null || modelMaterial == null )
            {
                throw new ArgumentException("AnimationNone2 script requires some prefabs to work.");
            }
        }

        void Update()
        {
            // Generate the area where animation will occur
            Bounds animBounds = component.GetComponent<MeshRenderer>().bounds;
            animBounds.Expand(1);

            if (animBounds.Contains(model.transform.position))
            {
                //ModelMove(model, component, text);
                ModelMove(model, component);
            }
            else
            {
                //ModelToOrigin(model, component, text);
                ModelToOrigin(model, component);
            }

            if (componentPair != null)
            {
                // ModelToOrigin(modelPair, componentPair, textPair);
                ModelToOrigin(modelPair, componentPair);
            }
        }

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
        /// <param name="scale"></param>
        public void Initialise(AssetVisualiser assetVisualiser, RtrbauFabrication fabrication, Transform elementParent, Transform fabricationParent)
        {
            visualiser = assetVisualiser;
            data = fabrication;
            element = elementParent;
            scale = fabricationParent;

            InferFromText();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Scale()
        {
            // Do nothing when 3D models involved.
        }

        /// <summary>
        /// 
        /// </summary>
        public void InferFromText()
        {
            // Read attributes

            foreach (KeyValuePair<DataFacet,RtrbauAttribute> facet in data.fabricationData)
            {
                if (facet.Key == DataFormats.animationnone2.formatFacets[0])
                {
                    componentName = Parser.ParseURI(Parser.ParseURI(facet.Value.attributeValue, '/', RtrbauParser.post), '.', RtrbauParser.pre);

                    component = visualiser.manager.FindAssetComponent(componentName);

                    // Create model
                    model = Instantiate(component);
                    model.name = this.name + componentName + this.GetHashCode();
                    model.transform.SetParent(scale, true);
                    model.GetComponent<MeshRenderer>().material = modelMaterial;
                    // Add line renderer for animated model
                    model.AddComponent<ElementsLine>();
                    model.AddComponent<ElementsLine>().Initialise(model, element.gameObject, lineMaterial);

                }
                else if (facet.Key == DataFormats.animationnone2.formatFacets[1])
                {
                    freeTranslationX = (facet.Value.attributeValue == "true");
                }
                else if (facet.Key == DataFormats.animationnone2.formatFacets[2])
                {
                    freeTranslationY = (facet.Value.attributeValue == "true");
                }
                else if (facet.Key == DataFormats.animationnone2.formatFacets[3])
                {
                    freeTranslationZ = (facet.Value.attributeValue == "true");
                }
                else if (facet.Key == DataFormats.animationnone2.formatFacets[4])
                {
                    freeRotationX = (facet.Value.attributeValue == "true");
                }
                else if (facet.Key == DataFormats.animationnone2.formatFacets[5])
                {
                    freeRotationY = (facet.Value.attributeValue == "true");
                }
                else if (facet.Key == DataFormats.animationnone2.formatFacets[6])
                {
                    freeRotationZ = (facet.Value.attributeValue == "true");
                }
                else if (facet.Key == DataFormats.animationnone2.formatFacets[7])
                {
                    componentPairName = Parser.ParseURI(Parser.ParseURI(facet.Value.attributeValue, '/', RtrbauParser.post), '.', RtrbauParser.pre);

                    componentPair = visualiser.manager.FindAssetComponent(componentPairName);

                    // Create model pair: does not have line renderer
                    modelPair = Instantiate(componentPair);
                    modelPair.name = this.name + componentPairName + this.GetHashCode();
                    modelPair.transform.SetParent(scale, true);
                    modelPair.GetComponent<MeshRenderer>().material = lineMaterial;

                }
                else if (facet.Key == DataFormats.animationnone2.formatFacets[8])
                {
                    inverseMovement = (facet.Value.attributeValue == "true");
                }
                else
                {
                    throw new ArgumentException(data.fabricationName + " cannot implement: " + facet.Value.attributeName + " received.");
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
        #endregion IFABRICATIONABLE_METHODS

        #region IVISUALISABLE_METHODS
        public void LocateIt()
        {
            // Fabrication location is managed by its element.
        }

        public void DestroyIt()
        {
            Destroy(this.gameObject);
        }

        #endregion IVISUALISABLE_METHODS

        #region CLASS_METHODS
        void CalculateMovement()
        {
            // Generate fabrication features from read attributes
            // Calculate magnitude of translation
            magnitudeTranslation = component.GetComponent<MeshRenderer>().bounds.size * 0.1f;
            // Calculate allowed directions to translate
            Vector3 freeTranslation = new Vector3(Convert.ToInt32(freeTranslationX), Convert.ToInt32(freeTranslationY), Convert.ToInt32(freeTranslationZ));
            // Calculate movement inversion
            Vector3 inverseTranslation;
            if (inverseMovement == true) { inverseTranslation = new Vector3(-1f, -1f, -1f); }
            else { inverseTranslation = new Vector3(1, 1, 1); }
            // Assign results to animation translation
            directionTranslation = Vector3.Normalize(Vector3.Scale(freeTranslation, inverseTranslation));
            // Calculate allowed directions to rotate
            Vector3 freeRotation = new Vector3(Convert.ToInt32(freeRotationX), Convert.ToInt32(freeRotationY), Convert.ToInt32(freeRotationZ));
            // Assign results to animation rotation
            directionRotation = Quaternion.Euler(Vector3.Normalize(freeRotation) * 2f);
        }

        void ModelMove(GameObject model, GameObject component)
        {
            Vector3 translation;

            if (componentPair != null)
            {
                translation = component.GetComponent<MeshRenderer>().bounds.center - componentPair.GetComponent<MeshRenderer>().bounds.center;
            }
            else
            {
                translation = new Vector3(1, 1, 1);
            }

            model.transform.position += Vector3.Scale(Vector3.Normalize(Vector3.Scale(directionTranslation, translation)), magnitudeTranslation);
            model.transform.rotation *= directionRotation;
        }

        void ModelToOrigin(GameObject model, GameObject component)
        {
            model.transform.position = component.transform.position;
            model.transform.rotation = component.transform.rotation;
        }
        #endregion CLASS_METHODS
    }
}
