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
        // List of loaded elements, key value pair for individual URI and relevant game object
        private List<KeyValuePair<OntologyElement,GameObject>> loadedElements;
        // private List<GameObject> createdElements;
        private GameObject lastElement;
        // There is only one primary element, so no need to hold a list of them
        private GameObject primaryElement;
        // private bool primaryCounter;
        // Key value pair for class URI and relevant game object
        private Dictionary<string,GameObject> secondaryElements;
        private int secondaryCounter;
        // Key value pair for class URI and relevant game object
        private Dictionary<string, GameObject> tertiaryElements;
        private int tertiaryCounter;
        // Key value pair for class URI and relevant game object
        private Dictionary<string, GameObject> quaternaryElements;
        private int quaternaryCounter;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public GameObject consultElement;
        public GameObject reportElement;
        public Material elementSeenMaterial;
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

            if (reportElement == null)
            { reportElement = Resources.Load("Rtrbau/Prefabs/Elements/Visualisations/ReportElement") as GameObject; }

            if (elementSeenMaterial == null)
            { elementSeenMaterial = Resources.Load("Rtrbau/Materials/RtrbauMaterialStandardGreyDark") as Material; }

            // Reference to the asset manager
            manager = assetManager;
            // Initialise variables
            loadedElements = new List<KeyValuePair<OntologyElement, GameObject>>();
            lastElement = null;
            primaryElement = null;
            // primaryCounter = false;
            secondaryElements = new Dictionary<string, GameObject>();
            secondaryCounter = 0;
            tertiaryElements = new Dictionary<string, GameObject>();
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
            RtrbauerEvents.StartListening("AssetVisualiser_LoadElement", LoadElement);
            RtrbauerEvents.StartListening("AssetVisualiser_LocateElement", LocateElement);
        }

        void OnDisable()
        {
            RtrbauerEvents.StopListening("AssetVisualiser_LoadElement", LoadElement);
            RtrbauerEvents.StopListening("AssetVisualiser_LocateElement", LocateElement);
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
            // Add asset visualiser to rtrbauer for control
            // Rtrbauer.instance.visualiser = this.gameObject;
        }

        public void ActivateIt() { }

        /// <summary>
        /// Identifies all <see cref="RtrbauElement"/> visualised and destroys them.
        /// Then reinitialises <see cref="AssetVisualiser"/> manager behaviour.
        /// Does not destroy <see cref="AssetVisualiser"/>.
        /// </summary>
        public void DestroyIt()
        {
            // Identify all rtrbau elements visualised and destroy
            if (primaryElement != null)
            {
                Destroy(primaryElement);
            }

            foreach (KeyValuePair<string,GameObject> element in secondaryElements)
            {
                Destroy(element.Value);
            }

            foreach (KeyValuePair<string, GameObject> element in tertiaryElements)
            {
                Destroy(element.Value);
            }

            // Initialise variables
            loadedElements = new List<KeyValuePair<OntologyElement, GameObject>>();
            lastElement = null;
            primaryElement = null;
            // primaryCounter = false;
            secondaryElements = new Dictionary<string, GameObject>();
            secondaryCounter = 0;
            tertiaryElements = new Dictionary<string, GameObject>();
            tertiaryCounter = 0;
        }

        public void ModifyMaterial(Material material) { }
        #endregion IVISUALISABLE_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        /// <summary>
        /// Adds <paramref name="element"/> as primary element.
        /// If null, then removes existing primary element.
        /// Primary element only makes sense to exist at one instant (step), otherwise always delete.
        /// </summary>
        /// <param name="element"></param>
        void AddPrimaryElement(GameObject element, RtrbauElementType type)
        {
            // If null, means primary element does not need re-assigment
            if (element == null)
            {
                if (primaryElement != null)
                {
                    // Confirm if primary element can be destroyed and do so
                    if (primaryElement.GetComponent<IElementable>().DestroyElement())
                    {
                        // Find in list of loaded elements and remove
                        loadedElements.Remove(loadedElements.Find(x => x.Value == primaryElement));
                        // Remove from primary location
                        primaryElement = null;
                    }
                    else
                    {
                        // If non-destroyable primary element is of report type
                        if (type == RtrbauElementType.Report)
                        {
                            // Move element from primary location to finalise location
                            // UPG: the idea is to create a fourth location where report elements can go to die when all nominates have created a new element report
                            // UPG: this requires an extra call for element reports to be unloaded when destroyed by theirselves because they run-out of nominates#

                        }
                        else
                        {
                            throw new ArgumentException("AssetVisualiser::AddPrimaryElement: this should never occurred.");
                        }
                    }
                    //// Destroy game object
                    //Destroy(primaryElement);
                    //// Find in list of loaded elements and remove
                    //loadedElements.Remove(loadedElements.Find(x => x.Value == primaryElement));
                    //// Remove from primary location
                    //primaryElement = null;
                }
                else { }
            }
            else
            {
                if (primaryElement != null)
                {
                    // Destroy game object
                    Destroy(primaryElement);
                    // Find in list of loaded elements and remove
                    loadedElements.Remove(loadedElements.Find(x => x.Value == primaryElement));
                }

                // Assign this element as primary
                primaryElement = element;
                // Alocate element to its position, scale and rotation
                SetElement(element, RtrbauElementLocation.Primary, 0);
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
        void AddElement (GameObject element, RtrbauElementType type, RtrbauElementLocation location, ref Dictionary<string,GameObject> locationElements, ref int locationCounter)
        {
            // Calculate number of elements per location
            int locationElementsNo;
            if ((int)location != 0) { locationElementsNo = ((int)location * 2) + 2; }
            else { throw new ArgumentException("AddElement function should only be used for non-primary locations, please use AddElementPrimary"); }

            // Remove primary element
            AddPrimaryElement(null, type);

            // Determine dictionary key according to element type
            string elementKey;
            // Check element type to return proper key
            if (type == RtrbauElementType.Consult) { elementKey = element.GetComponent<ElementConsult>().classElement.URL(); }
            // UPG: change for report?? when same class, always move previous to finalise (quaternary) location
            // UPG: when button complete operation, never allow to complete operation until all elements eliminated??
            else if (type == RtrbauElementType.Report) { elementKey = element.GetComponent<ElementReport>().classElement.URL(); }
            else { throw new ArgumentException("AssetVisualiser::AddElement: RtrbauElementType not implemented"); }

            // Remove element from same class if exist in same location
            GameObject elementSameClass;
            if (locationElements.TryGetValue(elementKey, out elementSameClass))
            {
                // Find class index to locate in location
                int classLocation = locationElements.Values.ToList().IndexOf(elementSameClass);
                // Destroy game object
                Destroy(elementSameClass);
                // Find in list of loaded elements and remove
                loadedElements.Remove(loadedElements.Find(x => x.Value == elementSameClass));
                // Update element of class in location
                locationElements[elementKey] = element;
                // Alocate element to its position, scale and rotation
                SetElement(element, location, classLocation);
            }
            else
            {
                // Remove earliest element on location list if location is full
                if (locationCounter == locationElementsNo) { locationCounter = 0; }
                if (locationElements.Count == locationElementsNo)
                {
                    GameObject firstElementLocation;
                    if (locationElements.TryGetValue(locationElements.Keys.First(), out firstElementLocation))
                    {
                        // Destroy game object
                        Destroy(firstElementLocation);
                        // Find in list of loaded elements and remove
                        loadedElements.Remove(loadedElements.Find(x => x.Value == firstElementLocation));
                        // Remove first element from location dicationary (may cause issues)
                        locationElements.Remove(locationElements.Keys.First());
                    }
                }
                // Assign element to location
                locationElements.Add(elementKey, element);
                // Alocate element to its position, scale and rotation
                SetElement(element, location, locationCounter);
                // Iterate over location element counter
                locationCounter++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="location"></param>
        /// <param name="locationCounter"></param>
        void SetElement(GameObject element, RtrbauElementLocation location, int locationCounter)
        {
            // Modify material of last element (in case it hasn't been the one which drove the new element)
            if (lastElement != null) { lastElement.GetComponent<IVisualisable>().ModifyMaterial(elementSeenMaterial); }
            // Assign element as child of visualiser
            element.transform.SetParent(this.transform, false);
            // Set position of element according to location
            SetPosition(element, location, locationCounter);
            // Set scale of element according to asset size
            SetScale(element);
            // Rotation is set in element according to viewers position
            // Assign element as last element
            lastElement = element;
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
            // But only in the case asset extents are bigger than element
            if (asset.extents.x > element.transform.localScale.x)
            {
                float sM = asset.extents.x / element.transform.localScale.x;
                float sX = element.transform.localScale.x * sM;
                float sY = element.transform.localScale.y * sM;
                float sZ = element.transform.localScale.z;

                element.transform.localScale = new Vector3(sX, sY, sZ);
            }
            else { }
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
        /// Creates new <see cref="RtrbauElement"/> of <see cref="RtrbauElementType"/>.
        /// If <paramref name="elementIndividual"/> found in list, then re-loads the <see cref="RtrbauElement"/>.
        /// </summary>
        /// <param name="elementIndividual"><see cref="OntologyElement"/> that refers to the ontology individual which the element represents</param>
        /// <param name="elementClass"><see cref="OntologyElement"/> that refers to the ontology class which the element represents</param>
        /// <param name="type"><see cref="RtrbauElementType"/> that determines the type of element to be created</param>
        void LoadElement(OntologyElement elementIndividual, OntologyElement elementClass, RtrbauElementType type)
        {
            Debug.Log("AssetVisualiser::LoadElement: element individual is: " + elementIndividual.entity.name);
            Debug.Log("AssetVisualiser::LoadElement: element class is: " + elementClass.entity.name);
            Debug.Log("AssetVisualiser::LoadElement: element loaded already: " + ElementLoaded(elementIndividual));

            // Find if individual element has already been loaded as rtrbau element
            if (!ElementLoaded(elementIndividual))
            {
                if (type == RtrbauElementType.Consult)
                {
                    GameObject newElement = GameObject.Instantiate(consultElement);
                    newElement.GetComponent<ElementConsult>().Initialise(this, elementIndividual, elementClass, lastElement);
                    loadedElements.Add(new KeyValuePair<OntologyElement, GameObject>(elementIndividual, newElement));
                }
                else if (type == RtrbauElementType.Report)
                {
                    GameObject newElement = GameObject.Instantiate(reportElement);
                    newElement.GetComponent<ElementReport>().Initialise(this, elementIndividual, elementClass, lastElement);
                    loadedElements.Add(new KeyValuePair<OntologyElement, GameObject>(elementIndividual, newElement));
                }
                else
                {
                    throw new ArgumentException("AssetVisualiser::LoadElement: RtrbauElementType not implemented");
                }
            }
            else
            {
                // UPG: ErrorHandling
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="location"></param>
        void LocateElement(GameObject element, RtrbauElementType type, RtrbauElementLocation location)
        {
            if (location == RtrbauElementLocation.Primary)
            {
                AddPrimaryElement(element, type);
            }
            else if (location == RtrbauElementLocation.Secondary)
            {
                AddElement(element, type, location, ref secondaryElements, ref secondaryCounter);
            }
            else if (location == RtrbauElementLocation.Tertiary)
            {
                AddElement(element, type, location, ref tertiaryElements, ref tertiaryCounter);
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