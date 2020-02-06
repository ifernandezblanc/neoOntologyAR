/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 07/08/2019
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
using System.Linq;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public static class Libraries
    {
        #region LIBRARIES
        #region DATA_LIBRARIES
        #region NAME_LIBRARIES
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> PositionX = new List<string>
        {
            "X","Location"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> PositionY = new List<string>
        {
            "Y", "Location"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> PositionZ = new List<string>
        {
            "Z", "Location"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> RotationX = new List<string>
        {
            "X","Rotation"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> RotationY = new List<string>
        {
            "Y","Rotation"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> RotationZ = new List<string>
        {
            "Z","Rotation"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementTranslationX = new List<string>
        {
            "X", "Translation", "Movement"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> FreedomDegreeTranslationX = new List<string>
        {
            "X", "Translation", "FreedomDegree"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementTranslationY = new List<string>
        {
            "Y", "Translation", "Movement"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> FreedomDegreeTranslationY = new List<string>
        {
            "Y", "Translation", "FreedomDegree"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementTranslationZ = new List<string>
        {
            "Z", "Translation", "Movement"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> FreedomDegreeTranslationZ = new List<string>
        {
            "Z", "Translation", "FreedomDegree"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementRotationX = new List<string>
        {
            "X", "Rotation", "Movement"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> FreedomDegreeRotationX = new List<string>
        {
            "X", "Rotation", "FreedomDegree"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementRotationY = new List<string>
        {
            "Y", "Rotation", "Movement"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> FreedomDegreeRotationY = new List<string>
        {
            "Y", "Rotation", "FreedomDegree"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementRotationZ = new List<string>
        {
            "Z", "Rotation", "Movement"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> FreedomDegreeRotationZ = new List<string>
        {
            "Z", "Rotation", "FreedomDegree"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementSpeed = new List<string>
        {
            "Movement", "Speed"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementInverse = new List<string>
        {
            "Movement", "Inverse"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementPair = new List<string>
        {
            "pair"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired 
        /// </summary>
        public static List<string> MessageType = new List<string>
        {
            "Message", "Type"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired 
        /// </summary>
        public static List<string> MessageAction = new List<string>
        {
            "Message", "Action"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired 
        /// </summary>
        public static List<string> MessageObject = new List<string>
        {
            "Message", "Object"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired 
        /// </summary>
        public static List<string> MessageValue = new List<string>
        {
            "Message", "Value"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired 
        /// </summary>
        public static List<string> MessageUnit = new List<string>
        {
            "Message", "Unit"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired 
        /// </summary>
        public static List<string> TimeRecord = new List<string>
        {
            "Start", "End", "State"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired 
        /// </summary>
        public static List<string> StateIdeal = new List<string>
        {
            "Diagnoses", "State"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired 
        /// </summary>
        public static List<string> StateNone = new List<string>
        {
            "Evaluates", "Monitors"
        };
        #endregion NAME_LIBRARIES

        #region RANGE_LIBRARIES
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> URILibrary = new List<string>
        {
            "anyURI"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> StringLibrary = new List<string>
        {
            "language", "Name", "NCName", "NMTOKEN", "normalizedString",
            "string", "token"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> NumericLibrary = new List<string>
        {
            "decimal", "double", "float", "int", "integer", "long",
            "negativeInteger", "nonPositiveInteger", "positiveInteger"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> BooleanLibrary = new List<string>
        {
            "boolean"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> DateTimeLibrary = new List<string>
        {
            "dateTime","dateTimeStamp"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> TextLibrary = StringLibrary.Concat(NumericLibrary).Concat(BooleanLibrary).Concat(DateTimeLibrary).ToList();

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired 
        /// </summary>
        public static List<string> StateLibrary = new List<string>
        {
            "State"
        };

        ///// <summary>
        ///// Describe script purpose
        ///// Add links when code has been inspired
        ///// </summary>
        //public static List<string> SetLibrary = new List<string>
        //{
        //    OntologyElementType.ClassIndividuals.ToString()
        //};


        #endregion RANGE_LIBRARIES

        #region VALUE_LIBRARIES
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> AudioFile = new List<string>
        {
            RtrbauFileType.wav.ToString()
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> ModelFile = new List<string>
        {
            RtrbauFileType.obj.ToString()
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> PictureFile = new List<string>
        {
            RtrbauFileType.jpg.ToString(), RtrbauFileType.png.ToString()
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> VideoFile = new List<string>
        {
            RtrbauFileType.mp4.ToString()
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> IconLibrary = new List<string>
        {
            "move", "disassemble", "assemble", "install", "remove",
            "replace", "hold", "tighten", "loosen", "examine", "lift",
            "push", "pull", "turn counter clockwise", "turn clockwise",
            "fill", "drain", "close", "open", "cut", "clean", "photograph",
            "measure", "screw", "unscrew"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> HologramLibrary = new List<string>
        {
            "arrow", "circle",
            // "square", "triangle"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementLibrary = new List<string>
        {
            "linear", "conical", "cylindrical", "helical", "planar", "prismatical", "spherical"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementRestrictionLibrary = new List<string>
        {
            "coincidence", "disjoint", "inclusion", "intersection", "parallel", "perpendicular"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> DatatypePropertyLibrary = new List<string>
        {
            OntologyPropertyType.DatatypeProperty.ToString()
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> ObjectPropertyLibrary = new List<string>
        {
            OntologyPropertyType.ObjectProperty.ToString()
        };
        #endregion VALUE_LIBRARIES
        #endregion DATA_LIBRARIES

        #region RECOMMENDATION_LIBRARIES
        #region ATTRIBUTE_LIBRARIES
        public static List<OntologyEntity> RecommendationStateAttributesLibrary = new List<OntologyEntity> 
        {
            new OntologyEntity("http://138.250.108.1:3003/api/files/owl/diagont#stepDiagnosesState"),
            new OntologyEntity("http://138.250.108.1:3003/api/files/owl/diagont#hasStateStatus"),
            new OntologyEntity("http://138.250.108.1:3003/api/files/owl/diagont#hasStateDominion"),
            new OntologyEntity("http://138.250.108.1:3003/api/files/owl/diagont#hasStatePhenomenon"),
            new OntologyEntity("http://138.250.108.1:3003/api/files/owl/diagont#hasMeasureUnit"),
            new OntologyEntity("http://138.250.108.1:3003/api/files/owl/diagont#refersToComponent")
        };
        #endregion ATTRIBUTE_LIBRARIES

        #region RANGE_LIBRARIES
        public static List<OntologyEntity> RecommendationStateRangesLibrary = new List<OntologyEntity>
        {
            new OntologyEntity("http://138.250.108.1:3003/api/files/owl/diagont#State"),
            new OntologyEntity("http://138.250.108.1:3003/api/files/owl/diagont#Status"),
            new OntologyEntity("http://138.250.108.1:3003/api/files/owl/diagont#Dominion"),
            new OntologyEntity("http://138.250.108.1:3003/api/files/owl/diagont#Phenomenon"),
            new OntologyEntity("http://138.250.108.1:3003/api/files/owl/diagont#Unit"),
            new OntologyEntity("http://138.250.108.1:3003/api/files/owl/orgont#Component")
        };
        #endregion RANGE_LIBRARIES

        #region SUBSET_LIBRARIES
        public static Dictionary<decimal, List<string>> RecommendationStateStatusSubsetsLibrary = new Dictionary<decimal, List<string>>
        {
            {1.0M, new List<string>(){"http://138.250.108.1:3003/api/files/owl/diagont#Faulty"}},
            {0.5M, new List<string>(){"http://138.250.108.1:3003/api/files/owl/diagont#SafelyDegraded", "http://138.250.108.1:3003/api/files/owl/diagont#UnsafelyDegraded"}},
            {0.0M, new List<string>(){"http://138.250.108.1:3003/api/files/owl/diagont#Normal"}},
        };
        #endregion SUBSET_LIBRARIES
        #endregion RECOMMENDATION_LIBRARIES
        #endregion LIBRARIES
    }
}
