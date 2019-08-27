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
    #region DATA_CLASSES
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary> 
    [Serializable]
    public class OntologyEntity
    {
        public string name;
        public string ontology;
        private string url;

        #region CONSTRUCTOR
        public OntologyEntity(string entityURI)
        {
            string entity = Parser.ParseURI(entityURI, '/', RtrbauParser.post);
            name = Parser.ParseURI(entity, '#', RtrbauParser.post);
            ontology = Parser.ParseURI(entity, '#', RtrbauParser.pre);
            url = Parser.ParseURI(entityURI, '/', RtrbauParser.pre);
        }
        #endregion CONSTRUCTOR

        #region METHODS
        public string URI()
        {
            return url + "/" + Entity();
        }
        public string Entity()
        {
            return ontology + "#" + name;
        }
        #endregion METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary> 
    [Serializable]
    public class OntologyElement : ILoadable
    {
        public OntologyEntity entity;
        public OntologyElementType type;

        #region CONSTRUCTOR
        public OntologyElement(string entityURI, OntologyElementType elementType)
        {
            entity = new OntologyEntity(entityURI);
            type = elementType;
        }
        #endregion CONSTRUCTOR

        #region ILOADABLE_METHODS
        public string URL()
        {
            return Parser.ParseDownOntElementURI(entity.name, entity.ontology, type);
        }
        public string FilePath()
        {
            string folder;

            if (Dictionaries.ontDataDirectories.TryGetValue(type, out folder)) { }
            else { throw new ArgumentException("Argument element error: ontology element type not implemented."); }

            return folder + '/' + entity.Entity() + ".json";
        }

        public string EventName()
        {
            // return type.ToString() + "_" + type.ToString() + "_" + entity.Entity();
            return "Ontology__" + type.ToString() + "__" + entity.Entity();
        }
        #endregion ILOADABLE_METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary> 
    [Serializable]
    public class OntologyDistance : ILoadable
    {
        public OntologyEntity startClass;
        public OntologyEntity endClass;
        public RtrbauDistanceType distanceType;

        #region CONSTRUCTOR
        public OntologyDistance (string startClassURI, RtrbauDistanceType distance)
        {
            startClass = new OntologyEntity(startClassURI);

            if (distance == RtrbauDistanceType.Component)
            {
                if (Rtrbauer.instance.component.componentURI != null)
                {
                    endClass = new OntologyEntity(Rtrbauer.instance.component.componentURI);
                }
                else
                {
                    throw new ArgumentException("Argument distance error: component class not declared.");
                }
            }
            else if (distance == RtrbauDistanceType.Operation)
            {
                if (Rtrbauer.instance.operation.operationURI != null)
                {
                    endClass = new OntologyEntity(Rtrbauer.instance.operation.operationURI);
                }
                else
                {
                    throw new ArgumentException("Argument distance error: operation class not declared.");
                }
            }
            else
            {
                throw new ArgumentException("Argument distance error: rtrbau distance type not implemented.");
            }

            distanceType = distance;
        }
        #endregion CONSTRUCTOR

        #region ILOADABLE_METHODS
        public string URL()
        {
            return Parser.ParseDownOntDistURI(startClass, endClass);
        }

        public string FilePath()
        {
            string folder;

            if (Dictionaries.distanceDataDirectories.TryGetValue(distanceType, out folder)){}
            else { throw new ArgumentException("Argument distance error: rtrbau distance type not implemented."); }

            return folder + "/" + startClass.Entity() + ".json";
        }

        public string EventName()
        {
            return "Distance__" + distanceType + "__" + startClass.Entity();
        }

        #endregion ILOADABLE_METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary> 
    public class RtrbauFile : ILoadable
    {
        public string name;
        public RtrbauFileType type;
        public RtrbauAugmentation augmentation;

        private string url;

        #region CONSTRUCTOR
        public RtrbauFile(string fileName, string fileType)
        {
            name = fileName;

            if (Enum.TryParse<RtrbauFileType>(fileType, out type)) {}
            else { throw new ArgumentException("Argument file error: file type not implemented."); }

            if (Dictionaries.FileAugmentations.TryGetValue(type, out augmentation)) { }
            else { throw new ArgumentException("Argument file error: file type not implement to an augmentation method."); }

            url = Parser.ParseDownFileURI(fileName, fileType);

        }

        public RtrbauFile(string filePath)
        {
            url = Parser.ParseURI(filePath, '/', RtrbauParser.pre) + "/";
            string file = Parser.ParseURI(filePath, '/', RtrbauParser.post);
            name = Parser.ParseURI(file, '.', RtrbauParser.pre);
            string fileType = Parser.ParseURI(file, '.', RtrbauParser.post);

            if (Enum.TryParse<RtrbauFileType>(fileType, out type)) {}
            else { throw new ArgumentException("Argument file error: file type not implemented."); }

            if (Dictionaries.FileAugmentations.TryGetValue(type, out augmentation)) { }
            else { throw new ArgumentException("Argument file error: file type not implement to an augmentation method."); }
        }
        #endregion CONSTRUCTOR

        #region ILOADABLE_METHODS
        public string URL()
        {
            return url + name + '.' + type.ToString();
        }

        public string FilePath()
        {
            string folder;

            if (Dictionaries.fileDataDirectories.TryGetValue(type, out folder)) { }
            else { throw new ArgumentException("Argument file error: file type not implemented."); }

            return folder + '/' + name + '.' + type.ToString();
        }

        public string EventName()
        {
            return "File__" + name + "__" + type.ToString();
        }
        #endregion ILOADABLE_METHODS
    }
    #endregion DATA_CLASSES
}
