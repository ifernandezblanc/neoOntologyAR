/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 08/07/2019
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class Rtrbauer : MonoBehaviour
    {

        #region SINGLETON_INSTANTIATION
        private static Rtrbauer rtrbauerManager;

        public static Rtrbauer instance
        {
            get
            {
                if (!rtrbauerManager)
                {
                    rtrbauerManager = FindObjectOfType(typeof(Rtrbauer)) as Rtrbauer;

                    if (!rtrbauerManager)
                    {
                        Debug.LogError("There needs to be an Rtrbauer script in the scene.");
                    }
                    else
                    {
                        rtrbauerManager.Initialise();
                    }
                } else { }

                return rtrbauerManager;
            }
        }
        #endregion SINGLETON_INSTANTIATION

        #region CLASS_MEMBERS
        public URIS uris;
        public Server server;
        public Ontology ontology;
        public Asset asset;
        public Component component;
        public Operation operation;
        public User user;
        public Environment environment;
        public GameObject viewer;
        private Dictionary<RtrbauFabricationName, GameObject> ConsultFabrications;
        private Dictionary<RtrbauFabricationName, GameObject> ReportFabrications;
        #endregion CLASS_MEMBERS

        #region GAMEOBJECT_PREFABS
        public GameObject paneller;
        private GameObject assetManager;
        #endregion GAMEOBJECT_PREFABS

        #region MONOBEHAVIOUR_METHODS
        void Awake()
        {
            // if (paneller == null || visualiser == null)
            if (paneller == null)
            {
                throw new ArgumentException("RtrbauVisualiser and RtrbauPaneller game objects should be attached");
            }
            else
            {
                // Declare configuration classes
                // IMPORTANT! This is default, however, things may change afterwards.
                server.serverURI = new Uri("http://138.250.108.1:3003");
                ontology.ontologyURI = new Uri(server.serverURI, "api/files/owl");
                asset.assetURI = ontology.ontologyURI.AbsoluteUri + "/" + "orgont#Asset";
                component.componentURI = ontology.ontologyURI.AbsoluteUri + "/" + "orgont#Component";
                // IMPORTANT! This should be considered while in configuration panels (Paneller: ClassSubclassPanel)
                // operation.operationURI = ontology.ontologyURI.AbsoluteUri + "/" + "repont#Operation";
                // Declare visualisation configuration classes
                // User
                foreach (RtrbauComprehensiveness comprehensiveness in Enum.GetValues(typeof(RtrbauComprehensiveness)))
                {
                    user.AssignComprehensiveness(comprehensiveness, true);
                }
                foreach (RtrbauDescriptiveness descriptiveness in Enum.GetValues(typeof(RtrbauDescriptiveness)))
                {
                    user.AssignDescriptiveness(descriptiveness, true);
                }
                // Environment
                environment.AssignSense(RtrbauSense.hearing, true);
                environment.AssignSense(RtrbauSense.sight, true);
                environment.AssignSense(RtrbauSense.kinaesthetic, true);
                // Viewer camera
                viewer = GameObject.FindGameObjectWithTag("MainCamera");
                // Fabrications
                ConsultFabrications = LoadFabrications(RtrbauElementType.Consult);
                ReportFabrications = LoadFabrications(RtrbauElementType.Report);
                // Start scene
                LoadPaneller();
            }
        }
        #endregion MONOBEHAVIOUR_METHODS

        #region INITIALISATION_METHODS

        #endregion INITIALISATION_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        /// <summary>
        /// 
        /// </summary>
        private void Initialise()
        {
            // GameObject.Find("RtrbauPanels").GetComponent<Paneller>().Initialise();
        }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<RtrbauFabricationName, GameObject> LoadFabrications(RtrbauElementType type)
        {
            if (type == RtrbauElementType.Consult || type == RtrbauElementType.Report)
            {
                Dictionary<RtrbauFabricationName, GameObject> fabrications = new Dictionary<RtrbauFabricationName, GameObject>();

                GameObject[] loaded = Resources.LoadAll<GameObject>("Rtrbau/Prefabs/Fabrications/Visualisations/" + type.ToString());

                Debug.Log("Rtrbauer::LoadFabrications: Rtrbau/Prefabs/Fabrications/Visualisations/" + type.ToString());

                foreach(GameObject fabrication in loaded)
                {
                    RtrbauFabricationName fabricationName;

                    if (Enum.TryParse<RtrbauFabricationName>(fabrication.name, out fabricationName))
                    {
                        fabrications.Add(fabricationName, fabrication);
                        // Debug.Log("Rtrbauer::LoadFabrications: preloaded fabrication is " + fabrication.name);
                    }
                    else { }
                }

                return fabrications;
            }
            else
            {
                throw new ArgumentException("Rtrbauer::LoadFabrications: RtrbauElementType not implemented.");
            }
        }
        #endregion PRIVATE

        #region PUBLIC
        /// <summary>
        /// Generates the RtrbauPaneller object to start 
        /// </summary>
        public void LoadPaneller()
        {
            GameObject rtrbauPaneller = Instantiate(paneller);
            rtrbauPaneller.transform.SetParent(this.transform.root, false);
            rtrbauPaneller.GetComponent<Paneller>().Initialise();
            paneller = rtrbauPaneller;

            // UPG: to load paneller as unique element in the game
            // paneller.GetComponent<Paneller>().Initialise();
            // Paneller.instance.Initialise();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentManager"></param>
        /// <returns></returns>
        public GameObject LoadVisualiser(AssetManager parentManager)
        {
            assetManager = parentManager.gameObject;
            GameObject assetVisualiser = new GameObject();
            assetVisualiser.AddComponent<AssetVisualiser>();
            assetVisualiser.GetComponent<AssetVisualiser>().Initialise(parentManager);
            return assetVisualiser;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReloadVisualiser()
        {
            if (assetManager != null)
            {
                assetManager.GetComponent<AssetManager>().assetVisualiser.GetComponent<AssetVisualiser>().DestroyIt();
            }
            else
            {
                throw new ArgumentException("Rtrbauer::ReloadVisualiser: AssetManager not loaded.");
            }
        }

        /// <summary>
        /// Returns a fabrication by <paramref name="name"/> of <paramref name="type"/>, otherwise returns null.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public GameObject FindFabrication(RtrbauFabricationName name, RtrbauElementType type)
        {
            GameObject fabrication;

            if (type == RtrbauElementType.Consult)
            {
                if (ConsultFabrications.TryGetValue(name, out fabrication)) { }
                else { fabrication = null; throw new ArgumentException("Rtrbauer::FindFabrications: Fabrication not found."); }
            }
            else if (type == RtrbauElementType.Report)
            {
                if (ReportFabrications.TryGetValue(name, out fabrication)) { }
                else { fabrication = null; throw new ArgumentException("Rtrbauer::FindFabrications: Fabrication not found."); }
            }
            else
            {
                throw new ArgumentException("Rtrbauer::FindFabrications: RtrbauElementType not implemented.");
            }

            return fabrication;
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}
