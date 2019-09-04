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
        public OntologyEntity attributeName;
        public OntologyEntity attributeRange;
        public string attributeValue;        
        public RtrbauFabricationType attributeType;

        #region CONSTRUCTOR
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        /// <param name="name"></param>
        /// <param name="range"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public RtrbauAttribute (OntologyEntity name, OntologyEntity range, string value, RtrbauFabricationType type)
        {
            attributeName = name;
            attributeRange = range;
            attributeValue = value;
            attributeType = type;
        }
        #endregion CONSTRUCTOR

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
        public OntologyEntity elementName;
        public OntologyEntity elementClass;
        public List<RtrbauAttribute> elementAttributes;

        #region CONSTRUCTOR
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        /// <param name="nameEntity"></param>
        /// <param name="classEntity"></param>
        /// <param name="attributes"></param>
        public RtrbauElement (OntologyEntity nameEntity, OntologyEntity classEntity, List<RtrbauAttribute> attributes)
        {
            elementName = nameEntity;
            elementClass = classEntity;
            elementAttributes = attributes;
        }

        /// <summary>
        /// IMP: to merge both class and invidual ontology elements into a single one to evaluate attributes for augmentation
        /// IMP: drops all properties that cannot be found in the matching class
        /// IMP: evaluates attributes and adapts them according to algorithm if necessary
        /// IMP: this assumes user.procedure == RtrbauElementType.Consult
        /// UPG: improve server to find properties ranges on its own so there is no need to match with a class
        /// </summary>
        /// <param name="individualElement"></param>
        /// <param name="classElement"></param>
        /// <param name="relationshipClassesElements"></param>
        public RtrbauElement (AssetManager assetManager, JsonIndividualValues individualElement, JsonClassProperties classElement, List<JsonClassProperties> relationshipClassesElements)
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
                            OntologyEntity attributeName;
                            OntologyEntity attributeRange;
                            string attributeValue;
                            RtrbauFabricationType attributeType;

                            attributeName = new OntologyEntity(classAttribute.ontName);

                            // When component distance = 1, then assign individual as obj file
                            if (classAttribute.ontRange == Rtrbauer.instance.component.componentURI)
                            {
                                OntologyEntity individualValue = new OntologyEntity(individualAttribute.ontValue);
                                // Changed way in which components are found in the scene
                                // It is dependent on visualiser being unique in the scene
                                // UPG: to adapt for when visualiser won't be unique
                                string componentName = assetManager.FindAssetComponent(individualValue.name);
                                Debug.Log(componentName);
                                Debug.Log(individualValue.name);
                                // But only when the component is found in the scene
                                // In that case, it is assumed the obj file was taken from the current server
                                // Otherwise, it is already now the attribute is of object type
                                // if (GameObject.Find(individualValue.name) != null)
                                if (componentName == individualValue.name)
                                {
                                    attributeRange = new OntologyEntity(Rtrbauer.instance.uris.XSD + "#anyURI");
                                    attributeValue = Rtrbauer.instance.server.serverURI + "api/files/obj/" + individualValue.name + ".obj";
                                    attributeType = RtrbauFabricationType.Observe;
                                }
                                else
                                {
                                    attributeRange = new OntologyEntity(classAttribute.ontRange);
                                    attributeValue = individualValue.ToString();
                                    attributeType = RtrbauFabricationType.Observe;
                                }
                            }
                            else if (classAttribute.ontType.Contains(OntologyPropertyType.DatatypeProperty.ToString()))
                            {
                                // Declare attribute as to observe when property of datatype
                                // Those object properties that point to empty classes, declare as to observe
                                attributeRange = new OntologyEntity(classAttribute.ontRange);
                                attributeValue = individualAttribute.ontValue;
                                attributeType = RtrbauFabricationType.Observe;

                            }
                            else if (classAttribute.ontType.Contains(OntologyPropertyType.ObjectProperty.ToString()))
                            {
                                // Declare attribute as to inspect when property of object type
                                // Unless the individual being pointed belongs to an empty class, which is then meant to observe
                                // This specific case is to control object properties that are used as pre-defined datasets
                                JsonClassProperties objectPropertyClass = relationshipClassesElements.Find(delegate (JsonClassProperties objectProperty) { return objectProperty.ontClass == classAttribute.ontRange; });

                                if (objectPropertyClass.ontProperties.Count != 0)
                                {
                                    Debug.Log(objectPropertyClass.ontClass);
                                    attributeRange = new OntologyEntity(classAttribute.ontRange);
                                    attributeValue = individualAttribute.ontValue;
                                    attributeType = RtrbauFabricationType.Inspect;
                                }
                                else
                                {
                                    // IMPORTANT: this changes the attribute range to string to allow icon creation
                                    // However, this only applies to consult elements
                                    // Report elements may require a new consult type
                                    attributeRange = new OntologyEntity(Rtrbauer.instance.uris.XSD + "#string");
                                    // attributeRange = new OntologyEntity(classAttribute.ontRange);
                                    attributeValue = Parser.ParseURI(individualAttribute.ontValue, '#', RtrbauParser.post);
                                    attributeType = RtrbauFabricationType.Observe;
                                }
                            }
                            else
                            {
                                throw new ArgumentException("Attribute format is not implemented in Rtrbau");
                            }

                            elementAttributes.Add(new RtrbauAttribute(attributeName, attributeRange, attributeValue, attributeType));
                            Debug.Log("RtrbauData: " + attributeName.name + " " + attributeRange.name + " " + attributeValue + " " + attributeType);
                        }
                        else
                        {
                            throw new ArgumentException("Attribute type does not coincide");
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentException("Individual does not belong to Class");
            }
        }
        #endregion CONSTRUCTOR

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
        public RtrbauFabricationName fabricationName;
        public RtrbauFabricationType fabricationType;
        public Dictionary<DataFacet, RtrbauAttribute> fabricationData;
        public RtrbauAugmentation fabricationAugmentation;
        public RtrbauInteraction fabricationInteraction;
        public RtrbauComprehensiveness fabricationComprehension;
        public RtrbauDescriptiveness fabricationDescription;

        #region CONSTRUCTOR
        public RtrbauFabrication (RtrbauFabricationName name, RtrbauFabricationType type, Dictionary<DataFacet, RtrbauAttribute> data)
        {
            fabricationName = name;
            fabricationType = type;
            fabricationData = data;
        }
        #endregion CONSTRUCTOR

    }
    #endregion RTRBAU_ELEMENTS

    #region AUTHORING_DATA

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class DataFacetRules
    {
        public List<string> facetNameRule;
        public List<string> facetRangeRule;
        public List<string> facetValueRule;
        public int facetRestrictivity;

        #region CONSTRUCTOR
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nameRules"></param>
        /// <param name="rangeRules"></param>
        /// <param name="valueRules"></param>
        public DataFacetRules(List<string> nameRules, List<string> rangeRules, List<string> valueRules)
        {
            facetRestrictivity = 0;
            if (nameRules != null) facetRestrictivity += 1;
            if (rangeRules != null) facetRestrictivity += 1;
            if (valueRules != null) facetRestrictivity += 1;
            
            facetNameRule = nameRules;
            facetRangeRule = rangeRules;
            facetValueRule = valueRules;
        }
        #endregion CONSTRUCTOR

        #region METHODS
        /// <summary>
        /// Given an attribute by <paramref name="attributeName"/>, <paramref name="attributeRange"/> and <paramref name="attributeValue"/>
        /// returns true if the attribute meets the facet's rules.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeRange"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public bool EvaluateFacet(string attributeName, string attributeRange, string attributeValue)
        {
            // RTRBAU ALGORITHM: Rules in loop 3
            bool nameMet = EvaluateAllRules(facetNameRule, attributeName);
            bool rangeMet = EvaluateAnyRule(facetRangeRule, attributeRange);
            bool valueMet = EvaluateAnyRule(facetValueRule, attributeValue);

            if (nameMet && rangeMet && valueMet)
            {
                return true;
            }
            else
            {
                return false;
            }
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
        public RtrbauFacetForm facetForm;
        public DataFacetRules facetRules;

        #region CONSTRUCTOR
        /// <summary>
        /// Generates a facet for a specific format independently
        /// of the combination of facet rules being used.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="rules"></param>
        public DataFacet (RtrbauFacetForm form, DataFacetRules rules)
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
        public RtrbauFabricationName formatName;
        public RtrbauFabricationType formatType;
        public List<DataFacet> formatFacets;
        public int formatRequiredFacets;
        // public List<KeyValuePair<RtrbauFacetForm, DataFacetRules>> formatFacets;

        #region CONSTRUCTOR
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
        #endregion CONSTRUCTOR

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
                if (attribute.attributeType == formatType)
                {
                    List<KeyValuePair<DataFacet, RtrbauAttribute>> assignableAttributes = new List<KeyValuePair<DataFacet, RtrbauAttribute>>();

                    foreach (DataFacet facet in formatFacets)
                    {
                        if (facet.facetRules.EvaluateFacet(attribute.attributeName.name, attribute.attributeRange.name, attribute.attributeValue))
                        {
                            // assignableFacets.Add(new KeyValuePair<KeyValuePair<RtrbauFacetForm, DataFacetRules>, RtrbauAttribute>(new KeyValuePair<RtrbauFacetForm, DataFacetRules>(facet.Key, facet.Value), attribute));
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

                //Debug.Log("EvaluateFormat: format: " + formatName + " facet: " + facet.Key.Key.facetName + "attributes assigned: " + facet.Value.Count);

                //foreach (RtrbauAttribute assigned in facet.Value)
                //{
                //    Debug.Log("EvaluateFormat: format: " + formatName + " facet: " + facet.Key.Key.facetName + " attribute: " + assigned.attributeName.name);
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
                //    foreach (KeyValuePair<DataFacet, RtrbauAttribute> attribute in fabrication.fabricationAttributes)
                //    {
                //        Debug.Log("EvaluateFormat: facet assigned: " + attribute.Key.facetRules.facetName + " with: " + attribute.Value.attributeName.name);
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
        public RtrbauAugmentation facetAugmentation;
        public List<RtrbauSense> facetSenses;

        #region CONSTRUCTOR
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
        #endregion CONSTRUCTOR

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
        public RtrbauInteraction facetInteraction;
        public List<RtrbauSense> facetSenses;

        #region CONSTRUCTOR
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
        #endregion CONSTRUCTOR
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class EnvironmentFormat
    {
        public RtrbauFabricationName formatName;
        public AugmentationFacet formatAugmentation;
        public InteractionFacet formatInteraction;

        #region CONSTRUCTOR
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
        #endregion CONSTRUCTOR

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
    //    #region CONSTRUCTOR
    //    #endregion CONSTRUCTOR
    //}

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    //[Serializable]
    //public class DescriptionFacet
    //{
    //    #region CONSTRUCTOR
    //    #endregion CONSTRUCTOR
    //}

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class UserFormat
    {
        public RtrbauFabricationName formatName;
        public RtrbauComprehensiveness formatComprehension;
        public RtrbauDescriptiveness formatDescription;

        #region CONSTRUCTOR
        public UserFormat (RtrbauFabricationName name, RtrbauComprehensiveness comprehension, RtrbauDescriptiveness description)
        {
            formatName = name;
            formatComprehension = comprehension;
            formatDescription = description;
        }
        #endregion CONSTRUCTOR

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
}
