/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 16/08/2019
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public static class EnvironmentFacets
    {
        #region FACETS
        #region AUGMENTATION_FACETS
        public static AugmentationFacet text = new AugmentationFacet(RtrbauAugmentation.Text, new List<RtrbauSense> { RtrbauSense.sight });
        public static AugmentationFacet icon = new AugmentationFacet(RtrbauAugmentation.Icon, new List<RtrbauSense> { RtrbauSense.sight });
        public static AugmentationFacet audio = new AugmentationFacet(RtrbauAugmentation.Audio, new List<RtrbauSense> { RtrbauSense.hearing });
        public static AugmentationFacet image = new AugmentationFacet(RtrbauAugmentation.Image, new List<RtrbauSense> { RtrbauSense.sight });
        public static AugmentationFacet video = new AugmentationFacet(RtrbauAugmentation.Audio, new List<RtrbauSense> { RtrbauSense.hearing, RtrbauSense.sight });
        public static AugmentationFacet model = new AugmentationFacet(RtrbauAugmentation.Model, new List<RtrbauSense> { RtrbauSense.sight });
        public static AugmentationFacet hologram = new AugmentationFacet(RtrbauAugmentation.Hologram, new List<RtrbauSense> { RtrbauSense.sight });
        public static AugmentationFacet animation = new AugmentationFacet(RtrbauAugmentation.Animation, new List<RtrbauSense> { RtrbauSense.sight });
        #endregion AUGMENTATION_FACETS

        #region INTERACTION_FACETS
        public static InteractionFacet gesturetap = new InteractionFacet(RtrbauInteraction.GestureTap, new List<RtrbauSense> { RtrbauSense.kinaesthetic });
        public static InteractionFacet gesturedoubletap = new InteractionFacet(RtrbauInteraction.GestureDoubleTap, new List<RtrbauSense> { RtrbauSense.kinaesthetic });
        public static InteractionFacet gesturehold = new InteractionFacet(RtrbauInteraction.GestureHold, new List<RtrbauSense> { RtrbauSense.kinaesthetic });
        public static InteractionFacet gesturenavigation = new InteractionFacet(RtrbauInteraction.GestureNavigation, new List<RtrbauSense> { RtrbauSense.kinaesthetic });
        public static InteractionFacet gesturemanipulation = new InteractionFacet(RtrbauInteraction.GestureManipulation, new List<RtrbauSense> { RtrbauSense.kinaesthetic });
        public static InteractionFacet gesturekeyboard = new InteractionFacet(RtrbauInteraction.GestureKeyboard, new List<RtrbauSense> { RtrbauSense.kinaesthetic });
        public static InteractionFacet command = new InteractionFacet(RtrbauInteraction.Command, new List<RtrbauSense> { RtrbauSense.hearing });
        public static InteractionFacet dictation = new InteractionFacet(RtrbauInteraction.Dictation, new List<RtrbauSense> { RtrbauSense.hearing });
        public static InteractionFacet none = new InteractionFacet(RtrbauInteraction.None, new List<RtrbauSense>() );
        #endregion INTERACTION_FACETS
        #endregion FACETS
    }
}
