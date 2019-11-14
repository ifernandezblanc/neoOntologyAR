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

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    #region ENUMERATES
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public enum OntologyElementType : int { Ontologies, ClassSubclasses, ClassIndividuals, ClassProperties, ClassExample, IndividualProperties, IndividualUpload}

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public enum OntologyPropertyType : int { ObjectProperty, DatatypeProperty }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public enum RtrbauElementType : int { Consult, Report }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public enum RtrbauFabricationType : int { Observe, Inspect, Record, Nominate }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public enum RtrbauDistanceType : int { Component, Operation }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public enum RtrbauFileType : int { wav, png, jpg, mp4, obj, xml, dat }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired 
    /// </summary>
    public enum RtrbauFacetRuleType : int { Any, All }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public enum RtrbauFabricationName : int {
        DefaultObserve, DefaultInspect, DefaultRecord, DefaultNominate,
        AudioTap1,
        TextNone1, TextNone2,
        IconNone1,
        ImageManipulation1,
        VideoManipulation1,
        ModelManipulation1,
        HologramNone1, HologramNone2,
        AnimationNone1, AnimationNone2,
        TextButtonTap1, TextButtonTap2,
        IconButtonTap1,
        AudioRecordHold1,
        ImageRecordHold1,
        VideoRecordHold1,
        ModelRecordHold1,
        TextDictation1,
        TextKeyboard1,
        TextPanelTap1, TextPanelTap2,
        IconPanelTap1
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public enum RtrbauFacetForm : int { source, required, optional }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public enum RtrbauSense : int { hearing, sight, kinaesthetic, touch, smell }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public enum RtrbauAugmentation : int {
        Registration,
        Text, Icon, Image,
        Audio, Video,
        Hologram, Model,
        Animation
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public enum RtrbauInteraction : int {
        None, GestureKeyboard,
        GestureTap, GestureDoubleTap, GestureHold,
        GestureNavigation, GestureManipulation,
        Command, Dictation
    }

    /// <summary>
    /// Dimensions in which the content is overlaid.
    /// It is overlaid that the more dimensions used, the easier to understand the content.
    /// </summary>
    public enum RtrbauComprehensiveness : int { oneD, twoD, threeD, fourD,  }

    /// <summary>
    /// Conceptualisation of the content overlaid.
    /// It can be either literal or symbolic.
    /// </summary>
    public enum RtrbauDescriptiveness : int { literal, symbolic }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public enum RtrbauElementLocation : int { Primary, Secondary, Tertiary, Quaternary, None }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public enum RtrbauPanel : int { Ontology, User, Asset, Operation }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public enum RtrbauParser : int { pre, post }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public enum RtrbauEvent : int { Download, ButtonClicked }

    #endregion ENUMERATES
}
