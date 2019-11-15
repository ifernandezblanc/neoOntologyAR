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

#region NAMESPACES
/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
#endregion


namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class PanelConfiguration : MonoBehaviour, IElementable
    {
        #region INITIALISATION_VARIABLES
        public string serverDefaultURI;
        public string userDefaultName;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        public string serverURI;
        public string userName;
        public RtrbauElementType userProcedure;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public GameObject userWriteButton;
        public GameObject userSelectReportButton;
        public GameObject userSelectConsultButton;
        public GameObject serverWriteButton;
        public GameObject serverConnectButton;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void OnEnable()
        {
            
        }

        void OnDisable()
        {
            
        }
        #endregion MONOBEHAVIOUR_METHODS

        #region INITIALISATION_METHODS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void Initialise()
        {
            if (serverWriteButton == null || serverConnectButton == null || userWriteButton == null || userSelectReportButton == null || userSelectConsultButton == null)
            {
                Debug.LogError("Configuration Panel: Fabrication buttons not found. Please assign them in PanelConfiguration script.");
            }
            else
            {
                serverDefaultURI = Rtrbauer.instance.server.AbsoluteUri;
                userDefaultName = Rtrbauer.instance.user.name;

                userWriteButton.SetActive(true);
                userSelectReportButton.SetActive(true);
                userSelectConsultButton.SetActive(true);

                userWriteButton.transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = userDefaultName;
            }
        }
        #endregion INITIALISATION_METHODS

        #region IELEMENTABLE_METHODS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void DownloadElement()
        {
            // ConfigureUser()
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void EvaluateElement()
        {
            // ConfigureUser()
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void LocateElement()
        {
            // ConfigureUser()
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        /// <returns>Describe script outcomes</returns>
        public bool DestroyElement()
        {
            // Destroy game object
            Destroy(this.gameObject);
            // Return game object was destroyed
            return true;
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void SelectFabrications()
        {
            // ConfigureUser()
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void CreateFabrications()
        {
            serverWriteButton.SetActive(true);
            serverConnectButton.SetActive(true);

            serverWriteButton.transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = serverDefaultURI;
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void InputIntoReport()
        {
            // Initialise server and user variables
            Rtrbauer.instance.server = new Uri(serverURI);
            Rtrbauer.instance.user.name = userName;
            Rtrbauer.instance.user.procedure = userProcedure;
            // Generate server and user variables to report
            string serverReport = serverURI + "api/files/owl/server#connected";
            string userReport = serverURI + "api/files/owl/server#" + userName;
            // Confirm user and server through log
            Debug.Log("PanelConfiguration::InputIntoReport: serverReport: " + serverReport);
            Debug.Log("PanelConfiguration::InputIntoReport: userReport: " + userReport);
            // Report server and user
            Reporter.instance.ReportElement(new OntologyEntity(serverReport));
            Reporter.instance.ReportElement(new OntologyEntity(userReport));
            // Report asset and component
            Reporter.instance.ReportElement(Rtrbauer.instance.asset);
            Reporter.instance.ReportElement(Rtrbauer.instance.component);
            // Move to next panel
            PanellerEvents.TriggerEvent("LoadAssets", Rtrbauer.instance.asset);
            // Destroy this element
            DestroyElement();
        }
        #endregion IELEMENTABLE_METHODS

        #region CLASS_METHODS

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void ConfigureUser(TextMeshProUGUI procedure)
        {
            RtrbauElementType proc;

            if (Enum.TryParse<RtrbauElementType>(procedure.text, out proc))
            {
                userName = userWriteButton.transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text;
                userProcedure = proc;
                Debug.Log("PanelConfiguration: ConfigureUser: User configured is: " + userName + " doing " + userProcedure.ToString());
                CreateFabrications();
            }
            else
            {
                Debug.LogError("Procedure type not implemented in Rtrbau");
            }
            
        }

        public void ConfigureServer(TextMeshPro serverWriteButton)
        {
            string serverURL = serverWriteButton.text;
            StartCoroutine(CheckServerConnection(serverURL));
            //// When working at home to avoid server connection:
            ////serverURI = serverURL;
            ////InputIntoReport();
            Debug.Log("PanelConfiguration::ConfigureServer: Server configured is: " + serverURL);
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        IEnumerator CheckServerConnection(string serverURL)
        {
            string serverPing = serverURL + "api/ping";

            using (UnityWebRequest webRequest = UnityWebRequest.Get(serverPing))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    serverConnectButton.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Server failed. Try another.";
                    Debug.Log("PanelConfiguration::CheckServerConnection: Server not found");
                }
                else
                {
                    serverConnectButton.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Server found.";
                    Debug.Log("PanelConfiguration::CheckServerConnection: Server found");
                    serverURI = serverURL;
                    InputIntoReport();
                }
            }
        }
        #endregion CLASS_METHODS
    }
}

