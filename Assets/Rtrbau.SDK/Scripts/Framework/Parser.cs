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

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public static class Parser
    {
        #region ELEMENT_PARSERS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static Type ParseOntElementType()
        {
            return null;
        }
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static Type ParseOntPropertyType()
        {
            return null;
        }
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static Type ParseOntValueType()
        {
            return null;
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        //public static OntologyEntity ParseOntElement(string uri)
        //{
        //    OntologyEntity element = new OntologyEntity();

        //    string url;

        //    element.name = Parser.ParseURI(uri, '#', ParsingType.post);

        //    url = Parser.ParseURI(uri, '#', ParsingType.pre);

        //    element.ontology = Parser.ParseURI(url, '/', ParsingType.post);

        //    return element;

        //}
        #endregion ELEMENT_PARSERS

        #region URI_PARSERS

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static string ParseURI(string uri, char parser, RtrbauParser parsing)
        {
            int uriIndex = uri.LastIndexOf(parser);

            if (uriIndex != -1)
            {
                if (parsing == RtrbauParser.pre)
                {
                    return uri.Substring(0, uriIndex);
                }
                else if (parsing == RtrbauParser.post)
                {
                    return uri.Substring(uriIndex + 1);
                }
                else
                {
                    throw new ArgumentException("Argument Error: parsing method invalid");
                }
            }
            else
            {
                throw new ArgumentException("Argument Error: uri structure not implemented");
            }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static string ParseDownOntElementURI(string name, string ontology, OntologyElementType type)
        {
            if (type == OntologyElementType.Ontologies)
            {
                return Rtrbauer.instance.server.serverURI + "api/ontologies/";
            }
            else if (type == OntologyElementType.ClassSubclasses)
            {
                return Rtrbauer.instance.server.serverURI + "api/ontologies/" + ontology + "/class/" + name + "/subclasses/";
            }
            else if (type == OntologyElementType.ClassIndividuals)
            {
                return Rtrbauer.instance.server.serverURI + "api/ontologies/" + ontology + "/class/" + name + "/individuals/";
            }
            else if (type == OntologyElementType.ClassProperties)
            {
                return Rtrbauer.instance.server.serverURI + "api/ontologies/" + ontology + "/class/" + name + "/properties/";
            }
            else if (type == OntologyElementType.IndividualProperties)
            {
                return Rtrbauer.instance.server.serverURI + "api/ontologies/" + ontology + "/individual/" + name + "/properties/";
            }            
            else
            {
                throw new ArgumentException("Argument Error: ontology element type not implemented");
            }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static string ParseDownOntDistURI(OntologyEntity startClass, OntologyEntity endClass)
        {
            return Rtrbauer.instance.server.serverURI + "api/ontologies/" + startClass.ontology + "/class/" + 
                startClass.name + "/distance/" + startClass.ontology + "/" + endClass.ontology + "/class/" + endClass.name;
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static string ParseDownFileURI(string name, string type)
        {
            RtrbauFileType filetype;

            if (Enum.TryParse<RtrbauFileType>(type, out filetype))
            {
                return Rtrbauer.instance.server.serverURI + "api/files/" + type + "/";
            }
            else
            {
                throw new ArgumentException("Argument Error: file type not implemented");
            }
        }

        #endregion URI_PARSERS

    }
}