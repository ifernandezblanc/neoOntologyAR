/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 08/07/2019
==============================================================================*/

#region NAMESPACES
/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public static class Initialiser
    {
        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void CreateDirectories()
        {
            foreach (string directory in Dictionaries.ontDataDirectories.Values)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                else {}
            }

            foreach (string directory in Dictionaries.distanceDataDirectories.Values)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                else {}
            }

            foreach (string directory in Dictionaries.fileDataDirectories.Values)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                else {}
            }

            if (!Directory.Exists(Dictionaries.reportsFileDirectory))
            {
                Directory.CreateDirectory(Dictionaries.reportsFileDirectory);
            }
            else { }

            if (!Directory.Exists(Dictionaries.logsFileDirectory))
            {
                Directory.CreateDirectory(Dictionaries.logsFileDirectory);
            }
            else { }

        }

        /// <summary>
        /// Describe script purpose
        /// Add links when code has been inspired
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void ListFabrications()
        {
            // To read from prefabs and assign accordingly (either on run-time or editor)
            // First loading scene or previously to scene load?
        }
    }
}
