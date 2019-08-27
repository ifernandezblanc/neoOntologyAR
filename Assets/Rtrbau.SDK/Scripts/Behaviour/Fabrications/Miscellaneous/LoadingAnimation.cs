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
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Rotates the icon being used for loading
    /// </summary>
    public class LoadingAnimation : MonoBehaviour
    {
        #region CLASS_VARIABLES
        private float loadingSpeed = 3;
        #endregion CLASS_VARIABLES 

        #region MONOBEHAVIOUR_VARIABLES
        void Start()
        {

        }

        void Update()
        {
            this.transform.Rotate(new Vector3(0, 0, loadingSpeed), Space.Self);
        }
        #endregion MONOBEHAVIOUR_VARIABLES
    }
}


