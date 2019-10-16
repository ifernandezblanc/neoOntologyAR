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
            {"boolean",typeof(bool)},
            {"dateTime", typeof(DateTime)}
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
            {OntologyElementType.ClassExample, Application.persistentDataPath + "/Rtrbau/ontologies/classes-example" },
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
        #region CONSULT_FORMATS
        public static List<Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>> ConsultFormats
            = new List<Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>>
            {
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.TextNone1, DataFormats.TextNone1, EnvironmentFormats.TextNone1, UserFormats.TextNone1),
                //new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                //(RtrbauFabricationName.TextNone2, DataFormats.TextNone2, EnvironmentFormats.TextNone2, UserFormats.TextNone2),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.IconNone1, DataFormats.IconNone1, EnvironmentFormats.IconNone1, UserFormats.IconNone1),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.AudioTap1, DataFormats.AudioTap1, EnvironmentFormats.AudioTap1, UserFormats.AudioTap1),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.ImageManipulation1, DataFormats.ImageManipulation1, EnvironmentFormats.ImageManipulation1, UserFormats.ImageManipulation1),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.VideoManipulation1, DataFormats.VideoManipulation1, EnvironmentFormats.VideoManipulation1, UserFormats.VideoManipulation1),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.ModelManipulation1, DataFormats.ModelManipulation1, EnvironmentFormats.ModelManipulation1, UserFormats.ModelManipulation1),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.HologramNone1, DataFormats.HologramNone1, EnvironmentFormats.HologramNone1, UserFormats.HologramNone1),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.HologramNone2, DataFormats.HologramNone2, EnvironmentFormats.HologramNone2, UserFormats.HologramNone2),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.AnimationNone1, DataFormats.AnimationNone1, EnvironmentFormats.AnimationNone1, UserFormats.AnimationNone1),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.AnimationNone2, DataFormats.AnimationNone2, EnvironmentFormats.AnimationNone2, UserFormats.AnimationNone2),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.TextButtonTap1, DataFormats.TextButtonTap1, EnvironmentFormats.TextButtonTap1, UserFormats.TextButtonTap1),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.IconButtonTap1, DataFormats.IconButtonTap1, EnvironmentFormats.IconButtonTap1, UserFormats.IconButtonTap1)
            };
        #endregion CONSULT_FORMATS

        #region REPORT_FORMATS
        public static List<Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>> ReportFormats
            = new List<Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>>
            {
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.TextDictation1, DataFormats.TextDictation1, EnvironmentFormats.TextDictation1, UserFormats.TextDictation1),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.TextKeyboard1, DataFormats.TextKeyboard1, EnvironmentFormats.TextKeyboard1, UserFormats.TextKeyboard1),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.AudioRecordHold1, DataFormats.AudioRecordHold1, EnvironmentFormats.AudioRecordHold1, UserFormats.AudioRecordHold1),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.ImageRecordHold1, DataFormats.ImageRecordHold1, EnvironmentFormats.ImageRecordHold1, UserFormats.ImageRecordHold1),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.VideoRecordHold1, DataFormats.VideoRecordHold1, EnvironmentFormats.VideoRecordHold1, UserFormats.VideoRecordHold1),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.ModelRecordHold1, DataFormats.ModelRecordHold1, EnvironmentFormats.ModelRecordHold1, UserFormats.ModelRecordHold1),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.TextPanelTap1, DataFormats.TextPanelTap1, EnvironmentFormats.TextPanelTap1, UserFormats.TextPanelTap1),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.IconPanelTap1, DataFormats.IconPanelTap1, EnvironmentFormats.IconPanelTap1, UserFormats.IconPanelTap1),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.TextNone2, DataFormats.TextNone2, EnvironmentFormats.TextNone2, UserFormats.TextNone2),
                new Tuple<RtrbauFabricationName, DataFormat, EnvironmentFormat, UserFormat>
                (RtrbauFabricationName.TextButtonTap2, DataFormats.TextButtonTap2, EnvironmentFormats.TextButtonTap2, UserFormats.TextButtonTap2)
            };
        #endregion REPORT_FORMATS


        // UPG: Add new formats to both dictionaries
        //#region DATA_FORMATS
        //public static List<DataFormat> DataFormatsConsult = new List<DataFormat>
        //{
        //    { DataFormats.TextNone1 },
        //    { DataFormats.IconNone1 },
        //    { DataFormats.AudioTap1 },
        //    { DataFormats.ImageManipulation1 },
        //    { DataFormats.VideoManipulation1 },
        //    { DataFormats.ModelManipulation1 },
        //    { DataFormats.HologramNone1 },
        //    { DataFormats.HologramNone2 },
        //    { DataFormats.AnimationNone1 },
        //    { DataFormats.AnimationNone2 },
        //    { DataFormats.TextButtonTap1 },
        //    { DataFormats.IconButtonTap1 }
        //};

        //public static List<DataFormat> DataFormatsReport = new List<DataFormat>
        //{
        //    { DataFormats.TextDictation1 },
        //    { DataFormats.TextKeyboard1 },
        //    { DataFormats.AudioRecordHold1 },
        //    { DataFormats.ImageRecordHold1 },
        //    { DataFormats.VideoRecordHold1 },
        //    { DataFormats.ModelRecordHold1 },
        //    { DataFormats.TextPanelTap1 },
        //    { DataFormats.IconPanelTap1 }
        //};
        //#endregion DATA_FORMATS

        //#region ENVIRONMENT_FORMATS
        //public static List<EnvironmentFormat> EnvironmentFormatsConsult = new List<EnvironmentFormat>
        //{
        //    { EnvironmentFormats.TextNone1 },
        //    { EnvironmentFormats.IconNone1 },
        //    { EnvironmentFormats.AudioTap1 },
        //    { EnvironmentFormats.ImageManipulation1 },
        //    { EnvironmentFormats.VideoManipulation1 },
        //    { EnvironmentFormats.ModelManipulation1 },
        //    { EnvironmentFormats.HologramNone1 },
        //    { EnvironmentFormats.HologramNone2 },
        //    { EnvironmentFormats.AnimationNone1 },
        //    { EnvironmentFormats.AnimationNone2 },
        //    { EnvironmentFormats.TextButtonTap1 },
        //    { EnvironmentFormats.IconButtonTap1 }
        //};

        //public static List<EnvironmentFormat> EnvironmentFormatsReport = new List<EnvironmentFormat>
        //{
        //    { EnvironmentFormats.TextKeyboard1 },
        //    { EnvironmentFormats.TextDictation1 },
        //    { EnvironmentFormats.AudioRecordHold1 },
        //    { EnvironmentFormats.ImageRecordHold1 },
        //    { EnvironmentFormats.VideoRecordHold1 },
        //    { EnvironmentFormats.ModelRecordHold1 },
        //    { EnvironmentFormats.TextPanelTap1 },
        //    { EnvironmentFormats.IconPanelTap1 }
        //};
        //#endregion ENVIRONMENT_FORMATS

        //#region USER_FORMATS
        //public static List<UserFormat> UserFormatsConsult = new List<UserFormat>
        //{
        //    { UserFormats.TextNone1 },
        //    { UserFormats.IconNone1 },
        //    { UserFormats.AudioTap1 },
        //    { UserFormats.ImageManipulation1 },
        //    { UserFormats.VideoManipulation1 },
        //    { UserFormats.ModelManipulation1 },
        //    { UserFormats.HologramNone1 },
        //    { UserFormats.HologramNone2 },
        //    { UserFormats.AnimationNone1 },
        //    { UserFormats.AnimationNone2 },
        //    { UserFormats.TextButtonTap1 },
        //    { UserFormats.IconButtonTap1 }
        //};

        //public static List<UserFormat> UserFormatsReport = new List<UserFormat>
        //{
        //    { UserFormats.TextKeyboard1 },
        //    { UserFormats.TextDictation1 },
        //    { UserFormats.AudioRecordHold1 },
        //    { UserFormats.ImageRecordHold1 },
        //    { UserFormats.VideoRecordHold1 },
        //    { UserFormats.ModelRecordHold1 },
        //    { UserFormats.TextPanelTap1 },
        //    { UserFormats.IconPanelTap1 }
        //};
        //#endregion USER_FORMATS
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