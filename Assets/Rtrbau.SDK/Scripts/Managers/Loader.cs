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
using System.Text;
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
        #region CLASS_MEMBERS 
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
                        Debug.LogError("Loader::Instance: There needs to be a Loader script in the scene.");
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
        #endregion CLASS_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        void Initialise() {}
        #endregion PRIVATE

        #region PUBLIC
        #region DOWNLOADERS
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
                    throw new ArgumentException("Loader::DownloadElement: Web error: " + webRequest.error + " from element " + element.EventName() + " of type " + element.type.ToString());
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
                    throw new ArgumentException("Loader::DownloadDistance: Web error: " + webRequest.error + " from element " + distance.EventName());
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

        #region RECOMMENDATION_DOWNLOADERS
        public void StartOntRecommendationDownload(OntologyRecommendation recommendation)
        {
            StartCoroutine(DownloadRecommendation(recommendation));
        }

        IEnumerator DownloadRecommendation(OntologyRecommendation recommendation)
        {
            OntologyRecommendation result = null;

            if (!File.Exists(recommendation.FilePath()))
            {
                UnityWebRequest webRequest = UnityWebRequest.Get(recommendation.URL());

                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    throw new ArgumentException("Loader::DownloadElement: Web error: " + webRequest.error + " from element " + recommendation.EventName() + " of type " + recommendation.type.ToString());
                }
                else
                {
                    File.WriteAllText(recommendation.FilePath(), webRequest.downloadHandler.text);
                    result = recommendation;
                }
            }
            else
            {
                result = recommendation;
            }

            LoaderEvents.TriggerEvent(recommendation.EventName(), result);
            // Debug.Log("Loader events: Trigger event: " + element.EventName());

        }
        #endregion RECOMMENDATION_DOWNLOADERS

        #region FILE_DOWNLOADERS
        public void StartFileDownload(OntologyFile file)
        {
            StartCoroutine(DownloadFile(file));
        }

        IEnumerator DownloadFile(OntologyFile file)
        {
            OntologyFile result = null;

            if (!File.Exists(file.FilePath()))
            {
                UnityWebRequest webRequest = UnityWebRequest.Get(file.URL());

                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    throw new ArgumentException("Loader::DownloadDistance: Web error: " + webRequest.error + " from element " + file.EventName());
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
        #endregion DOWNLOADERS

        #region UPLOADERS
        #region ONTOLOGY_UPLOADERS
        public void StartOntElementUpload(OntologyElementUpload element)
        {
            StartCoroutine(UploadElement(element));
        }

        IEnumerator UploadElement(OntologyElementUpload element)
        {
            OntologyElementUpload result = null;

            if (!File.Exists(element.FilePath()))
            {
                if (!File.Exists(element.individualElement.FilePath()))
                {
                    throw new ArgumentException("Loader::UploadElement: json file for " + element.individualElement.entity.Name() + " should exist.");
                }
                else
                {
                    if (!File.Exists(element.classElement.FilePath()))
                    {
                        throw new ArgumentException("Loader::UploadElement: json file for " + element.classElement.entity.Name() + " should exist.");
                    }
                    else
                    {
                        string individualFile = File.ReadAllText(element.individualElement.FilePath());
                        JsonIndividualValues elementIndividual = JsonUtility.FromJson<JsonIndividualValues>(individualFile);

                        string classFile = File.ReadAllText(element.classElement.FilePath());
                        JsonClassProperties elementClass = JsonUtility.FromJson<JsonClassProperties>(classFile);

                        JsonUploadIndividual upload = new JsonUploadIndividual(elementIndividual, elementClass);

                        string postData = JsonUtility.ToJson(upload);

                        Debug.Log("Loader::UploadElement: postData: " + postData);
                        Debug.Log("Loader::UploadElement: postString: " + element.URL());

                        // UnityWebRequest webRequest = UnityWebRequest.Post(element.URL(), postData);

                        // https://qiita.com/mattak/items/d01926bc57f8ab1f569a

                        UnityWebRequest webRequest = new UnityWebRequest(element.URL(), "POST");

                        byte[] postDataRaw = Encoding.UTF8.GetBytes(postData);
                        webRequest.uploadHandler = (UploadHandler) new UploadHandlerRaw(postDataRaw);
                        webRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
                        webRequest.SetRequestHeader("Content-Type", "application/json");

                        yield return webRequest.SendWebRequest();

                        if (webRequest.isNetworkError || webRequest.isHttpError)
                        {
                            throw new ArgumentException("Loader::UploadElement: web error: " + webRequest.error);
                        }
                        else
                        {
                            File.WriteAllBytes(element.FilePath(), webRequest.downloadHandler.data);
                            result = element;
                        }
                    }
                }
            }
            else
            {
                result = element;
            }

            LoaderEvents.TriggerEvent(element.EventName(), result);
            // Debug.Log("Loader::UploadElement: trigger event: " + element.EventName());
        }
        #endregion ONTOLOGY_UPLOADERS

        #region FILE_UPLOADERS
        public void StartFileUpload(OntologyFileUpload file)
        {
            StartCoroutine(UploadFile(file));
        }

        IEnumerator UploadFile(OntologyFileUpload fileUpload)
        {
            OntologyFileUpload result = null;

            Debug.Log("Loader::UploadFile: start file upload " + fileUpload.file.URL());

            if (!File.Exists(fileUpload.FilePath()))
            {
                throw new ArgumentException("Loader::UploadFile: file " + fileUpload.file.name + "." + fileUpload.file.type + " should exist.");
            }
            else
            {
                // Create a form data post request
                // https://docs.unity3d.com/Manual/UnityWebRequest-SendingForm.html
                // https://docs.unity3d.com/ScriptReference/Networking.MultipartFormFileSection-ctor.html

                // byte[] fileRawData = File.ReadAllBytes(fileUpload.FilePath());

                //WWWForm dataForm = new WWWForm();
                //dataForm.AddBinaryData("file", data, fileName, "multipart/form-data");

                List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
                formData.Add(new MultipartFormFileSection("file", fileUpload.FileData(), fileUpload.FileName(), fileUpload.UploadForm()));

                UnityWebRequest webRequest = UnityWebRequest.Post(fileUpload.URL(), formData);
                //webRequest.SetRequestHeader("content-type", "multipart/form-data");
                //webRequest.SetRequestHeader("cache-control", "no-cache");

                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    throw new ArgumentException("Loader::UploadElement: web error: " + webRequest.error);
                }
                else
                {
                    result = fileUpload;
                }

                //UnityWebRequest webRequest = new UnityWebRequest(fileUpload.URL(), "POST");

                //byte[] postDataRaw = File.ReadAllBytes(fileUpload.FilePath());
                //webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(postDataRaw);
                //webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                //webRequest.SetRequestHeader("Content-Type", "multipart/form-data");

                //yield return webRequest.SendWebRequest();

                //if (webRequest.isNetworkError || webRequest.isHttpError)
                //{
                //    throw new ArgumentException("Loader::UploadElement: web error: " + webRequest.error);
                //}
                //else
                //{
                //    result = fileUpload;
                //}

                //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
                //formData.Add(new MultipartFormFileSection("file", fileData, fileForm, fileName));

                //Debug.Log("Loader::UploadFile: file " + fileName + " uploaded to " + fileUpload.URL());

                //UnityWebRequest webRequest = UnityWebRequest.Post(fileUpload.URL(), formData);

                //yield return webRequest.SendWebRequest();

                //if (webRequest.isNetworkError || webRequest.isHttpError)
                //{
                //    throw new ArgumentException("Loader::UploadFile: web error: " + webRequest.error);
                //}
                //else
                //{
                //    result = fileUpload;
                //}
            }

            LoaderEvents.TriggerEvent(fileUpload.EventName(), result);
            // Debug.Log("Loader::UploadFile: trigger event: " + fileUpload.EventName());
        }
        #endregion FILE_UPLOADERS
        #endregion UPLOADERS
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}
