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
using System;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public static class DataFormats
    {
        #region FORMATS
        /// <summary>
        /// 
        /// </summary>
        #region DEFAULT_FORMATS
        public static DataFormat DefaultObserve = new DataFormat(RtrbauFabricationName.DefaultObserve, RtrbauFabricationType.Observe, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.TextFacet0)
        });

        public static DataFormat DefaultInspect = new DataFormat(RtrbauFabricationName.DefaultInspect, RtrbauFabricationType.Inspect, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.TextFacet2)
        });

        public static DataFormat DefaultRecord = new DataFormat(RtrbauFabricationName.DefaultRecord, RtrbauFabricationType.Record, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.TextFacet0)
        });

        public static DataFormat DefaultNominate = new DataFormat(RtrbauFabricationName.DefaultNominate, RtrbauFabricationType.Nominate, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.TextFacet3)
        });
        #endregion DEFAULT_FORMATS

        #region TEXT_FORMATS
        public static DataFormat TextNone1 = new DataFormat(RtrbauFabricationName.TextNone1, RtrbauFabricationType.Observe, new List<DataFacet>
        {
           new DataFacet(RtrbauFacetForm.source, DataFacets.TextFacet1)
        });
        #endregion TEXT_FORMATS

        #region ICON_FORMATS
        public static DataFormat IconNone1 = new DataFormat(RtrbauFabricationName.IconNone1, RtrbauFabricationType.Observe, new List<DataFacet>
        {
           new DataFacet(RtrbauFacetForm.source, DataFacets.IconFacet1)
        });
        #endregion ICON_FORMATS

        #region AUDIO_FORMATS
        public static DataFormat AudioTap1 = new DataFormat(RtrbauFabricationName.AudioTap1, RtrbauFabricationType.Observe, new List<DataFacet>
        {
           new DataFacet(RtrbauFacetForm.source, DataFacets.AudioFacet1)
        });
        #endregion AUDIO_FORMATS
        
        #region IMAGE_FORMATS
        public static DataFormat ImageManipulation1 = new DataFormat(RtrbauFabricationName.ImageManipulation1, RtrbauFabricationType.Observe, new List<DataFacet>
        {
           new DataFacet(RtrbauFacetForm.source, DataFacets.PictureFacet1)
        });
        #endregion IMAGE_FORMATS

        #region VIDEO_FORMATS
        public static DataFormat VideoManipulation1 = new DataFormat(RtrbauFabricationName.VideoManipulation1, RtrbauFabricationType.Observe, new List<DataFacet>
        {
           new DataFacet(RtrbauFacetForm.source, DataFacets.VideoFacet1)
        });
        #endregion VIDEO_FORMATS

        #region MODEL_FORMATS
        public static DataFormat ModelManipulation1 = new DataFormat(RtrbauFabricationName.ModelManipulation1, RtrbauFabricationType.Observe, new List<DataFacet>
        {
           new DataFacet(RtrbauFacetForm.source, DataFacets.ModelFacet1)
        });
        #endregion MODEL_FORMATS

        #region HOLOGRAM_FORMATS
        public static DataFormat HologramNone1 = new DataFormat(RtrbauFabricationName.HologramNone1, RtrbauFabricationType.Observe, new List<DataFacet>
        {
           new DataFacet(RtrbauFacetForm.source, DataFacets.HologramFacet1),
           new DataFacet(RtrbauFacetForm.required, DataFacets.HologramFacet2),
           new DataFacet(RtrbauFacetForm.optional, DataFacets.HologramFacet3)
        });

        public static DataFormat HologramNone2 = new DataFormat(RtrbauFabricationName.HologramNone2, RtrbauFabricationType.Observe, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.HologramFacet1),
            new DataFacet(RtrbauFacetForm.required, DataFacets.HologramFacet4),
            new DataFacet(RtrbauFacetForm.required, DataFacets.HologramFacet5),
            new DataFacet(RtrbauFacetForm.required, DataFacets.HologramFacet6),
            new DataFacet(RtrbauFacetForm.required, DataFacets.HologramFacet7),
            new DataFacet(RtrbauFacetForm.required, DataFacets.HologramFacet8),
            new DataFacet(RtrbauFacetForm.required, DataFacets.HologramFacet9),
            new DataFacet(RtrbauFacetForm.optional, DataFacets.HologramFacet3),
        });
        #endregion HOLOGRAM_FORMATS

        #region ANIMATION_FORMATS
        public static DataFormat AnimationNone1 = new DataFormat(RtrbauFabricationName.AnimationNone1, RtrbauFabricationType.Observe, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.AnimationFacet1),
            new DataFacet(RtrbauFacetForm.required, DataFacets.AnimationFacet4),
            new DataFacet(RtrbauFacetForm.required, DataFacets.AnimationFacet25),
            new DataFacet(RtrbauFacetForm.required, DataFacets.AnimationFacet26)
        });

        public static DataFormat AnimationNone2 = new DataFormat(RtrbauFabricationName.AnimationNone2, RtrbauFabricationType.Observe, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.AnimationFacet1),
            new DataFacet(RtrbauFacetForm.required, DataFacets.AnimationFacet19),
            new DataFacet(RtrbauFacetForm.required, DataFacets.AnimationFacet20),
            new DataFacet(RtrbauFacetForm.required, DataFacets.AnimationFacet21),
            new DataFacet(RtrbauFacetForm.required, DataFacets.AnimationFacet22),
            new DataFacet(RtrbauFacetForm.required, DataFacets.AnimationFacet23),
            new DataFacet(RtrbauFacetForm.required, DataFacets.AnimationFacet24),
            new DataFacet(RtrbauFacetForm.optional, DataFacets.AnimationFacet4),
            new DataFacet(RtrbauFacetForm.optional, DataFacets.AnimationFacet5)
        });

        // UPG: to create others with more generic approaches to adapt to other animations rather than these ones
        #endregion ANIMATION_FORMATS

        #region TEXTBUTTON_FORMATS
        public static DataFormat TextButtonTap1 = new DataFormat(RtrbauFabricationName.TextButtonTap1, RtrbauFabricationType.Inspect, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.TextFacet2)
        });
        #endregion TEXTBUTTON_FORMATS

        #region ICONBUTTON_FORMATS
        public static DataFormat IconButtonTap1 = new DataFormat(RtrbauFabricationName.IconButtonTap1, RtrbauFabricationType.Inspect, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.IconFacet2)
        });
        #endregion ICONBUTTON_FORMATS

        #region MODELBUTTON_FORMATS
        // UPG: to create model buttons when cd = 1 (needs code changes)
        #endregion MODELBUTTON_FORMATS

        #region AUDIORECORD_FORMATS
        public static DataFormat AudioRecordHold1 = new DataFormat(RtrbauFabricationName.AudioRecordHold1, RtrbauFabricationType.Record, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.AudioFacet1)
        });
        #endregion AUDIORECORD_FORMATS

        #region PICTURERECORD_FORMATS
        public static DataFormat ImageRecordHold1 = new DataFormat(RtrbauFabricationName.ImageRecordHold1, RtrbauFabricationType.Record, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.PictureFacet1)
        });
        #endregion PICTURERECORD_FORMATS

        #region VIDEORECORD_FORMATS
        public static DataFormat VideoRecordHold1 = new DataFormat(RtrbauFabricationName.VideoRecordHold1, RtrbauFabricationType.Record, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.VideoFacet1)
        });
        #endregion VIDEORECORD_FORMATS

        #region MODELRECORD_FORMATS
        public static DataFormat ModelRecordHold1 = new DataFormat(RtrbauFabricationName.ModelRecordHold1, RtrbauFabricationType.Record, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.ModelFacet1)
        });
        #endregion MODELRECORD_FORMATS

        #region ANIMATIONRECORD_FORMATS
        // UPG
        #endregion ANIMATIONRECORD_FORMATS

        #region DICTATION_FORMATS
        public static DataFormat TextDictation1 = new DataFormat(RtrbauFabricationName.TextDictation1, RtrbauFabricationType.Record, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.TextFacet1)
        });
        #endregion DICTATION_FORMATS

        #region TEXTRECORD_FORMATS
        public static DataFormat TextKeyboard1 = new DataFormat(RtrbauFabricationName.TextKeyboard1, RtrbauFabricationType.Record, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.TextFacet1)
        });
        #endregion TEXTRECORD_FORMATS

        #region MODELPANEL_FORMATS
        // UPG
        #endregion MODELPANEL_FORMATS

        #region TEXTPANEL_FORMATS
        public static DataFormat TextPanelTap1 = new DataFormat(RtrbauFabricationName.TextPanelTap1, RtrbauFabricationType.Nominate, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.TextFacet3)
        });
        #endregion TEXTPANEL_FORMATS

        #region ICONPANEL_FORMATS
        public static DataFormat IconPanelTap1 = new DataFormat(RtrbauFabricationName.IconPanelTap1, RtrbauFabricationType.Nominate, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.IconFacet3)
        });
        #endregion ICONPANEL_FORMATS
        #endregion FORMATS
    }
}
