﻿/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 23/07/2019
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class AssetManager : MonoBehaviour, IVisualisable
    {
        #region CLASS_VARIABLES
        public Bounds assetBounds;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECTS_MANAGED
        public GameObject asset;
        public GameObject assetModel;
        public List<GameObject> assetComponentsModels;
        public GameObject assetRegistrator;
        public GameObject assetVisualiser;
        #endregion GAMEOBJECTS_MANAGED

        #region GAMEOBJECTS_PREFABS
        private Material assetModelMaterial;
        private Material BoxMaterial;
        private Material BoxGrabbedMaterial;
        private Material HandleMaterial;
        private Material HandleGrabbedMaterial;
        private GameObject RotationHandleSlatePrefab;
        private GameObject ScaleHandleSlatePrefab;
        #endregion GAMEOBJECT_PREFABS

        #region INITIALISATION_METHODS
        public void Initialise()
        {
            Vector3 registeredPosition = this.transform.position;
            Quaternion registeredRotation = this.transform.rotation;

            // Tag game object as RtrbauAssetManager for other Rtrbau services to find it
            // Remember that tag has to be defined in editor in advance, otherwise will result in error
            this.gameObject.tag = RtrbauGameObjectTags.RtrbauAssetManager.ToString();

            // Set model at origin
            this.transform.position = new Vector3(0, 0, 0);
            this.transform.rotation = Quaternion.Euler(0, 0, 0);

            // Initialise asset manager reference
            asset = this.gameObject;

            // Initialise asset model reference;
            assetModel = this.transform.GetChild(0).gameObject;

            // Initialise asset model components references
            assetComponentsModels = ReturnModels(assetModel);

            // Create asset registrator
            assetRegistrator = new GameObject();
            assetRegistrator.name = "Registrator_" + asset.name;
            assetRegistrator.transform.SetParent(asset.transform, false);

            // Assign asset model to asset registrator
            assetModel.transform.SetParent(assetRegistrator.transform, true);

            // Initialise asset registrator
            InitialiseRegistrator();

            // Initialise asset visualiser
            InitialiseVisualiser();

            // Set back at registered position
            this.transform.position = registeredPosition;
            this.transform.rotation = registeredRotation;
        }
        #endregion INITIALISATION_METHODS

        #region MONOBEHAVIOUR_METHODS
        void Start() { assetBounds = new Bounds(); }
        void OnDestroy() { DestroyIt(); }
        #endregion MONOBEHAVIOUR_METHODS

        #region IVISUALISABLE_METHODS
        public void LocateIt()
        {
            /// <see cref="AssetManager"/> location managed by <see cref="PanelAssetRegistrator"/>.
        }

        public void ActivateIt() { }

        public void DestroyIt() { }

        public void ModifyMaterial(Material material) { }
        #endregion IVISUALISABLE_METHODS

        #region CLASS_METHODS
        #region INITIALISERS

        #endregion INITIALISERS

        #region PRIVATE
        List<GameObject> ReturnModels(GameObject assetModel)
        {
            List<GameObject> models = new List<GameObject>();

            MeshRenderer[] components = assetModel.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer component in components)
            {
                // Caculate asset bounds in local space
                assetBounds.Encapsulate(component.bounds);
                // Add manipulator in bounds centre for controlling mesh in fabrications
                GameObject componentManipulator = new GameObject();
                componentManipulator.name = "Manipulator_" + component.name;
                componentManipulator.transform.SetParent(component.transform, false);
                componentManipulator.transform.position = component.bounds.center;
                // Set component mesh as child of manipulator parent
                componentManipulator.transform.SetParent(assetModel.transform, true);
                component.transform.SetParent(componentManipulator.transform, true);
                // Add component to models list
                // models.Add(component.gameObject);
                models.Add(componentManipulator);
            }

            return models;
        }

        void InitialiseRegistrator()
        {
            LoadBoundingBox();
            LoadManipulationHandler();
        }


        void InitialiseVisualiser()
        {
            //assetVisualiser = new GameObject();
            //assetVisualiser.AddComponent<AssetVisualiser>();
            //assetVisualiser.GetComponent<AssetVisualiser>().Initialise(this);

            // UPG: to enable rtrbauer to control visualiser
            assetVisualiser = Rtrbauer.instance.LoadVisualiser(this);
        }

        /// <summary>
        /// Adds bounding box to asset registrator to control rotation of asset parent.
        /// </summary>
        void LoadBoundingBox()
        {
            // Load materials
            BoxMaterial = Resources.Load("MRTK/BoundingBox") as Material;
            BoxGrabbedMaterial = Resources.Load("MRTK/BoundingBoxGrabbed") as Material;
            HandleMaterial = Resources.Load("MRTK/BoundingBoxHandleWhite") as Material;
            HandleGrabbedMaterial = Resources.Load("MRTK/BoundingBoxHandleBlueGrabbed") as Material;
            RotationHandleSlatePrefab = Resources.Load("MRTK/MRTK_BoundingBox_RotateWidget") as GameObject;
            ScaleHandleSlatePrefab = Resources.Load("MRTK/MRTK_BoundingBox_ScaleWidget") as GameObject;

            // Load Box Collider
            // https://answers.unity.com/questions/22019/auto-sizing-primitive-collider-based-on-child-mesh.html
            assetRegistrator.AddComponent<BoxCollider>();
            assetRegistrator.GetComponent<BoxCollider>().center = assetRegistrator.transform.InverseTransformPoint(assetBounds.center);
            assetRegistrator.GetComponent<BoxCollider>().size = assetBounds.size;

            // Assign component and properties
            assetRegistrator.AddComponent<BoundingBox>();
            assetRegistrator.GetComponent<BoundingBox>().targetObject = asset;
            assetRegistrator.GetComponent<BoundingBox>().boundsOverride = assetRegistrator.GetComponent<BoxCollider>();
            assetRegistrator.GetComponent<BoundingBox>().CalculationMethod = BoundingBox.BoundsCalculationMethod.ColliderOnly;
            assetRegistrator.GetComponent<BoundingBox>().BoxMaterial = BoxMaterial;
            assetRegistrator.GetComponent<BoundingBox>().BoxGrabbedMaterial = BoxGrabbedMaterial;
            assetRegistrator.GetComponent<BoundingBox>().HandleMaterial = HandleMaterial;
            assetRegistrator.GetComponent<BoundingBox>().HandleGrabbedMaterial = HandleGrabbedMaterial;
            assetRegistrator.GetComponent<BoundingBox>().RotationHandleSlatePrefab = RotationHandleSlatePrefab;
            assetRegistrator.GetComponent<BoundingBox>().ScaleHandlePrefab = ScaleHandleSlatePrefab;
            // assetRegistrator.GetComponent<BoundingBox>().ShowScaleHandles = false;
            assetRegistrator.GetComponent<BoundingBox>().ShowScaleHandles = true;
            assetRegistrator.GetComponent<BoundingBox>().ShowRotationHandleForX = true;
            assetRegistrator.GetComponent<BoundingBox>().ShowRotationHandleForY = true;
            assetRegistrator.GetComponent<BoundingBox>().ShowRotationHandleForZ = true;
            // assetRegistrator.GetComponent<BoundingBox>().RotationHandleDiameter = 0.15f;
            assetRegistrator.GetComponent<BoundingBox>().BoundingBoxActivation = BoundingBox.BoundingBoxActivationType.ActivateByPointer;
        }

        /// <summary>
        /// Adds manipulation handler to asset registrator to control translation and rotation of asset parent.
        /// </summary>
        void LoadManipulationHandler()
        {
            // Assign near interaction grabbable
            assetRegistrator.AddComponent<NearInteractionGrabbable>();
            assetRegistrator.GetComponent<NearInteractionGrabbable>().ShowTetherWhenManipulating = true;
            // Assign manipulation handler
            assetRegistrator.AddComponent<ManipulationHandler>();
            assetRegistrator.GetComponent<ManipulationHandler>().HostTransform = asset.transform;
            assetRegistrator.GetComponent<ManipulationHandler>().ManipulationType = ManipulationHandler.HandMovementType.OneAndTwoHanded;
            assetRegistrator.GetComponent<ManipulationHandler>().TwoHandedManipulationType = ManipulationHandler.TwoHandedManipulation.MoveRotate;
            assetRegistrator.GetComponent<ManipulationHandler>().OneHandRotationModeFar = ManipulationHandler.RotateInOneHandType.MaintainOriginalRotation;
            assetRegistrator.GetComponent<ManipulationHandler>().OneHandRotationModeNear = ManipulationHandler.RotateInOneHandType.MaintainOriginalRotation;
        }
        #endregion PRIVATE

        #region PUBLIC
        /// <summary>
        /// Returns asset model bounds calculated from component models with meshes.
        /// </summary>
        /// <returns></returns>
        public Bounds ReturnAssetBoundsLocal()
        {
            Bounds bounds = new Bounds();
            foreach (GameObject model in assetComponentsModels)
            {
                // bounds.Encapsulate(model.GetComponent<MeshFilter>().mesh.bounds);
                bounds.Encapsulate(model.GetComponentInChildren<MeshFilter>().mesh.bounds);
            }
            return bounds;
        }
        /// <summary>
        /// Returns the asset's centre position in asset's local space coordinates.
        /// </summary>
        /// <returns></returns>
        public Vector3 ReturnAssetCentreLocal()
        {
            return assetRegistrator.GetComponent<BoxCollider>().center;
        }

        /// <summary>
        /// Returns asset component manipulator by component <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject FindAssetComponentManipulator(string name)
        {
            GameObject component = assetComponentsModels.Find(x => x.name == "Manipulator_" + name);

            if (component != null)
            {
                return component;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns asset component name if component found in scene by <paramref name="name"/>, otherwise returns null.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string FindAssetComponent(string name)
        {
            GameObject component = assetComponentsModels.Find(x => x.name == "Manipulator_" + name);

            if (component != null)
            {
                return component.transform.GetChild(0).name;
            }
            else
            {
                return null;
            }
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}