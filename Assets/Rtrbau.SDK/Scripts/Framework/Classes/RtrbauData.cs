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

#region NAMESPACES
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
#endregion


namespace Rtrbau
{
    #region RTRBAU_ELEMENTS
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class RtrbauAttribute
    {
        #region MEMBERS
        public OntologyEntity attributeName;
        public OntologyEntity attributeRange;
        public string attributeValue;
        public OntologyEntity attributeType;
        public RtrbauFabricationType fabricationType;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public RtrbauAttribute()
        {
            attributeName = new OntologyEntity();
            attributeRange = new OntologyEntity();
            attributeValue = null;
            attributeType = new OntologyEntity();
            fabricationType = RtrbauFabricationType.Observe;
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        /// <param name="name"></param>
        /// <param name="range"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public RtrbauAttribute(OntologyEntity name, OntologyEntity range, string value, OntologyEntity type, RtrbauFabricationType fabrication)
        {
            attributeName = name;
            attributeRange = range;
            attributeValue = value;
            attributeType = type;
            fabricationType = fabrication;
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
    public class RtrbauElement
    {
        #region MEMBERS
        public OntologyEntity elementName;
        public OntologyEntity elementClass;
        public List<RtrbauAttribute> elementAttributes;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public RtrbauElement()
        {
            elementName = new OntologyEntity();
            elementClass = new OntologyEntity();
            elementAttributes = new List<RtrbauAttribute>();
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        /// <param name="nameEntity"></param>
        /// <param name="classEntity"></param>
        /// <param name="attributes"></param>
        public RtrbauElement(OntologyEntity nameEntity, OntologyEntity classEntity, List<RtrbauAttribute> attributes)
        {
            elementName = nameEntity;
            elementClass = classEntity;
            elementAttributes = attributes;
        }

        /// <summary>
        /// Assumes <see cref="RtrbauElementType.Consult"/> for <see cref="User.procedure"/>.
        /// Discards properties in <paramref name="individualElement"/> not found in <paramref name="classElement"/>.
        /// Evaluates properties in <paramref name="individualElement"/> and adapts them accordingly to algorithm.
        /// </summary>
        /// <param name="individualElement"></param>
        /// <param name="classElement"></param>
        /// <param name="relationshipClassesElements"></param>
        public RtrbauElement(RtrbauElementType elementType, AssetManager assetManager, JsonIndividualValues individualElement, JsonClassProperties classElement, List<JsonClassProperties> relationshipClassesElements)
        {
            // IMP: to merge both class and invidual ontology elements into a single one to evaluate attributes for augmentation
            // IMP: drops all properties that cannot be found in the matching class
            // IMP: evaluates attributes and adapts them according to algorithm if necessary
            // IMP: this assumes user.procedure == RtrbauElementType.Consult
            // UPG: improve server to find properties ranges on its own so there is no need to match with a class

            if (elementType == RtrbauElementType.Consult)
            {
                // RTRBAU ALGORITHM: Loops 1 and 2 combined
                if (classElement.ontClass == individualElement.ontClass)
                {
                    elementName = new OntologyEntity(individualElement.ontIndividual);
                    elementClass = new OntologyEntity(classElement.ontClass);
                    elementAttributes = new List<RtrbauAttribute>();

                    foreach (JsonProperty classAttribute in classElement.ontProperties)
                    {
                        List<JsonValue> individualAttributes = individualElement.ontProperties.FindAll(delegate (JsonValue individualValue) { return individualValue.ontName == classAttribute.ontName; });

                        foreach (JsonValue individualAttribute in individualAttributes)
                        {
                            // Avoid checking names, that was checked before

                            if (individualAttribute.ontType == classAttribute.ontType)
                            {
                                // Assign variables to generate a new RtrbauAttribute
                                OntologyEntity attributeName = new OntologyEntity(classAttribute.ontName);
                                OntologyEntity attributeRange;
                                string attributeValue;
                                OntologyEntity attributeType;
                                RtrbauFabricationType fabricationType;

                                Debug.Log("RtrbauData::RtrbauElement::Constructor: component class is: " + Rtrbauer.instance.component.URI());
                                Debug.Log("RtrbauData::RtrbauElement::Constructor: attribute class is: " + classAttribute.ontRange);

                                // When component distance = 0, then assign individual as obj file
                                if (classAttribute.ontRange == Rtrbauer.instance.component.URI())
                                {
                                    OntologyEntity individualValue = new OntologyEntity(individualAttribute.ontValue);
                                    // Changed way in which components are found in the scene
                                    // It is dependent on visualiser being unique in the scene
                                    // UPG: to adapt for when visualiser won't be unique
                                    string componentName = assetManager.FindAssetComponent(individualValue.Name());
                                    Debug.Log("RtrbauData::RtrbauElement: componentName is" + componentName);
                                    Debug.Log("RtrbauData::RtrbauElement: individualValue is" + individualValue.Name());
                                    // But only when the component is found in the scene
                                    // In that case, it is assumed the obj file was taken from the current server
                                    // Otherwise, it is already known the attribute is of object type: so fabrication type should be inspect
                                    // if (GameObject.Find(individualValue.name) != null)
                                    if (componentName == individualValue.Name())
                                    {
                                        attributeRange = new OntologyEntity(Rtrbauer.instance.xsd.URI() + "anyURI");
                                        attributeValue = Rtrbauer.instance.server.AbsoluteUri + "api/files/obj/" + individualValue.Name() + ".obj";
                                        attributeType = new OntologyEntity(Rtrbauer.instance.owl.URI() + "DatatypeProperty");
                                        fabricationType = RtrbauFabricationType.Observe;
                                    }
                                    else
                                    {
                                        attributeRange = new OntologyEntity(classAttribute.ontRange);
                                        attributeValue = individualAttribute.ontValue;
                                        attributeType = new OntologyEntity(classAttribute.ontType);
                                        fabricationType = RtrbauFabricationType.Inspect;
                                        // Modified as per stated above: components unless found in scene should be of inspect type
                                        // attributeValue = individualValue.ToString();
                                        // attributeType = RtrbauFabricationType.Observe;
                                    }
                                }
                                else if (classAttribute.ontType.Contains(OntologyPropertyType.DatatypeProperty.ToString()))
                                {
                                    // Declare attribute as to observe when property of datatype
                                    // Those object properties that point to empty classes, declare as to observe
                                    attributeRange = new OntologyEntity(classAttribute.ontRange);
                                    attributeValue = individualAttribute.ontValue;
                                    attributeType = new OntologyEntity(classAttribute.ontType);
                                    fabricationType = RtrbauFabricationType.Observe;

                                }
                                else if (classAttribute.ontType.Contains(OntologyPropertyType.ObjectProperty.ToString()))
                                {
                                    // Declare attribute as to inspect when property of object type
                                    // Unless the individual being pointed belongs to an empty class, which is then meant to observe
                                    // This specific case is to control object properties that are used as pre-defined datasets
                                    JsonClassProperties objectPropertyClass = relationshipClassesElements.Find(delegate (JsonClassProperties objectProperty) { return objectProperty.ontClass == classAttribute.ontRange; });
                                    Debug.Log("RtrbauData::RtrbauElement: Event error:" + classAttribute.ontRange);
                                    if (objectPropertyClass.ontProperties.Count != 0)
                                    {
                                        Debug.Log("RtrbauData::RtrbauElement: " + objectPropertyClass.ontClass);
                                        attributeRange = new OntologyEntity(classAttribute.ontRange);
                                        attributeValue = individualAttribute.ontValue;
                                        attributeType = new OntologyEntity(classAttribute.ontType);
                                        fabricationType = RtrbauFabricationType.Inspect;
                                    }
                                    else
                                    {
                                        // IMPORTANT: this changes the attribute range to string to allow icon creation
                                        // However, this only applies to consult elements
                                        // Report elements may require a new consult type
                                        attributeRange = new OntologyEntity(Rtrbauer.instance.xsd.URI() + "string");
                                        // attributeRange = new OntologyEntity(classAttribute.ontRange);
                                        attributeValue = Parser.ParseURI(individualAttribute.ontValue, '#', RtrbauParser.post);
                                        attributeType = new OntologyEntity(Rtrbauer.instance.owl.URI() + "DatatypeProperty");
                                        fabricationType = RtrbauFabricationType.Observe;
                                    }
                                }
                                else
                                {
                                    throw new ArgumentException("RtrbauData::RtrbauElement: Attribute type is not implemented in Rtrbau");
                                }

                                elementAttributes.Add(new RtrbauAttribute(attributeName, attributeRange, attributeValue, attributeType, fabricationType));
                                Debug.Log("RtrbauData::RtrbauElement: " + attributeName.Name() + " " + attributeRange.Name() + " " + attributeValue + " " + attributeType.Name());
                            }
                            else
                            {
                                throw new ArgumentException("RtrbauData::RtrbauElement: Attribute type does not coincide");
                            }
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("RtrbauData::RtrbauElement: Individual does not belong to Class");
                }
            }
            else
            {
                throw new ArgumentException("RtrbauData::RtrbauElement: this declaration does not implement " + elementType.ToString() + " elements");
            }

        }

        /// <summary>
        /// Assumes <see cref="RtrbauElementType.Report"/> for <see cref="User.procedure"/>.
        /// Evaluates properties in <paramref name="classElement"/> according to <paramref name="exampleElement"/> and adapts them accordingly to algorithm.
        /// Additional evaluation according to individuals referenced through object properties are made directly by fabrications and consequent rationale.
        /// Maintains <see cref="RtrbauAttribute"/> class invariable.
        /// </summary>
        /// <param name="assetManager"></param>
        /// <param name="classElement"></param>
        /// <param name="exampleElement"></param>
        public RtrbauElement(RtrbauElementType elementType, AssetManager assetManager, OntologyElement individualElement, JsonClassProperties classElement, JsonIndividualValues exampleElement)
        {
            if (elementType == RtrbauElementType.Report)
            {
                // Remember to add in here the new individual name: so far: className + dateTime
                // To be possible to change it afterwards, also attributes: to be deleted and re-updated using fabrications

                // RTRBAU ALGORITHM: Loops 1 and 2 combined
                // UPG::ErrorHandling: modify to cope with the possibility that example does not exist or covers all attributes
                if (classElement.ontClass == exampleElement.ontClass)
                {
                    // Initialise element variables
                    elementName = individualElement.entity;
                    elementClass = new OntologyEntity(classElement.ontClass);
                    elementAttributes = new List<RtrbauAttribute>();

                    Debug.Log("RtrbauData::RtrbauElement: parsed individual name is " + elementName.Name() + " from class " + elementClass.Name());

                    // UPG: modify RtrbauElement to ensure aligns with nominating components as models if found on the scene
                    foreach (JsonProperty classAttribute in classElement.ontProperties)
                    {
                        Debug.Log("RtrbauData::RtrbauElement: classAttributeName is " + classAttribute.ontName);

                        // Find value to assign to class Attribute to generate fabrications: to be updated with user interactions
                        JsonValue exampleAttributeValue = exampleElement.ontProperties.Find(delegate (JsonValue exampleValue) { return exampleValue.ontName == classAttribute.ontName; });

                        // Debug.Log("RtrbauData::RtrbauElement: exampleAttributeValue is: " + exampleAttributeValue.ontValue);

                        // Check if value attribute exists for class attribute, otherwise give example default
                        Debug.Log("RtrbauData::RtrbauElement: exampleAttributeValue exists: " + !Equals(exampleAttributeValue, default(JsonValue)));

                        // ErrorHandling: when example attribute value does not exist, leave value empty
                        if (Equals(exampleAttributeValue, default(JsonValue)))
                        {
                            exampleAttributeValue = new JsonValue
                            {
                                ontName = classAttribute.ontName,
                                // UPG: to modify according to exampleAttributeValue.ontType
                                ontValue = "example",
                                ontType = classAttribute.ontType
                            };
                        }

                        Debug.Log("RtrbauData:RtrbauElement: exampleAttributeValue is: " + exampleAttributeValue.ontValue);

                        // Assign variables to generate a new RtrbauAttribute
                        // UPG: to modify assignment if necessary as for consult element fabrications
                        OntologyEntity attributeName = new OntologyEntity(classAttribute.ontName);
                        OntologyEntity attributeRange = new OntologyEntity(classAttribute.ontRange);
                        string attributeValue = exampleAttributeValue.ontValue;
                        OntologyEntity attributeType = new OntologyEntity(classAttribute.ontType);
                        RtrbauFabricationType fabricationType;

                        // Class and example attributes coincide by name: already checked above
                        // Check if class and example attributes also coincide in type
                        if (exampleAttributeValue.ontType == classAttribute.ontType)
                        {
                            // Assign attribute fabrication type and re-assign other features when necessary
                            // When attribute class coincides when component class, then assign individual as obj file
                            if (classAttribute.ontRange == Rtrbauer.instance.component.URI())
                            {
                                OntologyEntity exampleValue = new OntologyEntity(exampleAttributeValue.ontValue);
                                // For ElementReport any component can be accepted as an example value
                                // It only requires the obj format to be detected by data facets
                                // Only scene-found models would be selectable, so server location is assumed
                                // UPG: to find a better way to automatically generate obj files for model record
                                // UPG: when found, add a secondary fabrication for model nomination
                                attributeRange = new OntologyEntity(Rtrbauer.instance.xsd.URI() + "anyURI");
                                attributeValue = Rtrbauer.instance.server.AbsoluteUri + "api/files/obj/" + exampleValue.Name() + ".obj";
                                attributeType = new OntologyEntity(Rtrbauer.instance.owl.URI() + "DatatypeProperty");
                                fabricationType = RtrbauFabricationType.Record;
                            }
                            else if (classAttribute.ontType.Contains(OntologyPropertyType.DatatypeProperty.ToString()))
                            {
                                fabricationType = RtrbauFabricationType.Record;
                            }
                            else if (classAttribute.ontType.Contains(OntologyPropertyType.ObjectProperty.ToString()))
                            {
                                fabricationType = RtrbauFabricationType.Nominate;
                            }
                            else
                            {
                                throw new ArgumentException("RtrbauData::RtrbauElement: Attribute type is not implemented in Rtrbau");
                            }

                            // Assign attribute to list of element attributes as determined by rtrbau algorithm
                            elementAttributes.Add(new RtrbauAttribute(attributeName, attributeRange, attributeValue, attributeType, fabricationType));
                        }
                        else
                        {
                            throw new ArgumentException("RtrbauData::RtrbauElement: Attribute type does not coincide");
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("RtrbauData::RtrbauElement: Example does not belong to Class");
                }
            }
            else
            {
                throw new ArgumentException("RtrbauData::RtrbauElement: this declaration does not implement " + elementType.ToString() + " elements");
            }
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
    public class RtrbauFabrication
    {
        #region MEMBERS
        public RtrbauFabricationName fabricationName;
        public RtrbauFabricationType fabricationType;
        public Dictionary<DataFacet, RtrbauAttribute> fabricationData;
        public RtrbauAugmentation fabricationAugmentation;
        public RtrbauInteraction fabricationInteraction;
        public RtrbauComprehensiveness fabricationComprehension;
        public RtrbauDescriptiveness fabricationDescription;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public RtrbauFabrication()
        {
            fabricationName = RtrbauFabricationName.DefaultObserve;
            fabricationType = RtrbauFabricationType.Observe;
            fabricationData = new Dictionary<DataFacet, RtrbauAttribute>();
            fabricationAugmentation = RtrbauAugmentation.Registration;
            fabricationInteraction = RtrbauInteraction.None;
            fabricationComprehension = RtrbauComprehensiveness.oneD;
            fabricationDescription = RtrbauDescriptiveness.literal;
        }

        public RtrbauFabrication(RtrbauFabricationName name, RtrbauFabricationType type, Dictionary<DataFacet, RtrbauAttribute> data)
        {
            fabricationName = name;
            fabricationType = type;
            fabricationData = data;
        }
        #endregion CONSTRUCTORS

    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary> 
    [Serializable]
    public class RtrbauLocation
    {
        #region MEMBERS
        private RtrbauElementLocation locationType;
        private AssetVisualiser locationManager;
        private List<KeyValuePair<OntologyElement, GameObject>> locationElements;
        private int locationMaximum;
        private int locationCounter;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public RtrbauLocation(RtrbauElementLocation type, AssetVisualiser location)
        {
            if (type != RtrbauElementLocation.None) { locationType = type; }
            else { throw new ArgumentException("RtrbauData::RtrbauLocation: RtrbauElementLocation " + type.ToString() + "not implementable."); }

            locationManager = location;
            locationElements = new List<KeyValuePair<OntologyElement, GameObject>>();
            locationMaximum = ((int)type * 2) + 1;
            locationCounter = 0;
        }
        #endregion CONSTRUCTORS

        #region METHODS
        #region PRIVATE
        bool AllocateElement(int position, GameObject element)
        {
            Bounds bounds = locationManager.manager.ReturnAssetBoundsLocal();
            float pX;
            float pY;
            float pZ;

            if (locationType == RtrbauElementLocation.Primary)
            {
                if (position < 1)
                {
                    if (locationManager.elementsMode == RtrbauElementMode.Exterior)
                    {
                        pX = bounds.size.x;
                        pY = 0;
                        pZ = 0;

                        Debug.Log("RtrbauData::RtrbauLocation::SetPosition: element position is: (" + pX + "," + pY + "," + pZ + ")");
                        element.transform.localPosition = new Vector3(pX, pY, pZ);

                        return true;
                    }
                    else if (locationManager.elementsMode == RtrbauElementMode.Interior)
                    {
                        pX = 0.35f;
                        pY = 0;
                        pZ = 0.35f;

                        Debug.Log("RtrbauData::RtrbauLocation::SetPosition: element position follows user at: (" + pX + "," + pY + "," + pZ + ")");
                        element.AddComponent<Microsoft.MixedReality.Toolkit.Examples.Demos.EyeTracking.MoveWithCamera>().offsetToCamera = new Vector3(pX, pY, pZ);

                        return true;
                    }
                    else
                    {
                        throw new ArgumentException("RtrbauData::RtrbauLocation::SetPosition: RtrbauElementMode " + locationManager.elementsMode.ToString() + " not implemented for " + locationType.ToString());
                    }
                }
                else { return false; }
            }
            else if (locationType == RtrbauElementLocation.Secondary)
            {
                if (position < 3)
                {
                    if (locationManager.elementsMode == RtrbauElementMode.Exterior)
                    {
                        pX = bounds.size.x - position * bounds.size.x;
                        pY = bounds.size.y * 2f;
                        pZ = 0;

                        Debug.Log("RtrbauData::RtrbauLocation::SetPosition: element position is: (" + pX + "," + pY + "," + pZ + ")");
                        element.transform.localPosition = new Vector3(pX, pY, pZ);

                        return true;
                    }
                    else
                    {
                        throw new ArgumentException("RtrbauData::RtrbauLocation::SetPosition: RtrbauElementMode " + locationManager.elementsMode.ToString() + " not implemented for " + locationType.ToString());
                    }
                }
                else { return false; }
            }
            else if (locationType == RtrbauElementLocation.Tertiary)
            {
                if (position < 5)
                {
                    if (locationManager.elementsMode == RtrbauElementMode.Exterior)
                    {
                        pX = bounds.size.x * 2f;
                        pY = bounds.size.y * 2f;
                        pZ = -bounds.size.z * position;

                        Debug.Log("RtrbauData::RtrbauLocation::SetPosition: element position is: (" + pX + "," + pY + "," + pZ + ")");
                        element.transform.localPosition = new Vector3(pX, pY, pZ);

                        return true;
                    }
                    else
                    {
                        throw new ArgumentException("RtrbauData::RtrbauLocation::SetPosition: RtrbauElementMode " + locationManager.elementsMode.ToString() + " not implemented for " + locationType.ToString());
                    }
                }
                else { return false; }
            }
            else if (locationType == RtrbauElementLocation.Quaternary)
            {
                if (position < 5)
                {
                    if (locationManager.elementsMode == RtrbauElementMode.Exterior)
                    {
                        pX = -bounds.size.x * 2f;
                        pY = bounds.size.y * 2f;
                        pZ = -bounds.size.z * position;

                        Debug.Log("RtrbauData::RtrbauLocation::SetPosition: element position is: (" + pX + "," + pY + "," + pZ + ")");
                        element.transform.localPosition = new Vector3(pX, pY, pZ);

                        return true;
                    }
                    else if (locationManager.elementsMode == RtrbauElementMode.Interior)
                    {
                        pX = -0.35f;
                        pY = 0;
                        pZ = (-0.75f * position) + 1.5f;

                        Debug.Log("RtrbauData::RtrbauLocation::SetPosition: element position follows user at: (" + pX + "," + pY + "," + pZ + ")");
                        element.AddComponent<Microsoft.MixedReality.Toolkit.Examples.Demos.EyeTracking.MoveWithCamera>().offsetToCamera = new Vector3(pX, pY, pZ);

                        return true;
                    }
                    else
                    {
                        throw new ArgumentException("RtrbauData::RtrbauLocation::SetPosition: RtrbauElementMode " + locationManager.elementsMode.ToString() + " not implemented for " + locationType.ToString());
                    }
                }
                else { return false; }
            }
            else
            {
                throw new ArgumentException("RtrbauData::RtrbauLocation::AllocateElement: RtrbauElementLocation " + locationType.ToString() + " not implemented");
            }
        }

        void ScaleElement(GameObject elementObject)
        {
            // Determine asset size through model bounds
            Bounds asset = locationManager.manager.ReturnAssetBoundsLocal();

            // If element location mode is exterior
            if (locationManager.elementsMode == RtrbauElementMode.Exterior)
            {
                decimal aX = (decimal)asset.size.x;
                decimal eX = (decimal)elementObject.transform.localScale.x;
                Debug.Log("RtrbauData::RtrbauLocation::ScaleElement: x-axis asset extents is: " + aX);
                Debug.Log("RtrbauData::RtrbauLocation::ScaleElement: x-axis element local scale is: " + eX);
                Debug.Log("RtrbauData::RtrbauLocation::ScaleElement: asset extent is greater than element scale: " + (eX < aX));
                // Re-scale element to match horizontal asset extents (x-axis)
                // But only in the case asset extents are bigger than element
                if (eX < aX)
                {
                    float sM = asset.size.x / elementObject.transform.localScale.x;
                    if (sM < 2) { sM = 2f; }
                    Debug.Log("RtrbauData::RtrbauLocation::ScaleElement: element re-scale factor is: " + sM);
                    float sX = elementObject.transform.localScale.x * sM;
                    float sY = elementObject.transform.localScale.y * sM;
                    float sZ = elementObject.transform.localScale.z;

                    elementObject.transform.localScale = new Vector3(sX, sY, sZ);
                }
                else { }
            }
            else { }
        }
        #endregion PRIVATE

        #region PUBLIC
        public bool AddElement(OntologyElement elementClass, GameObject elementObject)
        {
            KeyValuePair<OntologyElement, GameObject> element = new KeyValuePair<OntologyElement, GameObject>(elementClass, elementObject);

            if (locationType != RtrbauElementLocation.Quaternary)
            {
                int existingElementIndex = locationElements.FindIndex(x => x.Key.EqualElement(elementClass));

                // If elementClass found
                if (existingElementIndex != -1)
                {
                    // Try to allocate elementObject
                    if (AllocateElement(existingElementIndex, elementObject))
                    {
                        // Once allocated, scale element
                        ScaleElement(elementObject);
                        // If elementClass position not empty
                        if (locationElements[existingElementIndex].Value != null)
                        {
                            // Unload elementObject from locationManager
                            locationManager.UnloadElement(locationElements[existingElementIndex].Key, locationElements[existingElementIndex].Value);
                        }
                        else { }
                        // Add new elementObject to locationElements in elementClass position
                        locationElements[existingElementIndex] = element;
                        // Break function with value true
                        return true;
                    }
                    else { throw new ArgumentException("RtrbauLocation::AddElement: AllocateElement returned false."); }
                }
                else
                {
                    // UPG: equal to Quaternary case, then move to a separate private function
                    if (locationCounter < locationMaximum)
                    {
                        // Add new element while locationElements not full yet
                        if (locationElements.Count() < locationMaximum + 1)
                        {
                            // Try to allocate elementObject
                            if (AllocateElement(locationCounter, elementObject))
                            {
                                // Once allocated, scale element
                                ScaleElement(elementObject);
                                // Add new element to locationElements
                                locationElements.Add(element);
                                // Update locationCounter
                                locationCounter += 1;
                                // Break function with value true
                                return true;
                            }
                            else { throw new ArgumentException("RtrbauLocation::AddElement: AllocateElement returned false."); }

                        }
                        else if (locationElements.Count() == locationMaximum + 1)
                        {
                            // Try to allocate elementObject
                            if (AllocateElement(locationCounter, elementObject))
                            {
                                // Once allocated, scale element
                                ScaleElement(elementObject);
                                // If elementClass position not empty
                                if (locationElements[locationCounter].Value != null)
                                {
                                    // Unload elementObject from locationManager
                                    locationManager.UnloadElement(locationElements[locationCounter].Key, locationElements[locationCounter].Value);
                                }
                                else { }
                                // Add new elementObject to locationElements in elementClass position
                                locationElements[locationCounter] = element;
                                // Update locationCounter
                                locationCounter += 1;
                                // Break function with value true
                                return true;
                            }
                            else { throw new ArgumentException("RtrbauLocation::AddElement: AllocateElement returned false."); }
                        }
                        else { throw new ArgumentException("RtrbauLocation::AddElement: locationCounter should not reach this value."); }
                    }
                    else if (locationCounter == locationMaximum)
                    {
                        // Reset locationCounter
                        locationCounter = 0;
                        // Try to allocate elementObject
                        if (AllocateElement(locationCounter, elementObject))
                        {
                            // Once allocated, scale element
                            ScaleElement(elementObject);
                            // If elementClass position not empty
                            if (locationElements[locationCounter].Value != null)
                            {
                                // Unload elementObject from locationManager
                                locationManager.UnloadElement(locationElements[locationCounter].Key, locationElements[locationCounter].Value);
                            }
                            else { }
                            // Add new elementObject to locationElements in elementClass position
                            locationElements[locationCounter] = element;
                            // Update locationCounter
                            locationCounter += 1;
                            // Break function with value true
                            return true;
                        }
                        else { throw new ArgumentException("RtrbauLocation::AddElement: AllocateElement returned false."); }
                    }
                    else { throw new ArgumentException("RtrbauLocation::AddElement: locationCounter should not reach this value."); }
                }
            }
            else
            {
                // UPG: equal to non-Quaternary, non-existing-element case, then move to a separate private function
                if (locationCounter < locationMaximum)
                {
                    // Add new element while locationElements not full yet
                    if (locationElements.Count() < locationMaximum + 1)
                    {
                        // Try to allocate elementObject
                        if (AllocateElement(locationCounter, elementObject))
                        {
                            // Once allocated, scale element
                            ScaleElement(elementObject);
                            // Add new element to locationElements
                            locationElements.Add(element);
                            // Update locationCounter
                            locationCounter += 1;
                            // Break function with value true
                            return true;
                        }
                        else { throw new ArgumentException("RtrbauLocation::AddElement: AllocateElement returned false."); }

                    }
                    else if (locationElements.Count() == locationMaximum + 1)
                    {
                        // Try to allocate elementObject
                        if (AllocateElement(locationCounter, elementObject))
                        {
                            // Once allocated, scale element
                            ScaleElement(elementObject);
                            // If elementClass position not empty
                            if (locationElements[locationCounter].Value != null)
                            {
                                // Unload elementObject from locationManager
                                locationManager.UnloadElement(locationElements[locationCounter].Key, locationElements[locationCounter].Value);
                            }
                            else { }
                            // Add new elementObject to locationElements in elementClass position
                            locationElements[locationCounter] = element;
                            // Update locationCounter
                            locationCounter += 1;
                            // Break function with value true
                            return true;
                        }
                        else { throw new ArgumentException("RtrbauLocation::AddElement: AllocateElement returned false."); }
                    }
                    else { throw new ArgumentException("RtrbauLocation::AddElement: locationCounter should not reach this value."); }
                }
                else if (locationCounter == locationMaximum)
                {
                    // Reset locationCounter
                    locationCounter = 0;
                    // Try to allocate elementObject
                    if (AllocateElement(locationCounter, elementObject))
                    {
                        // Once allocated, scale element
                        ScaleElement(elementObject);
                        // If elementClass position not empty
                        if (locationElements[locationCounter].Value != null)
                        {
                            // Unload elementObject from locationManager
                            locationManager.UnloadElement(locationElements[locationCounter].Key, locationElements[locationCounter].Value);
                        }
                        else { }
                        // Add new elementObject to locationElements in elementClass position
                        locationElements[locationCounter] = element;
                        // Update locationCounter
                        locationCounter += 1;
                        // Break function with value true
                        return true;
                    }
                    else { throw new ArgumentException("RtrbauLocation::AddElement: AllocateElement returned false."); }
                }
                else { throw new ArgumentException("RtrbauLocation::AddElement: locationCounter should not reach this value."); }
            }
        }

        public bool RemoveElement(OntologyElement elementClass)
        {
            int existingElementIndex = locationElements.FindIndex(x => x.Key.EqualElement(elementClass));

            if (existingElementIndex != -1)
            {
                // Create empty KeyValuePair to add to elementClass position in locationElements
                KeyValuePair<OntologyElement, GameObject> newElement = new KeyValuePair<OntologyElement, GameObject>(elementClass, null);
                // Remove elementObject from locationElements in elementClass position
                locationElements[existingElementIndex] = newElement;
                // Break function with value true
                return true;
            }
            else
            {
                // Break function with value false
                return false;
            }
        }

        public KeyValuePair<OntologyElement, GameObject> FindFirstElement()
        {
            if (locationElements.Count() > 0) { return locationElements.First(); }
            else { return new KeyValuePair<OntologyElement, GameObject>(); }
        }

        public void DebugLocationElements(string functionName)
        {
            foreach (KeyValuePair<OntologyElement, GameObject> element in locationElements)
            {
                Debug.Log("AssetVisualiser::" + functionName + "::RtrbauData::DebugLocationElements: " + locationType.ToString() + " location locates " + element.Key.entity.Name());
            }
        }
        #endregion PUBLIC
        #endregion METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary> 
    [Serializable]
    public class RtrbauRecommendation
    {
        #region MEMBERS
        public RecommendationFormat recommendationFormat;
        public RtrbauAttribute recommendationAttribute;
        public RtrbauElement recommendationTarget;
        public List<RtrbauElement> recommendationCases;
        public List<RtrbauElement> recommendations;
        #endregion MEMBERS

        #region CONSTRUCTORS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public RtrbauRecommendation()
        {
            recommendationFormat = new RecommendationFormat();
            recommendationAttribute = new RtrbauAttribute();
            recommendationTarget = new RtrbauElement();
            recommendationCases = new List<RtrbauElement>();
            recommendations = new List<RtrbauElement>();
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        /// <param name="format"></param>
        /// <param name="attribute"></param>
        /// <param name="range"></param>
        /// <param name="target"></param>
        /// <param name="cases"></param>
        public RtrbauRecommendation(RecommendationFormat format, RtrbauAttribute attribute, JsonIndividualValues target, List<JsonIndividualValues> cases)
        {
            // Generates clean RtrbauElements for target and cases individuals based on recomended range

            if (attribute.attributeName.URI() == format.formatAttribute.URI())
            {
                recommendationFormat = format;
                recommendationAttribute = attribute;

                if (target != null)
                {
                    if (target.ontClass == format.formatRange.URI())
                    {
                        RtrbauElement targetElement = CreateElement(target);

                        // Evaluate whether there the target meets the recommendation criteria
                        if (targetElement == null) { recommendationTarget = null; }
                        else { recommendationTarget = targetElement; }
                    }
                    else { throw new ArgumentException("RtrbauData::RtrbauRecommendation: Target individual does not belong to the recommendable class"); }
                }
                else { recommendationTarget = null; }

                recommendationCases = new List<RtrbauElement>();

                foreach (JsonIndividualValues individual in cases)
                {
                    if (individual.ontClass == format.formatRange.URI())
                    {
                        RtrbauElement individualElement = CreateElement(individual);

                        // Evaluate whether there are cases that meet the recommendation criteria
                        if (individualElement == null) { }
                        else { recommendationCases.Add(CreateElement(individual)); }
                    }
                    else { throw new ArgumentException("RtrbauData::RtrbauRecommendation: Case individual does not belong to the recommendable class"); }
                }
            }
            else { throw new ArgumentException("RtrbauData::RtrbauRecommendation: Fabrication attribute does not belong to the recommendable attribute"); }
        }
        #endregion CONSTRUCTORS

        #region METHODS
        #region PRIVATE
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        /// <param name="ontologyIndividual"></param>
        /// <returns></returns>
        RtrbauElement CreateElement(JsonIndividualValues ontologyIndividual)
        {
            // Generates rtrbau elements for ontology inferencing using recommendationFormat facets as template
            if (recommendationFormat.formatRange.URI() == ontologyIndividual.ontClass)
            {
                // Initialise element variables
                OntologyEntity elementName = new OntologyEntity(ontologyIndividual.ontIndividual);
                OntologyEntity elementClass = new OntologyEntity(ontologyIndividual.ontClass);
                List<RtrbauAttribute> elementAttributes = new List<RtrbauAttribute>();

                // Generate RtrbauAttributes using recommendationFormat facets
                foreach (RecommendationFacet formatFacet in recommendationFormat.formatFacets)
                {
                    // The recommendation algorithm only considers single occurrences of individual asserted attributes
                    // Find individual value to assign to recommendation facet attribute through attribute name comparison
                    // UPG: to modify rtrbau recommendations to consider attributes and relationships that are asserted to individuals more than once
                    JsonValue individualAttribute = ontologyIndividual.ontProperties.Find(delegate (JsonValue individualValue) { return individualValue.ontName == formatFacet.facetAttribute.URI(); });

                    // ErrorHandling: when individual value does not exist, leave value empty
                    if (!Equals(individualAttribute, default(JsonValue))) 
                    {
                        // Initialise RtrbauAttribute variables setting RtrbauFabricationType as default
                        OntologyEntity attributeName = new OntologyEntity(individualAttribute.ontName);
                        OntologyEntity attributeRange = formatFacet.facetRange;
                        string attributeValue = individualAttribute.ontValue;
                        OntologyEntity attributeType = new OntologyEntity(individualAttribute.ontType);
                        RtrbauFabricationType attributeFabrication = RtrbauFabricationType.Observe;

                        // Generate RtrbauAttribute instance and add to RtrbauElement list
                        elementAttributes.Add(new RtrbauAttribute(attributeName, attributeRange, attributeValue, attributeType, attributeFabrication));
                    }
                    else { }
                }

                // Generate new RtrbauElement instance to return
                // Return new RtrbauElement or null depending on whether RtrbauAttributes assigned coincide with the number of formatFacets
                if (elementAttributes.Count != recommendationFormat.formatFacets.Count) { return null; }
                else { return new RtrbauElement(elementName, elementClass, elementAttributes); }
            }
            else
            {
                throw new ArgumentException("RtrbauData::RtrbauRecommendation::CreateIndividual: Individual does not belong to Class");
            }
        }
        #endregion PRIVATE

        #region PUBLIC
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<decimal,RtrbauElement>> RecommendCases()
        {
            return recommendationFormat.RecommendIndividuals(recommendationAttribute, recommendationTarget, recommendationCases);
        }
        #endregion PUBLIC
        #endregion METHODS
    }


    #endregion RTRBAU_ELEMENTS

    #region AUTHORING_DATA
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class DataRule
    {
        #region MEMBERS
        public RtrbauFacetRuleType facetNameType;
        public List<string> facetNameRule;
        public RtrbauFacetRuleType facetRangeType;
        public List<string> facetRangeRule;
        public RtrbauFacetRuleType facetValueType;
        public List<string> facetValueRule;
        public RtrbauFacetRuleType facetTypeType;
        public List<string> facetTypeRule;
        public int facetRestrictivity;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public DataRule()
        {
            facetNameType = RtrbauFacetRuleType.Any;
            facetNameRule = new List<string>();
            facetRangeType = RtrbauFacetRuleType.Any;
            facetRangeRule = new List<string>();
            facetValueType = RtrbauFacetRuleType.Any;
            facetValueRule = new List<string>();
            facetTypeType = RtrbauFacetRuleType.Any;
            facetTypeRule = new List<string>();
            facetRestrictivity = 0;
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nameRules"></param>
        /// <param name="rangeRules"></param>
        /// <param name="valueRules"></param>
        public DataRule(RtrbauFacetRuleType nameRuleType, List<string> nameRules, RtrbauFacetRuleType rangeRuleType, List<string> rangeRules, RtrbauFacetRuleType valueRuleType, List<string> valueRules, RtrbauFacetRuleType typeRuleType, List<string> typeRules)
        {
            facetRestrictivity = 0;
            if (nameRules != null) facetRestrictivity += 1;
            if (rangeRules != null) facetRestrictivity += 1;
            if (valueRules != null) facetRestrictivity += 1;
            if (typeRules != null) facetRestrictivity += 1;

            facetNameType = nameRuleType;
            facetNameRule = nameRules;
            facetRangeType = rangeRuleType;
            facetRangeRule = rangeRules;
            facetValueType = valueRuleType;
            facetValueRule = valueRules;
            facetTypeType = typeRuleType;
            facetTypeRule = typeRules;
        }
        #endregion CONSTRUCTORS

        #region METHODS
        /// <summary>
        /// Given an attribute by <paramref name="attributeName"/>, <paramref name="attributeRange"/> and <paramref name="attributeValue"/>
        /// returns true if the attribute meets the facet's rules.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeRange"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public bool EvaluateFacet(string attributeName, string attributeRange, string attributeValue, string attributeType)
        {
            // RTRBAU ALGORITHM: Rules in loop 3
            bool nameMet = EvaluateRule(facetNameType, facetNameRule, attributeName);
            bool rangeMet = EvaluateRule(facetRangeType, facetRangeRule, attributeRange);
            bool valueMet = EvaluateRule(facetValueType, facetValueRule, attributeValue);
            bool typeMet = EvaluateRule(facetTypeType, facetTypeRule, attributeType);

            //// RTRBAU ALGORITHM: Rules in loop 3
            //bool nameMet = EvaluateAllRules(facetNameRule, attributeName);
            //bool rangeMet = EvaluateAnyRule(facetRangeRule, attributeRange);
            //bool valueMet = EvaluateAnyRule(facetValueRule, attributeValue);

            if (nameMet && rangeMet && valueMet && typeMet)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if a given <paramref name="attribute"/> matches <paramref name="rules"/> according to <paramref name="ruleType"/>.
        /// Otherwise, throws an exception.
        /// </summary>
        /// <param name="ruleType"></param>
        /// <param name="rules"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        bool EvaluateRule (RtrbauFacetRuleType ruleType, List<string> rules, string attribute)
        {
            bool ruleMet;
            if (ruleType == RtrbauFacetRuleType.All) { ruleMet = EvaluateAllRules(rules, attribute); }
            else if (ruleType == RtrbauFacetRuleType.Any) { ruleMet = EvaluateAnyRule(rules, attribute);  }
            else { throw new ArgumentException("RtrbauFacetRuleType not implemented"); }
            return ruleMet;
        }

        /// <summary>
        /// Returns true if a given <paramref name="attribute"/> contains all text string elements from <paramref name="rules"/>
        /// or if <paramref name="rules"/> is null, otherwise returns false.
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        bool EvaluateAllRules(List<string> rules, string attribute)
        {
            if (rules == null)
            {
                return true;
            }
            else
            {
                return rules.All(attribute.Contains);
            }
        }

        /// <summary>
        /// Returns true if a given <paramref name="attribute"/> contains any text string elements from <paramref name="rules"/>
        /// or if <paramref name="rules"/> is null, otherwise returns false.
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        bool EvaluateAnyRule(List<string> rules, string attribute)
        {
            if (rules == null)
            {
                return true;
            }
            else
            {
                return rules.Any(attribute.Contains);
            }
        }
        #endregion METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class DataFacet
    {
        #region MEMBERS
        public RtrbauFacetForm facetForm;
        public DataRule facetRules;
        #endregion MEMBERS

        #region CONSTRUCTOR
        public DataFacet()
        {
            facetForm = RtrbauFacetForm.source;
            facetRules = new DataRule();
        }

        /// <summary>
        /// Generates a facet for a specific format independently
        /// of the combination of facet rules being used.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="rules"></param>
        public DataFacet (RtrbauFacetForm form, DataRule rules)
        {
            facetForm = form;
            facetRules = rules;
        }
        #endregion CONSTRUCTOR
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class DataFormat
    {
        #region MEMBERS
        public RtrbauFabricationName formatName;
        public RtrbauFabricationType formatType;
        public List<DataFacet> formatFacets;
        public int formatRequiredFacets;
        // public List<KeyValuePair<RtrbauFacetForm, DataRule>> formatFacets;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public DataFormat()
        {
            formatName = RtrbauFabricationName.DefaultObserve;
            formatType = RtrbauFabricationType.Observe;
            formatFacets = new List<DataFacet>();
            formatRequiredFacets = 0;
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="facets"></param>
        public DataFormat(RtrbauFabricationName name, RtrbauFabricationType type, List<DataFacet> facets)
        {
            formatName = name;
            formatType = type;
            formatFacets = facets;

            formatRequiredFacets = 0;

            foreach (DataFacet facet in facets)
            {
                if (facet.facetForm == RtrbauFacetForm.source || facet.facetForm == RtrbauFacetForm.required)
                {
                    formatRequiredFacets += 1;
                }
            }
        }
        #endregion CONSTRUCTORS

        #region METHODS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public List<RtrbauFabrication> EvaluateFormat(RtrbauElement element)
        {
            // RTRBAU ALGORITHM: Loops 3 and 4 combined
            // Debug.Log("EvaluateFormat: " + formatName + " required facets: " + formatRequiredFacets);

            int requiredAttributesAssigned = 0;
            int formatsAssignable = 1;

            // Create a dictionary of lists to assign all attributes that meet each facet
            // Consider that there may be facets which are not met by any attribute
            Dictionary<DataFacet, List<RtrbauAttribute>> assignableFacets = new Dictionary<DataFacet, List<RtrbauAttribute>>();

            foreach (DataFacet facet in formatFacets)
            {
                assignableFacets.Add(facet, new List<RtrbauAttribute>());
            }

            // For each attribute being evaluated, check to which facets can be assigned
            // Assign attribute to the most restrictive facet
            foreach (RtrbauAttribute attribute in element.elementAttributes)
            {
                if (attribute.fabricationType == formatType)
                {
                    List<KeyValuePair<DataFacet, RtrbauAttribute>> assignableAttributes = new List<KeyValuePair<DataFacet, RtrbauAttribute>>();

                    foreach (DataFacet facet in formatFacets)
                    {
                        if (facet.facetRules.EvaluateFacet(attribute.attributeName.Name(), attribute.attributeRange.Name(), attribute.attributeValue, attribute.attributeType.Name()))
                        {
                            // assignableFacets.Add(new KeyValuePair<KeyValuePair<RtrbauFacetForm, DataRule>, RtrbauAttribute>(new KeyValuePair<RtrbauFacetForm, DataRule>(facet.Key, facet.Value), attribute));
                            assignableAttributes.Add(new KeyValuePair<DataFacet, RtrbauAttribute>(facet, attribute));
                        }
                    }

                    if (assignableAttributes.Count > 1)
                    {
                        assignableAttributes.Sort((x, y) => x.Key.facetRules.facetRestrictivity.CompareTo(y.Key.facetRules.facetRestrictivity));
                    }

                    if (assignableAttributes.Count > 0)
                    {
                        assignableFacets[assignableAttributes.Last().Key].Add(assignableAttributes.Last().Value);
                    }
                }
                else { }

            }

            // Generate new dictionary for facets to which attributes have been assigned
            Dictionary<DataFacet, List<RtrbauAttribute>> assignedFacets = new Dictionary<DataFacet, List<RtrbauAttribute>>();

            // Evaluate facets assigned
            // Consider that not all facets are required, so if they are not met is not a problem to create a fabrication
            foreach (KeyValuePair<DataFacet, List<RtrbauAttribute>> facet in assignableFacets)
            {
                if (facet.Key.facetForm.Equals(RtrbauFacetForm.optional))
                {
                    if (facet.Value.Count == 0)
                    {
                        formatsAssignable *= 1;
                    }
                    else
                    {
                        formatsAssignable *= facet.Value.Count;
                        assignedFacets.Add(facet.Key, facet.Value);
                    }
                }
                else if (facet.Key.facetForm.Equals(RtrbauFacetForm.source) || facet.Key.facetForm.Equals(RtrbauFacetForm.required))
                {
                    if (facet.Value.Count == 0)
                    {
                        formatsAssignable *= 0;
                    }
                    else
                    {
                        formatsAssignable *= facet.Value.Count;
                        requiredAttributesAssigned += 1;
                        assignedFacets.Add(facet.Key, facet.Value);
                    }
                }
                else
                {
                    throw new ArgumentException("Facet: " + nameof(facet.Key.facetRules) + " not correctly defined for format: " + formatName);
                }

                //Debug.Log("EvaluateFormat: format: " + formatName + " facet: " + nameof(facet.Key.facetRules) + " attributes assigned: " + facet.Value.Count);

                //foreach (RtrbauAttribute assigned in facet.Value)
                //{
                //    Debug.Log("EvaluateFormat: format: " + formatName + " facet: " + nameof(facet.Key.facetRules) + " attribute: " + assigned.attributeName.Name());
                //}
            }

            //Debug.Log("EvaluateFormat: " + formatName + " assignable formats: " + formatsAssignable);
            //Debug.Log("EvaluateFormat: " + formatName + " assignable attributes: " + requiredAttributesAssigned);
            //Debug.Log("EvaluateFormat: " + formatName + " assignable attributes check: " + assignableFacets.Count);

            if (formatsAssignable == 0)
            {
                // Previous calculations are used to check how many fabrications can be created according to
                // how many attributes have been assigned to which facets
                // remember that attributes can only be assigned to one facet
                // but facets can be assigned with more than one attribute
                return null;
            }
            else
            {
                // Create a list to returned assigned fabrications
                List<RtrbauFabrication> assignedFabrications = new List<RtrbauFabrication>();

                // Debug.Log("EvaluateFormat: assigned fabrications: " + assignedFabrications.Count());

                // Add members to the list according to the calculate number of formats assignable
                for (int i = 0; i < formatsAssignable; i++)
                {
                    assignedFabrications.Add(new RtrbauFabrication(formatName, formatType, new Dictionary<DataFacet, RtrbauAttribute>()));
                }

                // Assign facets and attributes to each fabrication created
                // Foreach facet, start getting the last attribute assign and go down the list for each new fabrication
                // This is a way to ensure all combinations are given
                foreach (KeyValuePair<DataFacet, List<RtrbauAttribute>> assignedFacet in assignedFacets)
                {
                    int attributesAssignedCounter = assignedFacet.Value.Count() - 1;

                    for (int i = 0; i < formatsAssignable; i++)
                    {
                        if (attributesAssignedCounter < 0)
                        {
                            attributesAssignedCounter = assignedFacet.Value.Count() - 1;
                            assignedFabrications[i].fabricationData.Add(assignedFacet.Key, assignedFacet.Value[attributesAssignedCounter]);
                            attributesAssignedCounter -= 1;
                        }
                        else
                        {
                            assignedFabrications[i].fabricationData.Add(assignedFacet.Key, assignedFacet.Value[attributesAssignedCounter]);
                            attributesAssignedCounter -= 1;
                        }
                    }
                }

                //foreach (RtrbauFabrication fabrication in assignedFabrications)
                //{
                //    Debug.Log("EvaluateFormat: fabrication assigned: " + fabrication.fabricationName);
                //    foreach (KeyValuePair<DataFacet, RtrbauAttribute> attribute in fabrication.fabricationData)
                //    {
                //        Debug.Log("EvaluateFormat: facet assigned: " + nameof(attribute.Key) + " with: " + attribute.Value.attributeName.Name());
                //    }
                //}

                return assignedFabrications;
            }
        }
        #endregion METHODS
    }
    #endregion AUTHORING_DATA

    #region AUTHORING_ENVIRONMENT
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class AugmentationFacet
    {
        #region MEMBERS
        public RtrbauAugmentation facetAugmentation;
        public List<RtrbauSense> facetSenses;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public AugmentationFacet()
        {
            facetAugmentation = RtrbauAugmentation.Registration;
            facetSenses = new List<RtrbauSense>();
        }

        /// <summary>
        /// Lists the <paramref name="senses"/> that apply to an <paramref name="augmentation"/>.
        /// </summary>
        /// <param name="augmentation"></param>
        /// <param name="senses"></param>
        public AugmentationFacet (RtrbauAugmentation augmentation, List<RtrbauSense> senses)
        {
            facetAugmentation = augmentation;
            facetSenses = senses;
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
    public class InteractionFacet
    {
        #region MEMBERS
        public RtrbauInteraction facetInteraction;
        public List<RtrbauSense> facetSenses;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public InteractionFacet()
        {
            facetInteraction = RtrbauInteraction.None;
            facetSenses = new List<RtrbauSense>();
        }

        /// <summary>
        /// List the <paramref name="senses"/> that apply to an <paramref name="interaction"/>
        /// </summary>
        /// <param name="interaction"></param>
        /// <param name="senses"></param>
        public InteractionFacet (RtrbauInteraction interaction, List<RtrbauSense> senses)
        {
            facetInteraction = interaction;
            facetSenses = senses;
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
    public class EnvironmentFormat
    {
        #region MEMBERS
        public RtrbauFabricationName formatName;
        public AugmentationFacet formatAugmentation;
        public InteractionFacet formatInteraction;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public EnvironmentFormat()
        {
            formatName = RtrbauFabricationName.DefaultObserve;
            formatAugmentation = new AugmentationFacet();
            formatInteraction = new InteractionFacet();
        }

        /// <summary>
        /// Describes the environmental attributes of a fabrication format <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="augmentation"></param>
        /// <param name="interaction"></param>
        public EnvironmentFormat (RtrbauFabricationName name, AugmentationFacet augmentation, InteractionFacet interaction)
        {
            formatName = name;
            formatAugmentation = augmentation;
            formatInteraction = interaction;
        }
        #endregion CONSTRUCTORS

        #region METHODS
        /// <summary>
        /// Returns format's augmentation and interaction methods if included within environment's permitted, otherwise returns null.
        /// </summary>
        /// <returns></returns>
        public Tuple<RtrbauAugmentation,RtrbauInteraction> EvaluateFormat()
        {
            int nonAugmentableSenses = formatAugmentation.facetSenses.Except(Rtrbauer.instance.environment.Senses()).Count();
            int nonInteractableSenses = formatInteraction.facetSenses.Except(Rtrbauer.instance.environment.Senses()).Count();

            //Debug.Log("EnvironmentFormat: " + formatName + " Evaluate: augmentation" + nonAugmentableSenses + " and interaction " + nonInteractableSenses);

            if (nonAugmentableSenses > 0 || nonInteractableSenses > 0)
            {
                return null;
            }
            else
            {
                return new Tuple<RtrbauAugmentation, RtrbauInteraction>(formatAugmentation.facetAugmentation, formatInteraction.facetInteraction);
            }
        }
        #endregion METHODS
    }
    #endregion AUTHORING_ENVIRONMENT

    #region AUTHORING_USER
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    //[Serializable]
    //public class ComprehensionFacet
    //{
    //    #region MEMBERS
    //    #endregion MEMBERS   
    //    #region CONSTRUCTORS
    //    #endregion CONSTRUCTORS
    //    #region METHODS
    //    #endregion METHODS
    //}

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    //[Serializable]
    //public class DescriptionFacet
    //{
    //    #region MEMBERS
    //    #endregion MEMBERS   
    //    #region CONSTRUCTORS
    //    #endregion CONSTRUCTORS
    //    #region METHODS
    //    #endregion METHODS
    //}

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class UserFormat
    {
        #region MEMBERS
        public RtrbauFabricationName formatName;
        public RtrbauComprehensiveness formatComprehension;
        public RtrbauDescriptiveness formatDescription;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public UserFormat()
        {
            formatName = RtrbauFabricationName.DefaultObserve;
            formatComprehension = RtrbauComprehensiveness.oneD;
            formatDescription = RtrbauDescriptiveness.literal;
        }

        public UserFormat (RtrbauFabricationName name, RtrbauComprehensiveness comprehension, RtrbauDescriptiveness description)
        {
            formatName = name;
            formatComprehension = comprehension;
            formatDescription = description;
        }
        #endregion CONSTRUCTORS

        #region METHODS
        /// <summary>
        /// Returns format's comprehesiveness and descriptiveness if included within user's permitted.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Tuple<RtrbauComprehensiveness,RtrbauDescriptiveness> EvaluateFormat()
        {
            bool comprehensivenessPermitted = Rtrbauer.instance.user.Comprehensiveness().Contains(formatComprehension);
            bool descriptivenessPermitted = Rtrbauer.instance.user.Descriptivenesses().Contains(formatDescription);

            //Debug.Log("UserFormat: " + formatName + " comprenhension: " + formatComprehension + " permitted " + comprehensivenessPermitted);
            //Debug.Log("UserFormat: " + formatName + " comprenhension: " + formatDescription + " permitted " + descriptivenessPermitted);

            if (!comprehensivenessPermitted || !descriptivenessPermitted)
            {
                return null;
            }
            else
            {
                return new Tuple<RtrbauComprehensiveness, RtrbauDescriptiveness>(formatComprehension, formatDescription);
            }
           
        }
        #endregion METHODS
    }
    #endregion AUTHORING_USER

    #region RECOMMENDATION
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class RecommendationRule
    {
        #region MEMBERS
        public RtrbauRecommendationRuleType ruleType;
        public Dictionary<decimal, List<string>> ruleSubsets;
        public Func<RtrbauAttribute, RtrbauAttribute, decimal> ruleEvaluation;
        #endregion MEMBERS

        #region CONSTRUCTORS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public RecommendationRule()
        {
            ruleType = RtrbauRecommendationRuleType.Binary;
            ruleSubsets = new Dictionary<decimal, List<string>>();
            ruleEvaluation = null;
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        /// <param name="typeRule"></param>
        public RecommendationRule(RtrbauRecommendationRuleType typeRule)
        {
            ruleType = typeRule;
            ruleSubsets = new Dictionary<decimal, List<string>>();

            if (typeRule == RtrbauRecommendationRuleType.Binary) { ruleEvaluation = EvaluateBinaryRule; }
            else if (typeRule == RtrbauRecommendationRuleType.Symmetric) { ruleEvaluation = EvaluateSymmetricRule; }
            else if (typeRule == RtrbauRecommendationRuleType.Component) { ruleEvaluation = EvaluateComponentRule; }
            else if (typeRule == RtrbauRecommendationRuleType.Subset) { throw new ArgumentException("RtrbauData::RecommendationRule: rule of type subset requires of a subset dictionary for subsets evaluation."); }
            else { throw new ArgumentException("RtrbauData::RecommendationRule: rule of type " + typeRule.ToString() + " has not been implemented yet"); }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        /// <param name="typeRule"></param>
        /// <param name="subsetsDictionary"></param>
        public RecommendationRule(RtrbauRecommendationRuleType typeRule, Dictionary<decimal, List<string>> subsetsDictionary)
        {
            if (typeRule == RtrbauRecommendationRuleType.Subset)
            {
                ruleType = typeRule;
                ruleSubsets = subsetsDictionary;
                ruleEvaluation = EvaluateSubsetRule;
            }
            else
            {
                throw new ArgumentException("RtrbauData::RecommendationRule: only rules of type subset require a subset dictionary.");
            }
        }
        #endregion CONSTRUCTORS

        #region METHODS
        #region PRIVATE
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        decimal EvaluateBinaryRule(RtrbauAttribute targetAttribute, RtrbauAttribute caseAttribute)
        {
            if (ruleType == RtrbauRecommendationRuleType.Binary)
            {
                if (caseAttribute.attributeValue == targetAttribute.attributeValue) { return 1; }
                else { return 0; }
            }
            else
            {
                throw new ArgumentException("RtrbauData::RecommendationRule::EvaluateBinaryRule: Rule has not been declared as of binary type.");
            }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        decimal EvaluateSymmetricRule(RtrbauAttribute targetAttribute, RtrbauAttribute caseAttribute)
        {
            if (ruleType == RtrbauRecommendationRuleType.Symmetric)
            {
                decimal targetValue;
                decimal caseValue;

                if (decimal.TryParse(targetAttribute.attributeValue, out targetValue))
                {
                    if (decimal.TryParse(caseAttribute.attributeValue, out caseValue))
                    {
                        return (1 - (Math.Abs(targetValue - caseValue) / Math.Max(targetValue, caseValue)));
                    }
                    else
                    {
                        throw new ArgumentException("RtrbauData::RecommendationRule::EvaluateSymmetricRule: caseAttribute.attributeValue should be a numeric string.");
                    }
                }
                else
                {
                    throw new ArgumentException("RtrbauData::RecommendationRule::EvaluateSymmetricRule: targetAttribute.attributeValue should be a numeric string.");
                }

            }
            else
            {
                throw new ArgumentException("RtrbauData::RecommendationRule::EvaluateSymmetricRule: Rule has not been declared as of symmetric type.");
            }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        decimal EvaluateSubsetRule(RtrbauAttribute targetAttribute, RtrbauAttribute caseAttribute)
        {
            if (ruleType == RtrbauRecommendationRuleType.Subset)
            {
                if (caseAttribute.attributeValue == targetAttribute.attributeValue)
                {
                    foreach (KeyValuePair<decimal,List<string>> subset in ruleSubsets)
                    {
                        if (subset.Value.All(caseAttribute.attributeValue.Contains)) { return subset.Key; }
                    }

                    return 0;
                }
                else { return 0; }
            }
            else
            {
                throw new ArgumentException("RtrbauData::RecommendationRule::EvaluateSubsetRule: Rule has not been declared as of subset type.");
            }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        decimal EvaluateComponentRule(RtrbauAttribute targetAttribute, RtrbauAttribute caseAttribute)
        {
            if (ruleType == RtrbauRecommendationRuleType.Component)
            {
                if (targetAttribute.attributeRange.URI() == Rtrbauer.instance.component.URI() && caseAttribute.attributeRange.URI() == Rtrbauer.instance.component.URI())
                {
                    AssetManager targetAsset = GameObject.FindWithTag(RtrbauGameObjectTags.RtrbauAssetManager.ToString()).GetComponent<AssetManager>();
                    
                    if (targetAsset != null)
                    {
                        // Considers generic individual URI to find components names
                        // The recieved RtrbauElements are "clean" versions for accurate ontology inferencing
                        string targetName = Parser.ParseURI(targetAttribute.attributeValue, '#', RtrbauParser.post);
                        string caseName = Parser.ParseURI(caseAttribute.attributeValue, '#', RtrbauParser.post);

                        GameObject targetModel = targetAsset.FindAssetComponentManipulator(targetName);
                        GameObject caseModel = targetAsset.FindAssetComponentManipulator(caseName);

                        if (targetModel != null)
                        {
                            if (caseModel != null)
                            {
                                decimal assetVolume = (decimal)(targetAsset.ReturnAssetBoundsLocal().size.sqrMagnitude);
                                Vector3 caseDirection = targetModel.transform.localPosition - caseModel.transform.localPosition;
                                decimal caseVolume = (decimal)(caseDirection.sqrMagnitude);
                                return (1 - (caseVolume / assetVolume));
                            }
                            else
                            {
                                return 0;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("RtrbauData::RecommendationRule::EvaluateComponentRule: Target Component should always be found in the scene.");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("RtrbauData::RecommendationRule::EvaluateComponentRule: There is not AssetManager in the scene; this rule cannot be evaluated otherwise.");
                    }
                }
                else
                {
                    throw new ArgumentException("RtrbauData::RecommendationRule::EvaluateComponentRule: case and target attributes should be of Rtrbauer.instance.component range.");
                }
            }
            else
            {
                throw new ArgumentException("RtrbauData::RecommendationRule::EvaluateComponentRule: Rule has not been declared as of subset type.");
            }
        }
        #endregion PRIVATE

        #region PUBLIC
        #endregion PUBLIC
        #endregion METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class RecommendationFacet
    {
        #region MEMBERS
        public RecommendationRule facetRule;
        public OntologyEntity facetAttribute;
        public OntologyEntity facetRange;
        #endregion MEMBERS

        #region CONSTRUCTORS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public RecommendationFacet()
        {
            facetRule = new RecommendationRule();
            facetAttribute = new OntologyEntity();
            facetRange = new OntologyEntity();
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public RecommendationFacet(RecommendationRule ruleFacet, OntologyEntity attributeFacet, OntologyEntity rangeFacet)
        {
            facetRule = ruleFacet;
            facetAttribute = attributeFacet;
            facetRange = rangeFacet;
        }
        #endregion CONSTRUCTORS

        #region METHODS
        #region PRIVATE
        #endregion PRIVATE

        #region PUBLIC
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public bool ValidateFacet(RtrbauAttribute attribute)
        {
            if (facetAttribute.URI() == attribute.attributeName.URI() && facetRange.URI() == attribute.attributeRange.URI()) { return true; }
            else { return false; }
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public decimal EvaluateFacet(RtrbauAttribute targetAttribute, RtrbauAttribute caseAttribute)
        {
            if (ValidateFacet(targetAttribute) && ValidateFacet(caseAttribute))
            {
                return facetRule.ruleEvaluation(targetAttribute, caseAttribute);
            }
            else { throw new ArgumentException("RtrbauData::RecommendationFacet::EvaluateFacet: Target and/or case attributes do not coincide with expected facet's attribute or range"); }
        }
        #endregion PUBLIC
        #endregion METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class RecommendationFormat
    {
        #region MEMBERS
        public RtrbauFabricationName formatName;
        public RtrbauFabricationType formatType;
        public OntologyEntity formatAttribute;
        public OntologyEntity formatRange;
        public List<RecommendationFacet> formatFacets;
        #endregion MEMBERS

        #region CONSTRUCTORS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public RecommendationFormat()
        {
            formatName = RtrbauFabricationName.DefaultObserve;
            formatType = RtrbauFabricationType.Observe;
            formatAttribute = new OntologyEntity();
            formatRange = new OntologyEntity();
            formatFacets = new List<RecommendationFacet>();
        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        public RecommendationFormat(RtrbauFabricationName name, RtrbauFabricationType type, OntologyEntity attribute, OntologyEntity range, List<RecommendationFacet> facets)
        {
            formatName = name;
            formatType = type;
            formatAttribute = attribute;
            formatRange = range;
            formatFacets = facets;
        }
        #endregion CONSTRUCTORS

        #region METHODS
        #region PRIVATE
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        /// <param name="individual"></param>
        /// <returns></returns>
        decimal EvaluateIndividual(RtrbauElement targetIndividual, RtrbauElement caseIndividual)
        {
            // Initialise case individual similarity result
            decimal caseSimilarity = 0;

            // Find the target and case individuals attributes that are valid for each recommendation facet
            // It considers that target and case individuals only include one attribute for each facet
            // UPG: to modify to accept more than one attribute assigned to each facet coming from target and case individuals
            foreach (RecommendationFacet formatFacet in formatFacets)
            {
                RtrbauAttribute targetAttribute = targetIndividual.elementAttributes.Find(formatFacet.ValidateFacet);
                RtrbauAttribute caseAttribute = caseIndividual.elementAttributes.Find(formatFacet.ValidateFacet);
                caseSimilarity += formatFacet.EvaluateFacet(targetAttribute, caseAttribute);
            }

            Debug.Log("RtrbauData::RecommendationFormat::EvaluateIndividual: caseIndividual " + caseIndividual.elementName.Entity() + " similarity is " + caseSimilarity);

            return caseSimilarity;
        }
        #endregion PRIVATE

        #region PUBLIC
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="targetIndividual"></param>
        /// <param name="caseIndividuals"></param>
        /// <returns></returns>
        public List<KeyValuePair<decimal,RtrbauElement>> RecommendIndividuals(RtrbauAttribute attribute, RtrbauElement targetIndividual, List<RtrbauElement> caseIndividuals)
        {
            if (attribute.attributeName.URI() == formatAttribute.URI() && attribute.attributeRange.URI() == formatRange.URI())
            {
                if (targetIndividual != null)
                {
                    // Recommends five cases according to similarity facets to the target
                    if (formatRange.URI() == targetIndividual.elementClass.URI())
                    {
                        List<KeyValuePair<decimal, RtrbauElement>> individualsResults = new List<KeyValuePair<decimal, RtrbauElement>>();

                        foreach (RtrbauElement caseIndividual in caseIndividuals)
                        {
                            decimal recommendationResult = EvaluateIndividual(targetIndividual, caseIndividual);

                            individualsResults.Add(new KeyValuePair<decimal, RtrbauElement>(recommendationResult, caseIndividual));
                        }

                        // Order results in descending numerical order
                        individualsResults.Sort((x, y) => y.Key.CompareTo(x.Key));

                        // Return the first 5
                        return individualsResults.GetRange(0, 5);
                    }
                    else
                    {
                        throw new ArgumentException("RtrbauData::RecommendationFormat::RecommendIndividuals: Target individual to which compare do not belong to the ontology class this format recommends.");
                    }
                }
                else
                {
                    // Otherwise, recommends the first five cases on the cases list with a value of 0
                    List<KeyValuePair<decimal, RtrbauElement>> individualsResults = new List<KeyValuePair<decimal, RtrbauElement>>();

                    foreach (RtrbauElement caseIndividual in caseIndividuals)
                    {
                        individualsResults.Add(new KeyValuePair<decimal, RtrbauElement>(0.0M, caseIndividual));
                    }

                    // Return the first 5
                    return individualsResults.GetRange(0, 5);
                }
            }
            else
            {
                throw new ArgumentException("RtrbauData::RecommendationFormat::RecommendIndividuals: Individuals to recommend do not belong to the ontology attribute or class this format recommends.");
            }
        }
        #endregion PUBLIC
        #endregion METHODS
    }
    #endregion RECOMMENDATION

}
