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
    public static class UserFormats
    {
        #region FORMATS
        #region TEXT_FORMATS
        public static UserFormat textnone1 = new UserFormat(RtrbauFabricationName.TextNone1, RtrbauComprehensiveness.oneD, RtrbauDescriptiveness.literal);
        #endregion TEXT_FORMATS

        #region ICON_FORMATS
        public static UserFormat iconnone1 = new UserFormat(RtrbauFabricationName.IconNone1, RtrbauComprehensiveness.twoD, RtrbauDescriptiveness.symbolic);
        #endregion ICON_FORMATS

        #region AUDIO_FORMATS
        public static UserFormat audiotap1 = new UserFormat(RtrbauFabricationName.AudioTap1, RtrbauComprehensiveness.fourD, RtrbauDescriptiveness.literal);
        #endregion AUDIO_FORMATS

        #region IMAGE_FORMATS
        public static UserFormat imagemanipulation1 = new UserFormat(RtrbauFabricationName.ImageManipulation1, RtrbauComprehensiveness.twoD, RtrbauDescriptiveness.literal);
        #endregion IMAGE_FORMATS

        #region VIDEO_FORMATS
        public static UserFormat videomanipulation1 = new UserFormat(RtrbauFabricationName.VideoManipulation1, RtrbauComprehensiveness.threeD, RtrbauDescriptiveness.literal);
        #endregion VIDEO_FORMATS

        #region MODEL_FORMATS
        public static UserFormat modelmanipulation1 = new UserFormat(RtrbauFabricationName.ModelManipulation1, RtrbauComprehensiveness.threeD, RtrbauDescriptiveness.literal);
        #endregion MODEL_FORMATS

        #region HOLOGRAM_FORMATS
        public static UserFormat hologramnone1 = new UserFormat(RtrbauFabricationName.HologramNone1, RtrbauComprehensiveness.threeD, RtrbauDescriptiveness.symbolic);
        public static UserFormat hologramnone2 = new UserFormat(RtrbauFabricationName.HologramNone2, RtrbauComprehensiveness.threeD, RtrbauDescriptiveness.symbolic);
        #endregion HOLOGRAM_FORMATS

        #region ANIMATION_FORMATS
        public static UserFormat animationnone1 = new UserFormat(RtrbauFabricationName.AnimationNone1, RtrbauComprehensiveness.fourD, RtrbauDescriptiveness.literal);
        public static UserFormat animationnone2 = new UserFormat(RtrbauFabricationName.AnimationNone2, RtrbauComprehensiveness.fourD, RtrbauDescriptiveness.literal);
        // UPG: to create others with more generic approaches to adapt to other animations rather than these ones
        #endregion ANIMATION_FORMATS

        #region TEXTBUTTON_FORMATS
        public static UserFormat textbuttontap1 = new UserFormat(RtrbauFabricationName.TextButtonTap1, RtrbauComprehensiveness.twoD, RtrbauDescriptiveness.literal);
        #endregion TEXTBUTTON_FORMATS

        #region ICONBUTTON_FORMATS
        public static UserFormat iconbuttontap1 = new UserFormat(RtrbauFabricationName.IconButtonTap1, RtrbauComprehensiveness.threeD, RtrbauDescriptiveness.symbolic);
        #endregion ICONBUTTON_FORMATS

        #region MODELBUTTON_FORMATS
        // UPG: to create model buttons when cd = 1 (needs code changes)
        #endregion MODELBUTTON_FORMATS

        #region AUDIORECORD_FORMATS
        public static UserFormat audiorecordhold1 = new UserFormat(RtrbauFabricationName.AudioRecordHold1, RtrbauComprehensiveness.fourD, RtrbauDescriptiveness.literal);
        #endregion AUDIORECORD_FORMATS

        #region PICTURERECORD_FORMATS
        public static UserFormat imagecordhold1 = new UserFormat(RtrbauFabricationName.ImageRecordHold1, RtrbauComprehensiveness.twoD, RtrbauDescriptiveness.literal);
        #endregion PICTURERECORD_FORMATS

        #region VIDEORECORD_FORMATS
        public static UserFormat videorecordhold1 = new UserFormat(RtrbauFabricationName.VideoRecordHold1, RtrbauComprehensiveness.threeD, RtrbauDescriptiveness.literal);
        #endregion VIDEORECORD_FORMATS

        #region MODELRECORD_FORMATS
        public static UserFormat modelrecordhold1 = new UserFormat(RtrbauFabricationName.ModelRecordHold1, RtrbauComprehensiveness.threeD, RtrbauDescriptiveness.literal);
        #endregion MODELRECORD_FORMATS

        #region ANIMATIONRECORD_FORMATS
        // UPG
        #endregion ANIMATIONRECORD_FORMATS

        #region DICTATION_FORMATS
        public static UserFormat textdictation1 = new UserFormat(RtrbauFabricationName.TextDictation1, RtrbauComprehensiveness.oneD, RtrbauDescriptiveness.literal);
        // oneD or twoD??
        #endregion DICTATION_FORMATS

        #region TEXTRECORD_FORMATS
        public static UserFormat textkeyboard1 = new UserFormat(RtrbauFabricationName.TextKeyboard1, RtrbauComprehensiveness.oneD, RtrbauDescriptiveness.literal);
        #endregion TEXTRECORD_FORMATS

        #region MODELPANEL_FORMATS
        // UPG
        #endregion MODELPANEL_FORMATS

        #region TEXTPANEL_FORMATS
        public static UserFormat textpaneltap1 = new UserFormat(RtrbauFabricationName.TextPanelTap1, RtrbauComprehensiveness.oneD, RtrbauDescriptiveness.literal);
        #endregion TEXTPANEL_FORMATS

        #region ICONPANEL_FORMATS
        public static UserFormat iconpaneltap1 = new UserFormat(RtrbauFabricationName.IconPanelTap1, RtrbauComprehensiveness.twoD, RtrbauDescriptiveness.symbolic);
        #endregion ICONPANEL_FORMATS
        #endregion FORMATS
    }
}
