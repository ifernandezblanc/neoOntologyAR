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
    public class Loader : MonoBehaviour
    {
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        #region SINGLETON_INSTANTIATION
        private static Loader loaderManager;

        public static Loader instance
        {
            get
            {
                if (!loaderManager)
                {
                    loaderManager = FindObjectOfType(typeof(Loader)) as Loader;

                    if (!loaderManager)
                    {
                        Debug.LogError("There needs to be a Loader script in the scene.");
                    }
                    else
                    {
                        loaderManager.Initialise();
                    }
                } else { }

                return loaderManager;
            }
        }
        #endregion SINGLETON_INSTANTIATION

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        #region CLASS_METHODS
        void Initialise()
        {

        }
        #endregion CLASS_METHODS

        #region ONTOLOGY_DOWNLOADERS
        public void StartOntElementDownload(OntologyElement element)
        {
            StartCoroutine(DownloadElement(element));
        }

        IEnumerator DownloadElement(OntologyElement element)
        {
            OntologyElement result = null;

            if (!File.Exists(element.FilePath()))
            {
                UnityWebRequest webRequest = UnityWebRequest.Get(element.URL());

                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    throw new ArgumentException("Web error: " + webRequest.error);
                }
                else
                {
                    File.WriteAllText(element.FilePath(), webRequest.downloadHandler.text);
                    result = element;
                }
            }
            else
            {
                result = element;
            }

            LoaderEvents.TriggerEvent(element.EventName(), result);
            // Debug.Log("Loader events: Trigger event: " + element.EventName());

        }
        #endregion ONTOLOGY_DOWNLOADERS

        #region DISTANCE_DOWNLOADERS
        public void StartOntDistanceDownload(OntologyDistance distance)
        {
            StartCoroutine(DownloadDistance(distance));
        }

        IEnumerator DownloadDistance(OntologyDistance distance)
        {
            OntologyDistance result = null;

            if (!File.Exists(distance.FilePath()))
            {
                UnityWebRequest webRequest = UnityWebRequest.Get(distance.URL());

                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    throw new ArgumentException("Web error: " + webRequest.error);
                }
                else
                {
                    File.WriteAllText(distance.FilePath(), webRequest.downloadHandler.text);
                    result = distance;
                }
            }
            else
            {
                result = distance;
            }

            LoaderEvents.TriggerEvent(distance.EventName(), result);
            // Debug.Log("Loader events: Trigger event: " + distance.EventName());
        }

        #endregion DISTANCE_DOWNLOADERS

        #region FILE_DOWNLOADERS
        public void StartFileDownload(RtrbauFile file)
        {
            StartCoroutine(DownloadFile(file));
        }

        IEnumerator DownloadFile(RtrbauFile file)
        {
            RtrbauFile result = null;

            if (!File.Exists(file.FilePath()))
            {
                UnityWebRequest webRequest = UnityWebRequest.Get(file.URL());

                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    throw new ArgumentException("Web error: " + webRequest.error);
                }
                else
                {
                    File.WriteAllBytes(file.FilePath(), webRequest.downloadHandler.data);
                    result = file;
                }
            }
            else
            {
                result = file;
            }

            LoaderEvents.TriggerEvent(file.EventName(), result);
            // Debug.Log("Loader events: Trigger event: " + file.EventName());
        }
        #endregion FILE_DOWNLOADERS

    }
}
