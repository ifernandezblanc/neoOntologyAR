/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 20/08/2019
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class IconNone1 : MonoBehaviour, IFabricationable, IVisualisable
    {
        #region INITIALISATION_VARIABLES
        public AssetVisualiser visualiser;
        public RtrbauFabrication data;
        public Transform element;
        public Transform scale;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public string iconName;
        public Sprite icon;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro text;
        public SpriteRenderer sprite;
        public MeshRenderer panel;
        public Material seenMaterial;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS

        #endregion CLASS_EVENTS

        #region MONOBEHVAIOUR_METHODS
        void Start()
        {
            if (text == null || sprite == null || panel == null || seenMaterial == null)
            {
                throw new ArgumentException("IconNone1 script requires some prefabs to work.");
            }
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
            Debug.Log("Root: " + this.transform.root.name);

            float sX = this.transform.localScale.x / scale.transform.localScale.x;
            float sY = this.transform.localScale.y / scale.transform.localScale.y;
            float sZ = this.transform.localScale.z / scale.transform.localScale.z;

            this.transform.localScale = new Vector3(sX, sY, sZ);

            // UPG: to autosize icon image
        }

        /// <summary>
        /// 
        /// </summary>
        public void InferFromText()
        {
            DataFacet iconnone1Source = DataFormats.IconNone1.formatFacets[0];
            RtrbauAttribute attribute;

            // Check data received meets fabrication requirements
            if (data.fabricationData.TryGetValue(iconnone1Source, out attribute))
            {
                // Add text for attributes name
                text.text = attribute.attributeName.name + ":";
                // Find icon that retrieves value
                Debug.Log(attribute.attributeValue);
                // iconName = Libraries.IconLibrary.Find(x => x.Contains(attribute.attributeValue));
                iconName = Libraries.IconLibrary.Find(x => attribute.attributeValue.Contains(x));
                string iconPath = "Rtrbau/Icons/" + iconName;
                Debug.Log(iconName);
                // Load icon's sprite
                icon = Resources.Load<Sprite>(iconPath);
                // Assign to sprite renderer
                sprite.sprite = icon;
                // Set correct size to sprite according to prefab configuration
                // UPG: modify for auto-sizing
                sprite.size = new Vector2(2.0f, 2.0f);
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
            panel.material = seenMaterial;
        }

        public void DestroyIt()
        {
            Destroy(this.gameObject);
        }
        #endregion IVISUALISABLE_METHODS

        #region CLASS_METHODS
        #endregion CLASS_METHODS
    }
}