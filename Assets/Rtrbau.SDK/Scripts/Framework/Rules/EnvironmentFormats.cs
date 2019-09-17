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
    public static class EnvironmentFormats
    {
        #region FORMATS
        /// <summary>
        /// 
        /// </summary>
        #region DEFAULT_FORMATS
        public static EnvironmentFormat DefaultObserve = new EnvironmentFormat(RtrbauFabricationName.DefaultObserve, EnvironmentFacets.text, EnvironmentFacets.none);
        public static EnvironmentFormat DefaultInspect = new EnvironmentFormat(RtrbauFabricationName.DefaultInspect, EnvironmentFacets.text, EnvironmentFacets.gesturetap);
        public static EnvironmentFormat DefaultRecord = new EnvironmentFormat(RtrbauFabricationName.DefaultRecord, EnvironmentFacets.text, EnvironmentFacets.gesturekeyboard);
        public static EnvironmentFormat DefaultNominate = new EnvironmentFormat(RtrbauFabricationName.DefaultNominate, EnvironmentFacets.text, EnvironmentFacets.gesturetap);
        #endregion DEFAULT_FORMATS

        #region TEXT_FORMATS
        public static EnvironmentFormat TextNone1 = new EnvironmentFormat(RtrbauFabricationName.TextNone1, EnvironmentFacets.text, EnvironmentFacets.none);
        //public static EnvironmentFormat TextNone2 = new EnvironmentFormat(RtrbauFabricationName.TextNone2, EnvironmentFacets.text, EnvironmentFacets.none);
        #endregion TEXT_FORMATS

        #region ICON_FORMATS
        public static EnvironmentFormat IconNone1 = new EnvironmentFormat(RtrbauFabricationName.IconNone1, EnvironmentFacets.icon, EnvironmentFacets.none);
        #endregion ICON_FORMATS

        #region AUDIO_FORMATS
        public static EnvironmentFormat AudioTap1 = new EnvironmentFormat(RtrbauFabricationName.AudioTap1, EnvironmentFacets.audio, EnvironmentFacets.gesturetap);
        #endregion AUDIO_FORMATS

        #region IMAGE_FORMATS
        public static EnvironmentFormat ImageManipulation1 = new EnvironmentFormat(RtrbauFabricationName.ImageManipulation1, EnvironmentFacets.image, EnvironmentFacets.gesturemanipulation);
        #endregion IMAGE_FORMATS

        #region VIDEO_FORMATS
        public static EnvironmentFormat VideoManipulation1 = new EnvironmentFormat(RtrbauFabricationName.VideoManipulation1, EnvironmentFacets.video, EnvironmentFacets.gesturemanipulation);
        #endregion VIDEO_FORMATS

        #region MODEL_FORMATS
        public static EnvironmentFormat ModelManipulation1 = new EnvironmentFormat(RtrbauFabricationName.ModelManipulation1, EnvironmentFacets.model, EnvironmentFacets.gesturemanipulation);
        #endregion MODEL_FORMATS

        #region HOLOGRAM_FORMATS
        public static EnvironmentFormat HologramNone1 = new EnvironmentFormat(RtrbauFabricationName.HologramNone1, EnvironmentFacets.hologram, EnvironmentFacets.none);
        public static EnvironmentFormat HologramNone2 = new EnvironmentFormat(RtrbauFabricationName.HologramNone2, EnvironmentFacets.hologram, EnvironmentFacets.none);
        #endregion HOLOGRAM_FORMATS

        #region ANIMATION_FORMATS
        public static EnvironmentFormat AnimationNone1 = new EnvironmentFormat(RtrbauFabricationName.AnimationNone1, EnvironmentFacets.animation, EnvironmentFacets.none);
        public static EnvironmentFormat AnimationNone2 = new EnvironmentFormat(RtrbauFabricationName.AnimationNone2, EnvironmentFacets.animation, EnvironmentFacets.none);
        // UPG: to create others with more generic approaches to adapt to other animations rather than these ones
        #endregion ANIMATION_FORMATS

        #region TEXTBUTTON_FORMATS
        public static EnvironmentFormat TextButtonTap1 = new EnvironmentFormat(RtrbauFabricationName.TextButtonTap1, EnvironmentFacets.text, EnvironmentFacets.gesturetap);

        #endregion TEXTBUTTON_FORMATS

        #region ICONBUTTON_FORMATS
        public static EnvironmentFormat IconButtonTap1 = new EnvironmentFormat(RtrbauFabricationName.IconButtonTap1, EnvironmentFacets.icon, EnvironmentFacets.gesturetap);
        #endregion ICONBUTTON_FORMATS

        #region MODELBUTTON_FORMATS
        // UPG: to create model buttons when cd = 1 (needs code changes)
        #endregion MODELBUTTON_FORMATS

        #region AUDIORECORD_FORMATS
        public static EnvironmentFormat AudioRecordHold1 = new EnvironmentFormat(RtrbauFabricationName.AudioRecordHold1, EnvironmentFacets.audio, EnvironmentFacets.gesturehold);
        #endregion AUDIORECORD_FORMATS

        #region PICTURERECORD_FORMATS
        public static EnvironmentFormat ImageRecordHold1 = new EnvironmentFormat(RtrbauFabricationName.ImageRecordHold1, EnvironmentFacets.image, EnvironmentFacets.gesturehold);
        #endregion PICTURERECORD_FORMATS

        #region VIDEORECORD_FORMATS
        public static EnvironmentFormat VideoRecordHold1 = new EnvironmentFormat(RtrbauFabricationName.VideoRecordHold1, EnvironmentFacets.video, EnvironmentFacets.gesturehold);
        #endregion VIDEORECORD_FORMATS

        #region MODELRECORD_FORMATS
        public static EnvironmentFormat ModelRecordHold1 = new EnvironmentFormat(RtrbauFabricationName.ModelRecordHold1, EnvironmentFacets.model, EnvironmentFacets.gesturehold);
        #endregion MODELRECORD_FORMATS

        #region ANIMATIONRECORD_FORMATS
        // UPG
        #endregion ANIMATIONRECORD_FORMATS

        #region DICTATION_FORMATS
        public static EnvironmentFormat TextDictation1 = new EnvironmentFormat(RtrbauFabricationName.TextDictation1, EnvironmentFacets.text, EnvironmentFacets.dictation);
        #endregion DICTATION_FORMATS

        #region TEXTRECORD_FORMATS
        public static EnvironmentFormat TextKeyboard1 = new EnvironmentFormat(RtrbauFabricationName.TextKeyboard1, EnvironmentFacets.text, EnvironmentFacets.gesturekeyboard);
        #endregion TEXTRECORD_FORMATS

        #region MODELPANEL_FORMATS
        // UPG
        #endregion MODELPANEL_FORMATS

        #region TEXTPANEL_FORMATS
        public static EnvironmentFormat TextPanelTap1 = new EnvironmentFormat(RtrbauFabricationName.TextPanelTap1, EnvironmentFacets.text, EnvironmentFacets.gesturetap);
        #endregion TEXTPANEL_FORMATS

        #region ICONPANEL_FORMATS
        public static EnvironmentFormat IconPanelTap1 = new EnvironmentFormat(RtrbauFabricationName.IconPanelTap1, EnvironmentFacets.text, EnvironmentFacets.gesturetap);
        #endregion ICONPANEL_FORMATS
        #endregion FORMATS
    }
}

