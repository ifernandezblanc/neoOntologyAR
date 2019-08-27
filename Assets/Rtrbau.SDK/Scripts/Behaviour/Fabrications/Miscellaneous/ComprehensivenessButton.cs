/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 25/07/2019
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
#endregion


namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class ComprehensivenessButton : MonoBehaviour
    {
        #region CLASS_VARIABLES
        public RtrbauComprehensiveness comprehension;
        public TextMeshProUGUI buttonText;
        public GameObject buttonPlate;
        private bool comprehensionActive = false;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public Material buttonMaterialActive;
        public Material buttonMaterialInactive;
        #endregion GAMEOBJECT_PREFABS

        #region MONOBEHAVIOUR_METHODS
        void Start()
        {
            if (buttonText == null || buttonPlate == null || buttonMaterialActive == null || buttonMaterialInactive == null)
            {
                throw new ArgumentException("Senses button requires references to object elements to work.");
            }
            else
            {
                comprehensionActive = Rtrbauer.instance.user.Comprehensiveness().Contains(comprehension);

                if (comprehensionActive)
                {
                    buttonText.text = comprehension + " active";
                    buttonPlate.GetComponent<MeshRenderer>().material = buttonMaterialActive;
                }
                else
                {
                    buttonText.text = comprehension + " inactive";
                    buttonPlate.GetComponent<MeshRenderer>().material = buttonMaterialInactive;
                }
            }
        }
        #endregion MONOBEHAVIOUR_METHODS

        #region CLASS_METHODS
        public void UpdateComprehension()
        {
            if (comprehensionActive)
            {
                buttonText.text = comprehension + " inactive";
                buttonPlate.GetComponent<MeshRenderer>().material = buttonMaterialInactive;
                comprehensionActive = false;
                Rtrbauer.instance.user.AssignComprehensiveness(comprehension, comprehensionActive);
            }
            else
            {
                buttonText.text = comprehension + " active";
                buttonPlate.GetComponent<MeshRenderer>().material = buttonMaterialActive;
                comprehensionActive = true;
                Rtrbauer.instance.user.AssignComprehensiveness(comprehension, comprehensionActive);
            }
        }
        #endregion CLASS_METHODS
    }
}