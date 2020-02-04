/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2020 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2020 Cranfield University. All Rights Reserved.
Copyright (c) 2020 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 02/02/2020
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public static class RecommendationFormats
    {
        #region FORMATS
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        #region DEFAULT_FORMATS
        #endregion DEFAULT_FORMATS

        #region TEXT_FORMATS
        #endregion TEXT_FORMATS

        #region ICON_FORMATS
        #endregion ICON_FORMATS

        #region AUDIO_FORMATS
        #endregion AUDIO_FORMATS

        #region IMAGE_FORMATS
        #endregion IMAGE_FORMATS

        #region VIDEO_FORMATS
        #endregion VIDEO_FORMATS

        #region MODEL_FORMATS
        #endregion MODEL_FORMATS

        #region HOLOGRAM_FORMATS
        #endregion ANIMATION_FORMATS

        #region TEXTBUTTON_FORMATS
        #endregion TEXTBUTTON_FORMATS

        #region ICONBUTTON_FORMATS
        #endregion ICONBUTTON_FORMATS

        #region MODELBUTTON_FORMATS
        #endregion MODELBUTTON_FORMATS

        #region AUDIORECORD_FORMATS
        #endregion AUDIORECORD_FORMATS

        #region PICTURERECORD_FORMATS
        #endregion PICTURERECORD_FORMATS

        #region VIDEORECORD_FORMATS
        #endregion VIDEORECORD_FORMATS

        #region MODELRECORD_FORMATS
        #endregion MODELRECORD_FORMATS

        #region ANIMATIONRECORD_FORMATS
        #endregion ANIMATIONRECORD_FORMATS

        #region DICTATION_FORMATS
        #endregion DICTATION_FORMATS

        #region TEXTRECORD_FORMATS
        #endregion TEXTRECORD_FORMATS

        #region TEXTPANEL_FORMATS
        public static RecommendationFormat TextPanelTap3 = new RecommendationFormat(RtrbauFabricationName.TextPanelTap3, RtrbauFabricationType.Nominate, Libraries.RecommendationStateAttributesLibrary[0], Libraries.RecommendationStateRangesLibrary[0], new List<RecommendationFacet> 
        {
            RecommendationFacets.StateStatusSubset, RecommendationFacets.StateDominionBinary, RecommendationFacets.StatePhenonmenonBinary, RecommendationFacets.StateUnitBinary, RecommendationFacets.StateComponentComponent
        });
        #endregion TEXTPANEL_FORMATS

        #region ICONPANEL_FORMATS
        #endregion ICONPANEL_FORMATS

        #region MODELPANEL_FORMATS
        #endregion MODELPANEL_FORMATS
        #endregion FORMATS
    }
}