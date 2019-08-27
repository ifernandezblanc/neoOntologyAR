/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 19/08/2019
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
#endregion

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class AssetVisualiser : MonoBehaviour, IVisualisable
    {
        #region INITIALISATION_VARIABLES
        public AssetManager manager;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        private GameObject lastElement;
        private List<GameObject> primaryElements;
        private int primaryCounter;
        private List<GameObject> secondaryElements;
        private int secondaryCounter;
        private List<GameObject> tertiaryElements;
        private int tertiaryCounter;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public GameObject consultElement;
        // public GameObject reportElement;
        #endregion GAMEOBJECT_PREFABS

        #region INITIALISATION_METHODS
        /// <summary>
        /// Initialises visualiser after Vuforia registration completed.
        /// Assumes asset is given with Rtrbau structure <paramref name="assetManager"/>.
        /// </summary>
        /// <param name="assetModel"></param>
        public void Initialise(AssetManager assetManager)
        {
            if (consultElement == null)
            { consultElement = Resources.Load("Rtrbau/Prefabs/Elements/Visualisations/ConsultElement") as GameObject; }

            // Reference to the asset manager
            manager = assetManager;
            // Initialise variables
            lastElement = null;
            primaryElements = new List<GameObject>();
            primaryCounter = 0;
            secondaryElements = new List<GameObject>();
            secondaryCounter = 0;
            tertiaryElements = new List<GameObject>();
            tertiaryCounter = 0;
            // Locate visualiser
            LocateIt();
        }
        #endregion INITIALISATION_METHODS

        #region MONOBEHAVIOUR_METHODS
        void Start() { }

        void Update() { }

        void OnEnable()
        {
            RtrbauerEvents.StartListening("LoadElement", LoadElement);
            RtrbauerEvents.StartListening("LocateElement", LocateElement);
        }

        void OnDisable()
        {
            RtrbauerEvents.StopListening("LoadElement", LoadElement);
            RtrbauerEvents.StopListening("LocateElement", LocateElement);
        }

        void OnDestroy() { DestroyIt(); }
        #endregion MONOBEHAVIOUR_METHODS

        #region IVISUALISABLE_METHODS
        public void LocateIt()
        {
            // Name asset visualiser
            this.gameObject.name = "Visualiser_" + manager.asset.name;
            // Assign as child of asset manager
            this.transform.SetParent(manager.transform, false);
            // Locate at asset centre
            this.transform.position = manager.CalculateAssetCentreWorld();
        }

        /// <summary>
        /// Identifies all rtrbau elements visualised and destroys them.
        /// Then, destroys itself;
        /// </summary>
        public void DestroyIt()
        {
            // Identify all rtrbau elements visualised and destroy
            foreach (GameObject element in primaryElements)
            { element.GetComponent<IVisualisable>().DestroyIt(); }

            foreach (GameObject element in secondaryElements)
            { element.GetComponent<IVisualisable>().DestroyIt(); }

            foreach (GameObject element in tertiaryElements)
            { element.GetComponent<IVisualisable>().DestroyIt(); }
        }
        #endregion IVISUALISABLE_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        /// <summary>
        /// Adds <paramref name="element"/> to visualisation location and 
        /// deletes oldest element and re-locates all.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="location"></param>
        /// <param name="locationElements"></param>
        /// <param name="locationCounter"></param>
        void AddElement (GameObject element, RtrbauElementLocation location, ref List<GameObject> locationElements, ref int locationCounter)
        {
            Bounds bounds = manager.CalculateAssetBounds();
            // Calculate the maximum number of elements that can be in a specified location
            // int locationElementsNo = (((int)location) * 4) + 5;
            // For a maximum of 1 element in primary location:
            int locationElementsNo = (int)location + 1;

            if (locationCounter == locationElementsNo) { locationCounter = 0; }

            if (locationElements.Count == locationElementsNo)
            {
                GameObject.Destroy(locationElements[0]);
                locationElements.RemoveAt(0);   
            }

            element.transform.SetParent(this.transform, false);
            locationElements.Add(element);
            SetSize(element, bounds);
            SetPosition(element, location, bounds, locationElementsNo, locationCounter);
            // SetFabrications(element);
            lastElement = element;
            locationCounter++;
        }

        /// <summary>
        /// /// <summary>
        /// Adapts <paramref name="element"/> scale (only x and y) if bigger than the asset.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="bounds"></param>
        void SetSize(GameObject element, Bounds bounds)
        {
            // UPGRADE REQUIRED
            // NEED TO ADAPT TO MODEL, IF MODEL SCALE IS HIGHER, THEN MULTIPLE (IF BIGGER THAN 1)

            // Calculate scales of element compared to asset
            List<bool> scales = new List<bool>
            {
                {element.transform.localScale.x > bounds.size.x },
                {element.transform.localScale.y > bounds.size.y }
                // {element.transform.localScale.z > bounds.size.z }
            };

            // If element scales are greather than one, then divide by the greatest
            if (scales.FindAll(x => x == true).Count != 0)
            {
                List<float> sizes = new List<float>
                {
                    {element.transform.localScale.x / bounds.size.x },
                    {element.transform.localScale.y / bounds.size.y }
                    // {element.transform.localScale.z / bounds.size.z }
                };

                sizes.Sort((x, y) => x.CompareTo(y));

                float sX = element.transform.localScale.x / sizes[sizes.Count - 1];
                float sY = element.transform.localScale.y / sizes[sizes.Count - 1];
                float sZ = element.transform.localScale.z;
                // float sZ = element.transform.localScale.z / sizes[sizes.Count - 1];

                element.transform.localScale = new Vector3(sX, sY, sZ);
            }
            else { }
        }

        /// <summary>
        /// Sets position of element according to the visualiser position and assets size.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="location"></param>
        /// <param name="bounds"></param>
        /// <param name="locationCounter"></param>
        void SetPosition(GameObject element, RtrbauElementLocation location, Bounds bounds, int elementsNo,  int locationCounter)
        {
            // Add manipulator to control element position from its center
            //GameObject elementLocator = new GameObject();
            //elementLocator.name = "Manipulator_" + element.name;
            //elementLocator.transform.SetParent(element.transform, false);
            // Bounds elementBounds = CalculateElementBounds(element);
            Bounds elementBounds = element.transform.GetComponentInChildren<BoxCollider>().bounds;
            //elementLocator.transform.position = elementBounds.center;

            // Variables to calculate position;
            float pX;
            float pY;
            // Calculate position different according to number of elements
            switch (elementsNo)
            {
                case 1:
                    pX = bounds.extents.x + (elementBounds.extents.x * 1.15f);
                    pY = elementBounds.extents.x * 1.15f;
                    break;
                case 2:
                    switch (locationCounter)
                    {
                        case 0:
                            pX = bounds.extents.x + (elementBounds.size.x * 1.65f);
                            pY = elementBounds.size.x * 1.65f;
                            break;
                        default:
                            pX = -bounds.extents.x - (elementBounds.size.x * 1.65f);
                            pY = elementBounds.size.x * 1.65f;
                            break;
                    }
                    break;
                default:
                    switch (locationCounter)
                    {
                        case 0:
                            pX = bounds.extents.x + (elementBounds.size.x * 2.75f);
                            pY = elementBounds.size.x * 2.75f;
                            break;
                        case 1:
                            pX = elementBounds.size.x * 2.75f;
                            pY = bounds.extents.y + (elementBounds.size.y * 2.75f);
                            break;
                        default:
                            pX = -bounds.extents.x - (elementBounds.size.x * 2.75f);
                            pY = elementBounds.size.x * 2.75f;
                            break;
                    }
                    break;
            }

            element.transform.localPosition += new Vector3(pX, pY, 0);

            //// Calculate position vector
            //// Calculate direction of position vector from visualiser origin:
            //Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            //Debug.Log(direction);
            //// Calculate offset magnitude of position vector according to element size:
            //Bounds elementBounds = CalculateElementBounds(element);
            //Vector3 magnitude = new Vector3(elementBounds.extents.x, elementBounds.extents.y, 0) * (int)location;
            //// Vector3 offset = (bounds.extents * 1.75f) + magnitude;
            //Vector3 offset = (bounds.extents) + magnitude;
            //// Vector3 offset = (bounds.extents + element.transform.GetComponentInChildren<BoxCollider>().bounds.extents) * magnitude;
            //Debug.Log(offset); 
            //// Vector3 origin = manager.CalculateAssetCentreWorld();
            //// UPG: offset to make rectangular shape instead of circular
            //element.transform.localPosition += Vector3.Scale(offset, direction);
        }

        Bounds CalculateElementBounds(GameObject element)
        {
            Bounds elementBounds = new Bounds(Vector3.zero, Vector3.zero);

            MeshRenderer[] elementMeshes = element.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer mesh in elementMeshes)
            {
                Debug.Log(mesh.gameObject.name);
                elementBounds.Encapsulate(mesh.bounds);
            }

            return elementBounds;
        }


        /// <summary>
        /// Activtes the CreateFabrications function from IElementable
        /// </summary>
        /// <param name="element"></param>
        //void SetFabrications(GameObject element)
        //{
        //    if (element.GetComponent<IElementable>() != null)
        //    {
        //        element.GetComponent<IElementable>().CreateFabrications();
        //    }
        //    else
        //    {
        //        throw new ArgumentException("IElementable not implemented.");
        //    }
        //}
        #endregion PRIVATE

        #region PUBLIC
        /// <summary>
        /// Creates new <paramref name="element"/> of <paramref name="type"/>.
        /// If <paramref name="element"/> found in list, then re-loaded instead.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="type"></param>
        public void LoadElement(OntologyElement element, RtrbauElementType type)
        {
            if (type == RtrbauElementType.Consult)
            {
                GameObject newElement = GameObject.Instantiate(consultElement);
                newElement.GetComponent<ElementConsult>().Initialise(this, element, lastElement);
            }
            else if (type == RtrbauElementType.Report)
            {
                Debug.LogError("Report Element not implemented yet.");
            }
            else
            {
                throw new ArgumentException("Rtrbau Element type not implemented");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="location"></param>
        public void LocateElement(GameObject element, RtrbauElementLocation location)
        {
            if (location == RtrbauElementLocation.Primary)
            {
                AddElement(element, location, ref primaryElements, ref primaryCounter);
            }
            else if (location == RtrbauElementLocation.Secondary)
            {
                AddElement(element, location, ref secondaryElements, ref secondaryCounter);
            }
            else if (location == RtrbauElementLocation.Tertiary)
            {
                AddElement(element, location, ref tertiaryElements, ref tertiaryCounter);
            }
            else
            {
                throw new ArgumentException("Rtrbau Element Location not implemented.");
            }

            // visualisedElements[location].Add(element);
            //GameObject locationVisualiser;
            //Debug.Log("Visualiser: LocateElement: " + element.GetComponent<ElementConsult>().individualElement.entity.name);
            //if (visualisedElements.TryGetValue(location, out locationVisualiser))
            //{
            //    locationVisualiser.GetComponent<VisualiserLocation>().AddElement(element);
            //}
            //else
            //{
            //    throw new ArgumentException("Visualisation location not implemented.");
            //}
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}
