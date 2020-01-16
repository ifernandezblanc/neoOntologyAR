/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2020 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2020 Cranfield University. All Rights Reserved.
Copyright (c) 2020 Babcock International Group. All Rights Reserved.
Copyright (c) Microsoft Corporation. All rights reserved.

Licensed under the MIT License. See LICENSE in the project root for license information.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 14/01/2020
==============================================================================*/


/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Uses MixedRealityKeyboard and MixedRealityKeyboardPreview from mrtk_development (14/01/2020)
    /// Centralises use of MixedRealityKeyboard since it was tested that only works repeatedly from one place
    /// Generated as a copy from SystemKeyboardExample from mrtk_development (14/01/2020)
    /// For non-Windows mixed reality keyboards just use Unity's default touchscreenkeyboard. 
    /// Unity's default touchscreenkeyboard will be used for MixedRealityKeyboard once Unity bug is fixed: 
    /// https://fogbugz.unity3d.com/default.asp?1137074_rttdnt8t1lccmtd3
    /// </summary>
    public class RtrbauKeyboard : MonoBehaviour
    {
        #region SINGLETON_INSTANTIATION
        private static RtrbauKeyboard rtrbauKeyboard;

        public static RtrbauKeyboard instance
        {
            get
            {
                if (!rtrbauKeyboard)
                {
                    rtrbauKeyboard = FindObjectOfType(typeof(RtrbauKeyboard)) as RtrbauKeyboard;

                    if (!rtrbauKeyboard)
                    {
                        Debug.LogError("There needs to be a RtrbauKeyboard script in the scene.");
                    }
                    else { }
                }
                else { }

                return rtrbauKeyboard;
            }
        }
        #endregion SINGLETON_INSTANTIATION

        #region CLASS_MEMBERS
#if WINDOWS_UWP
        private MixedRealityKeyboard keyboardWMR;
#elif UNITY_IOS || UNITY_ANDROID
        private TouchScreenKeyboard keyboardTS;
#endif
        [SerializeField]
        private TextMeshPro debugMessage;
        [SerializeField]
        private MixedRealityKeyboardPreview keyboardPreview;
        public TextMeshPro keyboardInput;
        #endregion CLASS_MEMBERS

        #region CLASS_EVENTS
        // private bool keyboardInUse;
        #endregion CLASS_EVENTS

        #region GAMEOBJECT_PREFABS

        #endregion GAMEOBJECT_PREFABS

        #region MONOBEHAVIOUR_METHODS
        void Awake() { }
        void Start() { InitialiseRtrbauKeyboard(); }
        void Update() { UpdateRtrbauKeyboard(); }
        #endregion MONOBEHAVIOUR_METHODS

        #region INITIALISATION_METHODS
        #endregion INITIALISATION_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        private void InitialiseRtrbauKeyboard() 
        {
            // Initialise variables
            keyboardInput = null;
            // keyboardInUse = false;
            // Initialise keyboard according to system
#if WINDOWS_UWP
            // Windows mixed reality keyboard initialization goes here
            keyboardWMR = gameObject.AddComponent<MixedRealityKeyboard>();

            if (keyboardPreview != null) { keyboardPreview.gameObject.SetActive(false); }

            if (keyboardWMR.OnShowKeyboard != null)
            {
                keyboardWMR.OnShowKeyboard.AddListener(StartRtrbauKeyboard);
            }

            if (keyboardWMR.OnHideKeyboard != null)
            {
                keyboardWMR.OnHideKeyboard.AddListener(StopRtrbauKeyboard);
            }
#elif UNITY_IOS || UNITY_ANDROID
            // Non-Windows mixed reality keyboard initialization goes here
#else
            // Editor mixed reality keyboard initialization goes here
#endif
        }

        private void StartRtrbauKeyboard()
        {
#if WINDOWS_UWP
            // Windows mixed reality keyboard start and re-start methods goes here
            if (keyboardPreview != null) { keyboardPreview.gameObject.SetActive(true); }
#elif UNITY_IOS || UNITY_ANDROID
            // Non-Windows mixed reality keyboard start and re-start methods goes here
#else
            // Editor mixed reality keyboard start and re-start goes here
            if (debugMessage != null) { debugMessage.text = "RtrbauKeyboard::StartRtrbauKeyboard: Keyboard tried to start but not supported"; }
#endif
            RtrbauDebug.instance.Log("RtrbauKeyboard::StartRtrbauKeyboard: Keyboard started");
        }

        private void StopRtrbauKeyboard()
        {
#if WINDOWS_UWP
            // Windows mixed reality keyboard stop methods goes here
            if (keyboardPreview != null) { keyboardPreview.Text = string.Empty; keyboardPreview.CaretIndex = 0; keyboardPreview.gameObject.SetActive(false); }
#elif UNITY_IOS || UNITY_ANDROID
            // Non-Windows mixed reality keyboard stop methods goes here
#else
            // Editor mixed reality keyboard stop goes here
            if (debugMessage != null) { debugMessage.text = "RtrbauKeyboard::StartRtrbauKeyboard: Keyboard tried to stop but not supported"; }
#endif
            CloseRtrbauKeyboard();
            RtrbauDebug.instance.Log("RtrbauKeyboard::StartRtrbauKeyboard: Keyboard stopped");
        }

        private void UpdateRtrbauKeyboard()
        {
            // UPG: Does not use keyboardInUse due to MixedRealityKeyboard not able to detect when keyboard closed with keyboard button
            // UPG: In such case OnHideKeyboard is not trigger and so StopRtrbauKeyboard does not work
#if WINDOWS_UWP
            // Windows mixed reality keyboard update goes here
            if (keyboardWMR.Visible)
            {
                if (debugMessage != null) { debugMessage.text = "Typing... " + keyboardWMR.Text; }

                if (keyboardPreview != null)
                {
                    keyboardPreview.Text = keyboardWMR.Text;
                    keyboardPreview.CaretIndex = keyboardWMR.CaretIndex;
                }

                if (keyboardInput != null) { keyboardInput.text = keyboardWMR.Text; }
            }
            else
            {
                string keyboardText = keyboardWMR.Text;

                if (string.IsNullOrEmpty(keyboardText))
                {
                    if (debugMessage != null) { debugMessage.text = "Focus to open keyboard"; }
                }
                else
                {
                    if (debugMessage != null) { debugMessage.text = "Typed: " + keyboardText; }
                }
            }
#elif UNITY_IOS || UNITY_ANDROID
            // Non-Windows mixed reality keyboard update goes here
            if (keyboardTS != null)
            {
                string keyboardText = touchscreenKeyboard.text;
                if (keyboardTS.visible)
                {
                    if (debugMessage != null) { debugMessage.text = "Typing... " + keyboardText; }
                    if (keyboardInput != null) { keyboardInput.text = keyboardText; }
                }
                else
                {
                    if (debugMessage != null) { debugMessage.text = "Typed: " + keyboardText; }
                    keyboardTS = null;
                }
            }
            else { }
#else
            // Editor mixed reality keyboard update goes here
            if (keyboardInput != null)
            {
                keyboardInput.text = "Keyboard not supported";
                StopRtrbauKeyboard();
            }
            else { }
#endif
        }

        private void CloseRtrbauKeyboard() 
        {
            // if (keybooardInUse) {}
            keyboardInput = null;
            // keyboardInUse = false;
            RtrbauDebug.instance.Log("RtrbauKeyboard::CloseRtrbauKeyboard: Keyboard closed");
            // else {RtrbauDebug.instance.Log("RtrbauKeyboard::CloseRtrbauKeyboard: Keyboard cannot be opened because it is in use");throw new ArgumentException("RtrbauKeyboard::CloseRtrbauKeyboard: Keyboard cannot be opened because it is in use"); }
        }
        #endregion PRIVATE

        #region PUBLIC
        public void OpenRtrbauKeyboard(TouchScreenKeyboardType keyboardType, TextMeshPro keyboardOutput)
        {
            // if (!keyboardInUse) { }
            keyboardInput = keyboardOutput;
            // keyboardInUse = true;
#if WINDOWS_UWP
            keyboardWMR.ClearKeyboardText();
            keyboardWMR.ShowKeyboard(keyboardWMR.Text, false);
#elif UNITY_IOS || UNITY_ANDROID
            keyboardTS = TouchScreenKeyboard.Open(string.Empty, keyboardType, false, false, false, false);
#endif
            RtrbauDebug.instance.Log("RtrbauKeyboard::OpenRtrbauKeyboard: Keyboard opened");
            // else {RtrbauDebug.instance.Log("RtrbauKeyboard::CloseRtrbauKeyboard: Keyboard cannot be opened because it is in use");throw new ArgumentException("RtrbauKeyboard::CloseRtrbauKeyboard: Keyboard cannot be opened because it is in use"); }
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}