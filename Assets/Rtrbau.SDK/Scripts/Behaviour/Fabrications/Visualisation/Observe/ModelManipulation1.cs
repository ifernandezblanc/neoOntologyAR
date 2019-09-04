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
using Microsoft.MixedReality.Toolkit.UI;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class ModelManipulation1 : MonoBehaviour, IFabricationable, IVisualisable
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
        public GameObject text;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public GameObject textPanel;
        public Material lineMaterial;
        public Material modelMaterial;
        public Material seenMaterial;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS

        #endregion CLASS_EVENTS

        #region MONOBEHVAIOUR_METHODS
        void Start()
        {
            if (textPanel == null || lineMaterial == null || modelMaterial == null || seenMaterial == null)
            {
                throw new ArgumentException("ModelManipulation1 script requires some prefabs to work.");
            }
        }

        void Update() { }

        void OnEnable() { }

        void OnDisable() { }
        #endregion MONOBEHVAIOUR_METHODS

        #region IFABRICATIONABLE_METHODS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetVisualiser"></param>
        /// <param name="fabrication"></param>
        /// <param name="element"></param>
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
            DataFacet source = DataFormats.ModelManipulation1.formatFacets[0];
            RtrbauAttribute attribute;

            // Check data received meets fabrication requirements
            if (data.fabricationData.TryGetValue(source, out attribute))
            {
                string name = Parser.ParseURI(Parser.ParseURI(attribute.attributeValue, '/', RtrbauParser.post), '.', RtrbauParser.pre);

                component = visualiser.transform.parent.gameObject.GetComponent<AssetManager>().FindAssetComponentManipulator(name);

                model = Instantiate(component);

                UpdateComponentModel();
                AddManipulationHandler();
                string note = attribute.attributeName.name + ": " + name;
                // Debug.Log("ModelManipulation1: " + note);
                AddTextPanel(note);
            }
            else
            {
                throw new ArgumentException(data.fabricationName + " cannot implement: " + attribute.attributeName + " received.");
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
            // Fabrication location is managed by its element.
        }

        public void ModifyMaterial()
        {
            text.transform.GetChild(0).GetComponent<MeshRenderer>().material = seenMaterial;
        }

        public void DestroyIt()
        {
            Destroy(this.gameObject);
        }
        #endregion IVISUALISABLE_METHODS

        #region CLASS_METHODS
        void UpdateComponentModel()
        {
            // Assign name
            model.name = this.name + name + this.GetHashCode();
            // Assign parent and location
            // model.transform.SetParent(scale, true);
            model.transform.SetParent(scale, false);
            // Change material
            model.GetComponentInChildren<MeshRenderer>().material = modelMaterial;
            // Assign initial location and rotation
            model.transform.position = component.transform.position;
            model.transform.rotation = component.transform.rotation;
        }

        void AddManipulationHandler()
        {
            // Add manipulation handler
            model.AddComponent<ManipulationHandler>();
            model.GetComponent<ManipulationHandler>().ManipulationType = ManipulationHandler.HandMovementType.OneAndTwoHanded;
            model.GetComponent<ManipulationHandler>().TwoHandedManipulationType = ManipulationHandler.TwoHandedManipulation.MoveRotateScale;
        }
                
        void AddTextPanel(string note)
        {
            text = Instantiate(textPanel);
            // Attach to model manipulator
            // text.transform.SetParent(model.transform.GetChild(0), false);
            text.transform.SetParent(model.transform, false);
            // Re-scale text panel
            float sX = text.transform.localScale.x / scale.transform.localScale.x;
            float sY = text.transform.localScale.y / scale.transform.localScale.y;
            float sZ = text.transform.localScale.z / scale.transform.localScale.z;
            text.transform.localScale = new Vector3(sX, sY, sZ);
            // Re-allocate above model
            // UPG: to put in a more accurate position considering component bounds and panel offset
            // float positionUP = model.GetComponentInChildren<MeshRenderer>().bounds.size.y;
            float positionUP = model.GetComponentInChildren<MeshRenderer>().bounds.extents.y + (text.GetComponentInChildren<BoxCollider>().bounds.size.y + text.GetComponentInChildren<BoxCollider>().transform.localScale.y);
            text.transform.localPosition += new Vector3(0, positionUP, 0);
            // Provide name
            text.transform.GetChild(1).GetComponent<TextMeshPro>().text = note;
            // Add line renderer to text panel
            text.AddComponent<ElementsLine>().Initialise(text, element.gameObject, lineMaterial);
        }
        #endregion CLASS_METHODS
    }
}
