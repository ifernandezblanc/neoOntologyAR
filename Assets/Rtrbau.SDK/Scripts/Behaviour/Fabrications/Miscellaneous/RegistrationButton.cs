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
    public class RegistrationButton : MonoBehaviour
    {

        #region SINGLETON_INSTANTIATION
        private static RegistrationButton registrationButton;
        public static RegistrationButton instance
        {
            get
            {
                if (!registrationButton)
                {
                    registrationButton = FindObjectOfType(typeof(RegistrationButton)) as RegistrationButton;

                    if (!registrationButton)
                    {
                        Debug.LogError("There needs to be an Paneller script in the scene.");
                    }
                    else
                    {
                        // registrationButton.Initialise();
                    }
                }
                else { }

                return registrationButton;
            }
        }
        #endregion SINGLETON_INSTANTIATION

        #region CLASS_VARIABLES
        private GameObject registrator;
        private bool registratorActive;
        private bool buttonActive;
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public Material buttonMaterialActive;
        public Material buttonMaterialInactive;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_METHODS
        public void Initialise(GameObject registratorObject)
        {
            if (buttonMaterialActive == null || buttonMaterialInactive == null)
            {
                throw new ArgumentException("Materials need to be referenced in this script.");
            }
            else if (registratorObject == null)
            {
                Debug.LogError("RegistratorObject needs to be sent for proper initialisation");
            }
            else
            {
                registrator = registratorObject;
                registratorActive = true;
                buttonActive = true;

                this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Deactivate Registration";
                this.transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material = buttonMaterialInactive;
            }
        }

        public void RegistrationActivation()
        {
            if (buttonActive)
            {
                if (registratorActive)
                {
                    registrator.SetActive(false);
                    this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Activate Registration";
                    this.transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material = buttonMaterialActive;
                    registratorActive = false;
                }
                else
                {
                    registrator.SetActive(true);
                    this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Deactivate Registration";
                    this.transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material = buttonMaterialInactive;
                    registratorActive = true;
                }
            }
            else { }
        }
        #endregion CLASS_METHODS
    }
}
