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
    public class SensesButton : MonoBehaviour
    {
        #region CLASS_VARIABLES
        public RtrbauSense sense;
        private bool senseActive = false;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro buttonText;
        public MeshRenderer buttonPlate;
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
                senseActive = Rtrbauer.instance.environment.Senses().Contains(sense);

                if (senseActive)
                {
                    buttonText.text = sense + " active";
                    buttonPlate.material = buttonMaterialActive;
                }
                else
                {
                    buttonText.text = sense + " inactive";
                    buttonPlate.material = buttonMaterialInactive;
                }
            }
        }
        #endregion MONOBEHAVIOUR_METHODS

        #region CLASS_METHODS
        public void UpdateSense()
        {
            if (senseActive)
            {
                buttonText.text = sense + " inactive";
                buttonPlate.material = buttonMaterialInactive;
                senseActive = false;
                Rtrbauer.instance.environment.AssignSense(sense, senseActive);
            }
            else
            {
                buttonText.text = sense + " active";
                buttonPlate.material = buttonMaterialActive;
                senseActive = true;
                Rtrbauer.instance.environment.AssignSense(sense, senseActive);
            }
        }
        #endregion CLASS_METHODS
    }
}