/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.
         Part of this code has been re-used from MRTK v2.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.
Copyright (c) Microsoft Corporation. All rights reserved.

Licensed under the MIT License. See LICENSE in the project root for license information.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 18/10/2019
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;
using UnityEngine;
#endregion

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
namespace Rtrbau
{
    /// <summary>
    /// An example script that delegates keyboard API access either to the WMR workaround
    /// (MixedRealityKeyboard) or Unity's TouchScreenKeyboard API depending on the platform.
    /// </summary>
    /// <remarks>
    /// Note that like Unity's TouchScreenKeyboard API, this script only supports WSA, iOS,
    /// and Android.
    /// </remarks>
    public class SystemKeyboard : MonoBehaviour
    {
        #region CLASS_VARIABLES
        #if WINDOWS_UWP
        private MixedRealityKeyboard wmrKeyboard;
        #elif UNITY_IOS || UNITY_ANDROID
        private TouchScreenKeyboard touchscreenKeyboard;
        #endif
        public static string KeyboardText = "";
        public Material typedMaterial;
        public Material untypedMaterial;
        public MeshRenderer typeButton;
        [SerializeField]
        private TextMeshPro recordedText;
        [SerializeField]
        private GameObject loadingPanel;
        #endregion CLASS_VARIABLES

        #region MONOBEHAVIOUR_METHODS
        private void Start()
        {
            if (typedMaterial == null || untypedMaterial == null || typeButton == null)
            {
                Debug.LogError("SystemKeyboard::Start: Keyboard button elements not found. Please add them to the script.");
            }
            else
            {
                if (this.gameObject.GetComponentInParent<ElementReport>() != null)
                { loadingPanel = this.gameObject.GetComponentInParent<ElementReport>().loadingPanel; }
                #if WINDOWS_UWP
                // Windows mixed reality keyboard initialization goes here
                wmrKeyboard = gameObject.AddComponent<MixedRealityKeyboard>();
                #elif UNITY_IOS || UNITY_ANDROID
                // non-Windows mixed reality keyboard initialization goes here
                #endif
            }
        }

        private void Update()
        {
            #if WINDOWS_UWP
            // Windows mixed reality keyboard update goes here
            KeyboardText = wmrKeyboard.Text;
            if (wmrKeyboard.Visible)
            {
                recordedText.text = KeyboardText;
                if (loadingPanel != null) { if(loadingPanel.active == false) { loadingPanel.SetActive(true); } }
            }
            else
            {
                if (KeyboardText == null || KeyboardText.Length == 0)
                {
                    recordedText.text = "Focus to open keyboard";
                    typeButton.material = untypedMaterial;
                    if (loadingPanel != null) { if(loadingPanel.active == true) { loadingPanel.SetActive(false); } }
                }
                else
                {
                    recordedText.text = KeyboardText;
                    typeButton.material = typedMaterial;
                    if (loadingPanel != null) { if(loadingPanel.active == true) { loadingPanel.SetActive(false); } }
                }
            }
            #elif UNITY_IOS || UNITY_ANDROID
            // non-Windows mixed reality keyboard initialization goes here
            // for non-Windows mixed reality keyboards just use Unity's default
            // touchscreenkeyboard. 
            // We will use touchscreenkeyboard once Unity bug is fixed
            // Unity bug tracking the issue https://fogbugz.unity3d.com/default.asp?1137074_rttdnt8t1lccmtd3
            if (touchscreenKeyboard != null)
            {
                KeyboardText = touchscreenKeyboard.text;
                if (TouchScreenKeyboard.visible)
                {
                    recordedText.text = KeyboardText;
                    if (loadingPanel != null) { if(loadingPanel.active == false) { loadingPanel.SetActive(true); } }
                }
                else
                {
                    recordedText.text = KeyboardText;
                    touchscreenKeyboard = null;
                    typeButton.material = typedMaterial;
                    if (loadingPanel != null) { if(loadingPanel.active == true) { loadingPanel.SetActive(false); } }
                }
            }
            #endif
        }
        #endregion MONOBEHAVIOUR_METHODS

        #region CLASS_METHODS
        public void OpenSystemKeyboard()
        {
            #if WINDOWS_UWP
            wmrKeyboard.ShowKeyboard();
            #elif UNITY_IOS || UNITY_ANDROID
            touchscreenKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
            #else
            recordedText.text = "Keyboard not supported";
            #endif
        }
        #endregion CLASS_METHODS
    }
}