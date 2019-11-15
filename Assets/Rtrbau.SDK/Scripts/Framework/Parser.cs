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
        public static string ParseNamingDateTime()
        {
            return DateTimeOffset.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss''zz");
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static string ParseNamingDateTimeXSD()
        {
            return DateTimeOffset.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss''zzz");
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static string ParseNamingDateTimeXSD(DateTimeOffset time)
        {
            return time.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss''zzz");
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static string ParseNamingNew()
        {
            // UPG: to modify returned to make it more robust
            return "_New";
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static string ParseAddDateTime(string uri)
        {
            return uri + "_" + ParseNamingDateTime();
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static string ParseAddDateTimeXSD(string uri)
        {
            return uri + "_" + ParseNamingDateTimeXSD();
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static string ParseAddNew(string uri)
        {
            return uri + "_" + ParseNamingNew();
        }
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
                    //Debug.Log("Parser::ParseURI: pre: " + uri.Substring(0, uriIndex));
                    return uri.Substring(0, uriIndex);
                    
                }
                else if (parsing == RtrbauParser.post)
                {
                    //Debug.Log("Parser::ParseURI: post: " + uri.Substring(uriIndex + 1));
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
        public static string ParseOntElementURI(OntologyEntity entity, OntologyElementType type)
        {
            if (type == OntologyElementType.Ontologies)
            {
                return Rtrbauer.instance.server.AbsoluteUri + "api/ontologies/";
            }
            else if (type == OntologyElementType.ClassSubclasses)
            {
                return Rtrbauer.instance.server.AbsoluteUri + "api/ontologies/" + entity.Ontology().Name() + "/class/" + entity.Name() + "/subclasses/";
            }
            else if (type == OntologyElementType.ClassIndividuals)
            {
                return Rtrbauer.instance.server.AbsoluteUri + "api/ontologies/" + entity.Ontology().Name() + "/class/" + entity.Name() + "/individuals/";
            }
            else if (type == OntologyElementType.ClassProperties)
            {
                return Rtrbauer.instance.server.AbsoluteUri + "api/ontologies/" + entity.Ontology().Name() + "/class/" + entity.Name() + "/properties/";
            }
            else if (type == OntologyElementType.ClassExample)
            {
                return Rtrbauer.instance.server.AbsoluteUri + "api/ontologies/" + entity.Ontology().Name() + "/class/" + entity.Name() + "/example/";
            }
            else if (type == OntologyElementType.IndividualProperties)
            {
                return Rtrbauer.instance.server.AbsoluteUri + "api/ontologies/" + entity.Ontology().Name() + "/individual/" + entity.Name() + "/properties/";
            }
            else if (type == OntologyElementType.IndividualUpload)
            {
                return Rtrbauer.instance.server.AbsoluteUri + "api/ontologies/" + entity.Ontology().Name() + "/individual/" + entity.Name() + "/input/";
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
        public static string ParseOntDistURI(OntologyEntity startClass, OntologyEntity endClass)
        {
            return Rtrbauer.instance.server.AbsoluteUri + "api/ontologies/" + startClass.Ontology().Name() + "/class/" + 
                startClass.Name() + "/distance/" + startClass.Ontology().Name() + "/" + endClass.Ontology().Name() + "/class/" + endClass.Name();
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public static string ParseFileURI(string name, string type)
        {
            RtrbauFileType filetype;

            if (Enum.TryParse<RtrbauFileType>(type, out filetype))
            {
                return Rtrbauer.instance.server.AbsoluteUri + "api/files/" + type + "/";
            }
            else
            {
                throw new ArgumentException("Argument Error: file type not implemented");
            }
        }
        #endregion URI_PARSERS

    }
}