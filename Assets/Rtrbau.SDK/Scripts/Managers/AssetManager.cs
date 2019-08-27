/*==============================================================================
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
        // private Bounds assetModelBounds;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECTS_MANAGED
        public GameObject asset;
        public GameObject assetModel;
        public GameObject assetRegistrator;
        public GameObject assetVisualiser;
        public List<GameObject> assetComponentsModels;
        #endregion GAMEOBJECTS_MANAGED

        #region GAMEOBJECTS_PREFABS
        private Material assetModelMaterial;
        private Material BoxMaterial;
        private Material BoxGrabbedMaterial;
        private Material HandleMaterial;
        private Material HandleGrabbedMaterial;
        private GameObject RotationHandleSlatePrefab;
        #endregion GAMEOBJECT_PREFABS

        #region INITIALISATION_METHODS
        public void Initialise()
        {
            // Initialise class variables
            // assetModelBounds = new Bounds(Vector3.zero, Vector3.zero);
            assetComponentsModels = new List<GameObject>();

            // Run additional visualisation initialiser functionalities
            LocateIt();
            LoadMaterials();
            LoadMeshes();
            LoadBoxCollider();
            LoadBoundingBox();
            LoadManipulationHandler();
        }

        public void InitialiseVisualiser()
        {
            GameObject visualiser = new GameObject();
            visualiser.AddComponent<AssetVisualiser>();
            visualiser.GetComponent<AssetVisualiser>().Initialise(this);
            assetVisualiser = visualiser;
        }
        #endregion INITIALISATION_METHODS

        #region MONOBEHAVIOUR_METHODS
        void OnDestroy() { DestroyIt(); }
        #endregion MONOBEHAVIOUR_METHODS

        #region IVISUALISABLE_METHODS
        /// <summary>
        /// Initialises and locate visualisation game objects
        /// </summary>
        public void LocateIt()
        {
            // Top parent of asset visualisation (holds all translations and rotations from target)
            asset = this.gameObject;
            // Needs to rotate 180 degrees on y-axis to align with asset target
            asset.transform.localRotation *= Quaternion.Euler(0, 180, 0);
            // Game object to control asset top parent manual registration
            // Should include loaded models as childs to keep them in a single copy
            assetRegistrator = new GameObject();
            assetRegistrator.name = "Registrator_" + asset.name;
            assetRegistrator.transform.SetParent(asset.transform, false);
            assetRegistrator.transform.position = asset.transform.position;
            // Game object parent of loaded asset component models
            assetModel = GameObject.Find("Model_" + asset.name);
            assetModel.transform.SetParent(assetRegistrator.transform, true);
            // List of game objects of loaded asset component models
            foreach (Transform child in assetModel.transform)
            {
                assetComponentsModels.Add(child.gameObject);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DestroyIt() { assetVisualiser.GetComponent<IVisualisable>().DestroyIt(); }
        #endregion IVISUALISABLE_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        /// <summary>
        /// Loads necessary materials to build asset registrator from project resources.
        /// </summary>
        void LoadMaterials()
        {
            assetModelMaterial = Resources.Load("Rtrbau/Materials/RtrbauMaterialStandardTransparentBlue") as Material;
            BoxMaterial = Resources.Load("MRTK/BoundingBox") as Material;
            BoxGrabbedMaterial = Resources.Load("MRTK/BoundingBoxGrabbed") as Material;
            HandleMaterial = Resources.Load("MRTK/BoundingBoxHandleWhite") as Material;
            HandleGrabbedMaterial = Resources.Load("MRTK/BoundingBoxHandleBlueGrabbed") as Material;
            RotationHandleSlatePrefab = Resources.Load("MRTK/MRTK_BoundingBox_RotateWidget") as GameObject;
        }

        /// <summary>
        /// Modifies loaded asset components models to adapt to rtrbau visualisation.
        /// </summary>
        void LoadMeshes()
        {
            foreach (GameObject component in assetComponentsModels)
            {
                // Change material for rtrbau visualisation
                component.GetComponent<MeshRenderer>().material = assetModelMaterial;
                // Add manipulator in bounds centre for controlling mesh in fabrications;
                GameObject componentManipulator = new GameObject();
                componentManipulator.name = "Manipulator_" + component.name;
                componentManipulator.transform.SetParent(component.transform, false);
                componentManipulator.transform.position = component.GetComponent<MeshRenderer>().bounds.center;
                // Calculate assetModelBounds for following initialisation functions
                // assetModelBounds.Encapsulate(component.GetComponent<MeshRenderer>().bounds);
            }
        }


        /// <summary>
        /// Adds box collider to asset registrator for enabling bounding box and manipulation handler.
        /// </summary>
        void LoadBoxCollider()
        {
            Bounds bounds = CalculateAssetBounds();

            // https://answers.unity.com/questions/22019/auto-sizing-primitive-collider-based-on-child-mesh.html
            assetRegistrator.AddComponent<BoxCollider>();
            assetRegistrator.GetComponent<BoxCollider>().center = assetRegistrator.transform.InverseTransformPoint(bounds.center);
            assetRegistrator.GetComponent<BoxCollider>().size = bounds.size;
        }

        /// <summary>
        /// Adds bounding box to asset registrator to control rotation of asset parent.
        /// </summary>
        void LoadBoundingBox()
        {
            assetRegistrator.AddComponent<BoundingBox>();
            assetRegistrator.GetComponent<BoundingBox>().targetObject = asset;
            assetRegistrator.GetComponent<BoundingBox>().boundsOverride = assetRegistrator.GetComponent<BoxCollider>();
            assetRegistrator.GetComponent<BoundingBox>().BoxMaterial = BoxMaterial;
            assetRegistrator.GetComponent<BoundingBox>().BoxGrabbedMaterial = BoxGrabbedMaterial;
            assetRegistrator.GetComponent<BoundingBox>().HandleMaterial = HandleMaterial;
            assetRegistrator.GetComponent<BoundingBox>().HandleGrabbedMaterial = HandleGrabbedMaterial;
            assetRegistrator.GetComponent<BoundingBox>().RotationHandleSlatePrefab = RotationHandleSlatePrefab;
            assetRegistrator.GetComponent<BoundingBox>().ShowScaleHandles = false;
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
        public Bounds CalculateAssetBounds()
        {
            Bounds assetBounds = new Bounds(Vector3.zero, Vector3.zero);

            foreach (GameObject component in assetComponentsModels)
            {
                assetBounds.Encapsulate(component.GetComponent<MeshRenderer>().bounds);
            }

            return assetBounds;
        }
        /// <summary>
        /// Returns the asset's centre position in world space coordinates.
        /// </summary>
        /// <returns></returns>
        public Vector3 CalculateAssetCentreWorld()
        {
            Vector3 bcCentre = assetRegistrator.GetComponent<BoxCollider>().center;
            return assetRegistrator.transform.TransformPoint(bcCentre);
        }

        /// <summary>
        /// Returns asset component model by <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject FindAssetComponent(string name)
        {
            GameObject component = assetComponentsModels.Find(x => x.name == name);

            if (component != null)
            {
                return component;
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


