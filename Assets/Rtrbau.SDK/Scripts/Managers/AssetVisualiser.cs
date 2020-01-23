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
        // Mode of elements visualisation;
        public RtrbauElementMode elementsMode;
        // List of loaded elements classified by OntologyElement individual they represent
        // private Dictionary<OntologyElement,Tuple<RtrbauElementLocation,GameObject>> elementsLoaded;
        // private Dictionary<OntologyElement, GameObject> elementsLoaded;
        private List<Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>> elementsLoaded;
        // List of elements locations
        private List<RtrbauLocation> elementsLocations;
        //// Elements primary location
        //RtrbauLocation primaryElements;
        //// Elements secondary location
        //RtrbauLocation secondaryElements;
        //// Elements tertiary location
        //RtrbauLocation tertiaryElements;
        //// Elements quaternary location
        //RtrbauLocation quaternaryElements;
        // Reference to last allocated game object element
        private GameObject elementLast;
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
        public void Initialise(AssetManager assetManager, GameObject elementConsult, GameObject elementReport, Material elementMaterial)
        {
            // Initialise visualiser manager
            manager = assetManager;
            // Initialise visualiser prefabs
            consultElement = elementConsult;
            reportElement = elementReport;
            elementSeenMaterial = elementMaterial;
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
            foreach (Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element in elementsLoaded)
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
            // Initialise loaded elements managers
            elementsMode = CalculateElementsVisualisationMode(manager);
            elementsLoaded = new List<Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>>();
            elementsLocations = new List<RtrbauLocation>();
            elementLast = null;
            
            // Initialise elements locations managers
            if (elementsMode == RtrbauElementMode.Exterior)
            {
                elementsLocations.Add(new RtrbauLocation(RtrbauElementLocation.Primary, this));
                elementsLocations.Add(new RtrbauLocation(RtrbauElementLocation.Secondary, this));
                elementsLocations.Add(new RtrbauLocation(RtrbauElementLocation.Tertiary, this));
                elementsLocations.Add(new RtrbauLocation(RtrbauElementLocation.Quaternary, this));
            }
            else if (elementsMode == RtrbauElementMode.Interior)
            {
                elementsLocations.Add(new RtrbauLocation(RtrbauElementLocation.Primary, this));
                elementsLocations.Add(new RtrbauLocation(RtrbauElementLocation.Quaternary, this));
            }
            else
            {
                throw new ArgumentException("AssetVisualiser::InitialiseVisualiserVariables: RtrbauElementMode not implemented yet.");
            }

            //primaryElements = new RtrbauLocation(RtrbauElementLocation.Primary, this);
            //secondaryElements = new RtrbauLocation(RtrbauElementLocation.Secondary, this);
            //tertiaryElements = new RtrbauLocation(RtrbauElementLocation.Tertiary, this);
            //quaternaryElements = new RtrbauLocation(RtrbauElementLocation.Quaternary, this);
        }

        RtrbauElementMode CalculateElementsVisualisationMode(AssetManager assetManager)
        {
            Bounds assetBounds = assetManager.ReturnAssetBoundsLocal();
            // If asset bounds are bigger than 1.5 meters, then elements visualisation is interior
            if (assetBounds.size.x > 1.5f || assetBounds.size.y > 1.5f || assetBounds.size.z > 1.5f) { return RtrbauElementMode.Interior; }
            else { return RtrbauElementMode.Exterior; }
        }
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
            int elementIndex = elementsLoaded.FindIndex(x => x.Item1.EqualElement(elementIndividual));

            if (elementIndex != -1) { return elementsLoaded[elementIndex].Item3; }
            else { return null; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadedElement"></param>
        /// <returns></returns>
        public bool ReloadElement(Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> loadedElement)
        {
            int elementIndex = elementsLoaded.FindIndex(x => x.Item1.EqualElement(loadedElement.Item1));

            if (elementIndex != -1)
            {
                elementsLoaded[elementIndex] = loadedElement;
                return true;
            }
            else { throw new ArgumentException("AssetVisualiser::LoadElement: Element not created yet."); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementObject"></param>
        public bool UnloadElement(OntologyElement elementClass, GameObject elementObject)
        {
            if (elementObject != null)
            {
                // GameObjects for elementsLoaded should always exist
                int elementIndex = elementsLoaded.FindIndex(x => x.Item3.Equals(elementObject));

                if (elementIndex != -1)
                {
                    // Remove element from location in case it is not quaternary
                    if (elementsMode == RtrbauElementMode.Exterior)
                    {
                        if (elementObject.GetComponent<ElementConsult>() != null || elementObject.GetComponent<ElementReport>() != null)
                        {
                            if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Primary) { elementsLocations[0].RemoveElement(elementClass); }
                            else if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Secondary) { elementsLocations[1].RemoveElement(elementClass); }
                            else if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Tertiary) { elementsLocations[2].RemoveElement(elementClass); }
                            else if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Quaternary) { elementsLocations[3].RemoveElement(elementClass); }
                            else { throw new ArgumentException("AssetVisualiser::UnloadElement: Element " + elementsLoaded[elementIndex].Item1.entity.Entity() + " not loaded yet."); }
                        }
                        else { throw new ArgumentException("AssetVisualiser::UnloadElement: RtrbauElementType not implemented."); }
                    }
                    else if (elementsMode == RtrbauElementMode.Interior)
                    {
                        if (elementObject.GetComponent<ElementConsult>() != null)
                        {
                            if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Primary) { elementsLocations[0].RemoveElement(elementClass); }
                            else if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Secondary) { elementsLocations[1].RemoveElement(elementClass); }
                            else if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Tertiary) { elementsLocations[2].RemoveElement(elementClass); }
                            else if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Quaternary) { throw new ArgumentException("AssetVisualiser::UnloadElement: Quaternary location should not be accesible for ElementConsult."); }
                            else { throw new ArgumentException("AssetVisualiser::UnloadElement: Element " + elementsLoaded[elementIndex].Item1.entity.Entity() + " not loaded yet."); }
                        }
                        else if (elementObject.GetComponent<ElementReport>() != null)
                        {
                            if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Primary) { elementsLocations[0].RemoveElement(elementClass); }
                            else if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Secondary) { elementsLocations[0].RemoveElement(elementClass); }
                            else if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Tertiary) { elementsLocations[0].RemoveElement(elementClass); }
                            else if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Quaternary) { elementsLocations[1].RemoveElement(elementClass); }
                            else { throw new ArgumentException("AssetVisualiser::UnloadElement: Element " + elementsLoaded[elementIndex].Item1.entity.Entity() + " not loaded yet."); }
                        }
                        else { throw new ArgumentException("AssetVisualiser::UnloadElement: RtrbauElementType not implemented."); }
                    }
                    else
                    {
                        throw new ArgumentException("AssetVisualiser::UnloadElement: RtrbauElementMode not implemented yet.");
                    }

                    //if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Primary) { primaryElements.RemoveElement(elementClass); }
                    //else if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Secondary) { secondaryElements.RemoveElement(elementClass); }
                    //else if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Tertiary) { tertiaryElements.RemoveElement(elementClass); }
                    //else if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Quaternary) { quaternaryElements.RemoveElement(elementClass); }
                    //else { throw new ArgumentException("AssetVisualiser::UnloadElement: Element " + elementsLoaded[elementIndex].Item1.entity.Entity() + " not loaded yet."); }

                    // Destroy elementObject
                    if (elementObject.GetComponent<IElementable>().DestroyElement()) { }
                    else { throw new ArgumentException("AssetVisualiser::UnloadElement: IElementable object should always be destroyable."); }
                    // Remove element from elementsLoaded
                    elementsLoaded.RemoveAt(elementIndex);
                    // Element unloaded and destroyed, return true
                    DebugElements("LocateElement_" + elementsLoaded[elementIndex].Item1.entity.Name());
                    return true;
                }
                else { throw new ArgumentException("AssetVisualiser::UnloadElement: Element not created/loaded yet."); }
            }
            else { return false; }

            // elementsLoaded.Remove(elementsLoaded.Find(x => x.Item2 == element));
            // Remove from location
            // Destroy elementObject
            // Remove from loaded list
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
            Debug.Log("AssetVisualiser::LoadElement: element individual is: " + elementIndividual.entity.Name());
            Debug.Log("AssetVisualiser::LoadElement: element class is: " + elementClass.entity.Name());
            DebugElements("LocateElement_" + elementIndividual.entity.Name());

            // Identify IElementable game object that represents the element to load
            int elementIndex = elementsLoaded.FindIndex(x => x.Item1.EqualElement(elementIndividual));

            // If IElementable game object not found, use RtrbauElementType to decide which IElementable to load
            if (elementIndex == -1)
            {
                if (type == RtrbauElementType.Consult)
                {
                    // Instantiate new GameObject for rtrbauElement
                    GameObject elementObject = GameObject.Instantiate(consultElement);
                    // Create new Tuple for elementsLoaded List
                    Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element = new Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>(elementIndividual, elementClass, elementObject, RtrbauElementLocation.None);
                    // Add to elementsLoaded list
                    elementsLoaded.Add(element);
                    // Initialise IElemenable GameObject referenced by elementObject
                    // UPG: create an IElementable function to initialise
                    elementObject.GetComponent<ElementConsult>().Initialise(this, elementIndividual, elementClass, elementLast);
                    
                    Debug.Log("AssetVisualiser::LoadElement: Element " + elementIndividual.entity.Name() + " loaded.");
                }
                else if (type == RtrbauElementType.Report)
                {
                    // Instantiate new GameObject for rtrbauElement
                    GameObject elementObject = GameObject.Instantiate(reportElement);
                    // Create new Tuple for elementsLoaded List
                    Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element = new Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>(elementIndividual, elementClass, elementObject, RtrbauElementLocation.None);
                    // Add to elementsLoaded list
                    elementsLoaded.Add(element);
                    // Initialise IElemenable GameObject referenced by elementObject
                    // UPG: create an IElementable function to initialise
                    elementObject.GetComponent<ElementReport>().Initialise(this, elementIndividual, elementClass, elementLast);

                    Debug.Log("AssetVisualiser::LoadElement: Element " + elementIndividual.entity.Name() + " loaded.");
                }
                else { throw new ArgumentException("AssetVisualiser::LoadElement: RtrbauElementType " + type.ToString() + "not implemented"); }
            }
            else
            {
                // UPG: ErrorHandling
                Debug.Log("AssetVisualiser::LoadElement: IElementable for " + elementIndividual.entity.Name() + "loaded already");
            }

            Debug.Log("AssetVisualiser::LoadElement: Number of elements loaded is: " + elementsLoaded.Count());
            DebugElements("LocateElement_" + elementIndividual.entity.Name());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementIndivididual"></param>
        /// <param name="elementType"></param>
        /// <param name="elementLocation"></param>
        void LocateElement(OntologyElement elementIndividual, RtrbauElementType elementType, RtrbauElementLocation elementLocation)
        {
            int elementIndex = elementsLoaded.FindIndex(x => x.Item1.EqualElement(elementIndividual));

            DebugElements("LocateElement_" + elementIndividual.entity.Name());

            if (elementIndex != -1)
            {
                OntologyElement elementClass = elementsLoaded[elementIndex].Item2;
                GameObject elementObject = elementsLoaded[elementIndex].Item3;

                if (elementsMode == RtrbauElementMode.Exterior)
                {
                    if (elementLocation == RtrbauElementLocation.Primary)
                    {
                        if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.None)
                        {
                            // Modify material of elementLast to seen
                            if (elementLast != null) { elementLast.GetComponent<IVisualisable>().ModifyMaterial(elementSeenMaterial); }
                            // Unparent elementObject from previous location
                            elementObject.transform.SetParent(this.transform, false);
                            // Set elementObject as elementLast
                            elementLast = elementObject;
                            // Unload and destroy element from primary location
                            // KeyValuePair<OntologyElement, GameObject> primaryFirstElement = primaryElements.FindFirstElement();
                            KeyValuePair<OntologyElement, GameObject> primaryFirstElement = elementsLocations[0].FindFirstElement();
                            UnloadElement(primaryFirstElement.Key, primaryFirstElement.Value);
                            // Add element to elementLocation
                            // primaryElements.AddElement(elementClass, elementObject);
                            elementsLocations[0].AddElement(elementClass, elementObject);
                            // Set loadedElement to new location (ReloadElement)
                            Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element = new Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>(elementIndividual, elementClass, elementObject, RtrbauElementLocation.Primary);
                            ReloadElement(element);
                            // Scale element loading panel once it has been located [UPG: moved to LocateElement in IElementable]
                            // if (elementType == RtrbauElementType.Report) { elementObject.GetComponent<ElementReport>().ScaleLoadingPanel(); }
                        }
                        else { throw new ArgumentException("AssetVisualiser::LocateElement: Element " + elementIndividual.entity.Entity() + " already loaded."); }
                    }
                    else if (elementLocation == RtrbauElementLocation.Secondary)
                    {
                        if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.None)
                        {
                            // Modify material of elementLast to seen
                            if (elementLast != null) { elementLast.GetComponent<IVisualisable>().ModifyMaterial(elementSeenMaterial); }
                            // Unparent elementObject from previous location
                            elementObject.transform.SetParent(this.transform, false);
                            // Set elementObject as elementLast
                            elementLast = elementObject;
                            // Unload and destroy element from primary location
                            // KeyValuePair<OntologyElement, GameObject> primaryFirstElement = primaryElements.FindFirstElement();
                            KeyValuePair<OntologyElement, GameObject> primaryFirstElement = elementsLocations[0].FindFirstElement();
                            UnloadElement(primaryFirstElement.Key, primaryFirstElement.Value);
                            // Add element to elementLocation
                            // secondaryElements.AddElement(elementClass, elementObject);
                            elementsLocations[1].AddElement(elementClass, elementObject);
                            // Set loadedElement to new location (ReloadElement)
                            Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element = new Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>(elementIndividual, elementClass, elementObject, RtrbauElementLocation.Secondary);
                            ReloadElement(element);
                            // Scale element loading panel once it has been located [UPG: moved to LocateElement in IElementable]
                            // if (elementType == RtrbauElementType.Report) { elementObject.GetComponent<ElementReport>().ScaleLoadingPanel(); }
                        }
                        else { throw new ArgumentException("AssetVisualiser::LocateElement: Element " + elementIndividual.entity.Entity() + " already loaded."); }
                    }
                    else if (elementLocation == RtrbauElementLocation.Tertiary)
                    {
                        if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.None)
                        {
                            // Modify material of elementLast to seen
                            if (elementLast != null) { elementLast.GetComponent<IVisualisable>().ModifyMaterial(elementSeenMaterial); }
                            // Unparent elementObject from previous location
                            elementObject.transform.SetParent(this.transform, false);
                            // Set elementObject as elementLast
                            elementLast = elementObject;
                            // Unload and destroy element from primary location
                            // KeyValuePair<OntologyElement, GameObject> primaryFirstElement = primaryElements.FindFirstElement();
                            KeyValuePair<OntologyElement, GameObject> primaryFirstElement = elementsLocations[0].FindFirstElement();
                            UnloadElement(primaryFirstElement.Key, primaryFirstElement.Value);
                            // Add element to elementLocation
                            // tertiaryElements.AddElement(elementClass, elementObject);
                            elementsLocations[2].AddElement(elementClass, elementObject);
                            // Set loadedElement to new location (ReloadElement)
                            Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element = new Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>(elementIndividual, elementClass, elementObject, RtrbauElementLocation.Tertiary);
                            ReloadElement(element);
                            // Scale element loading panel once it has been located [UPG: moved to LocateElement in IElementable]
                            // if (elementType == RtrbauElementType.Report) { elementObject.GetComponent<ElementReport>().ScaleLoadingPanel(); }
                        }
                        else { throw new ArgumentException("AssetVisualiser::LocateElement: Element " + elementIndividual.entity.Entity() + " already loaded."); }
                    }
                    else if (elementLocation == RtrbauElementLocation.Quaternary)
                    {
                        if (elementType == RtrbauElementType.Report)
                        {
                            Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element = new Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>(elementIndividual, elementClass, elementObject, RtrbauElementLocation.Quaternary);

                            if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Primary)
                            {
                                // Remove element from previous elementLocation
                                // primaryElements.RemoveElement(elementClass);
                                elementsLocations[0].RemoveElement(elementClass);
                                // Relocate element in new elementLocation
                                // quaternaryElements.AddElement(elementClass, elementObject);
                                elementsLocations[3].AddElement(elementClass, elementObject);
                                // Set loadedElement to new location
                                ReloadElement(element);
                            }
                            else if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Secondary)
                            {
                                // Remove element from previous elementLocation
                                // secondaryElements.RemoveElement(elementClass);
                                elementsLocations[1].RemoveElement(elementClass);
                                // Relocate element in new elementLocation
                                // quaternaryElements.AddElement(elementClass, elementObject);
                                elementsLocations[3].AddElement(elementClass, elementObject);
                                // Set loadedElement to new location
                                ReloadElement(element);
                            }
                            else if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Tertiary)
                            {
                                // Remove element from previous elementLocation
                                // tertiaryElements.RemoveElement(elementClass);
                                elementsLocations[2].RemoveElement(elementClass);
                                // Relocate element in new elementLocation
                                // quaternaryElements.AddElement(elementClass, elementObject);
                                elementsLocations[3].AddElement(elementClass, elementObject);
                                // Set loadedElement to new location
                                ReloadElement(element);
                            }
                            else if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Quaternary) { throw new ArgumentException("AssetVisualiser::LocateElement: Element already loaded."); }
                            else if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.None) { throw new ArgumentException("AssetVisualiser::LocateElement: Element not loaded yet."); }
                            else { throw new ArgumentException("AssetVisualiser::LocateElement: RtrbauElementLocation " + elementLocation.ToString() + " not implemented."); }
                        }
                        else if (elementType == RtrbauElementType.Consult) { throw new ArgumentException("AssetVisualiser::LocateElement: RtrbauLocation " + elementLocation.ToString() + " not implented for RtrbauElementType " + elementType.ToString()); }
                        else { throw new ArgumentException("AssetVisualiser::LocateElement: RtrbauElementType " + elementType.ToString() + " not implemented."); }
                    }
                    else
                    { throw new ArgumentException("AssetVisualiser::LocateElement: RtrbauElementLocation " + elementLocation.ToString() + " not implemented."); }
                }
                else if (elementsMode == RtrbauElementMode.Interior)
                {
                    if (elementLocation != RtrbauElementLocation.Quaternary)
                    {
                        if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.None)
                        {
                            // Modify material of elementLast to seen
                            if (elementLast != null) { elementLast.GetComponent<IVisualisable>().ModifyMaterial(elementSeenMaterial); }
                            // Unparent elementObject from previous location
                            elementObject.transform.SetParent(this.transform, false);
                            // Set elementObject as elementLast
                            elementLast = elementObject;
                            // Unload and destroy element from primary location
                            // KeyValuePair<OntologyElement, GameObject> primaryFirstElement = primaryElements.FindFirstElement();
                            KeyValuePair<OntologyElement, GameObject> primaryFirstElement = elementsLocations[0].FindFirstElement();
                            UnloadElement(primaryFirstElement.Key, primaryFirstElement.Value);
                            // Add element to elementLocation according to element type
                            int elementLocationIndex = (int)elementLocation;
                            if (elementObject.GetComponent<ElementConsult>() != null)
                            {
                                if (elementLocationIndex == 0) { elementsLocations[0].AddElement(elementClass, elementObject); }
                                else if (elementLocationIndex == 1 || elementLocationIndex == 2) { elementsLocations[1].AddElement(elementClass, elementObject); }
                                else { Debug.LogError("AssetVisualiser::LocateElement: This case should never occur."); }
                            }
                            else if (elementObject.GetComponent<ElementReport>() != null)
                            {
                                if (elementLocationIndex == 0 || elementLocationIndex == 1 || elementLocationIndex == 2) { elementsLocations[0].AddElement(elementClass, elementObject); }
                                else { Debug.LogError("AssetVisualiser::LocateElement: This case should never occur."); }
                            }
                            else { throw new ArgumentException("AssetVisualiser::LoadElement: RtrbauElementType " + elementType.ToString() + " not implemented."); }
                            // Set loadedElement to new location (ReloadElement)
                            Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element = new Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>(elementIndividual, elementClass, elementObject, RtrbauElementLocation.Secondary);
                            ReloadElement(element);
                            // Scale element loading panel once it has been located [UPG: moved to LocateElement in IElementable]
                            // if (elementType == RtrbauElementType.Report) { elementObject.GetComponent<ElementReport>().ScaleLoadingPanel(); }
                        }
                        else { throw new ArgumentException("AssetVisualiser::LocateElement: Element " + elementIndividual.entity.Entity() + " already loaded."); }
                    }
                    else if (elementLocation == RtrbauElementLocation.Quaternary)
                    {
                        if (elementType == RtrbauElementType.Report)
                        {
                            Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element = new Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>(elementIndividual, elementClass, elementObject, RtrbauElementLocation.Quaternary);

                            if (elementsLoaded[elementIndex].Item4 != RtrbauElementLocation.Quaternary)
                            {
                                // Remove element from previous elementLocation
                                // primaryElements.RemoveElement(elementClass);
                                elementsLocations[0].RemoveElement(elementClass);
                                // Relocate element in new elementLocation
                                // quaternaryElements.AddElement(elementClass, elementObject);
                                elementsLocations[1].AddElement(elementClass, elementObject);
                                // Set loadedElement to new location
                                ReloadElement(element);
                            }
                            else if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.Quaternary) { throw new ArgumentException("AssetVisualiser::LocateElement: Element already loaded."); }
                            else if (elementsLoaded[elementIndex].Item4 == RtrbauElementLocation.None) { throw new ArgumentException("AssetVisualiser::LocateElement: Element not loaded yet."); }
                            else { throw new ArgumentException("AssetVisualiser::LocateElement: RtrbauElementLocation " + elementLocation.ToString() + " not implemented."); }
                        }
                        else if (elementType == RtrbauElementType.Consult) { throw new ArgumentException("AssetVisualiser::LocateElement: RtrbauLocation " + elementLocation.ToString() + " not implented for RtrbauElementType " + elementType.ToString()); }
                        else { throw new ArgumentException("AssetVisualiser::LocateElement: RtrbauElementType " + elementType.ToString() + " not implemented."); }
                    }
                    else { throw new ArgumentException("AssetVisualiser::LocateElement: RtrbauElementLocation " + elementLocation.ToString() + " not implemented."); }
                }
                else { throw new ArgumentException("AssetVisualiser::LocateElement: RtrbauElementMode not implemented yet."); }
            }
            else { Debug.Log("AssetVisualiser::LocateElement: Element not loaded yet."); }

            DebugElements("LocateElement_" + elementIndividual.entity.Name());
        }
        #endregion EVENTS
        #endregion CLASS_METHODS

        #region DEBUG_METHODS
        void DebugElements(string functionName)
        {
            foreach (Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element in elementsLoaded)
            {
                Debug.Log("AssetVisualiser::" + functionName + "::DebugElements: loaded element is " + element.Item1.entity.Name());
            }

            foreach(RtrbauLocation elementsLocation in elementsLocations)
            {
                elementsLocation.DebugLocationElements(functionName);
            }

            //primaryElements.DebugLocationElements(functionName);
            //secondaryElements.DebugLocationElements(functionName);
            //tertiaryElements.DebugLocationElements(functionName);
            //quaternaryElements.DebugLocationElements(functionName);
        }
        #endregion DEBUG_METHODS
    }
}