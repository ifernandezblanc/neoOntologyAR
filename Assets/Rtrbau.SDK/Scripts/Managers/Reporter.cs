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
        public List<Tuple<DateTimeOffset, OntologyEntity, OntologyEntity, OntologyEntity, GameObject>> reportDictionary;
        #endregion CLASS_VARIABLES

        #region MONOBEHAVIOUR_METHODS
        void OnApplicationQuit() { SendReport(); }
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
                reportDictionary = new List<Tuple<DateTimeOffset, OntologyEntity, OntologyEntity, OntologyEntity, GameObject>>();
            }
        }

        /// <summary>
        /// Initialises report dictionary
        /// </summary>
        public void ReinitialiseReport()
        {
            // Destroy reportedElements GameObjects if exist
            foreach (Tuple<DateTimeOffset, OntologyEntity, OntologyEntity, OntologyEntity, GameObject> reportedElement in reportDictionary)
            {
                Destroy(reportedElement.Item5);
            }

            // Initialise report dictionary
            reportDictionary = new List<Tuple<DateTimeOffset, OntologyEntity, OntologyEntity, OntologyEntity, GameObject>>();

            // Update user
            Rtrbauer.instance.user.name = Parser.ParseAddDateTime("User_");

            // Re-initialise dictionary elements: server, user, asset and component
            OntologyEntity serverIndividual = new OntologyEntity(Rtrbauer.instance.server.AbsoluteUri + "api/files/owl/server#connected");
            OntologyEntity userRange = new OntologyEntity(Rtrbauer.instance.server.AbsoluteUri + "api/files/owl/orgont#Agent");
            OntologyEntity userIndividual = new OntologyEntity(Rtrbauer.instance.server.AbsoluteUri + "api/files/owl/orgont#" + Rtrbauer.instance.user.name);
            OntologyEntity assetRange = new OntologyEntity(Rtrbauer.instance.asset.URI());
            OntologyEntity componentRange = new OntologyEntity(Rtrbauer.instance.component.URI());

            // Report re-initialised elements
            ReportElement(null, null, serverIndividual);
            ReportElement(null, userRange, userIndividual);
            ReportElement(null, assetRange, null);
            ReportElement(null, componentRange, null);

            // UPG: to reinitialise with Vuforia to start the app again (start at configuration panel)
        }

        /// <summary>
        /// Inputs ontology entity into user's report.
        /// Loading entities created is a different function [TBD].
        /// </summary>
        public void ReportElement(OntologyEntity relationship, OntologyEntity range, OntologyEntity individual)
        {
            // Initialise reportedElement members
            GameObject reportedObject = Instantiate(reportElement, reportPanel.transform);
            OntologyEntity reportedRelationship = relationship;
            OntologyEntity reportedRange = range;
            OntologyEntity reportedIndividual = individual;

            // Include reportedElement to reportDictionary
            reportDictionary.Add(new Tuple<DateTimeOffset, OntologyEntity, OntologyEntity, OntologyEntity, GameObject>(DateTimeOffset.Now, reportedRelationship, reportedRange, reportedIndividual, reportedObject));

            // Add reportedElement text to its GameObject
            string reportedText = "";
            if (reportedRelationship != null) { reportedText += reportedRelationship.Entity() + " : "; }
            if (reportedRange != null) { reportedText += reportedRange.Entity() + " : "; }
            if (reportedIndividual != null) { reportedText += reportedIndividual.Entity(); }
            reportedObject.GetComponentInChildren<TextMeshPro>().text = reportedText;

            // Include same text to debugger
            RtrbauDebug.instance.Log("Reporter::ReportElement: " + Parser.ParseNamingDateTimeXSD() + " : " + reportedText);
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
                reportWriter.WriteLine("\"dateTime\": " + "\"" + Parser.ParseNamingDateTimeXSD(reportDictionary[i].Item1) + "\",");
                if (reportDictionary[i].Item2 != null) { reportWriter.WriteLine("\"relationship\": " + "\"" + reportDictionary[i].Item2.URI() + "\","); }
                else { reportWriter.WriteLine("\"relationship\": " + "\"\","); }
                if (reportDictionary[i].Item3 != null) { reportWriter.WriteLine("\"range\": " + "\"" + reportDictionary[i].Item3.URI() + "\","); }
                else { reportWriter.WriteLine("\"range\": " + "\"\","); }
                if (reportDictionary[i].Item4 != null) { reportWriter.WriteLine("\"individual\": " + "\"" + reportDictionary[i].Item4.URI() + "\""); }
                else { reportWriter.WriteLine("\"individual\": " + "\"\""); }
                if (i != reportDictionary.Count - 1) { reportWriter.WriteLine("},"); }
                else { reportWriter.WriteLine("}"); }
            }

            reportWriter.WriteLine("]");

            reportWriter.Flush();
            reportWriter.Close();

            Debug.Log("Report written.");

        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired 
        /// </summary>
        /// <param name="classEntity"></param>
        /// <returns></returns>
        public OntologyEntity FindLastReportedIndividualFromClass(OntologyEntity classEntity)
        {
            // Returns the last individual reported from a given class
            // Necessary for recommendation methods to find target
            // Determine reported elements that are individuals belonging to a class
            List<Tuple<DateTimeOffset, OntologyEntity, OntologyEntity, OntologyEntity, GameObject>> reportedClassIndividuals = reportDictionary.FindAll((x) => x.Item3 != null && x.Item4 != null);
            // Determine that last report element that belongs to the given class
            Tuple<DateTimeOffset, OntologyEntity, OntologyEntity, OntologyEntity, GameObject> lastClassIndividual = reportedClassIndividuals.FindLast((x) => x.Item3.URI() == classEntity.URI());
            // Initialise return variable
            // Return ontology entity from last individual, otherwise return null
            if (lastClassIndividual == null) { return null; } 
            else { return lastClassIndividual.Item4; }
        }

        #endregion CLASS_METHODS
    }
}
