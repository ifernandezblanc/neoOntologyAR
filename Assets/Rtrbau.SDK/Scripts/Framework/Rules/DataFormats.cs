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
        #region TEXT_FORMATS
        public static DataFormat textnone1 = new DataFormat(RtrbauFabricationName.TextNone1, RtrbauFabricationType.Observe, new List<DataFacet>
        {
           new DataFacet(RtrbauFacetForm.source, DataFacets.textFacet1)
        });
        #endregion TEXT_FORMATS

        #region ICON_FORMATS
        public static DataFormat iconnone1 = new DataFormat(RtrbauFabricationName.IconNone1, RtrbauFabricationType.Observe, new List<DataFacet>
        {
           new DataFacet(RtrbauFacetForm.source, DataFacets.iconFacet1)
        });
        #endregion ICON_FORMATS

        #region AUDIO_FORMATS
        public static DataFormat audiotap1 = new DataFormat(RtrbauFabricationName.AudioTap1, RtrbauFabricationType.Observe, new List<DataFacet>
        {
           new DataFacet(RtrbauFacetForm.source, DataFacets.audioFacet1)
        });
        #endregion AUDIO_FORMATS
        
        #region IMAGE_FORMATS
        public static DataFormat imagemanipulation1 = new DataFormat(RtrbauFabricationName.ImageManipulation1, RtrbauFabricationType.Observe, new List<DataFacet>
        {
           new DataFacet(RtrbauFacetForm.source, DataFacets.pictureFacet1)
        });
        #endregion IMAGE_FORMATS

        #region VIDEO_FORMATS
        public static DataFormat videomanipulation1 = new DataFormat(RtrbauFabricationName.VideoManipulation1, RtrbauFabricationType.Observe, new List<DataFacet>
        {
           new DataFacet(RtrbauFacetForm.source, DataFacets.videoFacet1)
        });
        #endregion VIDEO_FORMATS

        #region MODEL_FORMATS
        public static DataFormat modelmanipulation1 = new DataFormat(RtrbauFabricationName.ModelManipulation1, RtrbauFabricationType.Observe, new List<DataFacet>
        {
           new DataFacet(RtrbauFacetForm.source, DataFacets.modelFacet1)
        });
        #endregion MODEL_FORMATS

        #region HOLOGRAM_FORMATS
        public static DataFormat hologramnone1 = new DataFormat(RtrbauFabricationName.HologramNone1, RtrbauFabricationType.Observe, new List<DataFacet>
        {
           new DataFacet(RtrbauFacetForm.source, DataFacets.hologramFacet1),
           new DataFacet(RtrbauFacetForm.required, DataFacets.hologramFacet2),
           new DataFacet(RtrbauFacetForm.optional, DataFacets.hologramFacet3)
        });

        public static DataFormat hologramnone2 = new DataFormat(RtrbauFabricationName.HologramNone2, RtrbauFabricationType.Observe, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.hologramFacet1),
            new DataFacet(RtrbauFacetForm.required, DataFacets.hologramFacet4),
            new DataFacet(RtrbauFacetForm.required, DataFacets.hologramFacet5),
            new DataFacet(RtrbauFacetForm.required, DataFacets.hologramFacet6),
            new DataFacet(RtrbauFacetForm.required, DataFacets.hologramFacet7),
            new DataFacet(RtrbauFacetForm.required, DataFacets.hologramFacet8),
            new DataFacet(RtrbauFacetForm.required, DataFacets.hologramFacet9),
            new DataFacet(RtrbauFacetForm.optional, DataFacets.hologramFacet3),
        });
        #endregion HOLOGRAM_FORMATS

        #region ANIMATION_FORMATS
        public static DataFormat animationnone1 = new DataFormat(RtrbauFabricationName.AnimationNone1, RtrbauFabricationType.Observe, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.animationFacet1),
            new DataFacet(RtrbauFacetForm.required, DataFacets.animationFacet4),
            new DataFacet(RtrbauFacetForm.required, DataFacets.animationFacet25),
            new DataFacet(RtrbauFacetForm.required, DataFacets.animationFacet26)
        });

        public static DataFormat animationnone2 = new DataFormat(RtrbauFabricationName.AnimationNone2, RtrbauFabricationType.Observe, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.animationFacet1),
            new DataFacet(RtrbauFacetForm.required, DataFacets.animationFacet19),
            new DataFacet(RtrbauFacetForm.required, DataFacets.animationFacet20),
            new DataFacet(RtrbauFacetForm.required, DataFacets.animationFacet21),
            new DataFacet(RtrbauFacetForm.required, DataFacets.animationFacet22),
            new DataFacet(RtrbauFacetForm.required, DataFacets.animationFacet23),
            new DataFacet(RtrbauFacetForm.required, DataFacets.animationFacet24),
            new DataFacet(RtrbauFacetForm.optional, DataFacets.animationFacet4),
            new DataFacet(RtrbauFacetForm.optional, DataFacets.animationFacet5)
        });

        // UPG: to create others with more generic approaches to adapt to other animations rather than these ones
        #endregion ANIMATION_FORMATS

        #region TEXTBUTTON_FORMATS
        public static DataFormat textbuttontap1 = new DataFormat(RtrbauFabricationName.TextButtonTap1, RtrbauFabricationType.Inspect, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.textFacet2)
        });
        #endregion TEXTBUTTON_FORMATS

        #region ICONBUTTON_FORMATS
        public static DataFormat iconbuttontap1 = new DataFormat(RtrbauFabricationName.IconButtonTap1, RtrbauFabricationType.Inspect, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.iconFacet2)
        });
        #endregion ICONBUTTON_FORMATS

        #region MODELBUTTON_FORMATS
        // UPG: to create model buttons when cd = 1 (needs code changes)
        #endregion MODELBUTTON_FORMATS

        #region AUDIORECORD_FORMATS
        public static DataFormat audiorecordhold1 = new DataFormat(RtrbauFabricationName.AudioRecordHold1, RtrbauFabricationType.Record, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.audioFacet1)
        });
        #endregion AUDIORECORD_FORMATS

        #region PICTURERECORD_FORMATS
        public static DataFormat imagecordhold1 = new DataFormat(RtrbauFabricationName.ImageRecordHold1, RtrbauFabricationType.Record, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.pictureFacet1)
        });
        #endregion PICTURERECORD_FORMATS

        #region VIDEORECORD_FORMATS
        public static DataFormat videorecordhold1 = new DataFormat(RtrbauFabricationName.VideoRecordHold1, RtrbauFabricationType.Record, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.videoFacet1)
        });
        #endregion VIDEORECORD_FORMATS

        #region MODELRECORD_FORMATS
        public static DataFormat modelrecordhold1 = new DataFormat(RtrbauFabricationName.ModelRecordHold1, RtrbauFabricationType.Record, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.modelFacet1)
        });
        #endregion MODELRECORD_FORMATS

        #region ANIMATIONRECORD_FORMATS
        // UPG
        #endregion ANIMATIONRECORD_FORMATS

        #region DICTATION_FORMATS
        public static DataFormat textdictation1 = new DataFormat(RtrbauFabricationName.TextDictation1, RtrbauFabricationType.Record, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.textFacet1)
        });
        #endregion DICTATION_FORMATS

        #region TEXTRECORD_FORMATS
        public static DataFormat textkeyboard1 = new DataFormat(RtrbauFabricationName.TextKeyboard1, RtrbauFabricationType.Record, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.textFacet1)
        });
        #endregion TEXTRECORD_FORMATS

        #region MODELPANEL_FORMATS
        // UPG
        #endregion MODELPANEL_FORMATS

        #region TEXTPANEL_FORMATS
        public static DataFormat textpaneltap1 = new DataFormat(RtrbauFabricationName.TextPanelTap1, RtrbauFabricationType.Nominate, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.textFacet3)
        });
        #endregion TEXTPANEL_FORMATS

        #region ICONPANEL_FORMATS
        public static DataFormat iconpaneltap1 = new DataFormat(RtrbauFabricationName.IconPanelTap1, RtrbauFabricationType.Nominate, new List<DataFacet>
        {
            new DataFacet(RtrbauFacetForm.source, DataFacets.iconFacet3)
        });
        #endregion ICONPANEL_FORMATS
        #endregion FORMATS

        //#region FORMATS
        //#region AUDIO_FORMATS
        //public static DataFormat audioFormat1 = new DataFormat("audioFormat1", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.audioFacet1) }
        //});
        //#endregion AUDIO_FORMATS

        //#region TEXT_FORMATS
        //public static DataFormat textFormat1 = new DataFormat("textFormat1", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.textFacet1) }
        //});

        //public static DataFormat textFormat2 = new DataFormat("textFormat2", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.textFacet2) }
        //});

        //public static DataFormat textFormat3 = new DataFormat("textFormat3", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.textFacet3) }
        //});
        //#endregion TEXT_FORMATS

        //#region ICON_FORMATS
        //public static DataFormat iconFormat1 = new DataFormat("iconFormat1", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.iconFacet1) }
        //});
        //#endregion ICON_FORMATS

        //#region PICTURE_FORMATS
        //public static DataFormat pictureFormat1 = new DataFormat("pictureFormat1", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.pictureFacet1) }
        //});
        //#endregion PICTURE_FORMATS

        //#region VIDEO_FORMATS
        //public static DataFormat videoFormat1 = new DataFormat("videoFormat1", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.videoFacet1) }
        //});
        //#endregion VIDEO_FORMATS

        //#region MODEL_FORMATS
        //public static DataFormat modelFormat1 = new DataFormat("modelFormat1", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    {  new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.modelFacet1) }
        //});
        //#endregion MODEL_FORMATS

        //#region HOLOGRAM_FORMATS
        //public static DataFormat hologramFormat1 = new DataFormat("hologramFormat1", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.hologramFacet1) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.required, DataFacets.hologramFacet2) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.optional, DataFacets.hologramFacet3) }
        //});

        //public static DataFormat hologramFormat2 = new DataFormat("hologramFormat2", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.hologramFacet1) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.required, DataFacets.hologramFacet4) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.required, DataFacets.hologramFacet5) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.required, DataFacets.hologramFacet6) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.required, DataFacets.hologramFacet7) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.required, DataFacets.hologramFacet8) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.required, DataFacets.hologramFacet9) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.optional, DataFacets.hologramFacet3) }
        //});
        //#endregion HOLOGRAM_FORMATS

        //#region ANIMATION_FORMATS
        //public static DataFormat animationFormat1 = new DataFormat("animationFormat1", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.animationFacet1) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.required, DataFacets.animationFacet4) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.required, DataFacets.animationFacet25) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.required, DataFacets.animationFacet26) }
        //});

        //public static DataFormat animationFormat2 = new DataFormat("animationFormat2", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.animationFacet1) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.required, DataFacets.animationFacet19) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.required, DataFacets.animationFacet20) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.required, DataFacets.animationFacet21) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.required, DataFacets.animationFacet22) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.required, DataFacets.animationFacet23) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.required, DataFacets.animationFacet24) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.optional, DataFacets.animationFacet4) },
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.optional, DataFacets.animationFacet5) }
        //});

        //// UPG: to create others with more generic approaches to adapt to other animations rather than these ones
        //#endregion ANIMATION_FORMATS

        //#region TEXTBUTTON_FORMATS
        //public static DataFormat textbuttonFormat1 = new DataFormat("textbuttonFormat1", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.textFacet4) }
        //});
        //#endregion TEXTBUTTON_FORMATS

        //#region ICONBUTTON_FORMATS
        //public static DataFormat iconbuttonFormat1 = new DataFormat("iconbuttonFormat1", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.iconFacet2) }
        //});
        //#endregion ICONBUTTON_FORMATS

        //#region MODELBUTTON_FORMATS
        //// UPG: to create model buttons when cd = 1 (needs code changes)
        //#endregion MODELBUTTON_FORMATS

        //#region AUDIORECORD_FORMATS
        //public static DataFormat audiorecordFormat1 = new DataFormat("audiorecordFormat1", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.audioFacet1) }
        //});
        //#endregion AUDIORECORD_FORMATS

        //#region PICTURERECORD_FORMATS
        //public static DataFormat picturerecordFormat1 = new DataFormat("picturerecordFormat1", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.pictureFacet1) }
        //});
        //#endregion PICTURERECORD_FORMATS

        //#region VIDEORECORD_FORMATS
        //public static DataFormat videorecordFormat1 = new DataFormat("videorecordFormat1", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.videoFacet1) }
        //});
        //#endregion VIDEORECORD_FORMATS

        //#region MODELRECORD_FORMATS
        //public static DataFormat modelrecordFormat1 = new DataFormat("modelrecordFormat1", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.modelFacet1) }
        //});
        //#endregion MODELRECORD_FORMATS

        //#region ANIMATIONRECORD_FORMATS
        //#endregion ANIMATIONRECORD_FORMATS

        //#region SPEECHRECORD_FORMATS
        //public static DataFormat speechrecordFormat1 = new DataFormat("speechrecordFormat1", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.textFacet1) }
        //});
        //#endregion SPEECHRECORD_FORMATS

        //#region TEXTRECORD_FORMATS
        //public static DataFormat textrecordFormat1 = new DataFormat("textrecordFormat1", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.textFacet1) }
        //});

        //public static DataFormat textrecordFormat2 = new DataFormat("textrecordFormat2", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.textFacet2) }
        //});

        //public static DataFormat textrecordFormat3 = new DataFormat("textrecordFormat3", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.textFacet3) }
        //});
        //#endregion TEXTRECORD_FORMATS

        //#region TEXTPANEL_FORMATS
        //public static DataFormat textpanelFormat1 = new DataFormat("textpanelFormat1", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.textFacet5) }
        //});
        //#endregion TEXTPANEL_FORMATS

        //#region ICONPANEL_FORMATS
        //public static DataFormat iconpanelFormat1 = new DataFormat("iconpanelFormat1", new List<KeyValuePair<RtrbauFacetForm, DataFacetRules>>
        //{
        //    { new KeyValuePair<RtrbauFacetForm, DataFacetRules> (RtrbauFacetForm.source, DataFacets.iconFacet3) }
        //});
        //#endregion ICONPANEL_FORMATS
        //#endregion FORMATS
    }
}
