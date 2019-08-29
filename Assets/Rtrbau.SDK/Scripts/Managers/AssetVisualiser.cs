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
using System.Linq;
using Microsoft.MixedReality.Toolkit.UI;
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
        private List<KeyValuePair<OntologyElement,GameObject>> loadedElements;
        private List<GameObject> createdElements;
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
            loadedElements = new List<KeyValuePair<OntologyElement, GameObject>>();
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

        void OnDestroy() { }
        #endregion MONOBEHAVIOUR_METHODS

        #region IVISUALISABLE_METHODS
        public void LocateIt()
        {
            // Name asset visualiser
            this.gameObject.name = "Visualiser_" + manager.asset.name;
            // Assign as child of asset manager
            this.transform.SetParent(manager.transform, false);
            // Locate at asset centre
            this.transform.position = manager.ReturnAssetCentreWorld();
            // Rotate 180 degrees over y-axis to align with asset target rotation
            // this.transform.localRotation = Quaternion.Euler(0, 180, 0);
            // Changed to asset manager to be rotated
        }

        /// <summary>
        /// Identifies all rtrbau elements visualised and destroys them.
        /// Then, destroys itself;
        /// </summary>
        public void DestroyIt()
        {
            // Identify all rtrbau elements visualised and destroy
            // All elements are child, so they will be destroyed with this one
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
            // Calculate number of elements per location
            int locationElementsNo = (int)location + 1;

            // Remove earliest element on location list if location is full
            if (locationCounter == locationElementsNo) { locationCounter = 0; }

            if (locationElements.Count == locationElementsNo)
            {
                Destroy(locationElements[0]);
                locationElements.RemoveAt(0);
            }

            // Assign element to location
            locationElements.Add(element);

            // Assign element as child of visualiser
            element.transform.SetParent(this.transform, false);

            // Assign bounding box to element
            // CreateBoundingBox(element);

            // Set translation of element according to location
            SetTranslation(element, locationElementsNo, locationCounter);

            

            // Assign element as last element
            lastElement = element;
            // Iterate over location element counter
            locationCounter++;
        }


        void CreateBoundingBox(GameObject element)
        {
            element.AddComponent<BoundingBox>();
            element.GetComponent<BoundingBox>().ShowScaleHandles = false;
            element.GetComponent<BoundingBox>().ShowWireFrame = false;
            element.GetComponent<BoundingBox>().ShowRotationHandleForX = false;
            element.GetComponent<BoundingBox>().ShowRotationHandleForY = false;
            element.GetComponent<BoundingBox>().ShowRotationHandleForZ = false;
        }

        /// <summary>
        /// Sets position of element according to the visualiser position and assets size.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="location"></param>
        /// <param name="bounds"></param>
        /// <param name="locationCounter"></param>
        void SetTranslation(GameObject element, int locationElements,  int locationCounter)
        {
            // SetScale()???
            Bounds assetBounds = manager.ReturnAssetBoundsLocal();
            Vector3 elementSize = CalculateElementBounds(element).size;
            Vector3 elementScale = element.transform.localScale;
            // Variables to calculate position:
            // Assumes the origin of the element is at is asset's centre (asset visualiser origin)
            float aX = assetBounds.extents.x;
            float aY = assetBounds.extents.y;
            Debug.Log("aX: " + aX + " aY: " + aY);
            float eX = (elementSize.x * elementScale.x) / 2;
            float eY = (elementSize.x * elementScale.y) / 2;
            Debug.Log("eX: " + eX + " eY: " + eY);
            float oX = eX * 0.15f;
            float oY = eY * 0.15f;
            float pX;
            float pY;

            if (locationElements == 1)
            {
                pX = aX + eX + oX;
                pY = eY;
            }
            else if (locationElements == 2)
            {
                if (locationCounter == 0)
                {
                    pX = aX + 3 * eX + 2 * oX;
                    pY = eY;
                }
                else if (locationCounter == 1)
                {
                    pX = - aX - 3 * eX - 2 * oX;
                    pY = eY;
                }
                else
                {
                    throw new ArgumentException("Number of location counter not implemented.");
                }
            }
            else if (locationElements == 3)
            {
                if (locationCounter == 0)
                {
                    pX = aX + 5 * eX + 3 * oX;
                    pY = eY;
                }
                else if (locationCounter == 1)
                {
                    pX = 0;
                    pY = aY + eY + oY;
                }
                else if (locationCounter == 2)
                {
                    pX = - aX - 5 * eX - 3 * oX;
                    pY = eY;
                }
                else
                {
                    throw new ArgumentException("Number of location counter not implemented.");
                }
            }
            else
            {
                throw new ArgumentException("Number of location elements not implemented.");
            }

            Debug.Log("pX: " + pX + " pY: " + pY);
            element.transform.localPosition = new Vector3(pX, pY, 0);

            //// Calculate position different according to number of elements
            //switch (locationElements)
            //{
            //    case 1:
            //        // pX = bounds.extents.x + (elementBounds.extents.x * 1.15f);
            //        pX = assetBounds.extents.x;
            //        pY = elementBounds.extents.x * 1.15f;
            //        break;
            //    case 2:
            //        switch (locationCounter)
            //        {
            //            case 0:
            //                // pX = bounds.extents.x + (elementBounds.size.x * 1.65f);
            //                pX = assetBounds.extents.x + (elementBounds.extents.x * 1.15f);
            //                pY = elementBounds.size.x * 1.65f;
            //                break;
            //            default:
            //                // pX = -bounds.extents.x - (elementBounds.size.x * 1.65f);
            //                pX = -assetBounds.extents.x;
            //                pY = elementBounds.size.x * 1.65f;
            //                break;
            //        }
            //        break;
            //    default:
            //        switch (locationCounter)
            //        {
            //            case 0:
            //                // pX = bounds.extents.x + (elementBounds.size.x * 2.75f);
            //                pX = assetBounds.extents.x;
            //                pY = elementBounds.size.x * 2.75f;
            //                break;
            //            case 1:
            //                // pX = elementBounds.size.x * 2.75f;
            //                pX = 0;
            //                pY = assetBounds.extents.y + (elementBounds.size.y * 2.75f);
            //                break;
            //            default:
            //                // pX = -bounds.extents.x - (elementBounds.size.x * 2.75f);
            //                pX = -assetBounds.extents.x;
            //                pY = elementBounds.size.x * 2.75f;
            //                break;
            //        }
            //        break;
            //}

            // element.transform.localPosition = new Vector3(pX, pY, 0);

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

        /// <summary>
        /// /// <summary>
        /// Adapts <paramref name="element"/> scale (only x and y) if bigger than the asset.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="bounds"></param>
        void SetScale(GameObject element, Bounds bounds)
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

        Bounds CalculateElementBounds(GameObject element)
        {
            Bounds elementBounds = new Bounds();

            MeshFilter[] elements = element.GetComponentsInChildren<MeshFilter>();

            foreach (MeshFilter elementMesh in elements)
            {
                Debug.Log(elementMesh.gameObject.name);
                elementBounds.Encapsulate(elementMesh.mesh.bounds);
            }

            //element.AddComponent<BoxCollider>();
            //element.GetComponent<BoxCollider>().center = elementBounds.center;
            //element.GetComponent<BoxCollider>().size = elementBounds.size;

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


        bool ElementLoaded(OntologyElement element)
        {
            return loadedElements.Any(x => x.Key.entity.name == element.entity.name && x.Key.entity.ontology == element.entity.ontology && x.Key.type == element.type);
        }
        #endregion PRIVATE

        #region PUBLIC
        /// <summary>
        /// Returns gameobject corresponding to <paramref name="element"/> of <paramref name="type"/>.
        /// If not found, returns null.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public GameObject FindElement(OntologyElement element)
        {
            return loadedElements.Find(x => x.Key.entity.name == element.entity.name && x.Key.entity.ontology == element.entity.ontology && x.Key.type == element.type).Value;
        }

        /// <summary>
        /// Creates new <paramref name="element"/> of <paramref name="type"/>.
        /// If <paramref name="element"/> found in list, then re-loaded instead.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="type"></param>
        void LoadElement(OntologyElement element, RtrbauElementType type)
        {
            if (type == RtrbauElementType.Consult)
            {
                Debug.Log("LoadElement: loaded already:" + ElementLoaded(element));

                if (!ElementLoaded(element))
                {
                    GameObject newElement = GameObject.Instantiate(consultElement);
                    newElement.GetComponent<ElementConsult>().Initialise(this, element, lastElement);
                    loadedElements.Add(new KeyValuePair<OntologyElement, GameObject>(element, newElement));
                }
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
        void LocateElement(GameObject element, RtrbauElementLocation location)
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
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}
