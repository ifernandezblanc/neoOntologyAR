/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 24/08/2019
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
    /// UPG: TO COMPLETE WITH ICON SPRITE AND NEW PREFAB AS COPY OF TEXTBUTTONTAP1
    /// </summary>
    public class IconButtonTap1 : MonoBehaviour, IFabricationable, IVisualisable
    {
        #region INITIALISATION_VARIABLES
        public AssetVisualiser visualiser;
        public RtrbauFabrication data;
        public Transform element;
        public Transform scale;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public Sprite icon;
        public OntologyEntity relationshipAttribute;
        #endregion CLASS_VARIABLES

        #region FACET_VARIABLES
        public string nextIndividual;
        public string iconName;
        #endregion FACET_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro fabricationText;
        public SpriteRenderer fabricationSprite;
        public MeshRenderer fabricationSeenPanel;
        public Material fabricationSeenMaterial;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS

        #endregion CLASS_EVENTS

        #region MONOBEHVAIOUR_METHODS
        void Start()
        {
            if (fabricationText == null || fabricationSprite == null || fabricationSeenPanel == null || fabricationSeenMaterial == null)
            {
                throw new ArgumentException("IconButtonTap1 script requires some prefabs to work.");
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
            DataFacet iconfacet2 = DataFormats.IconButtonTap1.formatFacets[0];
            RtrbauAttribute attribute;

            // Check data received meets fabrication requirements
            if (data.fabricationData.TryGetValue(iconfacet2, out attribute))
            {
                // Add text for attributes name
                string attributeName = Parser.ParseURI(attribute.attributeValue, '#', RtrbauParser.post);
                fabricationText.text = attribute.attributeName.Name() + ":";
                // Find icon that retrieves value
                // iconName = Libraries.IconLibrary.Find(x => x.Contains(attribute.attributeValue));
                iconName = Libraries.IconLibrary.Find(x => attribute.attributeValue.Contains(x));
                string iconPath = "Rtrbau/Icons/" + iconName;

                
                fabricationText.text = attribute.attributeName.Name() + ": " + attribute.attributeValue;
                nextIndividual = attribute.attributeValue;

                relationshipAttribute = new OntologyEntity(attribute.attributeName.URI());
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
            DataFacet iconfacet2 = DataFormats.IconButtonTap1.formatFacets[0];
            RtrbauAttribute attribute;

            // Check data received meets fabrication requirements
            if (data.fabricationData.TryGetValue(iconfacet2, out attribute))
            {
                // Generate ontology entity to report
                OntologyEntity relationship = new OntologyEntity(attribute.attributeName.URI());
                // Report relationship attribute to load next RtrbauElement
                Reporter.instance.ReportElement(relationship);
                // Generate OntologyElement(s) to load next RtrbauElement
                OntologyElement individual = new OntologyElement(attribute.attributeValue, OntologyElementType.IndividualProperties);
                OntologyElement individualClass = new OntologyElement(attribute.attributeRange.URI(), OntologyElementType.ClassProperties);
                // Find if appointed element has already been loaded
                GameObject nextElement = visualiser.FindElement(individual);
                // If so update line renderer, otherwise load new RtrbauElement
                if (nextElement != null)
                {
                    Debug.Log("IconButtonTap1::OnNextVisualisation: element " + nextElement.name + " already loaded");
                    // Update element line renderer
                    element.gameObject.GetComponent<ElementsLine>().UpdateLineEnd(nextElement);
                }
                else
                {
                    Debug.Log("IconButtonTap1::OnNextVisualisation: load new RtrbauElement for " + individual.entity.Name());
                    // Modify parent RtrbauElement in expectance of a new RtrbauElement
                    element.GetComponent<ElementConsult>().ModifyMaterial(fabricationSeenMaterial);
                    // Load new RtrbauElement from AssetVisualiser, ensure user has selected the type of RtrbauElement to load
                    RtrbauerEvents.TriggerEvent("AssetVisualiser_CreateElement", individual, individualClass, Rtrbauer.instance.user.procedure);
                }
            }
            else
            {
                throw new ArgumentException(data.fabricationName + " cannot implement: " + attribute.attributeName + " received.");
            }
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
            // To complete
        }
        #endregion IVISUALISABLE_METHODS

        #region CLASS_METHODS
        #endregion CLASS_METHODS
    }
}
