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
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public static class Dictionaries
    {
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        #region DICTIONARIES_STATIC
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static Dictionary<string, OntologyPropertyType> OntologyPropertyTypes = new Dictionary<string, OntologyPropertyType>()
        {
            {"ObjectProperty",OntologyPropertyType.ObjectProperty},
            {"DatatypeProperty",OntologyPropertyType.DatatypeProperty}
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static Dictionary<string, Type> OntologyDataPropertyTypes = new Dictionary<string, Type>()
        {
            {"anyURI",typeof(RtrbauFile)},
            {"string",typeof(string)},
            {"double",typeof(double)},
            {"int",typeof(int)},
            {"boolean",typeof(bool)}
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static Dictionary<RtrbauElementType, OntologyElementType> OntElementProcedures = new Dictionary<RtrbauElementType, OntologyElementType>()
        {
            {RtrbauElementType.Report, OntologyElementType.ClassProperties},
            {RtrbauElementType.Consult, OntologyElementType.ClassIndividuals }
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static Dictionary<RtrbauFileType, RtrbauAugmentation> FileAugmentations = new Dictionary<RtrbauFileType, RtrbauAugmentation>()
        {
            {RtrbauFileType.wav, RtrbauAugmentation.Audio },
            {RtrbauFileType.jpg, RtrbauAugmentation.Image },
            {RtrbauFileType.png, RtrbauAugmentation.Image },
            {RtrbauFileType.mp4, RtrbauAugmentation.Video },
            {RtrbauFileType.obj, RtrbauAugmentation.Model},
            {RtrbauFileType.dat, RtrbauAugmentation.Registration},
            {RtrbauFileType.xml, RtrbauAugmentation.Registration}
        };
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static Dictionary<OntologyElementType, string> ontDataDirectories = new Dictionary<OntologyElementType, string>()
        {
            {OntologyElementType.Ontologies, Application.persistentDataPath + "/Rtrbau/ontologies/ontologies" },
            {OntologyElementType.ClassSubclasses, Application.persistentDataPath + "/Rtrbau/ontologies/classes-subclasses" },
            {OntologyElementType.ClassIndividuals, Application.persistentDataPath + "/Rtrbau/ontologies/classes-individuals" },
            {OntologyElementType.ClassProperties, Application.persistentDataPath + "/Rtrbau/ontologies/classes-properties" },
            {OntologyElementType.IndividualProperties, Application.persistentDataPath + "/Rtrbau/ontologies/individuals-properties" }
        };
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static Dictionary<RtrbauDistanceType, string> distanceDataDirectories = new Dictionary<RtrbauDistanceType, string>()
        {
            {RtrbauDistanceType.Component, Application.persistentDataPath + "/Rtrbau/distances/component"},
            {RtrbauDistanceType.Operation, Application.persistentDataPath + "/Rtrbau/distances/operation"}
        };
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static Dictionary<RtrbauFileType, string> fileDataDirectories = new Dictionary<RtrbauFileType, string>()
        {
            {RtrbauFileType.wav, Application.persistentDataPath + "/Rtrbau/files/wav" },
            {RtrbauFileType.jpg, Application.persistentDataPath + "/Rtrbau/files/jpg" },
            {RtrbauFileType.png, Application.persistentDataPath + "/Rtrbau/files/png" },
            {RtrbauFileType.mp4, Application.persistentDataPath + "/Rtrbau/files/mp4" },
            {RtrbauFileType.obj, Application.persistentDataPath + "/Rtrbau/files/obj" },
            {RtrbauFileType.xml, Application.persistentDataPath + "/Rtrbau/files/xml-dat" },
            {RtrbauFileType.dat, Application.persistentDataPath + "/Rtrbau/files/xml-dat" }
            // .xml and .dat files need to be in the same folder to allow Vuforia to load them for tracking purposes
        };
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static string reportsFileDirectory = Application.persistentDataPath + "/Rtrbau/reports";


        #endregion DICTIONARIES_STATIC

        #region DICTIONARIES_FORMATS
        // UPG: Add new formats to both dictionaries
        #region DATA_FORMATS
        public static List<DataFormat> DataFormatsConsult = new List<DataFormat>
        {
            { DataFormats.textnone1 },
            { DataFormats.iconnone1 },
            { DataFormats.audiotap1 },
            { DataFormats.imagemanipulation1 },
            { DataFormats.videomanipulation1 },
            { DataFormats.modelmanipulation1 },
            { DataFormats.hologramnone1 },
            { DataFormats.hologramnone2 },
            { DataFormats.animationnone1 },
            { DataFormats.animationnone2 },
            { DataFormats.textbuttontap1 },
            { DataFormats.iconbuttontap1 }
        };

        public static List<DataFormat> DataFormatsReport = new List<DataFormat>
        {
            { DataFormats.textdictation1 },
            { DataFormats.textkeyboard1 },
            { DataFormats.audiorecordhold1 },
            { DataFormats.imagecordhold1 },
            { DataFormats.videorecordhold1 },
            { DataFormats.modelrecordhold1 },
            { DataFormats.textpaneltap1 },
            { DataFormats.iconpaneltap1 }
        };
        #endregion DATA_FORMATS

        #region ENVIRONMENT_FORMATS
        public static List<EnvironmentFormat> EnvironmentFormatsConsult = new List<EnvironmentFormat>
        {
            { EnvironmentFormats.textnone1 },
            { EnvironmentFormats.iconnone1 },
            { EnvironmentFormats.audiotap1 },
            { EnvironmentFormats.imagemanipulation1 },
            { EnvironmentFormats.videomanipulation1 },
            { EnvironmentFormats.modelmanipulation1 },
            { EnvironmentFormats.hologramnone1 },
            { EnvironmentFormats.hologramnone2 },
            { EnvironmentFormats.animationnone1 },
            { EnvironmentFormats.animationnone2 },
            { EnvironmentFormats.textbuttontap1 },
            { EnvironmentFormats.iconbuttontap1 }
        };

        public static List<EnvironmentFormat> EnvironmentFormatsReport = new List<EnvironmentFormat>
        {
            { EnvironmentFormats.textkeyboard1 },
            { EnvironmentFormats.textdictation1 },
            { EnvironmentFormats.audiorecordhold1 },
            { EnvironmentFormats.imagecordhold1 },
            { EnvironmentFormats.videorecordhold1 },
            { EnvironmentFormats.modelrecordhold1 },
            { EnvironmentFormats.textpaneltap1 },
            { EnvironmentFormats.iconpaneltap1 }
        };
        #endregion ENVIRONMENT_FORMATS

        #region USER_FORMATS
        public static List<UserFormat> UserFormatsConsult = new List<UserFormat>
        {
            { UserFormats.textnone1 },
            { UserFormats.iconnone1 },
            { UserFormats.audiotap1 },
            { UserFormats.imagemanipulation1 },
            { UserFormats.videomanipulation1 },
            { UserFormats.modelmanipulation1 },
            { UserFormats.hologramnone1 },
            { UserFormats.hologramnone2 },
            { UserFormats.animationnone1 },
            { UserFormats.animationnone2 },
            { UserFormats.textbuttontap1 },
            { UserFormats.iconbuttontap1 }
        };

        public static List<UserFormat> UserFormatsReport = new List<UserFormat>
        {
            { UserFormats.textkeyboard1 },
            { UserFormats.textdictation1 },
            { UserFormats.audiorecordhold1 },
            { UserFormats.imagecordhold1 },
            { UserFormats.videorecordhold1 },
            { UserFormats.modelrecordhold1 },
            { UserFormats.textpaneltap1 },
            { UserFormats.iconpaneltap1 }
        };
        #endregion USER_FORMATS
        #endregion DICTIONARIES_FORMATS

        #region DICTIONARIES_CONFIGURATION
        
        #endregion DICTIONARIES_CONFIGURATION

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        #region DICTIONARIES_DYNAMIC
        ///// <summary>
        ///// Describe script purpose
        ///// Add links when code has been inspired
        ///// </summary>
        //public static Dictionary<RtrbauFabricationName, RtrbauAttributeType> Files = new Dictionary<RtrbauFabricationName, RtrbauAttributeType>();
        ///// <summary>
        ///// Describe script purpose
        ///// Add links when code has been inspired
        ///// </summary>
        //public static Dictionary<RtrbauFabricationName, RtrbauAttributeType> Ontologies = new Dictionary<RtrbauFabricationName, RtrbauAttributeType>();
        ///// <summary>
        ///// Describe script purpose
        ///// Add links when code has been inspired
        ///// </summary>
        //public static Dictionary<RtrbauFabricationName, RtrbauAttributeType> OntClassSubclasses = new Dictionary<RtrbauFabricationName, RtrbauAttributeType>();
        ///// <summary>
        ///// Describe script purpose
        ///// Add links when code has been inspired
        ///// </summary>
        //public static Dictionary<RtrbauFabricationName, RtrbauAttributeType> OntClassIndividuals = new Dictionary<RtrbauFabricationName, RtrbauAttributeType>();
        ///// <summary>
        ///// Describe script purpose
        ///// Add links when code has been inspired
        ///// </summary>
        //public static Dictionary<RtrbauFabricationName, RtrbauAttributeType> OntClasses = new Dictionary<RtrbauFabricationName, RtrbauAttributeType>();
        ///// <summary>
        ///// Describe script purpose
        ///// Add links when code has been inspired
        ///// </summary>
        //public static Dictionary<RtrbauFabricationName, RtrbauAttributeType> OntIndividuals = new Dictionary<RtrbauFabricationName, RtrbauAttributeType>();
        ///// <summary>
        ///// Describe script purpose
        ///// Add links when code has been inspired
        ///// </summary>
        //public static Dictionary<RtrbauFabricationName, RtrbauAttributeType> Fabrications = new Dictionary<RtrbauFabricationName, RtrbauAttributeType>();
        //    // To read from prefabs and assign accordingly (either on run-time or editor)

        #endregion DICTIONARIES_DYNAMIC

    }

}