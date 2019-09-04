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

        #region FACETS
        #region TEXT_FACETS
        public static DataFacetRules TextFacet0 = new DataFacetRules(All, null, Any, null, Any, null); // All properties will fit into here
        public static DataFacetRules TextFacet1 = new DataFacetRules(All, null, Any, Libraries.TextLibrary, Any, null);
        public static DataFacetRules TextFacet2 = new DataFacetRules(All, null, Any, null, Any, Libraries.IndividualLibrary);
        public static DataFacetRules TextFacet3 = new DataFacetRules(All, null, Any, Libraries.SetLibrary, Any, null); // Error in here, see above to solve
        #endregion TEXT_FACETS

        #region ICON_FACETS
        // public static DataFacetRules IconFacet1 = new DataFacetRules(null, Libraries.StringLibrary, Libraries.IconLibrary);
        public static DataFacetRules IconFacet1 = new DataFacetRules(All, null, Any, Libraries.StringLibrary, Any, Libraries.IconLibrary);
        public static DataFacetRules IconFacet2 = new DataFacetRules(All, Libraries.IconLibrary, Any, null, Any, Libraries.IndividualLibrary); // Error in here: need to change name rules evaluation (maybe accept type of rule evaluation as parameter?)
        public static DataFacetRules IconFacet3 = new DataFacetRules(All, Libraries.IconLibrary, Any, Libraries.SetLibrary, Any, null); // Error in here, see above to solve
        #endregion ICON_FACETS

        #region AUDIO_FACETS
        public static DataFacetRules AudioFacet1 = new DataFacetRules(All, null, Any, Libraries.URILibrary, Any, Libraries.AudioFileLibrary);
        #endregion AUDIO_FACETS

        #region PICTURE_FACETS
        public static DataFacetRules PictureFacet1 = new DataFacetRules(All, null, Any, Libraries.URILibrary, Any, Libraries.PictureFileLibrary);
        #endregion PICTURE_FACETS

        #region VIDEO_FACETS
        public static DataFacetRules VideoFacet1 = new DataFacetRules(All, null, Any, Libraries.URILibrary, Any, Libraries.VideoFileLibrary);
        #endregion VIDEO_FACETS

        #region MODEL_FACETS
        public static DataFacetRules ModelFacet1 = new DataFacetRules(All, null, Any, Libraries.URILibrary, Any, Libraries.ModelFileLibrary);
        #endregion MODEL_FACETS

        #region HOLOGRAM_FACETS
        public static DataFacetRules HologramFacet1 = new DataFacetRules(All, null, Any, Libraries.StringLibrary, Any, Libraries.HologramLibrary);
        public static DataFacetRules HologramFacet2 = new DataFacetRules(All, null, Any, Libraries.URILibrary, Any, Libraries.ModelFileLibrary);
        public static DataFacetRules HologramFacet3 = new DataFacetRules(All, null, Any, Libraries.StringLibrary, Any, Libraries.IconLibrary);
        public static DataFacetRules HologramFacet4 = new DataFacetRules(All, Libraries.PositionXLibrary, Any, Libraries.NumericLibrary, Any, null);
        public static DataFacetRules HologramFacet5 = new DataFacetRules(All, Libraries.PositionYLibrary, Any, Libraries.NumericLibrary, Any, null);
        public static DataFacetRules HologramFacet6 = new DataFacetRules(All, Libraries.PositionZLibrary, Any, Libraries.NumericLibrary, Any, null);
        public static DataFacetRules HologramFacet7 = new DataFacetRules(All, Libraries.RotationXLibrary, Any, Libraries.NumericLibrary, Any, null);
        public static DataFacetRules HologramFacet8 = new DataFacetRules(All, Libraries.RotationYLibrary, Any, Libraries.NumericLibrary, Any, null);
        public static DataFacetRules HologramFacet9 = new DataFacetRules(All, Libraries.RotationZLibrary, Any, Libraries.NumericLibrary, Any, null);
        #endregion HOLOGRAM_FACETS

        #region ANIMATION_FACETS
        public static DataFacetRules AnimationFacet1 = new DataFacetRules(All, null, Any, Libraries.URILibrary, Any, Libraries.ModelFileLibrary);
        public static DataFacetRules AnimationFacet2 = new DataFacetRules(All, null, Any, Libraries.StringLibrary, Any, Libraries.MovementLibrary);
        public static DataFacetRules AnimationFacet3 = new DataFacetRules(All, null, Any, Libraries.StringLibrary, Any, Libraries.MovementRestrictionLibrary);
        public static DataFacetRules AnimationFacet4 = new DataFacetRules(All, Libraries.MovementPairLibrary, Any, Libraries.URILibrary, Any, Libraries.ModelFileLibrary);
        public static DataFacetRules AnimationFacet5 = new DataFacetRules(All, Libraries.MovementInverseLibrary, Any, Libraries.BooleanLibrary, Any, null);
        public static DataFacetRules AnimationFacet6 = new DataFacetRules(All, Libraries.MovementSpeedLibrary, Any, Libraries.NumericLibrary, Any, null);
        public static DataFacetRules AnimationFacet7 = new DataFacetRules(All, Libraries.MovementTranslationXLibrary, Any, Libraries.BooleanLibrary, Any, null);
        public static DataFacetRules AnimationFacet8 = new DataFacetRules(All, Libraries.MovementTranslationYLibrary, Any, Libraries.BooleanLibrary, Any, null);
        public static DataFacetRules AnimationFacet9 = new DataFacetRules(All, Libraries.MovementTranslationZLibrary, Any, Libraries.BooleanLibrary, Any, null);
        public static DataFacetRules AnimationFacet10 = new DataFacetRules(All, Libraries.MovementRotationXLibrary, Any, Libraries.BooleanLibrary, Any, null);
        public static DataFacetRules AnimationFacet11 = new DataFacetRules(All, Libraries.MovementRotationYLibrary, Any, Libraries.BooleanLibrary, Any, null);
        public static DataFacetRules AnimationFacet12 = new DataFacetRules(All, Libraries.MovementRotationZLibrary, Any, Libraries.BooleanLibrary, Any, null);
        public static DataFacetRules AnimationFacet13 = new DataFacetRules(All, Libraries.MovementTranslationXLibrary, Any, Libraries.NumericLibrary, Any, null);
        public static DataFacetRules AnimationFacet14 = new DataFacetRules(All, Libraries.MovementTranslationYLibrary, Any, Libraries.NumericLibrary, Any, null);
        public static DataFacetRules AnimationFacet15 = new DataFacetRules(All, Libraries.MovementTranslationZLibrary, Any, Libraries.NumericLibrary, Any, null);
        public static DataFacetRules AnimationFacet16 = new DataFacetRules(All, Libraries.MovementRotationXLibrary, Any, Libraries.NumericLibrary, Any, null);
        public static DataFacetRules AnimationFacet17 = new DataFacetRules(All, Libraries.MovementRotationYLibrary, Any, Libraries.NumericLibrary, Any, null);
        public static DataFacetRules AnimationFacet18 = new DataFacetRules(All, Libraries.MovementRotationZLibrary, Any, Libraries.NumericLibrary, Any, null);
        public static DataFacetRules AnimationFacet19 = new DataFacetRules(All, Libraries.FreedomDegreeTranslationXLibrary, Any, Libraries.BooleanLibrary, Any, null);
        public static DataFacetRules AnimationFacet20 = new DataFacetRules(All, Libraries.FreedomDegreeTranslationYLibrary, Any, Libraries.BooleanLibrary, Any, null);
        public static DataFacetRules AnimationFacet21 = new DataFacetRules(All, Libraries.FreedomDegreeTranslationZLibrary, Any, Libraries.BooleanLibrary, Any, null);
        public static DataFacetRules AnimationFacet22 = new DataFacetRules(All, Libraries.FreedomDegreeRotationXLibrary, Any, Libraries.BooleanLibrary, Any, null);
        public static DataFacetRules AnimationFacet23 = new DataFacetRules(All, Libraries.FreedomDegreeRotationYLibrary, Any, Libraries.BooleanLibrary, Any, null);
        public static DataFacetRules AnimationFacet24 = new DataFacetRules(All, Libraries.FreedomDegreeRotationZLibrary, Any, Libraries.BooleanLibrary, Any, null);
        public static DataFacetRules AnimationFacet25 = new DataFacetRules(All, null, Any, Libraries.SetLibrary, Any, Libraries.MovementLibrary);
        public static DataFacetRules AnimationFacet26 = new DataFacetRules(All, null, Any, Libraries.SetLibrary, Any, Libraries.MovementRestrictionLibrary);
        #endregion ANIMATION_FACETS
        #endregion FACETS
    }
}
