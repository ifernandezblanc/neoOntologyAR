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
    public enum OntologyElementType : int { Ontologies, ClassSubclasses, ClassIndividuals, ClassProperties, IndividualProperties }

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
    public enum RtrbauFabricationName : int {
        AudioTap1,
        TextNone1, TextNone2, TextNone3,
        IconNone1,
        ImageManipulation1,
        VideoManipulation1,
        ModelManipulation1,
        HologramNone1, HologramNone2,
        AnimationNone1, AnimationNone2,
        TextButtonTap1,
        IconButtonTap1,
        AudioRecordHold1,
        ImageRecordHold1,
        VideoRecordHold1,
        ModelRecordHold1,
        TextDictation1,
        TextKeyboard1, TextKeyboard2, TextKeyboard3,
        TextPanelTap1,
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
        Audio, Image, Video, Model,
        Text, Icon, Hologram, Animation, Registration
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public enum RtrbauInteraction : int {
        GestureTap, GestureDoubleTap, GestureHold,
        GestureNavigation, GestureManipulation, GestureKeyboard,
        Command, Dictation, None
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
    public enum RtrbauElementLocation : int { Primary, Secondary, Tertiary }

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
