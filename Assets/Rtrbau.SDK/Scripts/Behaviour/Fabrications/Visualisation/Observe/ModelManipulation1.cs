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
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro fabricationText;
        public MeshRenderer fabricationSeenPanel;
        public Renderer fabricationBounds;
        public Material lineMaterial;
        public Material modelMaterial;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        public bool modelCreated;
        #endregion CLASS_EVENTS

        #region MONOBEHVAIOUR_METHODS
        void Start()
        {
            if (fabricationText == null || fabricationSeenPanel == null || fabricationBounds == null || lineMaterial == null || modelMaterial == null)
            {
                throw new ArgumentException("ModelManipulation1::Start: script requires some prefabs to work.");
            }
        }

        void Update() { }

        void OnEnable()
        {
            // For efficient use when parent fabrication is activated
            if (modelCreated)
            {
                model.SetActive(true);
            }
        }

        void OnDisable()
        {
            // For efficient use when parent fabrication is deactivated
            if (modelCreated)
            {
                model.SetActive(false);
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
        /// <param name="element"></param>
        public void Initialise(AssetVisualiser assetVisualiser, RtrbauFabrication fabrication, Transform elementParent, Transform fabricationParent)
        {
            // Is location necessary?
            // Maybe change inferfromtext by initialise in IFabricationable?
            visualiser = assetVisualiser;
            data = fabrication;
            element = elementParent;
            scale = fabricationParent;
            Scale();
            InferFromText();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Scale()
        {
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
            DataFacet source = DataFormats.ModelManipulation1.formatFacets[0];
            RtrbauAttribute attribute;

            // Check data received meets fabrication requirements
            if (data.fabricationData.TryGetValue(source, out attribute))
            {
                string name = Parser.ParseURI(Parser.ParseURI(attribute.attributeValue, '/', RtrbauParser.post), '.', RtrbauParser.pre);

                component = visualiser.transform.parent.gameObject.GetComponent<AssetManager>().FindAssetComponentManipulator(name);

                // Update fabrication text with component name
                string note = Parser.ParseNamingOntologyFormat(attribute.attributeName.Name()) + ": " + name;
                fabricationText.text = note;
                // Debug.Log("ModelManipulation1: " + note);
                // AddTextPanel(note);
                // Instantiate component model
                model = Instantiate(component);
                UpdateComponentModel();
                AddManipulationHandler();
                modelCreated = true;
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
            Destroy(model);
        }

        public void ModifyMaterial(Material material)
        {
            fabricationSeenPanel.material = material;
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
            // Add line renderer to fabrication text panel
            model.AddComponent<ElementsLine>().Initialise(model, this.gameObject, lineMaterial);
        }

        void AddManipulationHandler()
        {
            // Add manipulation handler
            model.AddComponent<ManipulationHandler>();
            model.GetComponent<ManipulationHandler>().ManipulationType = ManipulationHandler.HandMovementType.OneAndTwoHanded;
            model.GetComponent<ManipulationHandler>().TwoHandedManipulationType = ManipulationHandler.TwoHandedManipulation.MoveRotateScale;
        }
        #endregion CLASS_METHODS
    }
}
