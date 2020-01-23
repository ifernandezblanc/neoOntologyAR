/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 23/07/2019
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Rotates the icon being used for loading
    /// </summary>
    public class LoadingAnimation : MonoBehaviour
    {
        #region CLASS_MEMBERS
        [SerializeField]
        private float loadingSpeed = 45f;
        private decimal originalScaleX;
        private decimal originalScaleY;
        private decimal scaleFactorX;
        private decimal scaleFactorY;
        #endregion CLASS_MEMBERS

        #region GAMEOBJECT_PREFABS
        public TextMeshPro statusText;
        public SpriteRenderer iconSprite;
        public SpriteRenderer logoSprite;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        public bool scaleInitialised;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Awake() { InitialiseScaling(); }

        void Start() { }

        void Update() { RotateTransform(iconSprite.transform, loadingSpeed/3); RotateTransform(logoSprite.transform, loadingSpeed); }
        #endregion MONOBEHAVIOUR_METHODS

        #region CLASS_METHODS
        #region PRIVATE
        void RotateTransform(Transform transform, float speed)
        {
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * speed);
        }

        void InitialiseScaling()
        {
            originalScaleX = (decimal)this.transform.localScale.x;
            originalScaleY = (decimal)this.transform.localScale.y;
            scaleInitialised = true;
            Debug.Log("LoadingAnimation::ScaleLoadingImages: originalScaleX is " + originalScaleX + " and originalScaleY is " + originalScaleY);
        }

        void CalculateScaleFactors()
        {
            if (scaleInitialised != true) { InitialiseScaling(); }
            scaleFactorX = (decimal)this.transform.localScale.x / originalScaleX;
            scaleFactorY = (decimal)this.transform.localScale.y / originalScaleY;
            Debug.Log("LoadingAnimation::ScaleLoadingImages: currentScaleX is " + (decimal)this.transform.localScale.x + " and currentScaleY is " + (decimal)this.transform.localScale.y);
            Debug.Log("LoadingAnimation::ScaleLoadingImages: scaleFactorX is " + scaleFactorX + " and scaleFactorY is " + scaleFactorY);
        }

        void ScaleRectTransform(RectTransform transform)
        {
            // Calculate new delta values
            decimal currentDeltaX = (decimal)transform.sizeDelta.x;
            decimal currentDeltaY = (decimal)transform.sizeDelta.y;
            decimal transformDeltaX = currentDeltaX / scaleFactorX;
            decimal transformDeltaY = currentDeltaY / scaleFactorY;
            // Calculate new scale values
            decimal currentScaleX = (decimal)transform.localScale.x;
            decimal currentScaleY = (decimal)transform.localScale.y;
            decimal transformScaleX = currentScaleX * scaleFactorX;
            decimal transformScaleY = currentScaleY * scaleFactorY;
            // It is expected loadingPlate to be re-scaled over the y-axis
            // Modify x-axis values accordingly
            transform.sizeDelta = new Vector2((float)transformDeltaX, transform.sizeDelta.y);
            transform.localScale = new Vector3((float)transformScaleX, transform.localScale.y, transform.localScale.z);
        }

        void ScaleTransform(Transform transform)
        {
            // Calculate new scale values
            decimal currentScaleX = (decimal)transform.localScale.x;
            decimal currentScaleY = (decimal)transform.localScale.y;
            decimal transformScaleX = currentScaleX * scaleFactorX;
            decimal transformScaleY = currentScaleY * scaleFactorY;
            // It is expected loadingPlate to be re-scaled over the y-axis
            // Modify x-axis values accordingly
            transform.localScale = new Vector3((float)transformScaleX, transform.localScale.y, transform.localScale.z);
        }
        #endregion PRIVATE

        #region PUBLIC
        public void ScaleLoadingImages()
        {
            // Determine actual scales
            // Calculate factor scale for scaled loading plate
            CalculateScaleFactors();
            // Scale rectTransform of TextMeshPro
            ScaleRectTransform(statusText.rectTransform);
            // Scale transforms of SpriteRenderers
            ScaleTransform(iconSprite.transform);
            ScaleTransform(logoSprite.transform);
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}


