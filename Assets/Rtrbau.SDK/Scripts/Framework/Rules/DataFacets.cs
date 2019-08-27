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
        #region FACETS
        #region TEXT_FACETS
        public static DataFacetRules textFacet1 = new DataFacetRules(null, null, null); // All properties will fit into here
        public static DataFacetRules textFacet2 = new DataFacetRules(null, null, Libraries.IndividualLibrary);
        public static DataFacetRules textFacet3 = new DataFacetRules(null, Libraries.SetLibrary, null); // Error in here, see above to solve
        #endregion TEXT_FACETS

        #region ICON_FACETS
        // public static DataFacetRules iconFacet1 = new DataFacetRules(null, Libraries.StringLibrary, Libraries.IconLibrary);
        public static DataFacetRules iconFacet1 = new DataFacetRules(null, Libraries.StringLibrary, Libraries.IconLibrary);
        public static DataFacetRules iconFacet2 = new DataFacetRules(Libraries.IconLibrary, null, Libraries.IndividualLibrary); // Error in here: need to change name rules evaluation (maybe accept type of rule evaluation as parameter?)
        public static DataFacetRules iconFacet3 = new DataFacetRules(Libraries.IconLibrary, Libraries.SetLibrary, null); // Error in here, see above to solve
        #endregion ICON_FACETS

        #region AUDIO_FACETS
        public static DataFacetRules audioFacet1 = new DataFacetRules(null, Libraries.URILibrary, Libraries.AudioFileLibrary);
        #endregion AUDIO_FACETS

        #region PICTURE_FACETS
        public static DataFacetRules pictureFacet1 = new DataFacetRules(null, Libraries.URILibrary, Libraries.PictureFileLibrary);
        #endregion PICTURE_FACETS

        #region VIDEO_FACETS
        public static DataFacetRules videoFacet1 = new DataFacetRules(null, Libraries.URILibrary, Libraries.VideoFileLibrary);
        #endregion VIDEO_FACETS

        #region MODEL_FACETS
        public static DataFacetRules modelFacet1 = new DataFacetRules(null, Libraries.URILibrary, Libraries.ModelFileLibrary);
        #endregion MODEL_FACETS

        #region HOLOGRAM_FACETS
        public static DataFacetRules hologramFacet1 = new DataFacetRules(null, Libraries.StringLibrary, Libraries.HologramLibrary);
        public static DataFacetRules hologramFacet2 = new DataFacetRules(null, Libraries.URILibrary, Libraries.ModelFileLibrary);
        public static DataFacetRules hologramFacet3 = new DataFacetRules(null, Libraries.StringLibrary, Libraries.IconLibrary);
        public static DataFacetRules hologramFacet4 = new DataFacetRules(Libraries.PositionXLibrary, Libraries.NumericLibrary, null);
        public static DataFacetRules hologramFacet5 = new DataFacetRules(Libraries.PositionYLibrary, Libraries.NumericLibrary, null);
        public static DataFacetRules hologramFacet6 = new DataFacetRules(Libraries.PositionZLibrary, Libraries.NumericLibrary, null);
        public static DataFacetRules hologramFacet7 = new DataFacetRules(Libraries.RotationXLibrary, Libraries.NumericLibrary, null);
        public static DataFacetRules hologramFacet8 = new DataFacetRules(Libraries.RotationYLibrary, Libraries.NumericLibrary, null);
        public static DataFacetRules hologramFacet9 = new DataFacetRules(Libraries.RotationZLibrary, Libraries.NumericLibrary, null);
        #endregion HOLOGRAM_FACETS

        #region ANIMATION_FACETS
        public static DataFacetRules animationFacet1 = new DataFacetRules(null, Libraries.URILibrary, Libraries.ModelFileLibrary);
        public static DataFacetRules animationFacet2 = new DataFacetRules(null, Libraries.StringLibrary, Libraries.MovementLibrary);
        public static DataFacetRules animationFacet3 = new DataFacetRules(null, Libraries.StringLibrary, Libraries.MovementRestrictionLibrary);
        public static DataFacetRules animationFacet4 = new DataFacetRules(Libraries.MovementPairLibrary, Libraries.URILibrary, Libraries.ModelFileLibrary);
        public static DataFacetRules animationFacet5 = new DataFacetRules(Libraries.MovementInverseLibrary, Libraries.BooleanLibrary, null);
        public static DataFacetRules animationFacet6 = new DataFacetRules(Libraries.MovementSpeedLibrary, Libraries.NumericLibrary, null);
        public static DataFacetRules animationFacet7 = new DataFacetRules(Libraries.MovementTranslationXLibrary, Libraries.BooleanLibrary, null);
        public static DataFacetRules animationFacet8 = new DataFacetRules(Libraries.MovementTranslationYLibrary, Libraries.BooleanLibrary, null);
        public static DataFacetRules animationFacet9 = new DataFacetRules(Libraries.MovementTranslationZLibrary, Libraries.BooleanLibrary, null);
        public static DataFacetRules animationFacet10 = new DataFacetRules(Libraries.MovementRotationXLibrary, Libraries.BooleanLibrary, null);
        public static DataFacetRules animationFacet11 = new DataFacetRules(Libraries.MovementRotationYLibrary, Libraries.BooleanLibrary, null);
        public static DataFacetRules animationFacet12 = new DataFacetRules(Libraries.MovementRotationZLibrary, Libraries.BooleanLibrary, null);
        public static DataFacetRules animationFacet13 = new DataFacetRules(Libraries.MovementTranslationXLibrary, Libraries.NumericLibrary, null);
        public static DataFacetRules animationFacet14 = new DataFacetRules(Libraries.MovementTranslationYLibrary, Libraries.NumericLibrary, null);
        public static DataFacetRules animationFacet15 = new DataFacetRules(Libraries.MovementTranslationZLibrary, Libraries.NumericLibrary, null);
        public static DataFacetRules animationFacet16 = new DataFacetRules(Libraries.MovementRotationXLibrary, Libraries.NumericLibrary, null);
        public static DataFacetRules animationFacet17 = new DataFacetRules(Libraries.MovementRotationYLibrary, Libraries.NumericLibrary, null);
        public static DataFacetRules animationFacet18 = new DataFacetRules(Libraries.MovementRotationZLibrary, Libraries.NumericLibrary, null);
        public static DataFacetRules animationFacet19 = new DataFacetRules(Libraries.FreedomDegreeTranslationXLibrary, Libraries.BooleanLibrary, null);
        public static DataFacetRules animationFacet20 = new DataFacetRules(Libraries.FreedomDegreeTranslationYLibrary, Libraries.BooleanLibrary, null);
        public static DataFacetRules animationFacet21 = new DataFacetRules(Libraries.FreedomDegreeTranslationZLibrary, Libraries.BooleanLibrary, null);
        public static DataFacetRules animationFacet22 = new DataFacetRules(Libraries.FreedomDegreeRotationXLibrary, Libraries.BooleanLibrary, null);
        public static DataFacetRules animationFacet23 = new DataFacetRules(Libraries.FreedomDegreeRotationYLibrary, Libraries.BooleanLibrary, null);
        public static DataFacetRules animationFacet24 = new DataFacetRules(Libraries.FreedomDegreeRotationZLibrary, Libraries.BooleanLibrary, null);
        public static DataFacetRules animationFacet25 = new DataFacetRules(null, Libraries.SetLibrary, Libraries.MovementLibrary);
        public static DataFacetRules animationFacet26 = new DataFacetRules(null, Libraries.SetLibrary, Libraries.MovementRestrictionLibrary);
        #endregion ANIMATION_FACETS
        #endregion FACETS
    }
}
