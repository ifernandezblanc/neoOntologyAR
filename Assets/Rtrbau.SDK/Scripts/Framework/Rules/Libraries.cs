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
        #region NAME_LIBRARIES
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> PositionXLibrary = new List<string>
        {
            "X","Location"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> PositionYLibrary = new List<string>
        {
            "Y", "Location"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> PositionZLibrary = new List<string>
        {
            "Z", "Location"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> RotationXLibrary = new List<string>
        {
            "X","Rotation"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> RotationYLibrary = new List<string>
        {
            "Y","Rotation"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> RotationZLibrary = new List<string>
        {
            "Z","Rotation"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementTranslationXLibrary = new List<string>
        {
            "X", "Translation", "Movement"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> FreedomDegreeTranslationXLibrary = new List<string>
        {
            "X", "Translation", "FreedomDegree"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementTranslationYLibrary = new List<string>
        {
            "Y", "Translation", "Movement"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> FreedomDegreeTranslationYLibrary = new List<string>
        {
            "Y", "Translation", "FreedomDegree"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementTranslationZLibrary = new List<string>
        {
            "Z", "Translation", "Movement"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> FreedomDegreeTranslationZLibrary = new List<string>
        {
            "Z", "Translation", "FreedomDegree"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementRotationXLibrary = new List<string>
        {
            "X", "Rotation", "Movement"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> FreedomDegreeRotationXLibrary = new List<string>
        {
            "X", "Rotation", "FreedomDegree"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementRotationYLibrary = new List<string>
        {
            "Y", "Rotation", "Movement"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> FreedomDegreeRotationYLibrary = new List<string>
        {
            "Y", "Rotation", "FreedomDegree"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementRotationZLibrary = new List<string>
        {
            "Z", "Rotation", "Movement"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> FreedomDegreeRotationZLibrary = new List<string>
        {
            "Z", "Rotation", "FreedomDegree"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementSpeedLibrary = new List<string>
        {
            "Movement", "Speed"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementInverseLibrary = new List<string>
        {
            "Movement", "Inverse"
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> MovementPairLibrary = new List<string>
        {
            "pair"
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
        public static List<string> SetLibrary = new List<string>
        {
            OntologyElementType.ClassIndividuals.ToString()
        };

        
        #endregion RANGE_LIBRARIES

        #region VALUE_LIBRARIES
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> AudioFileLibrary = new List<string>
        {
            RtrbauFileType.wav.ToString()
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> ModelFileLibrary = new List<string>
        {
            RtrbauFileType.obj.ToString()
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> PictureFileLibrary = new List<string>
        {
            RtrbauFileType.jpg.ToString(), RtrbauFileType.png.ToString()
        };

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static List<string> VideoFileLibrary = new List<string>
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
        public static List<string> IndividualLibrary = new List<string>
        {
            "#"
        };
        #endregion VALUE_LIBRARIES
        #endregion LIBRARIES
    }
}
