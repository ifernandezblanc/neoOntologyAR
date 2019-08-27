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
    public class DescriptivenessButton : MonoBehaviour
    {
        #region CLASS_VARIABLES
        public RtrbauDescriptiveness description;
        public TextMeshProUGUI buttonText;
        public GameObject buttonPlate;
        private bool descriptionActive = false;
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
                descriptionActive = Rtrbauer.instance.user.Descriptivenesses().Contains(description);

                if (descriptionActive)
                {
                    buttonText.text = description + " active";
                    buttonPlate.GetComponent<MeshRenderer>().material = buttonMaterialActive;
                }
                else
                {
                    buttonText.text = description + " inactive";
                    buttonPlate.GetComponent<MeshRenderer>().material = buttonMaterialInactive;
                }
            }
        }
        #endregion MONOBEHAVIOUR_METHODS

        #region CLASS_METHODS
        public void UpdateDescription()
        {
            if (descriptionActive)
            {
                buttonText.text = description + " inactive";
                buttonPlate.GetComponent<MeshRenderer>().material = buttonMaterialInactive;
                descriptionActive = false;
                Rtrbauer.instance.user.AssignDescriptiveness(description, descriptionActive);
            }
            else
            {
                buttonText.text = description + " active";
                buttonPlate.GetComponent<MeshRenderer>().material = buttonMaterialActive;
                descriptionActive = true;
                Rtrbauer.instance.user.AssignDescriptiveness(description, descriptionActive);
            }
        }
        #endregion CLASS_METHODS
    }
}