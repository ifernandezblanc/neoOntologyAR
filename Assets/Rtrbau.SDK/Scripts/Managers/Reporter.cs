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
using TMPro;
using UnityEngine.UI;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class Reporter : MonoBehaviour
    {
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        #region SINGLETON_INSTANTIATION
        private static Reporter reporterManager;

        public static Reporter instance
        {
            get
            {
                if (!reporterManager)
                {
                    reporterManager = FindObjectOfType(typeof(Reporter)) as Reporter;

                    if (!reporterManager)
                    {
                        Debug.LogError("There needs to be an Rtrbauer script in the scene.");
                    }
                    else
                    {
                        reporterManager.Initialise();
                    }
                } else { }

                return reporterManager;
            }
        }
        #endregion SINGLETON_INSTANTIATION

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        #region CLASS_VARIABLES
        public GameObject reportElement;
        public GameObject reportPanel;
        public GameObject reportButton;
        public List<KeyValuePair<DateTime, OntologyEntity>> reportDictionary;
        #endregion CLASS_VARIABLES

        #region MONOBEHAVIOUR_METHODS
        // void OnDestroy()
        void OnApplicationQuit()
        {
            SendReport();
        }
        #endregion MONOBEHAVIOUR_METHODS

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        #region CLASS_METHODS
        void Initialise()
        {
            // reportElement = Resources.Load("Rtrbau/Prefabs/Fabrications/ReportElement") as GameObject;

            if (reportElement == null || reportPanel == null || reportButton == null)
            {
                Debug.LogError("There must be prefabs for element reporting.");
            }
            else
            {
                // reportPanel = this.transform.Find("ReportElements").gameObject;
                reportDictionary = new List<KeyValuePair<DateTime, OntologyEntity>>();
            }
        }

        /// <summary>
        /// Initialises report dictionary
        /// </summary>
        public void ReinitialiseReport()
        {
            // Destroy reported elements if exist
            HorizontalLayoutGroup[] reportedElements = reportPanel.GetComponentsInChildren<HorizontalLayoutGroup>();

            if (reportedElements.Length != 0)
            {
                foreach (HorizontalLayoutGroup reportedElement in reportedElements)
                {
                    Destroy(reportedElement.gameObject);
                }
            }

            // Initialise report dictionary
            reportDictionary = new List<KeyValuePair<DateTime, OntologyEntity>>();

            // Update user
            Rtrbauer.instance.user.name = "User" + "_" + DateTime.Now.ToString("dd-MM-yy_hh-mm-ss");

            // Re-initialise dictionary elements: server, user and asset
            string serverReport = Rtrbauer.instance.server.serverURI + "server" + "#" + "connected";
            string userReport = Rtrbauer.instance.server.serverURI + "user" + "#" + Rtrbauer.instance.user.name;

            // Report re-initialised elements
            ReportElement(new OntologyEntity(serverReport));
            ReportElement(new OntologyEntity(userReport));
            ReportElement(new OntologyEntity(Rtrbauer.instance.asset.assetURI));

            // UPG: to reinitialise with Vuforia to start the app again (start at configuration panel)
        }

        /// <summary>
        /// Inputs ontology entity into user's report.
        /// Loading entities created is a different function [TBD].
        /// </summary>
        public void ReportElement(OntologyEntity entity)
        {
            GameObject reportedElement;

            reportDictionary.Add(new KeyValuePair<DateTime, OntologyEntity>(DateTime.Now, entity));

            reportedElement = Instantiate(reportElement, reportPanel.transform);

            reportedElement.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = entity.ontology;
            reportedElement.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = entity.name;

            Debug.Log("Reporter: ReportElement: " + DateTime.Now + "_" + entity.ToString());
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void SendReport()
        {
            string reportpath = Dictionaries.reportsFileDirectory + "/" + Rtrbauer.instance.user.name + "-" + Rtrbauer.instance.user.procedure + ".json";

            StreamWriter reportWriter = new StreamWriter(reportpath, true);

            reportWriter.WriteLine("[");

            for (int i = 0; i < reportDictionary.Count; i++)
            {
                reportWriter.WriteLine("{");
                reportWriter.WriteLine("\"time\": " + "\"" + reportDictionary[i].Key.ToLongTimeString() + "\",");
                // reportWriter.WriteLine("\"entity\": " + JsonUtility.ToJson(reportDictionary[i].Value));
                reportWriter.WriteLine("\"entity\": " + "\"" + reportDictionary[i].Value.URI() + "\"");
                if (i != reportDictionary.Count - 1) { reportWriter.WriteLine("},"); }
                else { reportWriter.WriteLine("}"); }
            }

            reportWriter.WriteLine("]");

            reportWriter.Flush();
            reportWriter.Close();

            Debug.Log("Report written.");

        }
        #endregion CLASS_METHODS
    }
}
