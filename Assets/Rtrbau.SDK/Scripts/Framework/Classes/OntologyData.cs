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
using System.IO;
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
    public class Ontology
    {
        #region MEMBERS
        private string name;
        private string uri;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public Ontology()
        {
            name = null;
            uri = null;
        }

        public Ontology(OntologyStandardType standard)
        {
            if (Dictionaries.OntologyStandardTypes.TryGetValue(standard, out uri)) { name = standard.ToString(); }
            else { throw new ArgumentException("OntologyData::Ontology: OntologyStandardType not implemented."); }
        }

        public Ontology(string OntologyURI)
        {
            uri = OntologyURI;
            name = Parser.ParseURI(Parser.ParseURI(OntologyURI, '/', RtrbauParser.post), '#', RtrbauParser.pre);
        }
        #endregion CONSTRUCTORS

        #region METHODS
        public string Name() { return name; }

        public string URI() { return uri; }

        public bool EqualOntology(Ontology ontology) { return uri == ontology.URI(); }
        #endregion METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary> 
    [Serializable]
    public class OntologyEntity
    {
        #region MEMBERS
        private string name;
        private Ontology ontology;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public OntologyEntity()
        {
            name = null;
            ontology = null;
        }

        public OntologyEntity(string entityURI)
        {
            name = Parser.ParseURI(entityURI, '#', RtrbauParser.post);
            ontology = new Ontology(Parser.ParseURI(entityURI, '#', RtrbauParser.pre) + "#");
        }

        public OntologyEntity(Ontology entityOntology, string entityName)
        {
            name = entityName;
            ontology = entityOntology;
        }
        #endregion CONSTRUCTORS

        #region METHODS
        public string URI() { return ontology.URI() + name; }

        public string Entity() { return ontology.Name() + "#" + name; }

        public string Name() { return name; }

        public Ontology Ontology() { return ontology; }

        public bool EqualEntity(OntologyEntity entity) { return URI() == entity.URI(); }
        #endregion METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary> 
    [Serializable]
    public class OntologyElement : ILoadable
    {
        #region MEMBERS
        public OntologyEntity entity;
        public OntologyElementType type;
        #endregion MEMBERS

        #region CONSTRUCTOR
        public OntologyElement()
        {
            entity = new OntologyEntity();
            type = OntologyElementType.Ontologies;
        }

        public OntologyElement(string entityURI, OntologyElementType elementType)
        {
            entity = new OntologyEntity(entityURI);
            type = elementType;
        }
        #endregion CONSTRUCTOR

        #region ILOADABLE_METHODS
        public string URL()
        {
            return Parser.ParseOntElementURI(entity, type);
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

        #region CLASS_METHODS
        public bool EqualElement(OntologyElement element)
        {
            return entity.EqualEntity(element.entity) && type.Equals(element.type);
        }
        #endregion CLASS_METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary> 
    [Serializable]
    public class OntologyElementUpload : ILoadable
    {
        #region MEMBERS
        public OntologyElement individualElement;
        public OntologyElement classElement;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public OntologyElementUpload()
        {
            individualElement = new OntologyElement();
            classElement = new OntologyElement();
        }

        public OntologyElementUpload(OntologyElement elementIndividual, OntologyElement elementClass)
        {
            if (elementIndividual.entity.Ontology().Name() == elementClass.entity.Ontology().Name())
            {
                individualElement = elementIndividual;
                classElement = elementClass;
            }
            else
            {
                throw new ArgumentException("OntologyData::OntologyElementUpload: individual and class must belong to the same ontology.");
            }
        }
        #endregion CONSTRUCTORS

        #region ILOADABLE_METHODS
        public string URL()
        {
            return Parser.ParseOntElementURI(individualElement.entity, OntologyElementType.IndividualUpload);
        }

        public string FilePath()
        {
            string folder;

            if (Dictionaries.ontDataDirectories.TryGetValue(OntologyElementType.IndividualUpload, out folder)) { }
            else { throw new ArgumentException("Argument element error: ontology element type not implemented."); }

            return folder + '/' + individualElement.entity.Entity() + ".json";
        }

        public string EventName()
        {
            return "Ontology__" + OntologyElementType.IndividualUpload + "__" + individualElement.entity.Entity();
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
        #region MEMBERS
        public OntologyEntity startClass;
        public OntologyEntity endClass;
        public RtrbauDistanceType distanceType;
        #endregion MEMBERS

        #region CONSTRUCTOR
        public OntologyDistance()
        {
            startClass = new OntologyEntity();
            endClass = new OntologyEntity();
            distanceType = RtrbauDistanceType.Component;
        }

        public OntologyDistance (string startClassURI, RtrbauDistanceType distance)
        {
            startClass = new OntologyEntity(startClassURI);

            if (distance == RtrbauDistanceType.Component)
            {
                if (Rtrbauer.instance.component != null)
                {
                    endClass = Rtrbauer.instance.component;
                }
                else
                {
                    throw new ArgumentException("OntologyData::OntologyDistance::Constructor: Argument distance error: component class not declared.");
                }
            }
            else if (distance == RtrbauDistanceType.Operation)
            {
                if (Rtrbauer.instance.operation != null)
                {
                    endClass = Rtrbauer.instance.operation;
                }
                else
                {
                    throw new ArgumentException("OntologyData::OntologyDistance::Constructor: Argument distance error: operation class not declared.");
                }
            }
            else
            {
                throw new ArgumentException("OntologyData::OntologyDistance::Constructor: Argument distance error: rtrbau distance type not implemented.");
            }

            distanceType = distance;
        }
        #endregion CONSTRUCTOR

        #region ILOADABLE_METHODS
        public string URL()
        {
            return Parser.ParseOntDistURI(startClass, endClass);
        }

        public string FilePath()
        {
            string folder;

            if (Dictionaries.distanceDataDirectories.TryGetValue(distanceType, out folder)){}
            else { throw new ArgumentException("OntologyData::OntologyDistance::FilePath: Argument distance error: rtrbau distance type not implemented."); }

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
    public class OntologyFile : ILoadable
    {
        #region MEMBERS
        public string name;
        public RtrbauFileType type;
        public RtrbauAugmentation augmentation;
        private string url;
        #endregion MEMBERS

        #region CONSTRUCTOR
        public OntologyFile()
        {
            name = null;
            type = RtrbauFileType.wav;
            augmentation = RtrbauAugmentation.Registration;
            url = null;
        }

        public OntologyFile(string fileName, string fileType)
        {
            name = fileName;

            if (Enum.TryParse<RtrbauFileType>(fileType, out type)) {}
            else { throw new ArgumentException("OntologyData::OntologyFile::Constructor: Argument file error: file type not implemented."); }

            if (Dictionaries.FileAugmentations.TryGetValue(type, out augmentation)) { }
            else { throw new ArgumentException("OntologyData::OntologyFile::Constructor: Argument file error: file type not implement to an augmentation method."); }

            url = Parser.ParseFileURI(fileName, fileType);

        }

        public OntologyFile(string fileWebPath)
        {
            url = Parser.ParseURI(fileWebPath, '/', RtrbauParser.pre) + "/";
            string file = Parser.ParseURI(fileWebPath, '/', RtrbauParser.post);
            name = Parser.ParseURI(file, '.', RtrbauParser.pre);
            string fileType = Parser.ParseURI(file, '.', RtrbauParser.post);

            if (Enum.TryParse<RtrbauFileType>(fileType, out type)) {}
            else { throw new ArgumentException("OntologyData::OntologyFile::Constructor: Argument file error: file type not implemented."); }

            if (Dictionaries.FileAugmentations.TryGetValue(type, out augmentation)) { }
            else { throw new ArgumentException("OntologyData::OntologyFile::Constructor: Argument file error: file type not implement to an augmentation method."); }
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
            else { throw new ArgumentException("OntologyData::OntologyFile::FilePath: Argument file error: file type not implemented."); }

            return folder + '/' + name + '.' + type.ToString();
        }

        public string EventName()
        {
            return "File__" + name + "__" + type.ToString();
        }
        #endregion ILOADABLE_METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// UPG: To merge with <see cref="OntologyFile"/>
    /// </summary> 
    public class OntologyFileUpload: ILoadable
    {
        #region MEMBERS
        public OntologyFile file;
        private string form;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public OntologyFileUpload()
        {
            file = new OntologyFile();
            form = null;
        }

        public OntologyFileUpload(string fileName, string fileType)
        {
            file = new OntologyFile(fileName, fileType);
            form = "multipart/form-data";
        }

        public OntologyFileUpload(string filePath)
        {
            file = new OntologyFile(filePath);
            form = "multipart/form-data";
        }

        public OntologyFileUpload(OntologyFile uploadFile)
        {
            file = uploadFile;
            form = "multipart/form-data";
        }
        #endregion CONSTRUCTORS

        #region ILOADABLE_METHODS
        public string URL() { return Parser.ParseURI(file.URL(), '/', RtrbauParser.pre) + "/upload"; }

        public string FilePath() { return file.FilePath(); }

        public string EventName() { return file.EventName(); }
        #endregion ILOADABLE_METHODS

        #region CLASS_METHODS
        public string FileName() { return file.name + "." + file.type.ToString(); }

        public byte[] FileData()
        {
            if (!File.Exists(file.FilePath())) { return null; }
            else { return File.ReadAllBytes(file.FilePath()); }
        }

        public string UploadForm() { return form; }
        #endregion CLASS_METHODS
    }
    #endregion DATA_CLASSES
}
