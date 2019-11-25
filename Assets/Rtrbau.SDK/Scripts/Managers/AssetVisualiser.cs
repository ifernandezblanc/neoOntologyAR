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
        public bool UnloadElement(OntologyElement elementClass, GameObject elementObject)
        {
            if (elementObject != null)
            {
                // GameObjects for loadedElements should always exist
                int elementIndex = loadedElements.FindIndex(x => x.Item3.Equals(elementObject));

                if (elementIndex != -1)
                {
                    // Remove element from location in case it is not quaternary
                    if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.Primary) { primaryElements.RemoveElement(elementClass); }
                    else if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.Secondary) { secondaryElements.RemoveElement(elementClass); }
                    else if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.Tertiary) { tertiaryElements.RemoveElement(elementClass); }
                    else if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.Quaternary) { quaternaryElements.RemoveElement(elementClass); } 
                    else { throw new ArgumentException("AssetVisualiser::UnloadElement: Element " + loadedElements[elementIndex].Item1.entity.Entity() + " not loaded yet."); }
                    // Destroy elementObject
                    if (elementObject.GetComponent<IElementable>().DestroyElement()) { }
                    else { throw new ArgumentException("AssetVisualiser::UnloadElement: IElementable object should always be destroyable."); }
                    // Remove element from loadedElements
                    loadedElements.RemoveAt(elementIndex);
                    // Element unloaded and destroyed, return true
                    DebugElements("LocateElement_" + loadedElements[elementIndex].Item1.entity.Name());
                    return true;
                }
                else { throw new ArgumentException("AssetVisualiser::UnloadElement: Element not created/loaded yet."); }
            }
            else { return false; }

            // loadedElements.Remove(loadedElements.Find(x => x.Item2 == element));
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
                    
                    Debug.Log("AssetVisualiser::LoadElement: Element " + elementIndividual.entity.Name() + " loaded.");
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

                    Debug.Log("AssetVisualiser::LoadElement: Element " + elementIndividual.entity.Name() + " loaded.");
                }
                else
                {
                    throw new ArgumentException("AssetVisualiser::LoadElement: RtrbauElementType " + type.ToString() + "not implemented");
                }
            }
            else
            {
                // UPG: ErrorHandling
                Debug.Log("AssetVisualiser::LoadElement: IElementable for " + elementIndividual.entity.Name() + "loaded already");
            }

            Debug.Log("AssetVisualiser::LoadElement: Number of elements loaded is: " + loadedElements.Count());
            DebugElements("LocateElement_" + elementIndividual.entity.Name());
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

            DebugElements("LocateElement_" + elementIndividual.entity.Name());

            if (elementIndex != -1)
            {

                OntologyElement elementClass = loadedElements[elementIndex].Item2;
                GameObject elementObject = loadedElements[elementIndex].Item3;

                if (elementLocation == RtrbauElementLocation.Primary)
                {
                    if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.None)
                    {
                        // Modify material of lastElement to seen
                        if (lastElement != null) { lastElement.GetComponent<IVisualisable>().ModifyMaterial(elementSeenMaterial); }
                        // Unparent elementObject from previous location
                        elementObject.transform.SetParent(this.transform, false);
                        // Set elementObject as lastElement
                        lastElement = elementObject;
                        // Unload and destroy element from primary location
                        KeyValuePair<OntologyElement, GameObject> primaryFirstElement = primaryElements.FindFirstElement();
                        UnloadElement(primaryFirstElement.Key, primaryFirstElement.Value);
                        // Add element to elementLocation
                        primaryElements.AddElement(elementClass, elementObject);
                        // Set loadedElement to new location (ReloadElement)
                        Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element = new Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>(elementIndividual, elementClass, elementObject, RtrbauElementLocation.Primary);
                        ReloadElement(element);
                    }
                    else { throw new ArgumentException("AssetVisualiser::LocateElement: Element " + elementIndividual.entity.Entity() + " already loaded."); }
                }
                else if (elementLocation == RtrbauElementLocation.Secondary)
                {
                    if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.None)
                    {
                        // Modify material of lastElement to seen
                        if (lastElement != null) { lastElement.GetComponent<IVisualisable>().ModifyMaterial(elementSeenMaterial); }
                        // Unparent elementObject from previous location
                        elementObject.transform.SetParent(this.transform, false);
                        // Set elementObject as lastElement
                        lastElement = elementObject;
                        // Unload and destroy element from primary location
                        KeyValuePair<OntologyElement, GameObject> primaryFirstElement = primaryElements.FindFirstElement();
                        UnloadElement(primaryFirstElement.Key, primaryFirstElement.Value);
                        // Add element to elementLocation
                        secondaryElements.AddElement(elementClass, elementObject);
                        // Set loadedElement to new location (ReloadElement)
                        Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element = new Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation>(elementIndividual, elementClass, elementObject, RtrbauElementLocation.Secondary);
                        ReloadElement(element);
                    }
                    else { throw new ArgumentException("AssetVisualiser::LocateElement: Element " + elementIndividual.entity.Entity() + " already loaded."); }
                }
                else if (elementLocation == RtrbauElementLocation.Tertiary)
                {
                    if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.None)
                    {
                        // Modify material of lastElement to seen
                        if (lastElement != null) { lastElement.GetComponent<IVisualisable>().ModifyMaterial(elementSeenMaterial); }
                        // Unparent elementObject from previous location
                        elementObject.transform.SetParent(this.transform, false);
                        // Set elementObject as lastElement
                        lastElement = elementObject;
                        // Unload and destroy element from primary location
                        KeyValuePair<OntologyElement, GameObject> primaryFirstElement = primaryElements.FindFirstElement();
                        UnloadElement(primaryFirstElement.Key, primaryFirstElement.Value);
                        // Add element to elementLocation
                        tertiaryElements.AddElement(elementClass, elementObject);
                        // Set loadedElement to new location (ReloadElement)
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
                            // Remove element from previous elementLocation
                            primaryElements.RemoveElement(elementClass);
                            // Relocate element in new elementLocation
                            quaternaryElements.AddElement(elementClass, elementObject);
                            // Set loadedElement to new location
                            ReloadElement(element);
                        }
                        else if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.Secondary)
                        {
                            // Remove element from previous elementLocation
                            secondaryElements.RemoveElement(elementClass);
                            // Relocate element in new elementLocation
                            quaternaryElements.AddElement(elementClass, elementObject);
                            // Set loadedElement to new location
                            ReloadElement(element);
                        }
                        else if (loadedElements[elementIndex].Item4 == RtrbauElementLocation.Tertiary)
                        {
                            // Remove element from previous elementLocation
                            tertiaryElements.RemoveElement(elementClass);
                            // Relocate element in new elementLocation
                            quaternaryElements.AddElement(elementClass, elementObject);
                            // Set loadedElement to new location
                            ReloadElement(element);
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

            DebugElements("LocateElement_" + elementIndividual.entity.Name());
        }
        #endregion EVENTS
        #endregion CLASS_METHODS

        #region DEBUG_METHODS
        void DebugElements(string functionName)
        {
            foreach (Tuple<OntologyElement, OntologyElement, GameObject, RtrbauElementLocation> element in loadedElements)
            {
                Debug.Log("AssetVisualiser::" + functionName + "::DebugElements: loaded element is " + element.Item1.entity.Name());
            }

            primaryElements.DebugLocationElements(functionName);
            secondaryElements.DebugLocationElements(functionName);
            tertiaryElements.DebugLocationElements(functionName);
            quaternaryElements.DebugLocationElements(functionName);
        }
        #endregion DEBUG_METHODS
    }
}