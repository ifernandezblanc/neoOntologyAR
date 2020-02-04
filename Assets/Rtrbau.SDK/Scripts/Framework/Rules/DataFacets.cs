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
    public static class DataFacets
    {
        private static RtrbauFacetRuleType All = RtrbauFacetRuleType.All;
        private static RtrbauFacetRuleType Any = RtrbauFacetRuleType.Any;

        // Standard rule formatting: [All, Any, Any] (Originally design like that)
        // UPG: to comment all facet rules to ensure all are known for which fabrications apply
        // UPG: some range and type rules are not exclusive (when URILibrary, then it should already be DatatypeProperty)

        #region FACETS
        #region TEXT_FACETS
        public static DataRule TextFacet0 = new DataRule(All, null, Any, null, Any, null, Any, null); // All properties will fit into here
        public static DataRule TextFacet1 = new DataRule(All, null, Any, Libraries.TextLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule TextFacet2 = new DataRule(All, null, Any, null, Any, null, Any, Libraries.ObjectPropertyLibrary); // Includes two types: new report individual and new record individual
        public static DataRule TextFacet3 = new DataRule(All, null, Any, Libraries.StringLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule TextFacet4 = new DataRule(All, null, Any, Libraries.DateTimeLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule TextFacet5 = new DataRule(Any, Libraries.TimeRecord, Any, Libraries.DateTimeLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule TextFacet6 = new DataRule(All, null, Any, Libraries.BooleanLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule TextFacet7 = new DataRule(All, Libraries.StateIdeal, Any, Libraries.StateLibrary, Any, null, Any, Libraries.ObjectPropertyLibrary);
        public static DataRule TextFacet8 = new DataRule(Any, Libraries.StateNone, Any, Libraries.StateLibrary, Any, null, Any, Libraries.ObjectPropertyLibrary);
        //public static DataRule TextFacet3 = new DataRule(All, null, Any, Libraries.SetLibrary, Any, null); // Error in here, see above to solve
        //public static DataRule TextFacet4 = new DataRule(All, Libraries.MessageType, Any, Libraries.TextLibrary, Any, null);
        //public static DataRule TextFacet5 = new DataRule(All, Libraries.MessageAction, Any, Libraries.TextLibrary, Any, null);
        //public static DataRule TextFacet6 = new DataRule(All, Libraries.MessageObject, Any, Libraries.TextLibrary, Any, null);
        //public static DataRule TextFacet7 = new DataRule(All, Libraries.MessageValue, Any, Libraries.NumericLibrary, Any, null);
        //public static DataRule TextFacet8 = new DataRule(All, Libraries.MessageUnit, Any, Libraries.TextLibrary, Any, null);
        #endregion TEXT_FACETS

        #region ICON_FACETS
        // public static DataRule IconFacet1 = new DataRule(null, Libraries.StringLibrary, Libraries.IconLibrary);
        public static DataRule IconFacet1 = new DataRule(All, null, Any, Libraries.StringLibrary, Any, Libraries.IconLibrary, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule IconFacet2 = new DataRule(All, null, Any, null, Any, Libraries.IconLibrary, Any, Libraries.ObjectPropertyLibrary);
        // public static DataRule IconFacet3 = new DataRule(All, Libraries.IconLibrary, Any, Libraries.SetLibrary, Any, null); // Error in here, see above to solve
        #endregion ICON_FACETS

        #region AUDIO_FACETS
        public static DataRule AudioFacet1 = new DataRule(All, null, Any, Libraries.URILibrary, Any, Libraries.AudioFile, Any, Libraries.DatatypePropertyLibrary);
        #endregion AUDIO_FACETS

        #region PICTURE_FACETS
        public static DataRule PictureFacet1 = new DataRule(All, null, Any, Libraries.URILibrary, Any, Libraries.PictureFile, Any, Libraries.DatatypePropertyLibrary);
        #endregion PICTURE_FACETS

        #region VIDEO_FACETS
        public static DataRule VideoFacet1 = new DataRule(All, null, Any, Libraries.URILibrary, Any, Libraries.VideoFile, Any, Libraries.DatatypePropertyLibrary);
        #endregion VIDEO_FACETS

        #region MODEL_FACETS
        public static DataRule ModelFacet1 = new DataRule(All, null, Any, Libraries.URILibrary, Any, Libraries.ModelFile, Any, Libraries.DatatypePropertyLibrary);
        #endregion MODEL_FACETS

        #region HOLOGRAM_FACETS
        public static DataRule HologramFacet1 = new DataRule(All, null, Any, Libraries.StringLibrary, Any, Libraries.HologramLibrary, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule HologramFacet2 = new DataRule(All, null, Any, Libraries.URILibrary, Any, Libraries.ModelFile, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule HologramFacet3 = new DataRule(All, null, Any, Libraries.StringLibrary, Any, Libraries.IconLibrary, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule HologramFacet4 = new DataRule(All, Libraries.PositionX, Any, Libraries.NumericLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule HologramFacet5 = new DataRule(All, Libraries.PositionY, Any, Libraries.NumericLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule HologramFacet6 = new DataRule(All, Libraries.PositionZ, Any, Libraries.NumericLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule HologramFacet7 = new DataRule(All, Libraries.RotationX, Any, Libraries.NumericLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule HologramFacet8 = new DataRule(All, Libraries.RotationY, Any, Libraries.NumericLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule HologramFacet9 = new DataRule(All, Libraries.RotationZ, Any, Libraries.NumericLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        #endregion HOLOGRAM_FACETS

        #region ANIMATION_FACETS
        public static DataRule AnimationFacet1 = new DataRule(All, null, Any, Libraries.URILibrary, Any, Libraries.ModelFile, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet2 = new DataRule(All, null, Any, Libraries.StringLibrary, Any, Libraries.MovementLibrary, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet3 = new DataRule(All, null, Any, Libraries.StringLibrary, Any, Libraries.MovementRestrictionLibrary, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet4 = new DataRule(All, Libraries.MovementPair, Any, Libraries.URILibrary, Any, Libraries.ModelFile, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet5 = new DataRule(All, Libraries.MovementInverse, Any, Libraries.BooleanLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet6 = new DataRule(All, Libraries.MovementSpeed, Any, Libraries.NumericLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet7 = new DataRule(All, Libraries.MovementTranslationX, Any, Libraries.BooleanLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet8 = new DataRule(All, Libraries.MovementTranslationY, Any, Libraries.BooleanLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet9 = new DataRule(All, Libraries.MovementTranslationZ, Any, Libraries.BooleanLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet10 = new DataRule(All, Libraries.MovementRotationX, Any, Libraries.BooleanLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet11 = new DataRule(All, Libraries.MovementRotationY, Any, Libraries.BooleanLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet12 = new DataRule(All, Libraries.MovementRotationZ, Any, Libraries.BooleanLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet13 = new DataRule(All, Libraries.MovementTranslationX, Any, Libraries.NumericLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet14 = new DataRule(All, Libraries.MovementTranslationY, Any, Libraries.NumericLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet15 = new DataRule(All, Libraries.MovementTranslationZ, Any, Libraries.NumericLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet16 = new DataRule(All, Libraries.MovementRotationX, Any, Libraries.NumericLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet17 = new DataRule(All, Libraries.MovementRotationY, Any, Libraries.NumericLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet18 = new DataRule(All, Libraries.MovementRotationZ, Any, Libraries.NumericLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet19 = new DataRule(All, Libraries.FreedomDegreeTranslationX, Any, Libraries.BooleanLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet20 = new DataRule(All, Libraries.FreedomDegreeTranslationY, Any, Libraries.BooleanLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet21 = new DataRule(All, Libraries.FreedomDegreeTranslationZ, Any, Libraries.BooleanLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet22 = new DataRule(All, Libraries.FreedomDegreeRotationX, Any, Libraries.BooleanLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet23 = new DataRule(All, Libraries.FreedomDegreeRotationY, Any, Libraries.BooleanLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet24 = new DataRule(All, Libraries.FreedomDegreeRotationZ, Any, Libraries.BooleanLibrary, Any, null, Any, Libraries.DatatypePropertyLibrary);
        public static DataRule AnimationFacet25 = new DataRule(All, null, Any, null, Any, Libraries.MovementLibrary, Any, Libraries.ObjectPropertyLibrary);
        public static DataRule AnimationFacet26 = new DataRule(All, null, Any, null, Any, Libraries.MovementRestrictionLibrary, Any, Libraries.ObjectPropertyLibrary);
        #endregion ANIMATION_FACETS
        #endregion FACETS
    }
}
