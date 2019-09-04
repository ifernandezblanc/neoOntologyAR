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
    public class HologramNone1 : MonoBehaviour, IFabricationable, IVisualisable
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
        public GameObject hologram;
        // public GameObject text;
        #endregion CLASS_VARIABLES

        #region FACET_VARIABLES
        public string hologramName;
        public string componentName;
        public string iconName;
        #endregion FACET_VARIABLES

        #region GAMEOBJECT_PREFABS
        // public GameObject textPanel;
        public Material material;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS

        #endregion CLASS_EVENTS

        #region MONOBEHVAIOUR_METHODS
        void Start()
        {
            // if (textPanel == null || material == null)
            if (material == null)
            {
                throw new ArgumentException("HologramNone1 script requires some prefabs to work.");
            }
        }

        void Update()
        {
            // model.transform.position = component.transform.position;
            // model.transform.rotation = component.transform.rotation;
            // UpdateLineRenderer();
        }

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
            // Read attributes

            foreach (KeyValuePair<DataFacet, RtrbauAttribute> facet in data.fabricationData)
            {
                if (facet.Key == DataFormats.hologramnone1.formatFacets[0])
                {
                    // Find hologram that retrieves value
                    hologramName = Libraries.HologramLibrary.Find(x => x.Contains(facet.Value.attributeValue));
                    string hologramPath = "Rtrbau/Holograms/" + hologramName;
                    GameObject hologramOriginal = Resources.Load(hologramPath) as GameObject;
                    hologram = Instantiate(hologramOriginal);

                }
                else if (facet.Key == DataFormats.hologramnone1.formatFacets[1])
                {
                    // Find component that relates to hologram's model
                    componentName = Parser.ParseURI(Parser.ParseURI(facet.Value.attributeValue, '/', RtrbauParser.post), '.', RtrbauParser.pre);
                    component = visualiser.transform.parent.gameObject.GetComponent<AssetManager>().FindAssetComponentManipulator(componentName);
                    model = Instantiate(component);
                }
                else if (facet.Key == DataFormats.hologramnone1.formatFacets[2])
                {
                    // Find icon that retrieves value
                    iconName = Libraries.IconLibrary.Find(x => x.Contains(facet.Value.attributeValue));
                }
                else
                {
                    throw new ArgumentException(data.fabricationName + " cannot implement: " + facet.Value.attributeName + " received.");
                }
            }

            SetModel();
            SetHologram();
            // AddLineRenderer();
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
            // Do nothing in this case
        }

        public void DestroyIt()
        {
            Destroy(this.gameObject);
        }
        #endregion IVISUALISABLE_METHODS

        #region CLASS_METHODS
        void SetModel()
        {
            model.name = this.name + componentName + this.GetHashCode();
            model.transform.SetParent(scale, false);
            model.GetComponentInChildren<MeshRenderer>().material = material;
            model.transform.position = component.transform.position;
            model.transform.rotation = component.transform.rotation;
        }
        void SetHologram()
        {
            hologram.name = this.name + hologramName + this.GetHashCode();
            hologram.transform.SetParent(scale, false);
            SetHologramMaterial();
            SetHologramScale();
            SetHologramPosition();
        }

        void SetHologramMaterial()
        {
            MeshRenderer[] hologramMeshes = hologram.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mesh in hologramMeshes)
            {
                mesh.material = material;
            }
        }

        void SetHologramScale()
        {
            Bounds modelBounds = model.GetComponent<MeshRenderer>().bounds;

            // Calculate scales of element compared to asset
            List<bool> scales = new List<bool>
            {
                {hologram.transform.localScale.x > modelBounds.size.x },
                {hologram.transform.localScale.y > modelBounds.size.y },
                {hologram.transform.localScale.z > modelBounds.size.z }
            };

            // If element scales are greather than one, then divide by the greatest
            if (scales.FindAll(x => x == true).Count != 0)
            {
                List<float> sizes = new List<float>();

                for (int i = 0; i < 3; i++)
                {
                    if (modelBounds.size[i] < 1f)
                    {
                        sizes.Add(hologram.transform.localScale[i] * modelBounds.size[i]);
                    }
                    else
                    {
                        sizes.Add(hologram.transform.localScale[i] / modelBounds.size[i]);
                    }
                }

                sizes.Sort((x, y) => x.CompareTo(y));

                float sX = hologram.transform.localScale.x / sizes[sizes.Count - 1];
                float sY = hologram.transform.localScale.y / sizes[sizes.Count - 1];
                float sZ = hologram.transform.localScale.z / sizes[sizes.Count - 1];

                hologram.transform.localScale = new Vector3(sX, sY, sZ);
            }
            else { }

        }

        void SetHologramPosition()
        {
            // Set direction from asset origin to model origin
            Vector3 direction = model.GetComponentInChildren<MeshRenderer>().bounds.center - visualiser.transform.position;
            direction = direction / direction.magnitude;
            // Set further in the direction of the object
            hologram.transform.position = model.GetComponentInChildren<MeshRenderer>().bounds.center + (Vector3.Normalize(direction)*0.15f);
            hologram.transform.LookAt(model.transform);

            // To rotate the hologram according to specific positions due to 
            if (iconName != null)
            {
                Debug.Log(iconName + hologramName);

                if (iconName == "pull" && hologramName == "arrow")
                {
                    hologram.transform.GetChild(0).localRotation = Quaternion.Euler(90, 0, 0);
                    hologram.transform.GetChild(1).localRotation = Quaternion.Euler(90, 0, 0);
                }
                else if (iconName == "push" && hologramName == "arrow")
                {
                    hologram.transform.GetChild(0).localRotation = Quaternion.Euler(-90, 0, 0);
                    hologram.transform.GetChild(1).localRotation = Quaternion.Euler(-90, 0, 0);
                }
                else { }
            }
            else { }
        }
        #endregion CLASS_METHODS
    }
}
