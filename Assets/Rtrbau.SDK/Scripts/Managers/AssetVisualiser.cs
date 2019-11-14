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
        // List of loaded elements classified by OntologyElement individual they represent
        // private Dictionary<OntologyElement,Tuple<RtrbauElementLocation,GameObject>> loadedElements;
        // private Dictionary<OntologyElement, GameObject> loadedElements;
        private List<Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>> loadedElements;
        // Elements primary location
        RtrbauLocation primaryElements;
        // Elements secondary location
        RtrbauLocation secondaryElements;
        // Elements tertiary location
        RtrbauLocation tertiaryElements;
        // Elements quaternary location
        RtrbauLocation quaternaryElements;
        // Reference to last allocated game object element
        private GameObject lastElement;
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
            // Initialise visualiser manager
            manager = assetManager;

            // Initialise visualiser prefabs
            if (consultElement == null)
            { consultElement = Resources.Load("Rtrbau/Prefabs/Elements/Visualisations/ConsultElement") as GameObject; }
            if (reportElement == null)
            { reportElement = Resources.Load("Rtrbau/Prefabs/Elements/Visualisations/ReportElement") as GameObject; }
            if (elementSeenMaterial == null)
            { elementSeenMaterial = Resources.Load("Rtrbau/Materials/RtrbauMaterialStandardGreyDark") as Material; }

            // Initialise visualiser variables
            InitialiseVisualiserVariables();

            // Locate visualiser
            LocateIt();
        }
        #endregion INITIALISATION_METHODS

        #region MONOBEHAVIOUR_METHODS
        void Start() { }

        void Update() { }

        void OnEnable()
        {
            RtrbauerEvents.StartListening("AssetVisualiser_CreateElement", CreateElement);
            RtrbauerEvents.StartListening("AssetVisualiser_LocateElement", LocateElement);
        }

        void OnDisable()
        {
            RtrbauerEvents.StopListening("AssetVisualiser_CreateElement", CreateElement);
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
            foreach (Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element in loadedElements)
            {
                Destroy(element.Item3);
            }

            // Reinitialise visualiser variables
            InitialiseVisualiserVariables();
        }

        public void ModifyMaterial(Material material) { }
        #endregion IVISUALISABLE_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        void InitialiseVisualiserVariables()
        {
            loadedElements = new List<Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>>();
            primaryElements = new RtrbauLocation(RtrbauElementLocation.Primary, this);
            secondaryElements = new RtrbauLocation(RtrbauElementLocation.Secondary, this);
            tertiaryElements = new RtrbauLocation(RtrbauElementLocation.Tertiary, this);
            quaternaryElements = new RtrbauLocation(RtrbauElementLocation.Quaternary, this);
            lastElement = null;
        }
        ///// <summary>
        ///// Adds <paramref name="element"/> as primary element.
        ///// If null, then removes existing primary element.
        ///// Primary element only makes sense to exist at one instant (step), otherwise always delete.
        ///// </summary>
        ///// <param name="element"></param>
        //void AddPrimaryElement(GameObject element, RtrbauElementType type)
        //{
        //    // If null, means primary element does not need re-assigment
        //    if (element == null)
        //    {
        //        if (primaryElement != null)
        //        {
        //            // Confirm if primary element can be destroyed and do so
        //            if (primaryElement.GetComponent<IElementable>().DestroyElement())
        //            {
        //                // Find in list of loaded elements and remove
        //                //loadedElements.Remove(loadedElements.Find(x => x.Value. == primaryElement));
        //                loadedElements.Remove(loadedElements.Find(x => x.Item2 == primaryElement));
        //                // Remove from primary location
        //                primaryElement = null;
        //            }
        //            else
        //            {
        //                // If non-destroyable primary element is of report type
        //                if (type == RtrbauElementType.Report)
        //                {
        //                    // Move element from primary location to finalise location
        //                    // UPG: the idea is to create a fourth location where report elements can go to die when all nominates have created a new element report
        //                    // UPG: this requires an extra call for element reports to be unloaded when destroyed by theirselves because they run-out of nominates#

        //                }
        //                else
        //                {
        //                    throw new ArgumentException("AssetVisualiser::AddPrimaryElement: this should never occurred.");
        //                }
        //            }
        //            //// Destroy game object
        //            //Destroy(primaryElement);
        //            //// Find in list of loaded elements and remove
        //            //loadedElements.Remove(loadedElements.Find(x => x.Value == primaryElement));
        //            //// Remove from primary location
        //            //primaryElement = null;
        //        }
        //        else { }
        //    }
        //    else
        //    {
        //        if (primaryElement != null)
        //        {
        //            // Destroy game object
        //            Destroy(primaryElement);
        //            // Find in list of loaded elements and remove
        //            // loadedElements.Remove(loadedElements.Find(x => x.Value == primaryElement));
        //            loadedElements.Remove(loadedElements.Find(x => x.Item2 == primaryElement));
        //        }

        //        // Assign this element as primary
        //        primaryElement = element;
        //        // Alocate element to its position, scale and rotation
        //        SetElement(element, RtrbauElementLocation.Primary, 0);
        //    }
        //}

        ///// <summary>
        ///// Adds <paramref name="element"/> to visualisation location (other than primary) in orderer position. 
        ///// Deletes oldest element in location and element in primary location.
        ///// </summary>
        ///// <param name="element"></param>
        ///// <param name="location"></param>
        ///// <param name="locationElements"></param>
        ///// <param name="locationCounter"></param>
        //void AddElement (GameObject element, RtrbauElementType type, RtrbauElementLocation location, ref Dictionary<string,GameObject> locationElements, ref int locationCounter)
        //{
        //    // Calculate number of elements per location
        //    int locationElementsNo;
        //    if ((int)location != 0) { locationElementsNo = ((int)location * 2) + 2; }
        //    else { throw new ArgumentException("AddElement function should only be used for non-primary locations, please use AddElementPrimary"); }

        //    // Remove primary element
        //    AddPrimaryElement(null, type);

        //    // Determine dictionary key according to element type
        //    string elementKey;
        //    // Check element type to return proper key
        //    if (type == RtrbauElementType.Consult) { elementKey = element.GetComponent<ElementConsult>().classElement.URL(); }
        //    // UPG: change for report?? when same class, always move previous to finalise (quaternary) location
        //    // UPG: when button complete operation, never allow to complete operation until all elements eliminated??
        //    else if (type == RtrbauElementType.Report) { elementKey = element.GetComponent<ElementReport>().classElement.URL(); }
        //    else { throw new ArgumentException("AssetVisualiser::AddElement: RtrbauElementType not implemented"); }

        //    // Remove element from same class if exist in same location
        //    GameObject elementSameClass;
        //    if (locationElements.TryGetValue(elementKey, out elementSameClass))
        //    {
        //        // Find class index to locate in location
        //        int classLocation = locationElements.Values.ToList().IndexOf(elementSameClass);
        //        // Destroy game object
        //        Destroy(elementSameClass);
        //        // Find in list of loaded elements and remove
        //        // loadedElements.Remove(loadedElements.Find(x => x.Value == elementSameClass));
        //        loadedElements.Remove(loadedElements.Find(x => x.Item2 == elementSameClass));
        //        // Update element of class in location
        //        locationElements[elementKey] = element;
        //        // Alocate element to its position, scale and rotation
        //        SetElement(element, location, classLocation);
        //    }
        //    else
        //    {
        //        // Remove earliest element on location list if location is full
        //        if (locationCounter == locationElementsNo) { locationCounter = 0; }
        //        if (locationElements.Count == locationElementsNo)
        //        {
        //            GameObject firstElementLocation;
        //            if (locationElements.TryGetValue(locationElements.Keys.First(), out firstElementLocation))
        //            {
        //                // Destroy game object
        //                Destroy(firstElementLocation);
        //                // Find in list of loaded elements and remove
        //                // loadedElements.Remove(loadedElements.Find(x => x.Value == firstElementLocation));
        //                loadedElements.Remove(loadedElements.Find(x => x.Item2 == firstElementLocation));
        //                // Remove first element from location dicationary (may cause issues)
        //                locationElements.Remove(locationElements.Keys.First());
        //            }
        //        }
        //        // Assign element to location
        //        locationElements.Add(elementKey, element);
        //        // Alocate element to its position, scale and rotation
        //        SetElement(element, location, locationCounter);
        //        // Iterate over location element counter
        //        locationCounter++;
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="element"></param>
        ///// <param name="location"></param>
        ///// <param name="locationCounter"></param>
        //void SetElement(GameObject element, RtrbauElementLocation location, int locationCounter)
        //{
        //    // Modify material of last element (in case it hasn't been the one which drove the new element)
        //    if (lastElement != null) { lastElement.GetComponent<IVisualisable>().ModifyMaterial(elementSeenMaterial); }
        //    // Assign element as child of visualiser
        //    element.transform.SetParent(this.transform, false);
        //    // Set position of element according to location
        //    SetPosition(element, location, locationCounter);
        //    // Set scale of element according to asset size
        //    SetScale(element);
        //    // Rotation is set in element according to viewers position
        //    // Assign element as last element
        //    lastElement = element;
        //}


        ///// <summary>
        ///// Sets position of element according to the visualiser position and assets size.
        ///// </summary>
        ///// <param name="element"></param>
        ///// <param name="location"></param>
        ///// <param name="bounds"></param>
        ///// <param name="locationCounter"></param>
        //void SetPosition(GameObject element, RtrbauElementLocation location,  int locationCounter)
        //{
        //    Bounds assetBounds = manager.ReturnAssetBoundsLocal();
        //    // Variables to calculate position:
        //    float pX;
        //    float pY;
        //    float pZ;

        //    if (location == RtrbauElementLocation.Primary)
        //    {
        //        // Only one position in this location
        //        pX = assetBounds.size.x;
        //        pY = 0;
        //        pZ = 0;
        //    }
        //    else if (location == RtrbauElementLocation.Secondary)
        //    {
        //        if (locationCounter < 3)
        //        {
        //            pX = assetBounds.size.x - locationCounter * assetBounds.size.x;
        //            pY = assetBounds.size.y * 2f;
        //            pZ = 0;
        //        }
        //        else
        //        {
        //            pX = -assetBounds.size.x;
        //            pY = assetBounds.size.y * 2f;
        //            pZ = -(locationCounter - 2) * assetBounds.size.z;
        //        }
        //    }
        //    else if (location == RtrbauElementLocation.Tertiary)
        //    {
        //        pX = assetBounds.size.x * 2;
        //        pY = assetBounds.size.y * 2f;
        //        pZ = -assetBounds.size.z * locationCounter;
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Rtrbau Element Location not implemented.");
        //    }

        //    Debug.Log("pX: " + pX + " pY: " + pY + " pZ: " + pZ);
        //    element.transform.localPosition = new Vector3(pX, pY, pZ);
        //}

        ///// <summary>
        ///// /// <summary>
        ///// Adapts <paramref name="element"/> scale (only x and y) to adapt to asset size.
        ///// </summary>
        ///// <param name="element"></param>
        //void SetScale(GameObject element)
        //{
        //    // Determine asset size through model bounds
        //    Bounds asset = manager.ReturnAssetBoundsLocal();

        //    // Re-scale element to match horizontal asset extents (x-axis)
        //    // But only in the case asset extents are bigger than element
        //    if (asset.extents.x > element.transform.localScale.x)
        //    {
        //        float sM = asset.extents.x / element.transform.localScale.x;
        //        float sX = element.transform.localScale.x * sM;
        //        float sY = element.transform.localScale.y * sM;
        //        float sZ = element.transform.localScale.z;

        //        element.transform.localScale = new Vector3(sX, sY, sZ);
        //    }
        //    else { }
        //}
        //RtrbauElementLocation ElementLoadedLocation(OntologyElement elementIndividual)
        //{
        //    int elementIndex = loadedElements.FindIndex(x => x.Item1.Equals(elementIndividual));

        //    foreach (Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element in loadedElements)
        //    {
        //        Debug.Log("AssetVisualiser::ElementLoadedLocation: loaded element is " + element.Item1.entity.name);
        //    }

        //    if (elementIndex != -1) { return loadedElements[elementIndex].Item4; }
        //    else { throw new ArgumentException("AssetVisualiser::ElementLoadedLocation: Element " + elementIndividual.entity.Entity() + " not loaded yet."); }
        //}
        #endregion PRIVATE

        #region PUBLIC
        /// <summary>
        /// Returns gameobject corresponding to <see cref="IElementable"/> <paramref name="elementIndividual"/>.
        /// If not found, returns null.
        /// </summary>
        /// <param name="elementIndividual"></param>
        /// <returns></returns>
        public GameObject FindElement(OntologyElement elementIndividual)
        {
            int elementIndex = loadedElements.FindIndex(x => x.Item1.EqualElement(elementIndividual));

            if (elementIndex != -1) { return loadedElements[elementIndex].Item3; }
            else { return null; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadedElement"></param>
        /// <returns></returns>
        public bool ReloadElement(Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> loadedElement)
        {
            int elementIndex = loadedElements.FindIndex(x => x.Item1.EqualElement(loadedElement.Item1));

            if (elementIndex != -1)
            {
                loadedElements[elementIndex] = loadedElement;
                return true;
            }
            else { throw new ArgumentException("AssetVisualiser::LoadElement: Element not created yet."); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementObject"></param>
        public bool UnloadElement(GameObject elementObject)
        {
            if (elementObject != null)
            {
                // int elementIndex = loadedElements.FindIndex(x => x.Item3.GetInstanceID() == elementObject.GetInstanceID());
                int elementIndex = loadedElements.FindIndex(x => x.Item3.Equals(elementObject));

                if (elementIndex != -1)
                {
                    // Remove element from location
                    if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.Primary) { primaryElements.RemoveElement(elementObject); }
                    else if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.Secondary) { secondaryElements.RemoveElement(elementObject); }
                    else if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.Tertiary) { tertiaryElements.RemoveElement(elementObject); }
                    else if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.Quaternary) { quaternaryElements.RemoveElement(elementObject); }
                    else { throw new ArgumentException("AssetVisualiser::UnloadElement: Element " + loadedElements[elementIndex].Item1.entity.Entity() + " not loaded yet."); }
                    // Destroy elementObject
                    if (elementObject.GetComponent<IElementable>().DestroyElement()) { }
                    else { throw new ArgumentException("AssetVisualiser::UnloadElement: IElementable object should always be destroyable."); }
                    // Remove element from loadedElements
                    loadedElements.RemoveAt(elementIndex);
                    // Element unloaded and destroyed, return true
                    DebugElements("LocateElement_" + loadedElements[elementIndex].Item1.entity.name);
                    return true;
                }
                else { throw new ArgumentException("AssetVisualiser::UnloadElement: Element not created/loaded yet."); }
            }
            else { return false; }

            
            // loadedElements.Remove(loadedElements.Find(x => x.Item2 == element));
            // Remove from location
            // Remove from loaded list
            // Destroy
        }
        #endregion PUBLIC

        #region EVENTS
        /// <summary>
        /// Creates new <see cref="RtrbauElement"/> of <see cref="RtrbauElementType"/>.
        /// If <paramref name="elementIndividual"/> found in list, then re-loads the <see cref="IElementable"/>.
        /// </summary>
        /// <param name="elementIndividual"><see cref="OntologyElement"/> that refers to the ontology individual which the element represents</param>
        /// <param name="elementClass"><see cref="OntologyElement"/> that refers to the ontology class which the element represents</param>
        /// <param name="type"><see cref="RtrbauElementType"/> that determines the type of element to be created</param>
        void CreateElement(OntologyElement elementIndividual, OntologyElement elementClass, RtrbauElementType type)
        {
            Debug.Log("AssetVisualiser::LoadElement: element individual is: " + elementIndividual.entity.name);
            Debug.Log("AssetVisualiser::LoadElement: element class is: " + elementClass.entity.name);
            DebugElements("LocateElement_" + elementIndividual.entity.name);

            // Identify IElementable game object thar represents the element to load
            int elementIndex = loadedElements.FindIndex(x => x.Item1.EqualElement(elementIndividual));

            // If IElementable game object not found, use RtrbauElementType to decide which IElementable to load
            if (elementIndex == -1)
            {
                if (type == RtrbauElementType.Consult)
                {
                    // Instantiate new GameObject for rtrbauElement
                    GameObject elementObject = GameObject.Instantiate(consultElement);
                    // Create new Tuple for loadedElements List
                    Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element = new Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>(elementIndividual, elementClass, elementObject, RtrbauElementLocation.None);
                    // Add to loadedElements list
                    loadedElements.Add(element);
                    // Initialise IElemenable GameObject referenced by elementObject
                    // UPG: create an IElementable function to initialise
                    elementObject.GetComponent<ElementConsult>().Initialise(this, elementIndividual, elementClass, lastElement);
                    
                    Debug.Log("AssetVisualiser::LoadElement: Element " + elementIndividual.entity.name + " loaded.");
                }
                else if (type == RtrbauElementType.Report)
                {
                    // Instantiate new GameObject for rtrbauElement
                    GameObject elementObject = GameObject.Instantiate(reportElement);
                    // Create new Tuple for loadedElements List
                    Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element = new Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>(elementIndividual, elementClass, elementObject, RtrbauElementLocation.None);
                    // Add to loadedElements list
                    loadedElements.Add(element);
                    // Initialise IElemenable GameObject referenced by elementObject
                    // UPG: create an IElementable function to initialise
                    elementObject.GetComponent<ElementReport>().Initialise(this, elementIndividual, elementClass, lastElement);

                    Debug.Log("AssetVisualiser::LoadElement: Element " + elementIndividual.entity.name + " loaded.");
                }
                else
                {
                    throw new ArgumentException("AssetVisualiser::LoadElement: RtrbauElementType " + type.ToString() + "not implemented");
                }
            }
            else
            {
                // UPG: ErrorHandling
                Debug.Log("AssetVisualiser::LoadElement: IElementable for " + elementIndividual.entity.name + "loaded already");
            }

            Debug.Log("AssetVisualiser::LoadElement: Number of elements loaded is: " + loadedElements.Count());
            DebugElements("LocateElement_" + elementIndividual.entity.name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementObject"></param>
        /// <param name="elementType"></param>
        /// <param name="elementLocation"></param>
        void LocateElement(OntologyElement elementIndividual, RtrbauElementType elementType, RtrbauElementLocation elementLocation)
        {
            int elementIndex = loadedElements.FindIndex(x => x.Item1.EqualElement(elementIndividual));

            DebugElements("LocateElement_" + elementIndividual.entity.name);

            if (elementIndex != -1)
            {

                OntologyElement elementClass = loadedElements[elementIndex].Item2;
                GameObject elementObject = loadedElements[elementIndex].Item3;

                if (elementLocation == RtrbauElementLocation.Primary)
                {
                    if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.None)
                    {
                        if (lastElement != null) { lastElement.GetComponent<IVisualisable>().ModifyMaterial(elementSeenMaterial); }
                        elementObject.transform.SetParent(this.transform, false);
                        lastElement = elementObject;

                        UnloadElement(primaryElements.FindFirstElement());
                        primaryElements.AddElement(elementClass, elementObject);
                        Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element = new Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>(elementIndividual, elementClass, elementObject, RtrbauElementLocation.Primary);
                        ReloadElement(element);
                    }
                    else { throw new ArgumentException("AssetVisualiser::LocateElement: Element " + elementIndividual.entity.Entity() + " already loaded."); }
                }
                else if (elementLocation == RtrbauElementLocation.Secondary)
                {
                    if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.None)
                    {
                        if (lastElement != null) { lastElement.GetComponent<IVisualisable>().ModifyMaterial(elementSeenMaterial); }
                        elementObject.transform.SetParent(this.transform, false);
                        lastElement = elementObject;

                        UnloadElement(primaryElements.FindFirstElement());
                        secondaryElements.AddElement(elementClass, elementObject);
                        Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element = new Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>(elementIndividual, elementClass, elementObject, RtrbauElementLocation.Secondary);
                        ReloadElement(element);
                    }
                    else { throw new ArgumentException("AssetVisualiser::LocateElement: Element " + elementIndividual.entity.Entity() + " already loaded."); }
                }
                else if (elementLocation == RtrbauElementLocation.Tertiary)
                {
                    if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.None)
                    {
                        if (lastElement != null) { lastElement.GetComponent<IVisualisable>().ModifyMaterial(elementSeenMaterial); }
                        elementObject.transform.SetParent(this.transform, false);
                        lastElement = elementObject;

                        UnloadElement(primaryElements.FindFirstElement());
                        tertiaryElements.AddElement(elementClass, elementObject);
                        Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element = new Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>(elementIndividual, elementClass, elementObject, RtrbauElementLocation.Tertiary);
                        ReloadElement(element);
                    }
                    else { throw new ArgumentException("AssetVisualiser::LocateElement: Element " + elementIndividual.entity.Entity() + " already loaded."); }
                }
                else if (elementLocation == RtrbauElementLocation.Quaternary)
                {
                    if (elementType == RtrbauElementType.Report)
                    {
                        Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element = new Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>(elementIndividual, elementClass, elementObject, RtrbauElementLocation.Quaternary);

                        if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.Primary)
                        {
                            primaryElements.RemoveElement(elementClass);
                            quaternaryElements.AddElement(elementClass, elementObject);
                            loadedElements[elementIndex] = element;
                        }
                        else if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.Secondary)
                        {
                            secondaryElements.RemoveElement(elementClass);
                            quaternaryElements.AddElement(elementClass, elementObject);
                            loadedElements[elementIndex] = element;
                        }
                        else if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.Tertiary)
                        {
                            tertiaryElements.RemoveElement(elementClass);
                            quaternaryElements.AddElement(elementClass, elementObject);
                            loadedElements[elementIndex] = element;
                        }
                        else if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.Quaternary) { throw new ArgumentException("AssetVisualiser::LocateElement: Element already loaded."); }
                        else if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.None) { throw new ArgumentException("AssetVisualiser::LocateElement: Element not loaded yet."); }
                        else { throw new ArgumentException("AssetVisualiser::LocateElement: RtrbauElementLocation " + elementLocation.ToString() + " not implemented"); }
                    }
                    else if (elementType == RtrbauElementType.Consult) { throw new ArgumentException("AssetVisualiser::LocateElement: RtrbauLocation " + elementLocation.ToString() + " not implented for RtrbauElementType " + elementType.ToString()); }
                    else { throw new ArgumentException("AssetVisualiser::LocateElement: RtrbauElementType " + elementType.ToString() + " not implemented"); }
                }
                else
                { throw new ArgumentException("AssetVisualiser::LocateElement: RtrbauElementLocation " + elementLocation.ToString() + " not implemented"); }

            }
            else { Debug.Log("AssetVisualiser::LocateElement: Element not loaded yet."); }

            DebugElements("LocateElement_" + elementIndividual.entity.name);
        }
        #endregion EVENTS
        #endregion CLASS_METHODS

        #region DEBUG_METHODS
        void DebugElements(string functionName)
        {
            foreach (Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element in loadedElements)
            {
                Debug.Log("AssetVisualiser::" + functionName + "::DebugElements: loaded element is " + element.Item1.entity.name);
            }

            primaryElements.DebugLocationElements(functionName);
            secondaryElements.DebugLocationElements(functionName);
            tertiaryElements.DebugLocationElements(functionName);
            quaternaryElements.DebugLocationElements(functionName);
        }
        #endregion DEBUG_METHODS
    }
}