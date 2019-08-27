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
        public Material material;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS

        #endregion CLASS_EVENTS

        #region MONOBEHVAIOUR_METHODS
        void Start()
        {
            //if (textPanel == null || material == null)
            if (material == null)
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

            //if (componentPair != null)
            //{
            //    ModelToOrigin(modelPair, componentPair, textPair);
            //    ModelToOrigin(modelPair, componentPair, textPair);
            //}
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
            // Is location necessary?
            // Maybe change inferfromtext by initialise in IFabricationable?
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
                    model.GetComponent<MeshRenderer>().material = material;

                    // Create Line renderer
                    // Set line widht at 10% of element consult panel
                    float width = element.localScale.x * 0.01f;
                    model.AddComponent<LineRenderer>();
                    model.GetComponent<LineRenderer>().useWorldSpace = true;
                    model.GetComponent<LineRenderer>().material = material;
                    model.GetComponent<LineRenderer>().startWidth = width;
                    model.GetComponent<LineRenderer>().endWidth = width;
                    UpdateLineRenderer(model);

                    //// Create model text panel
                    //text = Instantiate(textPanel);
                    //// Attach to model manipulator
                    //text.transform.SetParent(model.transform.GetChild(0), false);
                    //// Re-scale text panel
                    //float sX = text.transform.localScale.x / scale.transform.localScale.x;
                    //float sY = text.transform.localScale.y / scale.transform.localScale.y;
                    //float sZ = text.transform.localScale.z / scale.transform.localScale.z;
                    //text.transform.localScale = new Vector3(sX, sY, sZ);
                    //// Re-allocate above model
                    //float positionUP = model.GetComponent<MeshRenderer>().bounds.size.y;
                    //text.transform.localPosition += new Vector3(0, positionUP, 0);
                    //// Provide name
                    //text.transform.GetChild(1).GetComponent<TextMeshPro>().text = facet.Value.attributeName.name + ": " + componentName;

                    //// Create Line renderer
                    //// Set line widht at 10% of element consult panel
                    //float width = element.localScale.x * 0.01f;
                    //text.AddComponent<LineRenderer>();
                    //text.GetComponent<LineRenderer>().useWorldSpace = true;
                    //text.GetComponent<LineRenderer>().material = material;
                    //text.GetComponent<LineRenderer>().startWidth = width;
                    //text.GetComponent<LineRenderer>().endWidth = width;
                    //UpdateLineRenderer(text);

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

                    // Create model pair
                    modelPair = Instantiate(componentPair);
                    modelPair.name = this.name + componentPairName + this.GetHashCode();
                    modelPair.transform.SetParent(scale, true);
                    modelPair.GetComponent<MeshRenderer>().material = material;

                    // Create Line renderer
                    // Set line widht at 10% of element consult panel
                    float width = element.localScale.x * 0.01f;
                    modelPair.AddComponent<LineRenderer>();
                    modelPair.GetComponent<LineRenderer>().useWorldSpace = true;
                    modelPair.GetComponent<LineRenderer>().material = material;
                    modelPair.GetComponent<LineRenderer>().startWidth = width;
                    modelPair.GetComponent<LineRenderer>().endWidth = width;
                    UpdateLineRenderer(modelPair);

                    ModelToOrigin(modelPair, componentPair);

                    //// Create model text panel
                    //textPair = Instantiate(textPanel);
                    //// Attach to model manipulator
                    //textPair.transform.SetParent(modelPair.transform.GetChild(0), false);
                    //// Re-scale text panel
                    //float sX = textPair.transform.localScale.x / scale.transform.localScale.x;
                    //float sY = textPair.transform.localScale.y / scale.transform.localScale.y;
                    //float sZ = textPair.transform.localScale.z / scale.transform.localScale.z;
                    //textPair.transform.localScale = new Vector3(sX, sY, sZ);
                    //// Re-allocate above model
                    //float positionUP = modelPair.GetComponent<MeshRenderer>().bounds.size.y;
                    //textPair.transform.localPosition += new Vector3(0, positionUP, 0);
                    //// Provide name
                    //textPair.transform.GetChild(1).GetComponent<TextMeshPro>().text = facet.Value.attributeName.name + ": " + componentName;

                    //// Create Line renderer
                    //// Set line widht at 10% of element consult panel
                    //float width = element.localScale.x * 0.01f;
                    //textPair.AddComponent<LineRenderer>();
                    //textPair.GetComponent<LineRenderer>().useWorldSpace = true;
                    //textPair.GetComponent<LineRenderer>().material = material;
                    //textPair.GetComponent<LineRenderer>().startWidth = width;
                    //textPair.GetComponent<LineRenderer>().endWidth = width;
                    //UpdateLineRenderer(textPair);

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
            UpdateLineRenderer(model);
        }
        //void ModelMove(GameObject model, GameObject component, GameObject text)
        //{
        //    Vector3 translation;

        //    if (componentPair != null)
        //    {
        //        translation = component.GetComponent<MeshRenderer>().bounds.center - componentPair.GetComponent<MeshRenderer>().bounds.center;
        //    }
        //    else
        //    {
        //        translation = new Vector3(1, 1, 1);
        //    }

        //    model.transform.position += Vector3.Scale(Vector3.Normalize(Vector3.Scale(directionTranslation, translation)), magnitudeTranslation);
        //    model.transform.rotation *= directionRotation;
        //    UpdateLineRenderer(text);
        //}

        void ModelToOrigin(GameObject model, GameObject component)
        {
            model.transform.position = component.transform.position;
            model.transform.rotation = component.transform.rotation;
            UpdateLineRenderer(model);
        }
        //void ModelToOrigin(GameObject model, GameObject component, GameObject text)
        //{
        //    model.transform.position = component.transform.position;
        //    model.transform.rotation = component.transform.rotation;
        //    UpdateLineRenderer(text);
        //}

        void UpdateLineRenderer(GameObject model)
        {
            // Set start and end of line in world coordinates
            Vector3 start = model.GetComponent<MeshRenderer>().bounds.center;
            Vector3 end = element.transform.position;
            model.GetComponent<LineRenderer>().SetPosition(0, start);
            model.GetComponent<LineRenderer>().SetPosition(1, end);

        }
        //void UpdateLineRenderer(GameObject text)
        //{
        //    // Set start and end of line in world coordinates
        //    Vector3 start = text.transform.position;
        //    Vector3 end = element.transform.position;
        //    text.GetComponent<LineRenderer>().SetPosition(0, start);
        //    text.GetComponent<LineRenderer>().SetPosition(1, end);

        //}
        #endregion CLASS_METHODS
    }
}
