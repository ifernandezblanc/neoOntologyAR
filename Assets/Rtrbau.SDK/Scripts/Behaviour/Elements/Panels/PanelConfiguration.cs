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
        public TextMeshPro userWrittenText;
        public TextMeshPro serverWrittenText;
        public TextMeshPro serverStatusText;
        public GameObject userSelectReportButton;
        public GameObject userSelectConsultButton;
        public GameObject serverConnectButton;
        public GameObject serverWriteButton;
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
            if (userWrittenText == null || serverWrittenText == null || serverStatusText == null || userSelectReportButton == null || userSelectConsultButton == null || serverConnectButton == null || serverWriteButton == null)
            {
                Debug.LogError("Configuration Panel: Fabrication buttons not found. Please assign them in PanelConfiguration script.");
            }
            else
            {
                serverDefaultURI = Rtrbauer.instance.server.AbsoluteUri;
                userDefaultName = Rtrbauer.instance.user.name;

                userWrittenText.transform.parent.parent.gameObject.SetActive(true);
                userSelectReportButton.SetActive(true);
                userSelectConsultButton.SetActive(true);

                userWrittenText.text = userDefaultName;
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

            serverWrittenText.text = serverDefaultURI;
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void InputIntoReport()
        {
            // Initialise Rtrbauer server and user variables
            Rtrbauer.instance.server = new Uri(serverURI);
            Rtrbauer.instance.user.name = userName;
            Rtrbauer.instance.user.procedure = userProcedure;
            // Initialise RtrbauDebug user log variables
            RtrbauDebug.instance.Initialise();
            // Initialise Reporter:
            // Generate server, user, asset and component variables to report
            OntologyEntity serverIndividual = new OntologyEntity(Rtrbauer.instance.server.AbsoluteUri + "api/files/owl/server#connected");
            OntologyEntity userRange = new OntologyEntity(Rtrbauer.instance.server.AbsoluteUri + "api/files/owl/orgont#Agent");
            OntologyEntity userIndividual = new OntologyEntity(Rtrbauer.instance.server.AbsoluteUri + "api/files/owl/orgont#" + Rtrbauer.instance.user.name);
            OntologyEntity assetRange = new OntologyEntity(Rtrbauer.instance.asset.URI());
            OntologyEntity componentRange = new OntologyEntity(Rtrbauer.instance.component.URI());
            // Confirm user and server through log
            RtrbauDebug.instance.Log("PanelConfiguration::InputIntoReport: serverReport: " + serverIndividual.URI());
            RtrbauDebug.instance.Log("PanelConfiguration::InputIntoReport: userReport: " + userIndividual.URI());
            // Report server, user, asset and component
            Reporter.instance.ReportElement(null, null, serverIndividual);
            Reporter.instance.ReportElement(null, userRange, userIndividual);
            Reporter.instance.ReportElement(null, assetRange, null);
            Reporter.instance.ReportElement(null, componentRange, null);
            // Move to next panel
            PanellerEvents.TriggerEvent("LoadAssets", Rtrbauer.instance.asset);
            // Destroy this element
            DestroyElement();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ActivateLoadingPlate() { }

        /// <summary>
        /// 
        /// </summary>
        public void DeactivateLoadingPlate() { }
        #endregion IELEMENTABLE_METHODS

        #region CLASS_METHODS

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void ConfigureUser(TextMeshPro procedureText)
        {
            RtrbauElementType proc;

            if (Enum.TryParse<RtrbauElementType>(procedureText.text, out proc))
            {
                if (!userWrittenText.text.Contains("Focus to open keyboard") || !userWrittenText.text.Contains("Keyboard not supported"))
                {
                    userName = userWrittenText.text;
                }
                else
                {
                    userName = userDefaultName;
                    userWrittenText.text = userDefaultName;
                }
                userProcedure = proc;
                Debug.Log("PanelConfiguration: ConfigureUser: User configured is: " + userName + " doing " + userProcedure.ToString());
                CreateFabrications();
            }
            else
            {
                Debug.LogError("Procedure type not implemented in Rtrbau");
            }
            
        }

        public void ConfigureServer(TextMeshPro writtenText)
        {
            // Identify server connection status
            string serverConnection = serverStatusText.text;
            // If server connection hasn't failed yet connect either to written address or default server
            if (serverConnection.Contains("Connect to server"))
            {
                if (writtenText.text.Contains("http"))
                {
                    StartCoroutine(CheckServerConnection(writtenText.text));
                    Debug.Log("PanelConfiguration::ConfigureServer: Server configured is: " + writtenText.text);
                }
                else
                {
                    StartCoroutine(CheckServerConnection(serverDefaultURI));
                    Debug.Log("PanelConfiguration::ConfigureServer: Server configured is: " + serverDefaultURI);
                }
            }
            else if (serverConnection.Contains("Server failed. Try another."))
            {
                if (writtenText.text.Contains("http"))
                {
                    StartCoroutine(CheckServerConnection(writtenText.text));
                    Debug.Log("PanelConfiguration::ConfigureServer: Server configured is: " + writtenText.text);
                }
                else
                {
                    writtenText.text = "Type server http address";
                    Debug.Log("PanelConfiguration::ConfigureServer: Server cannot be configured");
                }
            }
            else
            {
                writtenText.text = "Server already found?";
                Debug.Log("PanelConfiguration::ConfigureServer: Server already found?");
            }
            //// When working at home to avoid server connection:
            ////serverURI = serverURL;
            ////InputIntoReport();
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
                    serverStatusText.text = "Server failed. Try another.";
                    Debug.Log("PanelConfiguration::CheckServerConnection: Server not found");
                }
                else
                {
                    serverStatusText.text = "Server found.";
                    Debug.Log("PanelConfiguration::CheckServerConnection: Server found");
                    serverURI = serverURL;
                    InputIntoReport();
                }
            }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public void OpenKeyboard(TextMeshPro textOutput)
        {
            // Activate RtrbauKeyboard
            RtrbauKeyboard.instance.OpenRtrbauKeyboard(TouchScreenKeyboardType.Default, textOutput);
        }
        #endregion CLASS_METHODS
    }
}

