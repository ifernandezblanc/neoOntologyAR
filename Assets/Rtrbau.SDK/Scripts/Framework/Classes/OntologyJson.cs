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
using UnityEngine.Events;
#endregion


namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    #region JSON_CLASSES
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class JsonOntologies
    {
        #region MEMBERS
        public List<JsonOntology> ontOntologies;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public JsonOntologies()
        {
            ontOntologies = new List<JsonOntology>();
        }
        #endregion CONSTRUCTORS

        #region METHODS
        #endregion METHODS

    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class JsonOntology
    {
        #region MEMBERS
        public string ontPrefix;
        public string ontUri;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public JsonOntology()
        {
            ontPrefix = null;
            ontUri = null;
        }
        #endregion CONSTRUCTORS

        #region METHODS
        #endregion METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class JsonClassSubclasses
    {
        #region MEMBERS
        public string ontClass;
        public List<JsonSubclass> ontSubclasses;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public JsonClassSubclasses()
        {
            ontClass = null;
            ontSubclasses = new List<JsonSubclass>();
        }
        #endregion CONSTRUCTORS

        #region METHODS
        #endregion METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class JsonSubclass
    {
        #region MEMBERS
        public string ontSubclass;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public JsonSubclass()
        {
            ontSubclass = null;
        }
        #endregion CONSTRUCTORS

        #region METHODS
        #endregion METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class JsonClassIndividuals
    {
        #region MEMBERS
        public string ontClass;
        public List<JsonIndividual> ontIndividuals;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public JsonClassIndividuals()
        {
            ontClass = null;
            ontIndividuals = new List<JsonIndividual>();
        }
        #endregion CONSTRUCTORS

        #region METHODS
        #endregion METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class JsonIndividual
    {
        #region MEMBERS
        public string ontIndividual;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public JsonIndividual()
        {
            ontIndividual = null;
        }
        #endregion CONSTRUCTORS

        #region METHODS
        #endregion METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class JsonClassProperties
    {
        #region MEMBERS
        public string ontClass;
        public List<JsonProperty> ontProperties;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public JsonClassProperties()
        {
            ontClass = null;
            ontProperties = new List<JsonProperty>();
        }
        #endregion CONSTRUCTORS

        #region METHODS
        #endregion METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class JsonProperty
    {
        #region MEMBERS
        public string ontName;
        public string ontRange;
        public string ontType;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public JsonProperty()
        {
            ontName = null;
            ontRange = null;
            ontType = null;
        }
        #endregion CONSTRUCTORS

        #region METHODS
        #endregion METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class JsonIndividualValues
    {
        #region MEMBERS
        public string ontIndividual;
        public string ontClass;
        public List<JsonValue> ontProperties;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public JsonIndividualValues()
        {
            ontIndividual = null;
            ontClass = null;
            ontProperties = new List<JsonValue>();
        }
        #endregion CONSTRUCTORS

        #region METHODS
        #endregion METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class JsonValue
    {
        #region MEMBERS
        public string ontName;
        public string ontValue;
        public string ontType;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public JsonValue()
        {
            ontName = null;
            ontValue = null;
            ontType = null;
        }
        #endregion CONSTRUCTORS

        #region METHODS
        #endregion METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class JsonDistance
    {
        #region MEMBERS
        public string ontStartClass;
        public string ontEndClass;
        public string ontDistance;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public JsonDistance()
        {
            ontStartClass = null;
            ontEndClass = null;
            ontDistance = null;
        }
        #endregion CONSTRUCTORS

        #region METHODS
        #endregion METHODS
    }

    #endregion JSON_CLASSES
}
