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
        private GameObject primaryElement;
        // private bool primaryCounter;
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
            primaryElement = null;
            // primaryCounter = false;
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
            this.transform.localPosition = manager.ReturnAssetCentreLocal();
            // Rotate 180 degrees over y-axis to align with asset target rotation
            // this.transform.localRotation = Quaternion.Euler(0, 180, 0);
            // Changed to asset manager to be rotated
        }

        public void ModifyMaterial() { }

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
        /// Adds <paramref name="element"/> as primary element.
        /// If null, then removes existing primary element.
        /// Primary element only makes sense to exist at one instant (step), otherwise always delete.
        /// </summary>
        /// <param name="element"></param>
        void AddPrimaryElement(GameObject element)
        {
            if (element == null)
            {
                if (primaryElement != null)
                {
                    Destroy(primaryElement);
                    // Find in list of loaded elements and remove
                    loadedElements.Remove(loadedElements.Find(x => x.Value == primaryElement));
                    // Assign primary element as null
                    primaryElement = null;
                    
                }
                else { }
            }
            else
            {
                if (primaryElement != null)
                {
                    Destroy(primaryElement);
                    loadedElements.Remove(loadedElements.Find(x => x.Value == primaryElement));
                }

                // Assign this element as primary
                primaryElement = element;

                // Replicate AddElement function
                if (lastElement != null) { lastElement.GetComponent<IVisualisable>().ModifyMaterial(); }
                element.transform.SetParent(this.transform, false);
                SetPosition(element, RtrbauElementLocation.Primary, 0);
                SetScale(element);
                lastElement = element;
            }
        }

        /// <summary>
        /// Adds <paramref name="element"/> to visualisation location (other than primary) in orderer position. 
        /// Deletes oldest element in location and element in primary location.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="location"></param>
        /// <param name="locationElements"></param>
        /// <param name="locationCounter"></param>
        void AddElement (GameObject element, RtrbauElementLocation location, ref List<GameObject> locationElements, ref int locationCounter)
        {
            // Calculate number of elements per location
            int locationElementsNo;
            if ((int)location != 0) { locationElementsNo = ((int)location * 2) + 2; }
            //if ((int)location != 0) { locationElementsNo = 5; }
            else { throw new ArgumentException("AddElement function should only be used for non-primary locations, please use AddElementPrimary"); }
            // else { locationElementsNo = 1; }
            // Should only be used for other locations rather than primary

            // Remove primary element
            AddPrimaryElement(null);

            // Remove earliest element on location list if location is full
            if (locationCounter == locationElementsNo) { locationCounter = 0; }
            if (locationElements.Count == locationElementsNo)
            {
                GameObject destroyable = locationElements[0];
                Destroy(destroyable);
                loadedElements.Remove(loadedElements.Find(x => x.Value == destroyable));
                locationElements.RemoveAt(0);
            }

            // Modify material of last element (in case it hasn't been the one which drove the new element)
            if (lastElement != null) { lastElement.GetComponent<IVisualisable>().ModifyMaterial(); }
            // Assign element to location
            locationElements.Add(element);
            // Assign element as child of visualiser
            element.transform.SetParent(this.transform, false);
            // Set position of element according to location
            SetPosition(element, location, locationCounter);
            // Set scale of element according to asset size
            SetScale(element);
            // Rotation is set in element according to viewers position
            // Assign element as last element
            lastElement = element;
            // Iterate over location element counter
            locationCounter++;
        }


        /// <summary>
        /// Sets position of element according to the visualiser position and assets size.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="location"></param>
        /// <param name="bounds"></param>
        /// <param name="locationCounter"></param>
        void SetPosition(GameObject element, RtrbauElementLocation location,  int locationCounter)
        {
            Bounds assetBounds = manager.ReturnAssetBoundsLocal();
            // Variables to calculate position:
            float pX;
            float pY;
            float pZ;

            if (location == RtrbauElementLocation.Primary)
            {
                // Only one position in this location
                pX = assetBounds.size.x;
                pY = 0;
                pZ = 0;
            }
            else if (location == RtrbauElementLocation.Secondary)
            {
                if (locationCounter < 3)
                {
                    pX = assetBounds.size.x - locationCounter * assetBounds.size.x;
                    pY = assetBounds.size.y * 2f;
                    pZ = 0;
                }
                else
                {
                    pX = -assetBounds.size.x;
                    pY = assetBounds.size.y * 2f;
                    pZ = -(locationCounter - 2) * assetBounds.size.z;
                }
            }
            else if (location == RtrbauElementLocation.Tertiary)
            {
                pX = assetBounds.size.x * 2;
                pY = assetBounds.size.y * 2f;
                pZ = -assetBounds.size.z * locationCounter;
            }
            else
            {
                throw new ArgumentException("Rtrbau Element Location not implemented.");
            }

            //if (location == RtrbauElementLocation.Primary)
            //{
            //    // Only one position in this location
            //    pX = assetBounds.size.x;
            //    pY = 0;
            //    pZ = 0;
            //}
            //else if (location == RtrbauElementLocation.Secondary)
            //{
            //    // Two positions in this location
            //    if (locationCounter == 0)
            //    {
            //        pX = assetBounds.size.x;
            //        pY = assetBounds.size.y;
            //        pZ = - assetBounds.size.z;
            //    }
            //    else if (locationCounter == 1)
            //    {
            //        pX = -assetBounds.size.x;
            //        pY = assetBounds.size.y;
            //        pZ = -assetBounds.size.z;
            //    }
            //    else
            //    {
            //        throw new ArgumentException("Number of location counter not implemented.");
            //    }
            //}
            //else if (location == RtrbauElementLocation.Tertiary)
            //{
            //    if (locationCounter == 0)
            //    {
            //        pX = assetBounds.size.x;
            //        pY = assetBounds.size.y;
            //        pZ = -2*assetBounds.size.z;
            //    }
            //    else if (locationCounter == 1)
            //    {
            //        pX = assetBounds.size.x;
            //        pY = 0;
            //        pZ = -2 * assetBounds.size.z;
            //    }
            //    else if (locationCounter == 2)
            //    {
            //        pX = -assetBounds.size.x;
            //        pY = assetBounds.size.y;
            //        pZ = -2 * assetBounds.size.z;
            //    }
            //    else if (locationCounter == 3)
            //    {
            //        pX = -assetBounds.size.x;
            //        pY = 0;
            //        pZ = -2 * assetBounds.size.z;
            //    }
            //    else
            //    {
            //        throw new ArgumentException("Number of location counter not implemented.");
            //    }
            //}
            //else
            //{
            //    throw new ArgumentException("Number of location elements not implemented.");
            //}

            Debug.Log("pX: " + pX + " pY: " + pY + " pZ: " + pZ);
            element.transform.localPosition = new Vector3(pX, pY, pZ);
        }

        /// <summary>
        /// /// <summary>
        /// Adapts <paramref name="element"/> scale (only x and y) to adapt to asset size.
        /// </summary>
        /// <param name="element"></param>
        void SetScale(GameObject element)
        {
            // Determine asset size through model bounds
            Bounds asset = manager.ReturnAssetBoundsLocal();

            // Re-scale element to match horizontal asset extents (x-axis)
            float sM = asset.extents.x / element.transform.localScale.x;
            float sX = element.transform.localScale.x * sM;
            float sY = element.transform.localScale.y * sM;
            float sZ = element.transform.localScale.z;

            element.transform.localScale = new Vector3(sX, sY, sZ);
        }

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
                AddPrimaryElement(element);
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
