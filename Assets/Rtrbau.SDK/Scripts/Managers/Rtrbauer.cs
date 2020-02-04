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
        public Uri server;
        public Ontology rdf;
        public Ontology rdfs;
        public Ontology xsd;
        public Ontology owl;
        public OntologyEntity asset;
        public OntologyEntity component;
        public OntologyEntity operation;
        public User user;
        public Environment environment;
        public GameObject viewer;
        public bool archivedFabrications;
        private Dictionary<RtrbauFabricationName, GameObject> ConsultFabrications;
        private Dictionary<RtrbauFabricationName, GameObject> ReportFabrications;
        #endregion CLASS_MEMBERS

        #region GAMEOBJECT_PREFABS
        public GameObject paneller;
        public GameObject elementConsult;
        public GameObject elementReport;
        public Material elementMaterial;
        private GameObject assetManager;
        #endregion GAMEOBJECT_PREFABS

        #region MONOBEHAVIOUR_METHODS
        void Awake()
        {
            // if (paneller == null || visualiser == null)
            if (paneller == null || elementConsult == null || elementReport == null || elementMaterial == null)
            {
                throw new ArgumentException("RtrbauVisualiser and RtrbauPaneller game objects should be attached");
            }
            else {}
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
            // Declare Rtrbau configuration ontologies and server
            server = new Uri("http://138.250.108.1:3003");
            rdf = new Ontology(OntologyStandardType.rdf);
            rdfs = new Ontology(OntologyStandardType.rdfs);
            xsd = new Ontology(OntologyStandardType.xsd);
            owl = new Ontology(OntologyStandardType.owl);
            Ontology orgont = new Ontology(server.AbsoluteUri + "api/files/owl/orgont#");
            asset = new OntologyEntity(orgont, "Asset");
            component = new OntologyEntity(orgont, "Component");
            user = new User();
            environment = new Environment();
            // Modify user values
            user.name = Parser.ParseAddDateTime("User_");
            foreach (RtrbauComprehensiveness comprehensiveness in Enum.GetValues(typeof(RtrbauComprehensiveness)))
            {
                user.AssignComprehensiveness(comprehensiveness, true);
            }
            foreach (RtrbauDescriptiveness descriptiveness in Enum.GetValues(typeof(RtrbauDescriptiveness)))
            {
                user.AssignDescriptiveness(descriptiveness, true);
            }
            // Modify environment values
            environment.AssignSense(RtrbauSense.hearing, true);
            environment.AssignSense(RtrbauSense.sight, true);
            environment.AssignSense(RtrbauSense.kinaesthetic, true);
            // Assign user camera
            viewer = GameObject.FindGameObjectWithTag("MainCamera");
            // Assign fabrications
            ConsultFabrications = LoadFabrications(RtrbauElementType.Consult);
            ReportFabrications = LoadFabrications(RtrbauElementType.Report);
            // Start scene
            LoadPaneller();
        }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<RtrbauFabricationName, GameObject> LoadFabrications(RtrbauElementType type)
        {
            if (type == RtrbauElementType.Consult || type == RtrbauElementType.Report)
            {
                Dictionary<RtrbauFabricationName, GameObject> fabrications = new Dictionary<RtrbauFabricationName, GameObject>();

                string fabricationsPath;

                Debug.Log("Rtrbauer::LoadFabrications: fabrications are archived: " + archivedFabrications);

                if (archivedFabrications == true) { fabricationsPath = "Rtrbau/Prefabs/Archive/Fabrications/Visualisations/" + type.ToString(); }
                else { fabricationsPath = "Rtrbau/Prefabs/Fabrications/Visualisations/" + type.ToString(); }

                GameObject[] loaded = Resources.LoadAll<GameObject>(fabricationsPath);

                Debug.Log("Rtrbauer::LoadFabrications: " + fabricationsPath);

                foreach(GameObject fabrication in loaded)
                {
                    RtrbauFabricationName fabricationName;

                    if (Enum.TryParse<RtrbauFabricationName>(fabrication.name, out fabricationName))
                    {
                        fabrications.Add(fabricationName, fabrication);
                        Debug.Log("Rtrbauer::LoadFabrications: preloaded fabrication is " + fabrication.name);
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
            assetVisualiser.GetComponent<AssetVisualiser>().Initialise(parentManager, elementConsult, elementReport, elementMaterial);
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
